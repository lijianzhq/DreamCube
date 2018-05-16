namespace Mini.Foundation.Office
{
    public interface IExcelHelper
    {
        IDataTableToExcel DataTableToExcel { get; }
        IExcelToDataTable ExcelToDataTable { get; }
    }
}