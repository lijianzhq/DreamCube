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
        public void TestMethod_GetBetweenStr1()
        {
            String str = MyString.GetBetweenStr("2010央视主打热播剧《血色沉香》全34集[国语字幕]", "《", "》");
            Assert.IsTrue(String.Equals(str, "血色沉香"));
        }

        [Test]
        public void TestMethod_GetBetweenStr()
        {
            String str = MyString.GetBetweenStr("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "<strong>", "</strong>");
            Assert.IsTrue(String.Equals(str, "8.5/10 (255,815 votes) Top 250: #46"));
        }

        [Test]
        public void TestMethod_LeftOf()
        {
            String str = MyString.LeftOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "COLOR:");
            Assert.IsTrue(String.Equals(str, "◎IMDB评分 <span style=\""));

            str = MyString.LeftOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", 4);
            Assert.IsTrue(String.Equals(str, "◎IMD"));

            str = MyString.LeftOf("ftp://ftpuser@127.0.0.1/{yyyy-MM-dd}", "/", false, "", "ftp://".Length);
            Assert.IsTrue(String.Equals(str, "ftp://ftpuser@127.0.0.1"));
        }

        [Test]
        public void TestMethod_LastLeftOf()
        {
            String str = MyString.LeftOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "strong");
            Assert.IsTrue(String.Equals(str, "◎IMDB评分 <span style=\"COLOR: red\"><"));

            str = MyString.LastLeftOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "strong");
            Assert.IsTrue(String.Equals(str, "◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</"));
        }

        [Test]
        public void TestMethod_RightOf()
        {
            String str = MyString.RightOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "COLOR:");
            Assert.IsTrue(String.Equals(str, " red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>"));

            str = MyString.RightOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", 4);
            Assert.IsTrue(String.Equals(str, "pan>"));
        }

        [Test]
        public void TestMethod_LastRightOf()
        {
            String str = MyString.RightOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "strong");
            Assert.IsTrue(String.Equals(str, ">8.5/10 (255,815 votes) Top 250: #46</strong></span>"));

            str = MyString.LastRightOf("◎IMDB评分 <span style=\"COLOR: red\"><strong>8.5/10 (255,815 votes) Top 250: #46</strong></span>", "strong");
            Assert.IsTrue(String.Equals(str, "></span>"));
        }
    }
}
