using System.Data;

using NPOI.SS.UserModel;

namespace Mini.Foundation.Office
{
    public interface IDataTableToExcel
    {
        void AddCellImage(ICell cell, byte[] image);
        void AddSheet(DataTable table, string sheetName = "Sheet1", string[] ignoreColumns = null);
        void SaveToFile(string filePath);
    }
}