using System;
using System.Data;

using Mini.Foundation.IOC;
using Mini.Foundation.Office;

namespace Test_Framework
{
    class TestExcel
    {
        public static void Start()
        {
            ContainerFactory.RegisterConfigFile(new String[] { @"config/ioc.xml" });
            IExcelHelper excelHelper = new ExcelHelper();
            var table = excelHelper.ExcelToDataTable.GetDataTable();
            for (var i = 0; i < table.Rows.Count; i++)
            {
                for (var r = 0; r < table.Columns.Count; r++)
                {
                    Console.Write("\t{0}", table.Rows[i][r]);
                }
                Console.WriteLine();
            }
        }
    }
}
