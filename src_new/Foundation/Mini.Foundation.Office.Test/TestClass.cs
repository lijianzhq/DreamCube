using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Mini.Foundation.Office;

namespace Mini.Foundation.Office.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            String filePath = "";
            using (var excel = new ExcelWrapper())
            {
                var table = new DataTable();
                table.Columns.Add("姓名");
                table.Columns.Add("年龄");
                table.Columns.Add("身高");
                table.Rows.Add("lijian", "31", "160");
                table.Rows.Add("zhq", "31", "156");
                excel.AddSheet(table);
                filePath = excel.Save();
            }
            using (var excel = new ExcelWrapper(filePath))
            {
                Assert.IsTrue(excel.SheetNames.Count == 2);
                var table = excel.GetDataTable("Sheet2", true);
                Assert.IsNotNull(table);
                Assert.IsTrue(table.Rows.Count == 2);
                Assert.IsTrue(table.Columns.Count == 3);
                Assert.IsTrue(table.Columns[0].ColumnName == "姓名");
                Assert.IsTrue(table.Columns[1].ColumnName == "年龄");
                Assert.IsTrue(table.Columns[2].ColumnName == "身高");
            }
        }

        [Test]
        public void TestMethod2()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
             {
                 using (var excel = new ExcelWrapper(""))
                 {
                 }
             });
            Assert.Throws(typeof(FileNotFoundException), () =>
            {
                using (var excel = new ExcelWrapper(@"d:\a.xlsx"))
                {
                }
            });
        }

        [Test]
        public void TestMethod3()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFile/passwords.xlsx");
            using (var excel = new ExcelWrapper(path))
            {
                var table = excel.GetDataTable("新开通供应商", true);
                Assert.IsTrue(table.Rows.Count == 2991);
            }

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFile/emails.xlsx");
            using (var excel = new ExcelWrapper(path))
            {
                var table = excel.GetDataTable("Sheet1", true);
                Assert.IsTrue(table.Rows.Count == 4592);
            }
        }
    }
}
