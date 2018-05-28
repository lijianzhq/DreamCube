using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class MyWebUtilityTest
    {
        [Test]
        public void TestMethod()
        {
            var client = new WebClient();
            var data = client.DownloadData("http://www.dytt8.net/html/gndy/dyzz/20180520/56877.html");
            var encoding = Encoding.UTF8;
            MyWebUtility.TryGetWebPageEncoding(data, ref encoding);
            Assert.AreEqual(encoding, Encoding.GetEncoding("gb2312"));
        }
    }
}
