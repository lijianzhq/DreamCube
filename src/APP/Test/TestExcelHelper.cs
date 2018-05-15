using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DreamCube.Foundation.Office;

namespace Test
{
    class TestExcelHelper
    {
        public static void Start(String excelFilePath)
        {
            var helper = new ExcelHelper();
            var ds = helper.ExcelToDataTable.Init(excelFilePath)
                                   .GetDataTables();

            if (ds != null)
            {
                for (var i = 0; i < ds.Tables.Count; i++)
                {
                    var table = ds.Tables[i];
                    Console.WriteLine("table:{0}", table.TableName);
                    Console.WriteLine("rowsCount:{0},columnsCount:{1}", table.Rows.Count, table.Columns.Count);
                }
            }
        }
    }
}
