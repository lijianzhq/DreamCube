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
    public class MyEnumerable_Test
    {
        [Test]
        public void Test_JoinEx()
        {
            var target = new String[] { "lijian", "zhq" };
            Assert.IsTrue(String.Equals("lijian;zhq", target.JoinEx(";")));
        }

        [Test]
        public void Test_JoinEx2()
        {
            var target = new String[] { "lijian;", "zhq" };
            Assert.Throws(typeof(ArgumentException), () => { String.Equals("lijian;zhq", target.JoinEx(";")); });
        }
    }
}
