using NUnit.Framework;
using System;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class MyUrlTest
    {
        [Test]
        public void TestCombine()
        {
            String url1 = "http://baidu.com";
            String url2 = "/login/index.aspx";
            Assert.AreEqual(MyUrl.Combine(url1, url2), "http://baidu.com/login/index.aspx");

            url1 = "http://baidu.com/";
            url2 = "/login/index.aspx";
            Assert.AreEqual(MyUrl.Combine(url1, url2), "http://baidu.com/login/index.aspx");

            url1 = "http://baidu.com/login.aspx";
            url2 = "/login/index.aspx";
            Assert.AreEqual(MyUrl.Combine(url1, url2), "http://baidu.com/login/index.aspx");

            url1 = "http://baidu.com/login/login.aspx";
            url2 = "/login/index.aspx";
            Assert.AreEqual(MyUrl.Combine(url1, url2), "http://baidu.com/login/index.aspx");

            url1 = "http://baidu.com/login/login.aspx";
            url2 = "login/index.aspx";
            Assert.AreEqual(MyUrl.Combine(url1, url2), "http://baidu.com/login/login/index.aspx");
        }
    }
}
