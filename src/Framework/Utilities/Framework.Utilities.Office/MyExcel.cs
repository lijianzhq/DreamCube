using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Win32API;
using DreamCube.Foundation.Basic.Win32API.API;

namespace DreamCube.Framework.Utilities.Office
{
    /// <summary>
    /// Excel操作辅助类
    /// </summary>
    public class MyExcel : IDisposable
    {
        #region "私有字段"

        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private WeakDictionaryCachePool<String, DataTable> dataCache = null;

        /// <summary>
        /// 内部对Excel进程的引用
        /// </summary>
        private Excel.Application innerExcelApp = null;

        /// <summary>
        /// 保存当前Excel文件的路径
        /// </summary>
        private String filePath = String.Empty;

        /// <summary>
        /// 表示当前的工作簿
        /// </summary>
        private Excel.Workbook currentWorkbook = null;

        /// <summary>
        /// 表示当前工作表
        /// </summary>
        private Excel.Worksheet currentWorksheet = null;

        /// <summary>
        /// Com组件的缺省参数
        /// </summary>
        private static Object missingValue = System.Reflection.Missing.Value;

        /// <summary>
        /// 工作表数量
        /// </summary>
        private Int32? workSheetsCount = null;

        /// <summary>
        /// 缓存所有工作表的表名
        /// </summary>
        private Dictionary<Int32, String> workSheetNames = null;

        /// <summary>
        /// 标志是否已经释放过内存
        /// </summary>
        private Boolean hasDisposed = false;

        /// <summary>
        /// 当前工作表的名称
        /// </summary>
        private String currentWorksheetName = null;

        /// <summary>
        /// 每一个工作表的数据行/列的缓存区
        /// </summary>
        private Dictionary<String, Int32> dataRowCountCache = new Dictionary<String, Int32>();
        private Dictionary<String, Int32> dataColumnCountCache = new Dictionary<String, Int32>();

        /// <summary>
        /// 保存字母与数字的映射关系，A-Z分别对应：1-26
        /// </summary>
        private static Dictionary<String, Int32> wordNumberMapper = new Dictionary<String, Int32>();

        #endregion

        #region "属性"

        /// <summary>
        /// 表示此Excel文档是否包含标题列
        /// </summary>
        public Boolean HasTitleRow
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取内部的Excel引用
        /// </summary>
        public Excel.Application InnerExcelApp
        {
            get { return this.innerExcelApp; }
        }

        /// <summary>
        /// 先判断缓冲区是否为空
        /// </summary>
        private WeakDictionaryCachePool<String, DataTable> DataCache
        {
            get
            {
                if (dataCache == null)
                {
                    WeakDictionaryCachePool<String, DataTable> temp = new WeakDictionaryCachePool<String, DataTable>();
                    Interlocked.CompareExchange(ref this.dataCache, temp, null);
                }
                return dataCache;
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private String ConnectionString
        {
            get
            {
                String connectionTemplate = this.VersionType == OfficeVersionType.Office2003 ?
                                            Properties.Resources.Excel2003ConnectionString :
                                            Properties.Resources.Excel2007ConnectionString;
                return String.Format(connectionTemplate, this.filePath);
            }
        }

        /// <summary>
        /// 当前工作表的名字
        /// </summary>
        public String CurrentWorksheetName
        {
            get
            {
                if (this.currentWorksheet != null)
                    return this.currentWorksheet.Name;
                return null;
            }
        }

        /// <summary>
        /// 返回Excel的版本枚举值
        /// </summary>
        public OfficeVersionType VersionType
        {
            get;
            private set;
        }

        /// <summary>
        /// 所有工作表的表名
        /// </summary>
        public Dictionary<Int32, String> WorkSheetNames
        {
            get
            {
                if (this.workSheetNames == null)
                {
                    Dictionary<Int32, String> temp = new Dictionary<Int32, String>();
                    for (Int32 i = 1; i <= this.WorkSheetsCount; i++)
                        temp.Add(i, ((Excel.Worksheet)this.currentWorkbook.Worksheets[i]).Name);
                    Interlocked.CompareExchange(ref this.workSheetNames, temp, null);
                }
                return this.workSheetNames;
            }
        }

        /// <summary>
        /// 工作表数量
        /// </summary>
        public Int32 WorkSheetsCount
        {
            get
            {
                if (workSheetsCount == null)
                    workSheetsCount = this.currentWorkbook.Worksheets.Count;
                return workSheetsCount.Value;
            }
        }

        /// <summary>
        /// Excel的数据行数
        /// </summary>
        public Int32 DataRowCount
        {
            get
            {
                if (!this.dataRowCountCache.ContainsKey(this.CurrentWorksheetName))
                    this.dataRowCountCache.Add(this.CurrentWorksheetName, this.currentWorksheet.UsedRange.Rows.Count);
                return dataRowCountCache[this.CurrentWorksheetName];
            }
        }

        /// <summary>
        /// Excel的数据列数
        /// </summary>
        public Int32 DataColumnCount
        {
            get
            {
                if (!this.dataColumnCountCache.ContainsKey(this.CurrentWorksheetName))
                    this.dataColumnCountCache.Add(this.CurrentWorksheetName, this.currentWorksheet.UsedRange.Columns.Count);
                return this.dataColumnCountCache[this.CurrentWorksheetName];
            }
        }

        /// <summary>
        /// 字母与数字的一一对应关系
        /// </summary>
        private static Dictionary<String, Int32> WordNumberMapper
        {
            get
            {
                //先把字母缓存起来A=1----Z=26
                if (wordNumberMapper.Count == 0)
                {
                    Int32 aWordIndex = MyString.ToAsciiCode("A");
                    for (Int32 i = 1; i <= 26; i++)
                    {
                        wordNumberMapper.Add(MyString.RecoverFromAsciiCode(aWordIndex, ""), i);
                        aWordIndex++;
                    }
                }
                return wordNumberMapper;
            }
        }

        #endregion

        #region "构造函数"

        /// <summary>
        /// 构造函数
        /// 如果根据提供的路径找不到Excel文件，则会在输入的路径位置创建新的Excel文件
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="hasTitleRow">
        /// 这个参数非常重要；在读取Excel数据的操作中，
        /// 如果此参数为true时，表示含有标题行（第一行为标题行），
        /// 则在读取数据的时候，会自动的跳过第一行的数据；
        /// 反之，把第一行的数据也读取出来。
        /// </param>
        /// <param name="defaultSheetName">默认加载Excel的哪一个工作表，如果不传入此参数，则默认加载第一个工作表</param>
        public MyExcel(String filePath, Boolean hasTitleRow, String defaultSheetName)
        {
            Boolean needCreate = false;
            this.HasTitleRow = hasTitleRow;
            ///如果传入的Excel文件不存在，则创建新的Excel文件
            if (!MyIO.IsFileExists(filePath))
            {
                needCreate = true;
            }
            else
            {
                //把文件的只读属性去掉
                MyIO.MakeFileCanWrite(filePath);
            }
            this.filePath = filePath;
            if (this.filePath.EndsWith(".xls"))
                this.VersionType = OfficeVersionType.Office2003;
            else
                this.VersionType = OfficeVersionType.Office2007;
            //加载默认的工作本
            this.LoadDefaultExcelWorkSheets(needCreate, defaultSheetName);
        }

        public MyExcel(String filePath, String defaultSheetName) :
            this(filePath, false, defaultSheetName)
        { }

        /// <summary>
        /// 构造函数
        /// 如果根据提供的路径找不到Excel文件，则会在输入的路径位置创建新的Excel文件(默认是不包含标题列的）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        public MyExcel(String filePath) :
            this(filePath, false, "")
        { }

        #endregion

        #region "实例方法"

        /// <summary>
        /// 保存方法
        /// </summary>
        public void Save()
        {
            Int32 workBookCount = this.innerExcelApp.Workbooks.Count;
            for (Int32 i = 1; i <= workBookCount; i++)
            {
                this.innerExcelApp.Workbooks[i].Save();
            }
            this.Dispose();
        }

        /// <summary>
        /// 把内部的缓冲区升级为强引用类型的；
        /// 当前高速循环excel获取数据的时候，可以允许临时升级缓冲数据位强引用以提高效率
        /// 注意：必须在使用GetCellDataOfCurrentSheet()方法前调用此方法才有效果的；并且高速循环用完毕之后，记得降级为原来的弱引用
        /// </summary>
        public void UpDataInnerCacheToStrongReference()
        {
            this.DataCache.UpToStrongReference();
        }

        /// <summary>
        /// 把内部的缓冲区升级为强引用类型的；
        /// 当前高速循环excel获取数据的时候，可以允许临时升级缓冲数据位强引用以提高效率
        /// 注意：必须在使用GetCellDataOfCurrentSheet()方法前调用此方法才有效果的
        /// </summary>
        public void DownDataInnerCacheToWeakReference()
        {
            this.DataCache.DownToWeakReference();
        }

        /// <summary>
        /// 把内存中的数据写入到Excel的IO文件中
        /// </summary>
        public void Flush()
        {
            this.innerExcelApp.SaveWorkspace(missingValue);
        }

        /// <summary>
        /// 设置当前操作的工作表
        /// </summary>
        /// <param name="sheetName"></param>
        public void SetCurrentWorksheet(String sheetName)
        {
            if (String.IsNullOrEmpty(sheetName)) return;
            this.currentWorksheet = (Excel.Worksheet)this.currentWorkbook.Worksheets[sheetName];
        }

        /// <summary>
        /// 设置当前操作的工作表
        /// 序号是从1开始的
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrentWorksheet(Int32 index)
        {
            if (index > this.WorkSheetsCount || index <= 0)
                throw new ArgumentException("设置excel工作表的序号不正确");
            this.currentWorksheet = (Excel.Worksheet)this.currentWorkbook.Worksheets[index];
        }

        /// <summary>
        /// 获取所有单元格的别名，并返回别名列表
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllCellNames()
        {
            Excel.Names names = this.innerExcelApp.ActiveWorkbook.Names;
            List<String> nameList = new List<String>();
            if (names != null)
            {
                for (Int32 i = 1; i <= names.Count; i++)
                    nameList.Add(names.Item(i).Name);
            }
            return nameList;
        }

        /// <summary>
        /// 根据单元格的名称获取单元格的地址，例如：Sheet1!$BA$12
        /// (Excel中，每一个单元格都可以指定一个别名的，更改原来的默认名称）
        /// </summary>
        /// <param name="cellName">单元格的别名（必须是别名）</param>
        /// <returns>单元格对应的地址；格式为：Sheet1!$BA$12</returns>
        public String GetCellAddressByName(String cellNanme)
        {
            if (!String.IsNullOrEmpty(cellNanme))
            {
                Excel.Names names = this.innerExcelApp.ActiveWorkbook.Names;
                if (names != null)
                {
                    for (Int32 i = 1; i <= names.Count; i++)
                    {
                        if (names.Item(i).Name == cellNanme)
                            return MyString.Right(names.Item(i).RefersTo.ToString(), "!");
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 把标准单元格名称转换成序号，返回数组【x,y】
        /// </summary>
        /// <param name="cellName">标准单元格名称，例如：A1，A2，AA1</param>
        /// <returns></returns>
        public Int32[] ConvertCellNameToIndex(String cellName)
        {
            if (String.IsNullOrEmpty(cellName)) return null;
            cellName = cellName.ToUpper();
            Int32 columnCharIndex = 0;
            for (; columnCharIndex < cellName.Length; columnCharIndex++)
            {
                //A-Z:65-90;a-z:97-122
                Int32 code = MyString.ToAsciiCode(cellName[columnCharIndex]);
                if (code >= 65 && code <= 90) { }
                else break;
            }
            String row = cellName.Substring(columnCharIndex);
            String column = cellName.Substring(0, columnCharIndex);
            return new Int32[] { Int32.Parse(row), (Int32)ParseColumnCharToIndex(column) };
        }

        /// <summary>
        /// 将数字转换为Excel工作表中的表示列的字母
        /// </summary>
        /// <param name="cellIndex">单元格的序号数组【x,y】</param>
        /// <returns></returns>
        public String ConvertCellIndexToName(Int32[] cellIndex)
        {
            String sTempString = ParseIndexToClumnChar(cellIndex[0]);
            return sTempString + cellIndex[1].ToString();
        }

        /// <summary>
        /// 把单元格的地址(例如：$C$8）转换成序号，返回数组【x,y】
        /// </summary>
        /// <param name="cellAddressName">单元格的地址值(例如：$C$8）</param>
        /// <returns></returns>
        public Int32[] ConvertCellAddressToIndex(String cellAddressName)
        {
            if (String.IsNullOrEmpty(cellAddressName)) return null;
            String row = MyString.RightOfLast(cellAddressName, "$");
            String column = MyString.LeftOfLast(cellAddressName, "$" + row).Substring(1);
            return new Int32[] { Int32.Parse(row), (Int32)ParseColumnCharToIndex(column) };
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="x">X单元格</param>
        /// <param name="y">Y单元格</param>
        /// <param name="value">设置到单元格中的值</param>
        public void SetCellValue(Int32 x, Int32 y, String value)
        {
            Excel.Range range = GetCell(x, y);
            if (range != null)
            {
                range.Value = value;
            }
        }

        /// <summary>
        /// 替换文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public void ReplaceText(String text, String value)
        {
            Excel.Range useRange = this.currentWorksheet.UsedRange;
            Int32 columnCount = useRange.Columns.Count;
            Int32 rowCount = useRange.Rows.Count;
            for (var i = 1; i <= rowCount; i++) {
                for (var j = 1; j <= columnCount; j++) {
                    String itemValue = MyObject.ToStringEx(GetCellValueEx(i, j), "");
                    if (String.Compare(itemValue, text, true) == 0)
                    {
                        SetCellValue(i, j, value);
                    }
                }
            }
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="cellName">单元格别名名称</param>
        /// <param name="value">设置到单元格中的值</param>
        public void SetCellValue(String cellName, String value)
        {
            Int32[] xy = ConvertCellAddressToIndex(GetCellAddressByName(cellName));
            if (xy == null) return;
            SetCellValue(xy[0], xy[1], value);
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="cellName">单元格名称（单元格必须是标准的名字，不能是别名）例如：A1；AA1；AB2</param>
        /// <param name="value">设置到单元格中的值</param>
        public void SetCellValueEx(String cellName, String value)
        {
            Int32[] xy = ConvertCellNameToIndex(cellName);
            if (xy == null) return;
            SetCellValue(xy[0], xy[1], value);
        }

        /// <summary>
        /// 向指定的单元格插入图片
        /// </summary>
        /// <param name="cellName">单元格别名名称</param>
        /// <param name="imagePath">图片的路径</param>
        /// <param name="imageWidth">图片的宽度（像素为单位）</param>
        /// <param name="imageHeight">图片的高度（像素为单位）</param>
        public void InsertImage(String cellName, String imagePath, Single imageWidth, Single imageHeight)
        {
            Int32[] xy = ConvertCellAddressToIndex(GetCellAddressByName(cellName));
            if (xy == null) return;
            InsertImage(xy[0], xy[1], imagePath, imageWidth, imageHeight);
        }

        /// <summary>
        /// 向指定的单元格插入图片
        /// </summary>
        /// <param name="cellName">单元格名称（单元格必须是标准的名字，不能是别名）</param>
        /// <param name="imagePath">图片的路径</param>
        /// <param name="imageWidth">图片的宽度（像素为单位）</param>
        /// <param name="imageHeight">图片的高度（像素为单位）</param>
        public void InsertImageEx(String cellName, String imagePath, Single imageWidth, Single imageHeight)
        {
            Int32[] xy = ConvertCellNameToIndex(cellName);
            if (xy == null) return;
            InsertImage(xy[0], xy[1], imagePath, imageWidth, imageHeight);
        }

        /// <summary>
        /// 复制excel文档
        /// </summary>
        public void CopyExcel()
        {
            this.currentWorksheet.UsedRange.Copy();
        }

        /// <summary>
        /// 插入图片到指定单元格
        /// </summary>
        /// <param name="x">X单元格</param>
        /// <param name="y">Y单元格</param>
        /// <param name="imagePath">图片的路径</param>
        /// <param name="imageWidth">图片的宽度（像素为单位）</param>
        /// <param name="imageHeight">图片的高度（像素为单位）</param>
        public void InsertImage(Int32 x, Int32 y, String imagePath, Single imageWidth, Single imageHeight)
        {
            Excel.Range range = GetCell(x, y);
            if (range != null)
            {
                Single picLeft = Convert.ToSingle(range.Left);
                Single picTop = Convert.ToSingle(range.Top);
                Int32[] widthHeight = MyImage.GetImageWidthHeight(imagePath);
                if (imageWidth == -1) imageWidth = widthHeight[0];
                if (imageHeight == -1) imageHeight = widthHeight[1];
                //行高: 1毫米＝2.7682个单位 1厘米＝27.682个单位 1个单位＝0.3612毫米 
                //列宽： 1毫米＝0.4374个单位 1厘米＝4.374 个单位 1个单位＝2.2862毫米
                //1英寸大约是2.54厘米 
                //1英寸大约是96像素 
                //1毫米大约是3.77像素
                range.RowHeight = imageHeight / 3.77 * 2.7682 + 4 * 2.7682;  //增加边界
                range.ColumnWidth = imageWidth / 3.77 * 0.4374 + 9 * 0.4374; //增加边界
                this.currentWorksheet.Shapes.AddPicture(imagePath,
                                Microsoft.Office.Core.MsoTriState.msoFalse,
                                Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, imageWidth, imageHeight);
            }
        }

        /// <summary>
        /// 获取指定的单元格
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Excel.Range GetCell(Int32 x, Int32 y)
        {
            if (x <= 0)
                throw new ArgumentException("传入的行号X小于等于0；像Excel COM这种序号都是从1开始的");
            if (y <= 0)
                throw new ArgumentException("传入的行号Y小于等于0；像Excel COM这种序号都是从1开始的");

            Excel.Range range = this.currentWorksheet.Cells[x, y] as Excel.Range;
            return range;
        }

        /// <summary>
        /// 序号从1开始算起的（获取当前工作表的单元格数据）
        /// 此方法不会对Excel的数据进行缓存，每读取一次就会进行一次COM的互操作进行读Excel，
        /// 此方法效率慢，但是在一边修改一边读取Excel的情况，就必须使用此方法。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Object GetCellValueEx(Int32 x, Int32 y)
        {
            if (x <= 0 || x > this.DataRowCount)
                throw new ArgumentException("提供Excel的x坐标值不合法");
            if (y <= 0 || y > this.DataColumnCount)
                throw new ArgumentException("提供Excel的y坐标值不合法");

            return ((Excel.Range)this.currentWorksheet.Cells[x, y]).Value;
        }

        /// <summary>
        /// 根据单元格名称，读取单元格的值
        /// </summary>
        /// <param name="cellName">单元格名称（单元格必须是标准的名字，不能是别名）；例如：A1；AA1；AB2</param>
        /// <returns></returns>
        public Object GetCellValueEx(String cellName)
        {
            Int32[] xy = ConvertCellNameToIndex(cellName);
            return ((Excel.Range)this.currentWorksheet.Cells[xy[0], xy[1]]).Value;
        }

        /// <summary>
        /// 根据单元格名称，读取单元格的值
        /// </summary>
        /// <param name="cellName">单元格别名名称</param>
        /// <returns></returns>
        public Object GetCellValue(String cellName)
        {
            Int32[] xy = ConvertCellAddressToIndex(GetCellAddressByName(cellName));
            return ((Excel.Range)this.currentWorksheet.Cells[xy[0], xy[1]]).Value;
        }

        /// <summary>
        /// 序号从1开始算起的（获取当前工作表的单元格数据）
        /// 注意：此方法会在第一次调用的时候进行缓存整个Excel表格的数据，
        /// 后续访问将会非常的快，在打开Excel仅仅为了读取数据的情况下，使用此方法效率非常高；
        /// 但是，如果打开Excel的时候，是有修改也有读取的时候，就不可以使用此方法了，因为此方法
        /// 在第一次缓存了整个Excel的数据，而后续的读取数据的操作是直接从缓存中读取的，所以可能
        /// 会出现读取到脏数据的情况。
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">竖坐标</param>
        /// <returns></returns>
        public Object GetCellValue(Int32 x, Int32 y)
        {
            if (x <= 0 || x > this.DataRowCount)
                throw new ArgumentException("提供Excel的x坐标值不合法");
            if (y <= 0 || y > this.DataColumnCount)
                throw new ArgumentException("提供Excel的y坐标值不合法");

            DataTable table = null;
            if (!DataCache.TryGetValue(this.currentWorksheet.Name, out table))
            {
                table = new DataTable();
                //"[Sheet1$A:Z]" 读取A-Z之间的数据
                String sqlText = String.Format("SELECT * FROM [{0}$]", this.currentWorksheet.Name);
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(sqlText, this.ConnectionString))
                {
                    DataSet data = new DataSet();
                    adapter.Fill(data);
                    table = data.Tables[0];
                }
                if (!HasTitleRow)
                {
                    //上面的方法获取所有数据时会忽略Excel的第一行数据的，所以要补充回来
                    DataRow row = table.NewRow();
                    for (Int32 i = 1; i <= this.DataColumnCount; i++)
                    {
                        Object excelCellValue = ((Excel.Range)this.currentWorksheet.Cells[1, i]).Value;
                        if (excelCellValue == null) continue;
                        row[i - 1] = excelCellValue;
                    }
                    table.Rows.InsertAt(row, 0);
                }
                DataCache.TryAdd(this.currentWorksheet.Name, table);
            }
            return table.Rows[x - 1][y - 1];
        }



        #endregion

        #region "实现接口方法"

        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MyExcel()
        {
            this.Dispose(true);
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 当释放控件时，释放此Excel进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void parentControl_Disposed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 隐藏所有工具栏
        /// </summary>
        private void HideCommandBars()
        {
            Int32 length = this.innerExcelApp.ActiveWindow.Application.CommandBars.Count;
            for (Int32 i = 1; i <= length; i++)
            {
                try
                {
                    this.innerExcelApp.ActiveWindow.Application.CommandBars[i].Visible = false;
                }
                catch (COMException)
                { }
            }
        }

        /// <summary>
        /// 隐藏公式菜单栏
        /// </summary>
        private void HideFormulaBar()
        {
            this.innerExcelApp.ActiveWindow.Application.DisplayFormulaBar = false;
            this.innerExcelApp.ActiveWindow.Application.ShowMenuFloaties = false;
            this.innerExcelApp.ActiveWindow.DisplayHeadings = false;
            this.innerExcelApp.ActiveWindow.DisplayZeros = false;

            //this.innerExcelApp.ActiveWindow.EnableResize = false;
            this.innerExcelApp.ActiveWindow.FreezePanes = false;
            this.innerExcelApp.ActiveWindow.Application.DisplayCommentIndicator =
                Excel.XlCommentDisplayMode.xlNoIndicator;
            this.innerExcelApp.ActiveWindow.Application.DisplayFunctionToolTips = false;
            this.innerExcelApp.ActiveWindow.Application.DisplayStatusBar = false;
            this.innerExcelApp.ActiveWindow.Application.ShowDevTools = false;
            this.innerExcelApp.ActiveWindow.Application.ShowMenuFloaties = false;
            this.innerExcelApp.ActiveWindow.Application.ShowToolTips = false;
            this.innerExcelApp.ActiveWindow.Application.ShowStartupDialog = false;
            this.innerExcelApp.ActiveWindow.Application.ShowMenuFloaties = false;

        }

        private void Dispose(Boolean callFromFinalize)
        {
            if (hasDisposed) return;
            hasDisposed = true;
            if (!callFromFinalize)
            {
                //理论上此处应该释放托管资源的
            }
            try
            {
                //此处释放非托管资源
                this.currentWorkbook.Save();
                this.currentWorkbook.Close();
                this.innerExcelApp.Workbooks.Close();
                this.innerExcelApp.Quit();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(this.currentWorksheet);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(this.innerExcelApp);
                //强制杀掉excel进程
                if (this.innerExcelApp != null)
                {
                    IntPtr excelPid = user32.GetWindowThreadProcessId(this.innerExcelApp.Hwnd);
                    System.Diagnostics.Process p = null;
                    if (MyProcess.TryGetProcessById(excelPid.ToInt32(), out p))
                        p.Kill();
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 加载Excel工作本[也就是加载第一个工作本
        /// </summary>
        /// <param name="isCreate">表示是否是新建Excel</param>
        /// <param name="defaultSheetName">默认加载Excel的哪一个工作表，如果不传入此参数，则默认加载第一个工作表</param>
        private void LoadDefaultExcelWorkSheets(Boolean isCreate, String defaultSheetName)
        {
            if (this.innerExcelApp == null)
                this.innerExcelApp = new Excel.Application();
            if (!String.IsNullOrEmpty(this.filePath))
            {
                if (isCreate)
                {
                    this.innerExcelApp.Workbooks.Add(missingValue);
                }
                else
                {
                    this.innerExcelApp.Workbooks.Open(this.filePath,
                                        missingValue, missingValue, missingValue, missingValue,
                                        missingValue, missingValue, missingValue, missingValue,
                                        missingValue, missingValue, missingValue, missingValue,
                                        missingValue, missingValue);
                }
            }

            //设置当前操作的工作本
            this.currentWorkbook = this.innerExcelApp.Workbooks[1];
            //设置当前的工作表
            if (String.IsNullOrEmpty(defaultSheetName))
                SetCurrentWorksheet(1);
            else SetCurrentWorksheet(defaultSheetName);
        }

        /// <summary>
        /// 把Excel列的字母转换成序号，例如：A=1；AA=27；AB=28
        /// </summary>
        /// <param name="charStr"></param>
        /// <returns></returns>
        private static Int32 ParseColumnCharToIndex(String charStr)
        {
            //长度代表平方数
            Int32 length = charStr.Length;
            Double columnIndex = 0;
            for (Int32 i = 0; i < length; i++)
                columnIndex += WordNumberMapper[charStr[i].ToString()] * Math.Pow(26, length - i - 1);
            return (Int32)columnIndex;
        }

        /// <summary>
        /// 把Excel列序号转换成字母，例如：A=1；AA=27；AB=28
        /// </summary>
        /// <param name="charStr"></param>
        /// <returns></returns>
        private static String ParseIndexToClumnChar(Int32 index)
        {
            List<Int32> oList = new List<Int32>();
            if (index <= 26)
            {
                oList.Add(index);
            }
            else
            {
                Int32 iRemainder = 0;
                do
                {
                    iRemainder = index % 26;
                    //余数
                    oList.Add(iRemainder);
                    index = (Int32)Math.Floor((Double)(index / 26));
                } while (!(index == 0));
                //直到商为0的时候才停止循环
            }
            String sTempString = "";
            for (Int32 j = oList.Count - 1; j >= 0; j += -1)
                sTempString += Strings.Chr(Strings.Asc("A") + oList[j] - 1);
            return sTempString;
        }

        #endregion

        #region "枚举值"

        public enum InsertValueType
        {
            Image, //图片
            Normal //普通字符串格式
        }

        #endregion

        #region "静态方法"

        /// <summary>
        /// 尝试加载excel表格数据到datatable中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="table"></param>
        /// <param name="sheetName">excel表名</param>
        /// <param name="handleExceptionInTry">如何处理异常</param>
        /// <returns></returns>
        public static Boolean TryGetSheetData(String filePath, out DataTable table, String sheetName = "", HandleExceptionInTry handleExceptionInTry = HandleExceptionInTry.ReturnAndMakeLog)
        {
            table = null;
            try
            {
                table = GetSheetData(filePath, sheetName);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExceptionInTry)
                {
                    case HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex.Message);
                        return false;
                    default:
                    case HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 把excel表格数据一次性加载到DataTable中，当处理大量数据的时候，可以大大的提高处理效率
        /// </summary>
        /// <param name="filePath">excel文件路径</param>
        /// <param name="sheetName">excel工作表明，默认为：Sheet1</param>
        /// <returns></returns>
        public static DataTable GetSheetData(String filePath, String sheetName = "")
        {
            String connectionString = "";
            if (filePath.EndsWith(".xls")) connectionString = Properties.Resources.Excel2003ConnectionString;
            else connectionString = Properties.Resources.Excel2007ConnectionString;

            //sql语句
            String sql = "SELECT * FROM {0}";
            connectionString = String.Format(connectionString, filePath);
            if (String.IsNullOrEmpty(sheetName))
                sheetName = "[Sheet1$]";
            else
                sheetName = "[" + sheetName + "$]";
            sql = String.Format(sql, sheetName);
            System.Data.DataSet oDataSet = new System.Data.DataSet();
            System.Data.OleDb.OleDbDataAdapter oOleDataAdapter = new System.Data.OleDb.OleDbDataAdapter(sql, connectionString);
            oOleDataAdapter.Fill(oDataSet);
            return oDataSet.Tables[0];
        }

        /// <summary>
        /// 把datatable的数据导出到excel文档中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="excelFilePath">excel文件路径，如果指定的excel文件存在，则会替换</param>
        public static void ExportDataTableToExcel(DataTable table, String excelFilePath)
        {
            if (table == null) return;
            // 表开始
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
            sb.AppendLine(" <Worksheet ss:Name=\"Sheet1\">");
            sb.AppendLine("  <Table ss:DefaultColumnWidth=\"90\">");

            // 输出标题
            sb.AppendLine("   <Row>");
            for (int i = 0; i <= table.Columns.Count - 1; i++)
                sb.AppendLine("    <Cell><Data ss:Type=\"String\"><![CDATA[" + table.Columns[i].ColumnName + "]]></Data></Cell>");
            sb.AppendLine("   </Row>");

            //输出所有列数据
            for (int i = 0; i <= table.Rows.Count - 1; i++)
            {
                sb.AppendLine("   <Row>");
                for (int j = 0; j <= table.Columns.Count - 1; j++)
                {
                    sb.AppendLine("    <Cell><Data ss:Type=\"String\"><![CDATA[" + table.Rows[i][j] + "]]></Data></Cell>");
                }
                sb.AppendLine("   </Row>");
            }

            // 表结束
            sb.AppendLine("  </Table>");
            sb.AppendLine(" </Worksheet>");
            sb.AppendLine("</Workbook>");
            MyIO.Write(excelFilePath, sb.ToString(), false, null);
        }

        /// <summary>
        /// 判断指定的Excel文件是否被打开了
        /// </summary>
        /// <param name="excelFilePath">excel文件路径</param>
        /// <returns></returns>
        public static Boolean IsOpen(String excelFilePath)
        {
            return MyIO.IsFileUsing(excelFilePath);
        }

        /// <summary>
        /// 杀掉所有的excel进程
        /// </summary>
        public static Boolean TryKillAllExcelApp(HandleExceptionInTry handleExceptionInTry = HandleExceptionInTry.ReturnAndIgnoreLog)
        {
            return MyProcess.TryKillProcessByName("excel", null, handleExceptionInTry);
        }

        #endregion
    }
}
