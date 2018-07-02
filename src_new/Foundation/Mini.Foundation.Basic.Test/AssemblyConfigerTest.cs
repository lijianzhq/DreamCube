#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0)
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
    public class AssemblyConfigerTest
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");

            var asmConfig = new AssemblyConfiger();
            var cachePathConfig = asmConfig.ConfigFileReader.AppSettings("CachePath");
            Assert.AreEqual(cachePathConfig, "~/cache/");
        }
    }
}

#endif