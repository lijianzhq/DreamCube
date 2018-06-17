using System;
using NUnit.Framework;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class MyConverterTest
    {
        [Test]
        public void TestMethod()
        {
            int? i = MyConvert.ChangeType<int?>("123");
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Value == 123);
            DateTime dt = MyConvert.ChangeType<DateTime>("20:33");
            char c = MyConvert.ChangeType<char>("x");
            Guid g = MyConvert.ChangeType<Guid>("{32F92EEB-A703-4eb7-A9F8-62E09F87D03F}");
            Assert.IsNotNull(g);
            Assert.IsTrue(g.ToString("N").ToUpper() == "32F92EEBA7034eb7A9F862E09F87D03F".ToUpper());
            Version v = MyConvert.ChangeType<Version>("1.2.3.4");
            DateTime? k = MyConvert.ChangeType<DateTime?>(null);
            Assert.IsNull(k);
            DateTime? k2 = MyConvert.ChangeType<DateTime?>("2018-05-05");
            Assert.IsNotNull(k2);
        }
    }
}
