using System.Collections.Generic;
using System.Data;

namespace Mini.Foundation.Office
{
    public interface IExcelToDataTable
    {
        List<string> SheetNames { get; }

        DataTable GetDataTable(string sheetName = "Sheet1", bool isFirstRowColumnName = false);
        DataSet GetDataTables(string[] sheetNames = null, bool isFirstRowColumnName = false);
        IExcelToDataTable Init(string excelFilePath);
    }
}