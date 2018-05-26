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
            int? i = MyConverter.ChangeType<int?>("123");
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Value == 123);
            DateTime dt = MyConverter.ChangeType<DateTime>("20:33");
            char c = MyConverter.ChangeType<char>("x");
            Guid g = MyConverter.ChangeType<Guid>("{32F92EEB-A703-4eb7-A9F8-62E09F87D03F}");
            Assert.IsNotNull(g);
            Assert.IsTrue(g.ToString("N").ToUpper() == "32F92EEBA7034eb7A9F862E09F87D03F".ToUpper());
            Version v = MyConverter.ChangeType<Version>("1.2.3.4");
            DateTime? k = MyConverter.ChangeType<DateTime?>(null);
            Assert.IsNull(k);
            DateTime? k2 = MyConverter.ChangeType<DateTime?>("2018-05-05");
            Assert.IsNotNull(k2);
        }
    }
}
