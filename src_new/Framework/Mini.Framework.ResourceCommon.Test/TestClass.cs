using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mini.Framework.ResourceCommon;

namespace Mini.Framework.ResourceCommon.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            // Assert.Pass("Your first passing test");

            //Assert.AreEqual(StrResourceManager.Current.GetString("judgeUserRole"), "PQ_REJ_JUDGE");
            Assert.AreEqual(StrResourceManager.Current.GetString("judgeUserRole", @"\Mini.Framework.ResourceCommon.Test.xml", "cn"), "PQ_REJ_JUDGE");
            Assert.AreEqual(StrResourceManager.Current.GetString("judgeUserRole", @"\Mini.Framework.ResourceCommon.Test.xml"), "PQ_REJ_JUDGE");
            Assert.AreEqual(StrResourceManager.Current.GetString("judgeUserRole", @"\1.xml"), "1111");
            Assert.AreEqual(StrResourceManager.Current.GetString("judgeUserRole", @"\test\1.xml"), "test1111");
        }
    }
}
