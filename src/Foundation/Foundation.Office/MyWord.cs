using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Objects;

namespace DreamCube.Foundation.Office
{
    /// <summary>
    /// 操作word文档
    /// </summary>
    public class MyWord : IDisposable
    {
        #region "静态字段"

        private static ThreadLocker locker = new ThreadLocker();
        private static Int32 lockTimeout = 1000; //进入写锁超时时间为1秒钟

        /// <summary>
        /// Com组件的缺省参数
        /// </summary>
        private static Object missingValue = System.Reflection.Missing.Value;

        #endregion

        #region "字段"

        /// <summary>
        /// 对wordapp的引用
        /// </summary>
        private Word.Application innerWordApp;

        /// <summary>
        /// 标志是否已经释放过内存了
        /// </summary>
        private Boolean hasDisposed = false;

        /// <summary>
        /// 对word的文档的引用
        /// </summary>
        private Word.Document innerDoc;

        /// <summary>
        /// 保存当前Word文件的路径
        /// </summary>
        private String filePath = String.Empty;

        /// <summary>
        /// 标志是否是创建新的文件；true为创建新的word文档，false为打开存在的word文档
        /// </summary>
        private Boolean isCreateNew = false;

        #endregion

        #region "属性"

        /// <summary>
        /// 获取总页数
        /// </summary>
        public Int32 PageCount
        {
            get
            {
                if (this.innerDoc == null) return 0;
                else
                {
                    return this.innerDoc.ComputeStatistics(Word.WdStatistic.wdStatisticPages, ref  missingValue);
                }
            }
        }

        /// <summary>
        /// 统计文档的字数
        /// </summary>
        /// <returns></returns>
        public Int32 CharactersCount
        {
            get
            {
                if (this.innerDoc == null) return 0;
                else
                {
                    return this.innerDoc.Characters.Count;//文档字数
                }
            }
        }

        /// <summary>
        /// 获取内部的Excel引用
        /// </summary>
        public Word.Application InnerWordApp
        {
            get { return this.innerWordApp; }
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 构造函数
        /// 如果根据提供的路径找不到word文件，则会在输入的路径位置创建新的word文件
        /// </summary>
        /// <param name="filePath">word文件路径</param>
        public MyWord(String filePath)
            : this(filePath, true)
        { }

        /// <summary>
        /// 构造函数
        /// 如果根据提供的路径找不到word文件，则会在输入的路径位置创建新的word文件
        /// </summary>
        /// <param name="filePath">word文件路径</param>
        /// <param name="showWin">控制打开word文档是否显示窗口</param>
        public MyWord(String filePath, Boolean showWin)
        {
            //如果传入的word文件不存在，则创建新的word文件
            if (!MyIO.IsFileExists(filePath))
            {
                ///如果传入的Excel文件不存在，则创建新的Excel文件
                if (!MyIO.IsFileExists(filePath))
                    isCreateNew = true;
            }
            else
            {
                //把文件的只读属性去掉
                MyIO.MakeFileCanWrite(filePath);
            }
            this.filePath = filePath;

            //初始化word文档
            this.Initial(showWin);
        }

        /// <summary>
        /// 复制所有文档内容到剪切板上
        /// </summary>
        public void CopyDoc()
        {
            this.innerDoc.Select();
            this.innerDoc.Range().Copy();
        }

        /// <summary>
        /// 替换文本
        /// </summary>
        /// <param name="oldText">需要被替换的文本</param>
        /// <param name="newText">新文本</param>
        public void ReplaceText(String oldText, String newText)
        {
            Word.WdFindWrap find = Word.WdFindWrap.wdFindContinue;
            Word.WdReplace replace = Word.WdReplace.wdReplaceAll;
            innerWordApp.Selection.Find.Execute(oldText,
                missingValue, missingValue, missingValue, missingValue,
                missingValue, true, find, true, newText, replace,
                missingValue, missingValue, missingValue, missingValue);
        }

        /// <summary>
        /// 在word文档中插入一个excel文档的内容
        /// </summary>
        /// <param name="excelPath"></param>
        public void AppendExcel(String excelPath)
        {
            MyExcel excel = new MyExcel(excelPath);
            excel.CopyExcel();
            try
            {
                Int32 tryTimes = 1;
                Int32 tryTime = 1;
                while (tryTime <= tryTimes)
                {
                    try
                    {
                        Object units = Word.WdUnits.wdStory;
                        this.innerWordApp.ActiveWindow.Selection.EndKey(ref units);
                        this.innerWordApp.ActiveWindow.Selection.PasteAndFormat(Word.WdRecoveryType.wdFormatOriginalFormatting);
                        break;
                    }
                    catch (Exception ex)
                    {
                        String msg = String.Format("把文档：{0}合并到文档：{1}出错，异常信息：{2}！", excelPath, this.filePath, ex.Message);
                        MyLog.MakeLog(msg, MyLog.LogType.Exception);
                        Thread.Sleep(500);
                        tryTime++;
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            finally
            {
                if (excel != null) excel.Dispose();
            }
        }

        /// <summary>
        /// 设置页面为纵向
        /// </summary>
        public void SetPageOrientPortrait()
        {
            this.innerWordApp.ActiveDocument.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;//页面方向
        }

        /// <summary>
        /// 设置页面为横向
        /// </summary>
        public void SetPageOrientLandscape()
        {
            this.innerWordApp.ActiveDocument.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;//页面方向
        }

        /// <summary>
        /// 插入一个新页面
        /// </summary>
        public void InsertNewPage()
        {
            Int32 tryTimes = 3;
            Int32 tryTime = 1;
            while (tryTime < tryTimes)
            {
                try
                {
                    Object oPageBreak = Word.WdBreakType.wdPageBreak;//分页符 
                    this.innerDoc.ActiveWindow.Selection.InsertBreak(oPageBreak);
                    this.innerDoc.ActiveWindow.Selection.MoveUp();
                    //方法2
                    //object count = 14; 
                    //object WdLine = Word.WdUnits.wdLine;//换一行; 
                    //this.innerWordApp.Selection.MoveDown(ref WdLine, ref count, ref missingValue);//移动焦点 
                    ////方法3
                    //this.innerWordApp.Selection.TypeParagraph();  
                    break;
                }
                catch (Exception ex)
                {
                    MyLog.MakeLog("在文档：{0}插入新页的时候出错，错误信息：" + ex.Message);
                    tryTime++;
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// 尝试移除所有空白页面
        /// </summary>
        public Boolean TryRemoveAllEmptyPage(HandleExceptionInTry handleExcp = HandleExceptionInTry.ReturnAndMakeLog)
        {
            try
            {
                RemoveAllEmptyPage();
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 清除所有的空白页面
        /// </summary>
        public void RemoveAllEmptyPage()
        {
            Int32 pageCount = this.PageCount;
            for (Int32 i = 1; i <= pageCount; )
            {
                String pageStr = GetPageString(i);
                if (String.IsNullOrEmpty(pageStr) || String.IsNullOrEmpty(pageStr.Replace("\r", "").Replace("\t", "").Replace("\f", "")))
                {
                    RemovePage(i);
                    //重新计算页码
                    pageCount = this.PageCount;
                    i = 1;
                }
                else i++;
            }
        }

        /// <summary>
        /// 获取指定页码的文本
        /// </summary>
        /// <returns></returns>
        public String GetPageString(Int32 pageIndex)
        {
            Int32 pageCount = this.PageCount;
            if (pageIndex < 1 || pageIndex > pageCount) return String.Empty;
            Word.Range page = null;
            if (pageIndex == pageCount)
            {
                page = this.innerDoc.Range(this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex, missingValue).Start, missingValue);
            }
            else
            {
                page = this.innerDoc.Range(this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex, missingValue).Start,
                        this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex + 1, missingValue).Start);
            }
            if (page != null) return page.Text;
            return String.Empty;
        }

        /// <summary>
        /// 移除指定页码
        /// </summary>
        /// <param name="pageIndex">序号从1开始算</param>
        public void RemovePage(Int32 pageIndex)
        {
            Int32 pageCount = this.PageCount;
            if (pageIndex < 1 || pageIndex > pageCount) return;
            Word.Range page = null;
            if (pageIndex == pageCount)
            {
                page = this.innerDoc.Range(this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex, missingValue).Start, missingValue);
            }
            else 
            {
                page = this.innerDoc.Range(this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex, missingValue).Start,
                        this.innerDoc.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pageIndex + 1, missingValue).Start);
            }
            if (page != null) page.Delete(ref missingValue, ref missingValue);
        }

        /// <summary>
        /// append数据到word文档中
        /// </summary>
        /// <param name="html"></param>
        public void AppendData(String html)
        {
            try
            {
                Boolean hasSetValue = false;
                Int32 iWaitSeconds = 15; //等待15秒
                Int32 iSeconds = 0;
                //用另外一个线程去复制值
                Thread thread = new Thread(new ParameterizedThreadStart((parameters) =>
                {
                    try
                    {
                        Object[] inputParameters = parameters as Object[];
                        //可能剪切板的数据为空的时候会导致异常
                        try
                        {
                            //Clipboard.Clear(); //移除剪切板的数据
                        }
                        catch (Exception)
                        { }
                        if (inputParameters != null)
                        {
                            String bookMarkValue = Convert.ToString(inputParameters[0]);
                            WebBrowser wb = new WebBrowser();
                            wb.DocumentText = bookMarkValue;
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);

                            //选中WebBrowser上内容，并复制到剪贴板   
                            wb.Document.ExecCommand("SelectAll", false, null);
                            wb.Document.ExecCommand("Copy", false, null);
                            //黏贴值
                            Word.Document tempDoc = inputParameters[1] as Word.Document;
                            tempDoc.ActiveWindow.Selection.Range.PasteAndFormat(Word.WdRecoveryType.wdFormatOriginalFormatting);
                            hasSetValue = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MyLog.MakeLog(ex);
                    }
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start(new Object[] { html, this.innerDoc });

                //设置等待循环
                while (iSeconds <= iWaitSeconds && !hasSetValue)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    iSeconds++;
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 在word文档中加入另一个word的内容
        /// </summary>
        /// <param name="wordPath"></param>
        public void AppendWord(String wordPath)
        {
            MyWord word = new MyWord(wordPath, true);
            word.CopyDoc();
            try
            {
                Int32 tryTimes = 1;
                Int32 tryTime = 1;
                while (tryTime <= tryTimes)
                {
                    try
                    {
                        Object units = Word.WdUnits.wdStory;
                        this.innerWordApp.ActiveWindow.Selection.EndKey(ref units);
                        this.innerWordApp.ActiveWindow.Selection.PasteAndFormat(Word.WdRecoveryType.wdFormatOriginalFormatting);
                        //顺利结束的话，就跳出循环，否则重新尝试执行一次
                        break;
                    }
                    catch (Exception ex)
                    {
                        String msg = String.Format("把文档：{0}合并到文档：{1}出错，异常信息：{2}！", wordPath, this.filePath, ex.Message);
                        MyLog.MakeLog(msg, MyLog.LogType.Exception);
                        Thread.Sleep(500);
                        tryTime++;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            finally
            {
                if (word != null) word.Dispose();
            }
        }

        /// <summary>
        /// 把word文档保存为pdf文件
        /// </summary>
        /// <param name="fileFullName">pdf文件的输出路径，如果不传入此参数，则默认输出到本目录</param>
        public void SaveAsPdf(String fileFullName = "")
        {
            if (String.IsNullOrEmpty(fileFullName))
                fileFullName = MyString.LeftOfLast(this.filePath, ".") + ".pdf";
            Object format = Word.WdSaveFormat.wdFormatPDF;
            this.innerDoc.SaveAs(fileFullName,format, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue,
                                missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue);
        }

        /// <summary>
        /// 把word文档保存为swf文件
        /// </summary>
        /// <param name="fileFullName">swf文件的输出路径，如果不传入此参数，则默认输出到本目录</param>
        /// <param name="flashPrinterAppPath">FlashPrinter.exe所在的路径</param>
        public void SaveAsSwf(String fileFullName = "", String flashPrinterAppPath = "")
        {
            if (String.IsNullOrEmpty(fileFullName))
                fileFullName = MyString.LeftOfLast(this.filePath, ".") + ".swf";
            // 通过注册表获取FlashPrinter.exe注册的路径。。
            if (String.IsNullOrEmpty(flashPrinterAppPath))
                flashPrinterAppPath = MyObject.ToStringEx(MyRegistry.Basic.GetLocalMachineSubKeyPropertyValue(@"SOFTWARE\Macromedia\FlashPaper Printer\2\Installation", "AppPath"));
            if (String.IsNullOrEmpty(flashPrinterAppPath)) return;
            // 合并需要的参数信息。
            String param = String.Format("{0} -o {1} {2}", flashPrinterAppPath, fileFullName, this.filePath);
            MyCMD.RunCmd(new String[] { param });
        }

        /// <summary>
        /// 保存对word文档的修改
        /// </summary>
        public void Save()
        {
            if (isCreateNew)
            {
                this.innerDoc.SaveAs(filePath, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, 
                    missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue);
            }
            else
            {
                this.innerDoc.Save();
            }
            this.Dispose();
        }

        /// <summary>
        /// 在指定的行序号前面插入行
        /// </summary>
        /// <param name="tableIndex">表格序号</param>
        /// <param name="rowIndex">行序号</param>
        public void TableInsertRow(Int32 tableIndex, Int32 rowIndex)
        {
            try
            {
                //this.innerDoc.Tables[tableIndex].Rows.Add(rowIndex);
                Int32 rowsCount = this.innerDoc.Tables[tableIndex].Rows.Count;
                if (rowIndex > rowsCount)
                {
                    this.innerDoc.Tables[tableIndex].Rows.Add();
                }
                else
                {
                    this.innerDoc.Tables[tableIndex].Rows.Add(this.innerDoc.Tables[tableIndex].Rows[rowIndex]);
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 删除指定的行
        /// </summary>
        /// <param name="tableIndex">表格序号</param>
        /// <param name="rowIndex">行序号</param>
        public void TableDelRow(Int32 tableIndex, Int32 rowIndex)
        {
            try
            {
                this.innerDoc.Tables[tableIndex].Rows[rowIndex].Delete();
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 获取指定表格的行数
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        public Int32 GetTableRowsCount(Int32 tableIndex)
        {
            try
            {
                return this.innerDoc.Tables[tableIndex].Rows.Count;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return 0;
        }

        /// <summary>
        /// 获取table某行单元格数
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Int32 GetTableCellsCount(Int32 tableIndex, Int32 rowIndex)
        {
            try
            {
                return this.innerDoc.Tables[tableIndex].Rows[rowIndex].Cells.Count;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return 0;
        }

        /// <summary>
        /// 获取table某行单元格数
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        public Int32 GetTableCellsCount(Int32 tableIndex)
        {
            try
            {
                return this.innerDoc.Tables[tableIndex].Columns.Count;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return 0;
        }

        /// <summary>
        /// 获取表格(序号都是从1开始算起)
        /// </summary>
        /// <param name="tableIndex">表格序号</param>
        /// <param name="rowIndex">表格行号</param>
        /// <param name="cellIndex">表格列号</param>
        /// <param name="value">插入单元格的数据</param>
        /// <param name="valueType">插入的值类型</param>
#if NET20
        public void SetTableCellValue(Int32 tableIndex, Int32 rowIndex, Int32 cellIndex, String value, InsertValueType valueType)
#else 
        public void SetTableCellValue(Int32 tableIndex,Int32 rowIndex,Int32 cellIndex,String value,InsertValueType valueType = InsertValueType.Normal )
#endif
        {
            if (String.IsNullOrEmpty(value)) return;
            try
            {
                Word.Table table = this.innerDoc.Tables[tableIndex];
                //对富文本进行格式化处理
                if (valueType == InsertValueType.RichText)
                {
                    Boolean hasSetValue = false;
                    Int32 iWaitSeconds = 15; //等待15秒
                    Int32 iSeconds = 0;

                    //用另外一个线程去复制值
                    Thread thread = new Thread(new ParameterizedThreadStart((parameters) =>
                    {
                        try
                        {
                            Object[] inputParameters = parameters as Object[];
                            Clipboard.Clear(); //移除剪切板的数据
                            if (inputParameters != null)
                            {
                                String inputValue = Convert.ToString(inputParameters[2]);
                                WebBrowser wb = new WebBrowser();
                                wb.DocumentText = inputValue;
                                Int32 i = 0;
                                while (wb.DocumentText != inputValue && i < 60)
                                {
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(1000);
                                    i++;
                                }
                                //选中WebBrowser上内容，并复制到剪贴板   
                                wb.Document.ExecCommand("SelectAll", false, null);
                                wb.Document.ExecCommand("Copy", false, null);
                                //Clipboard.GetDataObject().GetData(typeof(String));
                                //Clipboard.SetData("Html Format", bookMarkValue);
                                //黏贴值
                                Int32 innerRowIndex = (Int32)inputParameters[0];
                                Int32 innerCellIndex = (Int32)inputParameters[1];
                                Word.Document tempDoc = inputParameters[3] as Word.Document;
                                table.Rows[innerRowIndex].Cells[innerCellIndex].Range.PasteAndFormat(Word.WdRecoveryType.wdFormatOriginalFormatting);
                                //tempDoc.Save(); //内部保存一下
                                hasSetValue = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MyLog.MakeLog(ex);
                        }
                    }));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start(new Object[] { rowIndex, cellIndex, value, this.innerDoc });

                    //设置等待循环
                    while (iSeconds <= iWaitSeconds && !hasSetValue)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                        iSeconds++;
                    }
                }
                else if (valueType == InsertValueType.Normal)
                {
                    table.Rows[rowIndex].Cells[cellIndex].Range.Text = value;
                }
                else if (valueType == InsertValueType.Image)
                {
                    table.Rows[rowIndex].Cells[cellIndex].Range.InlineShapes.AddPicture(value, missingValue, missingValue, missingValue);
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 根据书签名设置书签值
        /// </summary>
        /// <param name="bookMarkName">书签名</param>
        /// <param name="value">对应的值</param>
        /// <param name="valueType">值类型（可以指定文字或者图片）</param>
        /// <param name="replace">指定是否替换原书签的值，true：为替换写入；false：为增加写入（默认是false，不替换）</param>
        public void SetBookMarkValue(String bookMarkName, String value, InsertValueType valueType, Boolean replace)
        {
            if (String.IsNullOrEmpty(value)) value = " ";
            //防止书签不存在抛出的异常
            try
            {
                //对富文本进行格式化处理
                if (valueType == InsertValueType.RichText)
                {
                    Boolean hasSetValue = false;
                    Int32 iWaitSeconds = 15; //等待15秒
                    Int32 iSeconds = 0;

                    //用另外一个线程去复制值
                    Thread thread = new Thread(new ParameterizedThreadStart((parameters) =>
                    {
                        try
                        {
                            Object[] inputParameters = parameters as Object[];
                            //可能剪切板的数据为空的时候会导致异常
                            try
                            {
                                //Clipboard.Clear(); //移除剪切板的数据
                            }
                            catch (Exception)
                            { }
                            if (inputParameters != null)
                            {
                                String bookMarkValue = Convert.ToString(inputParameters[1]);
                                WebBrowser wb = new WebBrowser();
                                wb.DocumentText = bookMarkValue;
                                Int32 i = 0;
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);
                                //while (wb.DocumentText != bookMarkValue && i < 60)
                                //{
                                //    Application.DoEvents();
                                //    System.Threading.Thread.Sleep(1000);
                                //    i++;
                                //}
                                //增加线程安全代码，因为剪切板必须是单线程访问的，否则会出现错乱问题。
                                //if (locker.TryEnterWriteLock(lockTimeout))
                                //{
                                    try
                                    {
                                        //选中WebBrowser上内容，并复制到剪贴板   
                                        wb.Document.ExecCommand("SelectAll", false, null);
                                        wb.Document.ExecCommand("Copy", false, null);
                                        //Clipboard.GetDataObject().GetData(typeof(String));
                                        //Clipboard.SetData("Html Format", bookMarkValue);
                                        //黏贴值
                                        Word.Document tempDoc = inputParameters[2] as Word.Document;
                                        String tempBookMarkName = Convert.ToString(inputParameters[0]);
                                        tempDoc.Bookmarks[tempBookMarkName].Range.PasteAndFormat(Word.WdRecoveryType.wdFormatOriginalFormatting);
                                        //tempDoc.Save(); //内部保存一下
                                        hasSetValue = true;
                                    }
                                    finally
                                    {
                                        //locker.ExitWriteLock();
                                    }
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            MyLog.MakeLog(ex);
                        }
                    }));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start(new Object[] { bookMarkName, value, this.innerDoc });

                    //设置等待循环
                    while (iSeconds <= iWaitSeconds && !hasSetValue)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                        iSeconds++;
                    }
                }
                else if (valueType == InsertValueType.Normal)
                {
                    //替换掉书签原有的值
                    if (replace)
                        this.innerDoc.Bookmarks[bookMarkName].Range.Text = value;
                    else
                        this.innerDoc.Bookmarks[bookMarkName].Range.Text += value;
                }
                else if (valueType == InsertValueType.Image)
                {
                    //替换掉书签原有的值
                    this.innerDoc.Bookmarks[bookMarkName].Range.InlineShapes.AddPicture(value, missingValue, missingValue, missingValue);
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 设置书签的值(支持传入html格式的文本等）【为了兼容旧版本的调用，保留了这个调用接口，这个接口默认是采用替换方式写入书签的值的】
        /// </summary>
        /// <param name="bookMarkName">书签名称</param>
        /// <param name="value">插入书签位置的数据</param>
        /// <param name="valueType">插入的值类型</param>
#if NET20
        public void SetBookMarkValue(String bookMarkName, String value, InsertValueType valueType)
#else 
        public void SetBookMarkValue(String bookMarkName, String value, InsertValueType valueType = InsertValueType.Normal)
#endif
        {
            SetBookMarkValue(bookMarkName, value, valueType, true);
        }

        /// <summary>
        /// 获取所有的书签名
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllBookMarks()
        {
            if (this.innerDoc == null) return null;
            List<String> bookMarks = new List<String>();
            foreach (Word.Bookmark bm in this.innerDoc.Bookmarks)
               bookMarks.Add(bm.Name);
            return bookMarks;
        }

        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MyWord()
        {
            this.Dispose(true);
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 初始化此对象
        /// </summary>
        private void Initial(Boolean showWin)
        {
            this.innerWordApp = new Word.Application();
            if (isCreateNew)
            {
                this.innerDoc = innerWordApp.Documents.Add(missingValue, missingValue, missingValue, missingValue);
            }
            else
            {
                this.innerDoc = innerWordApp.Documents.Open(this.filePath,
                                                            missingValue, missingValue,
                                                            missingValue, missingValue,
                                                            missingValue, missingValue,
                                                            missingValue, missingValue,
                                                            missingValue, missingValue,
                                                            showWin, missingValue,
                                                            missingValue, missingValue,
                                                            missingValue);
            }
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
                //this.innerDoc.Save();
                //this.innerDoc.Close();
                Object doSave = true;
                Word._Document doc = this.innerDoc as Word._Document;
                if(doc!=null) doc.Close(ref doSave, ref  missingValue, ref missingValue);
                //this.innerWordApp.Documents.Close(ref doSave, ref missingValue, ref missingValue);
                Word._Application app = this.innerWordApp as Word._Application;
                if (app != null) app.Quit(ref missingValue, ref missingValue, ref missingValue);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.innerDoc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.innerWordApp);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        #endregion

        #region "枚举值"

        public enum InsertValueType
        {
            RichText, //富文本
            Image, //图片
            Normal //普通字符串格式
        }

        #endregion

        #region "静态方法"

        /// <summary>
        /// 把word保存为swf文件
        /// </summary>
        /// <param name="filePath">word文档的路径</param>
        /// <param name="targetFilePath">生成的目标swf文件路径</param>
        /// <param name="flashPrinterAppPath">FlashPrinter.exe所在的路径</param>
        public static void ConvertToSwf(String filePath, String targetFilePath = "", String flashPrinterAppPath = "")
        {
            MyWord word = null;
            try
            {
                if (!File.Exists(filePath))
                    return;
                word = new MyWord(filePath);
                word.SaveAsSwf(targetFilePath, flashPrinterAppPath);
            }
            finally
            {
                if (word != null)
                    word.Dispose();
            }
        }

        /// <summary>
        /// 把word保存为pdf文件
        /// </summary>
        /// <param name="filePath">word文档的完整路径</param>
        /// <param name="targetFilePath">输出的pdf文件的路径</param>
        public static void ConvertToPdf(String filePath, String targetFilePath = "")
        {
            MyWord word = null;
            try
            {
                if (!File.Exists(filePath))
                    return;
                word = new MyWord(filePath);
                word.SaveAsPdf(targetFilePath);
            }
            finally
            {
                if (word != null)
                    word.Dispose();
            }
        }

        /// <summary>
        /// 杀掉所有的word进程
        /// </summary>
        public static Boolean TryKillAllWordApp(HandleExceptionInTry handleExceptionInTry = HandleExceptionInTry.ReturnAndIgnoreLog)
        {
            return MyProcess.TryKillProcessByName("winword", null, handleExceptionInTry);
        }

        #endregion
    }
}
