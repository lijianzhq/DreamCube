using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 字符串的相关方法
    /// </summary>
    public static class MyString
    {
        /// <summary>
        /// 从给定的字符串，获取两个字符串之间的内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startChar"></param>
        /// <param name="endChar"></param>
        /// <returns></returns>
#if NET20
        public static String GetBetweenStr(String value, String startChar, String endChar)
#else
        public static String GetBetweenStr(this String value, String startChar, String endChar)
#endif
        {
            if (IsInvisibleString(value)) return String.Empty;
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

        /// <summary>
        /// 取标志字符的右边部分（从最后开始匹配标志字符）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <param name="defaultValue">当target为NULL或者空串时，返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String LastRightOf(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String LastRightOf(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.LastIndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(index + value.Length) : defaultValue;
        }

        /// <summary>
        /// 取标志字符的左边部分（从最后开始匹配标志字符）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <param name="defaultValue">获取不到的时候，返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String LastLeftOf(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String LastLeftOf(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.LastIndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(0, index) : defaultValue;
        }

        /// <summary>
        /// 取标志字符的左边部分
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <param name="defaultValue">获取失败的时候，返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String LeftOf(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String LeftOf(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.IndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(0, index) : defaultValue;
        }

        /// <summary>
        /// 获取左边Length长度的字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NET20
        public static String LeftOf(String target, Int32 length)
#else
        public static String LeftOf(this String target, Int32 length)
#endif
        {
            if (length <= 0) return "";
            if (length >= target.Length) return target;
            return target.Substring(0, length);
        }

        /// <summary>
        /// 获取右边Length长度的字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NET20
        public static String RightOf(String target, Int32 length)
#else
        public static String RightOf(this String target, Int32 length)
#endif
        {
            if (length <= 0) return "";
            if (length >= target.Length) return target;
            return target.Substring(target.Length - length, length);
        }

        /// <summary>
        /// 取标志字符的右边部分
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <param name="defaultValue">获取失败返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String RightOf(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String RightOf(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.IndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(index + value.Length) : defaultValue;
        }
    }
}
