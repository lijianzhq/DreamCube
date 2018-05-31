using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Office
{
    public class ExcelToDataTable : IExcelToDataTable
    {
        #region "field"

        /// <summary>  
        /// 使用NOPI读取Excel数据  
        /// </summary>  
        private IWorkbook _workbook;
        /// <summary>
        /// 是否已经初始化了
        /// </summary>
        private Boolean _hasInitial = false;
        private List<String> _sheetNames = null;

        public virtual List<String> SheetNames
        {
            get
            {
                ThrowIfNotInitialized();
                if (_sheetNames != null) return _sheetNames;
                var count = _workbook.NumberOfSheets; //获取所有SheetName
                var sheetNameList = new List<String>();
                for (var i = 0; i < count; i++)
                    sheetNameList.Add(_workbook.GetSheetAt(i).SheetName);
                _sheetNames = sheetNameList;
                return _sheetNames;
            }
        }

        #endregion

        #region "creator"

        public ExcelToDataTable()
        { }

        #endregion

        #region public method  

        /// <summary>
        /// 根据文件初始化对象
        /// </summary>
        /// <param name="excelFilePath"></param>
        public virtual IExcelToDataTable Init(String excelFilePath)
        {
            MyArgumentsHelper.ThrowsIfFileNotExist(excelFilePath, nameof(excelFilePath));
            var fs = new FileStream(excelFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            // 2007版本  
            if (excelFilePath.IndexOf(".xlsx") > 0)
                _workbook = new XSSFWorkbook(fs);
            // 2003版本  
            else if (excelFilePath.IndexOf(".xls") > 0)
                _workbook = new HSSFWorkbook(fs);
            _hasInitial = true;
            return this;
        }

        /// <summary>
        /// 将excel中的所有的表格数据导入到DataTable中
        /// </summary>
        /// <param name="sheetNames">如果不传入该参数，则默认读取所有的 sheet 数据到DataSet</param>
        /// <param name="isFirstRowColumnName">第一行是否是DataTable的列名</param>  
        /// <returns></returns>
        public virtual DataSet GetDataTables(String[] sheetNames = null, Boolean isFirstRowColumnName = false)
        {
            ThrowIfNotInitialized();
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
        /// <returns>返回的DataTable</returns>  
        public virtual DataTable GetDataTable(String sheetName = "Sheet1", Boolean isFirstRowColumnName = false)
        {
            ThrowIfNotInitialized();
            MyArgumentsHelper.ThrowsIfIsInvisibleString(sheetName, nameof(sheetName));
            ISheet sheet = null;
            var data = new DataTable();
            data.TableName = sheetName;
            int startRow = 0;

            sheet = _workbook.GetSheet(sheetName);
            if (sheet == null) throw new Exception(String.Format("excel file does not has the sheet named [{0}]!", sheetName));

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

        #endregion

        #region "protected method"

        protected virtual void ThrowIfNotInitialized()
        {
            if (!_hasInitial)
                throw new Exception("object does not initialized!");
        }

        #endregion
    }
}
