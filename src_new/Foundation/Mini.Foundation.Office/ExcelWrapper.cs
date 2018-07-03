using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
#if !NET20
using System.Linq;
#endif

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Office
{
    /// <summary>
    /// excel的封装类
    /// </summary>
    public partial class ExcelWrapper : IDisposable
    {
        #region "fields"

        protected ICellStyle _defaultHeaderCellStyle = null;
        /// <summary>
        /// 缓存单元格最大的宽度（用于在构造execl的过程中，根据列内容的大小，调整宽度）
        /// key： columnindex
        /// value：max dataLength
        /// </summary>
        protected Dictionary<Int32, Int32> _cellMaxDataLength = new Dictionary<Int32, Int32>();

        /// <summary>  
        /// 使用NOPI读取Excel数据  
        /// </summary>  
        protected IWorkbook _workbook;
        /// <summary>
        /// 是否已经初始化了
        /// </summary>
        protected Boolean _hasInitial = false;
        protected List<String> _sheetNames = null;
        protected List<String> _noHidenSheetNames = null;

        /// <summary>
        /// 文件路径
        /// </summary>
        protected String _fileFullPath = String.Empty;

        #endregion

        #region "property"

        /// <summary>
        /// 创建表头的样式
        /// </summary>
        /// <returns></returns>
        public virtual ICellStyle DefaultHeaderCellStyle
        {
            get
            {
                if (_defaultHeaderCellStyle == null)
                {
                    //_headerCellStyle = (HSSFCellStyle)_workbook.CreateCellStyle();
                    //_headerCellStyle.Alignment = LY.TEC.Excel.SS.UserModel.HorizontalAlignment.CENTER;
                    //_headerCellStyle.FillForegroundColor = 23;
                    //_headerCellStyle.BorderBottom = LY.TEC.Excel.SS.UserModel.BorderStyle.THIN;
                    //_headerCellStyle.BottomBorderColor = 23;
                    //_headerCellStyle.BorderLeft = LY.TEC.Excel.SS.UserModel.BorderStyle.THIN;
                    //_headerCellStyle.LeftBorderColor = 55;
                    //_headerCellStyle.BorderRight = LY.TEC.Excel.SS.UserModel.BorderStyle.THIN;
                    //_headerCellStyle.RightBorderColor = 55;
                    //_headerCellStyle.BorderTop = LY.TEC.Excel.SS.UserModel.BorderStyle.THIN;
                    //_headerCellStyle.TopBorderColor = 22;
                    //_headerCellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                    //IFont font = _workbook.CreateFont();
                    //font.FontHeightInPoints = 10;
                    //font.Boldweight = 700;
                    //font.Color = 9;
                    //_headerCellStyle.SetFont(font);
                    _defaultHeaderCellStyle = _workbook.CreateCellStyle();
                    _defaultHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    _defaultHeaderCellStyle.FillForegroundColor = 23;
                    _defaultHeaderCellStyle.BorderBottom = BorderStyle.Thin;
                    _defaultHeaderCellStyle.BottomBorderColor = 23;
                    _defaultHeaderCellStyle.BorderLeft = BorderStyle.Thin;
                    _defaultHeaderCellStyle.LeftBorderColor = 55;
                    _defaultHeaderCellStyle.BorderRight = BorderStyle.Thin;
                    _defaultHeaderCellStyle.RightBorderColor = 55;
                    _defaultHeaderCellStyle.BorderTop = BorderStyle.Thin;
                    _defaultHeaderCellStyle.TopBorderColor = 22;
                    _defaultHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                    IFont font = _workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    font.Color = 9;
                    _defaultHeaderCellStyle.SetFont(font);

                }
                return _defaultHeaderCellStyle;
            }
            set
            {
                _defaultHeaderCellStyle = value;
            }
        }

        /// <summary>
        /// 获取excel的所有sheet名称
        /// </summary>
        public virtual List<String> SheetNames
        {
            get
            {
                //if (_sheetNames != null) return _sheetNames;
                if (_workbook == null) return null;
                var count = _workbook.NumberOfSheets; //获取所有SheetName
                var sheetNameList = new List<String>();
                for (var i = 0; i < count; i++)
                    sheetNameList.Add(_workbook.GetSheetAt(i).SheetName);
                _sheetNames = sheetNameList;
                return _sheetNames;
            }
        }

        /// <summary>
        /// 获取excel的所有sheet名称（不包括隐藏的）
        /// </summary>
        public virtual List<String> NoHiddenSheetNames
        {
            get
            {
                if (_noHidenSheetNames != null) return _noHidenSheetNames;
                if (_workbook == null) return null;
                var count = _workbook.NumberOfSheets; //获取所有SheetName
                var sheetNameList = new List<String>();
                for (var i = 0; i < count; i++)
                {
                    if (!_workbook.IsSheetHidden(i))
                    {
                        var sheet = _workbook.GetSheetAt(i);
                        sheetNameList.Add(sheet.SheetName);
                    }
                }
                _noHidenSheetNames = sheetNameList;
                return _noHidenSheetNames;
            }
        }

        #endregion

        #region "creator"

        /// <summary>
        /// datatable数据加载到excel文件中
        /// </summary>
        public ExcelWrapper() : this("", true)
        { }

        /// <summary>
        /// </summary>
        /// <param name="fileFullPath">excel文件路径</param>
        /// <param name="createIfNotExist">指示如果指定路径文件不存在，是否创建新的文件</param>
        /// <exception cref="ArgumentNullException">excelFilePath参数为null并且createIfNotExist为false</exception>
        /// <exception cref="FileNotFoundException">excelFilePath参数不为null并且createIfNotExist为false</exception>
        public ExcelWrapper(String fileFullPath, Boolean createIfNotExist = false)
        {
            //this.Init(fileFullPath, createIfNotExist);
            this.Init2(fileFullPath, createIfNotExist);
        }

        #endregion

        #region "public method"

        /// <summary>
        /// 将excel中的所有的表格数据导入到DataTable中
        /// </summary>
        /// <param name="sheetNames">如果不传入该参数，则默认读取所有的 sheet 数据到DataSet</param>
        /// <param name="isFirstRowColumnName">第一行是否是DataTable的列名</param>  
        /// <returns></returns>
        public virtual DataSet GetDataTables(String[] sheetNames = null, Boolean isFirstRowColumnName = false)
        {
            sheetNames = sheetNames ?? SheetNames.ToArray();
            var ds = new DataSet();
            foreach (var sheetName in sheetNames)
            {
                ds.Tables.Add(GetDataTable(sheetName, isFirstRowColumnName));
            }
            return ds;
        }

        /// <summary>  
        /// 将excel中的数据导入到DataTable中  
        /// </summary>  
        /// <param name="sheetName">excel工作薄sheet的名称</param>  
        /// <param name="isFirstRowColumnName">第一行是否是DataTable的列名</param>  
        /// <param name="containHiddenSheet">是否包含隐藏表单</param>  
        /// <returns>返回的DataTable</returns>  
        public virtual DataTable GetDataTable(String sheetName = "Sheet1", Boolean isFirstRowColumnName = false, Boolean containHiddenSheet = false)
        {
            MyArgumentsHelper.ThrowsIfIsInvisibleString(sheetName, nameof(sheetName));
            ISheet sheet = null;
            var data = new DataTable();
            data.TableName = sheetName;
            int startRow = 0;

            sheet = _workbook.GetSheet(sheetName);
            if (sheet == null || !ContainsSheet(sheetName)) throw new Exception(String.Format("excel file does not has the sheet named [{0}]!", sheetName));

            var firstRow = sheet.GetRow(0);
            if (firstRow == null) return data;
            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数  
            startRow = isFirstRowColumnName ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;

            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
            {
                var column = new DataColumn(Convert.ToChar('A' + i).ToString());
                if (isFirstRowColumnName)
                {
                    var columnName = firstRow.GetCell(i).StringCellValue;
                    column = new DataColumn(columnName);
                }
                data.Columns.Add(column);
            }

            //最后一列的标号  
            int rowCount = sheet.LastRowNum;
            for (int i = startRow; i <= rowCount; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue; //没有数据的行默认是null　　　　　　　  

                DataRow dataRow = data.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; ++j)
                {
                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null  
                        dataRow[j] = row.GetCell(j, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                }
                data.Rows.Add(dataRow);
            }
            return data;
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        public virtual String Save()
        {
            return SaveToFile(this._fileFullPath);
        }

        /// <summary>
        /// 保存workbook到磁盘上（另存为指定的其他目录）
        /// </summary>
        /// <param name="fileFullPath"></param>
        public virtual String SaveToFile(String fileFullPath)
        {
            //生成文件
            using (MemoryStream ms = new MemoryStream())
            {
                _workbook.Write(ms);
                ms.Flush();
                ms.Position = 0L;
                using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
                {
                    //Byte[] buffer = new Byte[1024];
                    //Int32 offset = 0;
                    //Int32 read = 0;
                    //while ((read = ms.Read(buffer, offset, buffer.Length)) > 0)
                    //{
                    //    fs.Write(buffer, offset, read);
                    //    offset += read;
                    //}
                    //fs.Flush();
                    Byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
            return fileFullPath;
        }

        /// <summary>
        /// 添加一个sheet
        /// </summary>
        /// <param name="table"></param>
        /// <param name="sheetName"></param>
        /// <param name="ignoreColumns"></param>
        public virtual void AddSheet(DataTable table, String sheetName = "Sheet2", String[] ignoreColumns = null)
        {
            MyArgumentsHelper.ThrowsIfNull(table, nameof(table));
            MyArgumentsHelper.ThrowsIfIsInvisibleString(sheetName, nameof(sheetName));
            ISheet sheet = null;
            sheet = _workbook.GetSheet(sheetName);
            if (sheet != null) throw new Exception(String.Format("The sheet name[{0}] has already existed!", sheetName));
            sheet = _workbook.CreateSheet(sheetName);
            //渲染列头
            IRow row = sheet.CreateRow(0);//添加第一行（列头）
            //设置表头样式
            ICellStyle headCellStyle = DefaultHeaderCellStyle;
            Int32 excelColumnIndex = 0;//指示excel的列序号
            for (var i = 0; i < table.Columns.Count; i++)
            {
                String columnName = table.Columns[i].ColumnName;
                if (IsColumnIgnore(columnName, ignoreColumns)) continue;
                ICell cell = row.CreateCell(i);
                cell.CellStyle = headCellStyle;
                cell.SetCellValue(columnName);
                //根据列名的长度，调整execl单元格列宽
                Int32 datalength = 0;
                if (UpdateSetMaxColumnWidth(excelColumnIndex, columnName, out datalength))
                    sheet.SetColumnWidth(excelColumnIndex, GetColumnWidth(datalength));
                excelColumnIndex++;
            }
            excelColumnIndex = 0;
            //渲染数据
            for (var r = 0; r < table.Rows.Count; r++)
            {
                IRow dataRow = sheet.CreateRow(r + 1);
                for (var c = 0; c < table.Columns.Count; c++)
                {
                    String columnName = table.Columns[c].ColumnName;
                    if (IsColumnIgnore(columnName, ignoreColumns)) continue;
                    ICell dataCell = dataRow.CreateCell(c);
                    String cellData = Convert.ToString(table.Rows[r][c]);
                    dataCell.SetCellValue(cellData);
                    //判断列宽的问题（自动根据内容的长度，调整excel单元格列宽）
                    Int32 datalength = 0;
                    if (UpdateSetMaxColumnWidth(excelColumnIndex, cellData, out datalength))
                        sheet.SetColumnWidth(excelColumnIndex, GetColumnWidth(datalength));
                    excelColumnIndex++;
                }
            }
        }

        /// <summary>
        /// 图片在单元格等比缩放居中显示
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="image">图片二进制流</param>
        public virtual void AddCellImage(ICell cell, Byte[] image)
        {
#if !(NETSTANDARD1_3 || NETSTANDARD2_0)
            if (image.Length == 0) return;//空图片处理
            double scalx = 0;//x轴缩放比例
            double scaly = 0;//y轴缩放比例
            int Dx1 = 0;//图片左边相对excel格的位置(x偏移) 范围值为:0~1023,超过1023就到右侧相邻的单元格里了
            int Dy1 = 0;//图片上方相对excel格的位置(y偏移) 范围值为:0~256,超过256就到下方的单元格里了
            bool bOriginalSize = false;//是否显示图片原始大小 true表示图片显示原始大小  false表示显示图片缩放后的大小
            //计算单元格的长度和宽度
            double CellWidth = cell.Row.Sheet.GetColumnWidth(cell.ColumnIndex);
            double CellHeight = cell.Sheet.GetRow(cell.RowIndex).Height;
            //单元格长度和宽度与图片的长宽单位互换是根据实例得出
            CellWidth = CellWidth / 35;
            CellHeight = CellHeight / 15;
            //计算图片的长度和宽度
            MemoryStream ms = new MemoryStream(image);
            Image Img = Bitmap.FromStream(ms, true);
            double ImageOriginalWidth = Img.Width;//原始图片的长度
            double ImageOriginalHeight = Img.Height;//原始图片的宽度
            double ImageScalWidth = 0;//缩放后显示在单元格上的图片长度
            double ImageScalHeight = 0;//缩放后显示在单元格上的图片宽度
            if (CellWidth > ImageOriginalWidth && CellHeight > ImageOriginalHeight)//单元格的长度和宽度比图片的大，说明单元格能放下整张图片，不缩放
            {
                ImageScalWidth = ImageOriginalWidth;
                ImageScalHeight = ImageOriginalHeight;
                bOriginalSize = true;
            }
            else
            {
                //需要缩放，根据单元格和图片的长宽计算缩放比例
                bOriginalSize = false;
                if (ImageOriginalWidth > CellWidth && ImageOriginalHeight > CellHeight)//图片的长和宽都比单元格的大的情况
                {
                    double WidthSub = ImageOriginalWidth - CellWidth;//图片长与单元格长的差距
                    double HeightSub = ImageOriginalHeight - CellHeight;//图片宽与单元格宽的差距
                    if (WidthSub > HeightSub)//长的差距比宽的差距大时,长度x轴的缩放比为1，表示长度就用单元格的长度大小，宽度y轴的缩放比例需要根据x轴的比例来计算
                    {
                        scalx = 1;
                        scaly = (CellWidth / ImageOriginalWidth) * ImageOriginalHeight / CellHeight;//计算y轴的缩放比例,CellWidth / ImageWidth计算出图片整体的缩放比例,然后 * ImageHeight计算出单元格应该显示的图片高度,然后/ CellHeight就是高度的缩放比例
                    }
                    else
                    {
                        scaly = 1;
                        scalx = (CellHeight / ImageOriginalHeight) * ImageOriginalWidth / CellWidth;
                    }
                }
                else if (ImageOriginalWidth > CellWidth && ImageOriginalHeight < CellHeight)//图片长度大于单元格长度但图片高度小于单元格高度，此时长度不需要缩放，直接取单元格的，因此scalx=1，但图片高度需要等比缩放
                {
                    scalx = 1;
                    scaly = (CellWidth / ImageOriginalWidth) * ImageOriginalHeight / CellHeight;
                }
                else if (ImageOriginalWidth < CellWidth && ImageOriginalHeight > CellHeight)//图片长度小于单元格长度但图片高度大于单元格高度，此时单元格高度直接取单元格的，scaly = 1,长度需要等比缩放
                {
                    scaly = 1;
                    scalx = (CellHeight / ImageOriginalHeight) * ImageOriginalWidth / CellWidth;
                }
                ImageScalWidth = scalx * CellWidth;
                ImageScalHeight = scaly * CellHeight;
            }
            Dx1 = Convert.ToInt32((CellWidth - ImageScalWidth) / CellWidth * 1023 / 2);
            Dy1 = Convert.ToInt32((CellHeight - ImageScalHeight) / CellHeight * 256 / 2);
            int pictureIdx = cell.Sheet.Workbook.AddPicture((Byte[])image, PictureType.JPEG);
            IClientAnchor anchor = cell.Sheet.Workbook.GetCreationHelper().CreateClientAnchor();
            //anchor.AnchorType = AnchorType.MoveDontResize;
            anchor.Col1 = cell.ColumnIndex;
            anchor.Col2 = cell.ColumnIndex + 1;
            anchor.Row1 = cell.RowIndex;
            anchor.Row2 = cell.RowIndex + 1;
            anchor.Dy1 = Dy1;//图片下移量
            anchor.Dx1 = Dx1;//图片右移量，通过图片下移和右移，使得图片能居中显示，因为图片不同文字，图片是浮在单元格上的，文字是钳在单元格里的
            IDrawing patriarch = cell.Sheet.CreateDrawingPatriarch();
            IPicture pic = patriarch.CreatePicture(anchor, pictureIdx);
            if (bOriginalSize)
            {
                pic.Resize();//显示图片原始大小 
            }
            else
            {
                //pic.Resize(scalx, scaly);//等比缩放   
            }
#else
            throw new NotImplementedException();
#endif

        }

        #endregion

        #region "protected method"

        protected virtual Boolean ContainsSheet(String sheetName, Boolean containHiddenSheet = false)
        {
            var names = containHiddenSheet ? SheetNames : NoHiddenSheetNames;
            return names.Contains(sheetName);
        }

        protected virtual String EnsureFileExist()
        {
            //如果不存在 则从嵌入资源内读取 BlockSet.xml 
            Assembly asm = Assembly.GetExecutingAssembly();//读取嵌入式资源
            if (String.IsNullOrEmpty(_fileFullPath))
                _fileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{ Guid.NewGuid().ToString("N")}.xlsx");
            else if (File.Exists(_fileFullPath))
                return _fileFullPath;
            using (Stream sm = asm.GetManifestResourceStream("Mini.Foundation.Office.empty.xlsx"))
            {
                using (var fs = File.Create(_fileFullPath))
                {
                    var buffer = new Byte[1024 * 100];
                    var read = -1;
                    do
                    {
                        read = sm.Read(buffer, 0, buffer.Length);
                        if (read > 0) fs.Write(buffer, 0, read);
                    } while (read > 0);
                }
            }
            return _fileFullPath;
        }

        /// <summary>
        /// 根据文件初始化对象（如果指定的路径不存在，则会新增一个excel）
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="createIfNotExist"></param>
        /// <exception cref="ArgumentNullException">excelFilePath参数为null并且createIfNotExist为false</exception>
        /// <exception cref="FileNotFoundException">excelFilePath参数不为null并且createIfNotExist为false</exception>
        protected virtual void Init2(String excelFilePath, Boolean createIfNotExist = false)
        {
            if (File.Exists(excelFilePath))
            {
                var fs = new FileStream(_fileFullPath, FileMode.Open, FileAccess.ReadWrite);
                // 2007版本  
                if (_fileFullPath.IndexOf(".xlsx") > 0)
                    _workbook = new XSSFWorkbook(fs);
                // 2003版本  
                else if (_fileFullPath.IndexOf(".xls") > 0)
                    _workbook = new HSSFWorkbook(fs);
            }
            else
            {
                if (!createIfNotExist)
                    MyArgumentsHelper.ThrowsIfFileNotExist(_fileFullPath, nameof(excelFilePath));
                // 2007版本  
                if (_fileFullPath.IndexOf(".xlsx") > 0)
                    _workbook = new XSSFWorkbook();
                // 2003版本  
                else if (_fileFullPath.IndexOf(".xls") > 0)
                    _workbook = new HSSFWorkbook();
            }
        }

        /// <summary>
        /// 根据文件初始化对象（如果指定的路径不存在，则会新增一个excel）
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="createIfNotExist"></param>
        /// <exception cref="ArgumentNullException">excelFilePath参数为null并且createIfNotExist为false</exception>
        /// <exception cref="FileNotFoundException">excelFilePath参数不为null并且createIfNotExist为false</exception>
        protected virtual void Init(String excelFilePath, Boolean createIfNotExist = false)
        {
            _fileFullPath = excelFilePath;
            if (createIfNotExist) EnsureFileExist();
            MyArgumentsHelper.ThrowsIfFileNotExist(_fileFullPath, nameof(excelFilePath));
            //var file = new FileInfo(_fileFullPath);
            var fs = new FileStream(_fileFullPath, FileMode.Open, FileAccess.ReadWrite);
            // 2007版本  
            if (_fileFullPath.IndexOf(".xlsx") > 0)
                _workbook = new XSSFWorkbook(fs);
            // 2003版本  
            else if (_fileFullPath.IndexOf(".xls") > 0)
                _workbook = new HSSFWorkbook(fs);
        }

        /// <summary>
        /// 判断列是否被忽略掉
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="ignoreColumns"></param>
        /// <returns></returns>
        protected virtual Boolean IsColumnIgnore(String columnName, String[] ignoreColumns)
        {
            if (ignoreColumns == null) return false;
            var _ignoreColumnList = new List<string>(ignoreColumns);
            //if (_ignoreColumnListComparer == null)
            //{
            //    CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            //    _ignoreColumnListComparer = StringComparer.Create(culture, true);
            //}
#if NET20
            return _ignoreColumnList.Contains(columnName);
#else
            return _ignoreColumnList.Contains(columnName, StringComparer.CurrentCultureIgnoreCase);
#endif
        }

        /// <summary>
        /// 根据数据的长度，获取单元格的宽度
        /// </summary>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        protected virtual Int32 GetColumnWidth(Int32 dataLength)
        {
            return Math.Min((dataLength + 1) * 256, 30720);
        }

        /// <summary>
        /// 获取合适的列宽值
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="cellData"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        protected virtual Boolean UpdateSetMaxColumnWidth(Int32 columnIndex, String cellData, out Int32 dataLength)
        {
            dataLength = Encoding.GetEncoding(936).GetBytes(cellData).Length;
            if (_cellMaxDataLength.ContainsKey(columnIndex))
            {
                Int32 currentLength = _cellMaxDataLength[columnIndex];
                //不用更新最大值
                if (currentLength >= dataLength) return false;
            }
            _cellMaxDataLength[columnIndex] = dataLength;
            return true;
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_workbook != null)
                    _workbook.Close();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}