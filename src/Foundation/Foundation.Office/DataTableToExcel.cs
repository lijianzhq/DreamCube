using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Office
{
    public class DataTableToExcel : IDataTableToExcel
    {
        #region "fields"

        protected HSSFCellStyle _headerCellStyle = null;
        protected IWorkbook _workbook = new HSSFWorkbook();
        /// <summary>
        /// 缓存单元格最大的宽度（用于在构造execl的过程中，根据列内容的大小，调整宽度）
        /// key： columnindex
        /// value：max dataLength
        /// </summary>
        protected Dictionary<Int32, Int32> _cellMaxDataLength = new Dictionary<Int32, Int32>();

        #endregion

        #region "property"

        /// <summary>
        /// 创建表头的样式
        /// </summary>
        /// <param name="_workbook"></param>
        /// <returns></returns>
        public virtual HSSFCellStyle HeaderCellStyle
        {
            get
            {
                if (_headerCellStyle == null)
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
                    _headerCellStyle = (HSSFCellStyle)_workbook.CreateCellStyle();
                    _headerCellStyle.Alignment = HorizontalAlignment.Center;
                    _headerCellStyle.FillForegroundColor = 23;
                    _headerCellStyle.BorderBottom = BorderStyle.Thin;
                    _headerCellStyle.BottomBorderColor = 23;
                    _headerCellStyle.BorderLeft = BorderStyle.Thin;
                    _headerCellStyle.LeftBorderColor = 55;
                    _headerCellStyle.BorderRight = BorderStyle.Thin;
                    _headerCellStyle.RightBorderColor = 55;
                    _headerCellStyle.BorderTop = BorderStyle.Thin;
                    _headerCellStyle.TopBorderColor = 22;
                    _headerCellStyle.FillPattern = FillPattern.SolidForeground;
                    IFont font = _workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    font.Color = 9;
                    _headerCellStyle.SetFont(font);

                }
                return _headerCellStyle;
            }
            set
            {
                _headerCellStyle = value;
            }
        }

        #endregion

        #region "creator"

        /// <summary>
        /// datatable数据加载到excel文件中
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_sheetName">指定生成的工作簿名</param>
        /// <param name="ignoreColumns">忽略的列名</param>
        public DataTableToExcel()
        { }

        #endregion

        #region "public method"

        /// <summary>
        /// 保存workbook到磁盘上
        /// </summary>
        /// <param name="filePath"></param>
        public virtual void SaveToFile(String filePath)
        {
            //生成文件
            using (MemoryStream ms = new MemoryStream())
            {
                _workbook.Write(ms);
                ms.Flush();
                ms.Position = 0L;
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
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
        }

        /// <summary>
        /// 图片在单元格等比缩放居中显示
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="image">图片二进制流</param>
        public virtual void AddCellImage(ICell cell, Byte[] image)
        {
            if (image.Length == 0) return;//空图片处理
            double scalx = 0;//x轴缩放比例
            double scaly = 0;//y轴缩放比例
            int Dx1 = 0;//图片左边相对excel格的位置(x偏移) 范围值为:0~1023,超过1023就到右侧相邻的单元格里了
            int Dy1 = 0;//图片上方相对excel格的位置(y偏移) 范围值为:0~256,超过256就到下方的单元格里了
            bool bOriginalSize = false;//是否显示图片原始大小 true表示图片显示原始大小  false表示显示图片缩放后的大小
            ///计算单元格的长度和宽度
            double CellWidth = cell.Row.Sheet.GetColumnWidth(cell.ColumnIndex);
            double CellHeight = cell.Sheet.GetRow(cell.RowIndex).Height;
            //单元格长度和宽度与图片的长宽单位互换是根据实例得出
            CellWidth = CellWidth / 35;
            CellHeight = CellHeight / 15;
            ///计算图片的长度和宽度
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
        }

        /// <summary>
        /// 添加一个sheet
        /// </summary>
        /// <param name="table"></param>
        /// <param name="sheetName"></param>
        public virtual void AddSheet(DataTable table, String sheetName = "Sheet1", String[] ignoreColumns = null)
        {
            MyValidation.ArgumentNotNull(table, "table");
            MyValidation.ArgumentNotNullOrEmpty(sheetName, "sheetName");
            ISheet sheet = null;
            sheet = _workbook.GetSheet(sheetName);
            if (sheet != null) throw new Exception(String.Format("The sheet name[{0}] has already existed!", sheetName));
            sheet = _workbook.CreateSheet(sheetName);
            //渲染列头
            IRow row = sheet.CreateRow(0);//添加第一行（列头）
            //设置表头样式
            HSSFCellStyle headCellStyle = HeaderCellStyle;
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

        #endregion

        #region "protected method"

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
            return _ignoreColumnList.Contains(columnName, StringComparer.CurrentCultureIgnoreCase);
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

        #endregion
    }

}