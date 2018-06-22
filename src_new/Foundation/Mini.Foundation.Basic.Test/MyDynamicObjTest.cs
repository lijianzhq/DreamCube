using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class MyDynamicObjTest
    {
        [Test]
        public void TestMethod()
        {
            var dic = MyDynamicObj.GetDynamicObj(new { Name = "Jianl", Age = 40 });
            Assert.AreEqual(dic.Count, 2);
            Assert.AreEqual(dic["Name"], "Jianl");
            Assert.AreEqual(dic["Age"], 40);

            Int32? aa = 100;
            dic = MyDynamicObj.GetDynamicObj(aa);
            Assert.AreEqual(dic.Count, 1);

            dic = MyDynamicObj.GetDynamicObj(new { Name = "Jianl", Age = 40 }, null, it => it.Name == "Name");
            Assert.AreEqual(dic.Count, 1);
            Assert.AreEqual(dic["Age"], 40);
        }
    }
}
