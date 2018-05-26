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
    public class MyStringTest
    {
        [Test]
        public void TestMethod()
        {
            String str = MyString.GetBetweenStr("2010央视主打热播剧《血色沉香》全34集[国语字幕]", "《", "》");
            Assert.IsTrue(String.Equals(str, "血色沉香"));
        }
    }
}
