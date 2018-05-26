using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mini.Foundation.Basic.Utility
{
    public static class MyString
    {

        public static String GetBetweenStr(String value, String startChar, String endChar)
        {
            Regex rg = new Regex("(?<=(" + startChar + "))[.\\s\\S]*?(?=(" + endChar + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(value).Value;
        }

        /// <summary>
        /// net20,net35调用String.IsNullOrEmpty，其他类库调用String.IsNullOrWhiteSpace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsInvisibleString(String value)
        {
#if NET20 || NET35
            return String.IsNullOrEmpty(value);
#else
            return String.IsNullOrWhiteSpace(value);
#endif
        }
    }
}
