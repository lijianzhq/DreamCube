using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Mini.Framework.Database;

namespace Mini.Framework.Database.Test
{
    [TestFixture]
    public class TestClass
    {
        protected DB _db = null;

        [SetUp]
        public void Setup()
        {
            String filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "dataFiles/test.accdb");
            Assert.IsTrue(File.Exists(filePath));
            _db = new DB(new AccessProvider(filePath));
        }

        [Test]
        public void TestMethod1()
        {
            //using (var context = _db.BeginExecuteContext())
            //{
            //    Assert.IsTrue(1 == context.ExecuteNonQuery("insert into user(username) values('lijian')"));
            //}

            using (var context = _db.BeginExecuteContext())
            {
                context.ExecuteNonQuery("delete from students");
            }

            using (var context = _db.BeginExecuteContext())
            {
                Assert.IsTrue(1 == context.ExecuteNonQuery("insert into Students(stuname) values('lijian')"));
            }

            using (var context = _db.BeginExecuteContext())
            {
                Assert.IsTrue("lijian" == context.ExecuteScalar("select stuname from students  where stuname='lijian'").ToString());
            }

            using (var context = _db.BeginExecuteContext())
            {
                var table = context.GetDataTable("select stuname from students  where stuname='lijian'");
                Assert.IsTrue(table.Rows.Count == 1);
                Assert.IsTrue(table.Rows[0]["stuname"].ToString() == "lijian");
            }
        }

        [Test]
        public void TestMethod2()
        {
            //using (var context = _db.BeginExecuteContext())
            //{
            //    Assert.IsTrue(1 == context.ExecuteNonQuery("insert into user(username) values('lijian')"));
            //}

            using (var context = _db.BeginExecuteContext())
            {
                context.ExecuteNonQuery("delete from students");
            }

            using (var context = _db.BeginExecuteContext())
            {
                Assert.IsTrue(1 == context.ExecuteNonQuery("insert into Students(stuname) values('lijian')"));
                Assert.IsTrue(1 == context.ExecuteNonQuery("insert into Students(stuname) values('zhq')"));
            }

            using (var context = _db.BeginExecuteContext())
            {
                Assert.IsTrue(2 == context.ExecuteScalar<Int32>("select count(*) from students"));
            }
        }
    }
}
