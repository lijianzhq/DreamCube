#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0)
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class ConfigFileReaderTest
    {
        [Test]
        public void TestMethod()
        {
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mini.Foundation.Basic.Test.config");
            var configer = new ConfigFileReader(path);
            var cachePathConfig = configer.AppSettings("CachePath");
            Assert.AreEqual(cachePathConfig, "~/cache/");

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LY.TEC.Data.config");
            configer = new ConfigFileReader(path);
            cachePathConfig = configer.AppSettings("CachePath");
            Assert.AreEqual(cachePathConfig, "~/cache/");
            Assert.IsNotNull(configer.ConnectionString("MATMIS2"));
        }
    }
}

#endif