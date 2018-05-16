using System;

using Mini.Foundation.IOC;

namespace Mini.Foundation.Office
{
    public class ExcelHelper : IExcelHelper
    {
        private IExcelToDataTable _excelToDataTable;
        private Object _locker1 = new Object();
        public virtual IExcelToDataTable ExcelToDataTable
        {
            get
            {
                if (_excelToDataTable == null)
                {
                    lock (_locker1)
                    {
                        if (_excelToDataTable == null)
                            _excelToDataTable = ContainerFactory.GetContainer().Resolve<IExcelToDataTable>();
                    }
                }
                return _excelToDataTable;
            }
        }

        private IDataTableToExcel _dataTableToExcel;
        private Object _locker2 = new Object();
        public virtual IDataTableToExcel DataTableToExcel
        {
            get
            {
                if (_dataTableToExcel == null)
                {
                    lock (_locker2)
                    {
                        if (_dataTableToExcel == null)
                            _dataTableToExcel = ContainerFactory.GetContainer().Resolve<IDataTableToExcel>();
                    }
                }
                return _dataTableToExcel;
            }
        }
    }
}