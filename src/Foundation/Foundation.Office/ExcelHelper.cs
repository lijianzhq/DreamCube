using System;

using DreamCube.Foundation.IOC;

namespace DreamCube.Foundation.Office
{
    public class ExcelHelper
    {
        private IExcelToDataTable _excelToDataTable;
        private Object _locker1 = new Object();
        public IExcelToDataTable ExcelToDataTable
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
        public IDataTableToExcel DataTableToExcel
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