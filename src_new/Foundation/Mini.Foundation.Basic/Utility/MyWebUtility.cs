using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
#if !NETSTANDARD1_0 && !NETSTANDARD1_3
using System.Web;
#endif

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// web开发遇到的一些常见的公共方法
    /// </summary>
    public static class MyWebUtility
    {
        /// <summary>
        /// 根据网页的二进制数据，获取对应的编码（从网页的charset字符集去获取）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="doNotThrowException">转换编码失败的时候，不要抛出异常</param>
        /// <returns></returns>
        public static Boolean TryGetWebPageEncoding(Byte[] data, ref Encoding encoding, Boolean doNotThrowException = true)
        {
            if (data == null || data.Length == 0) return false;
            String strWebData = Encoding.UTF8.GetString(data, 0, data.Length);
            //获取网页字符编码描述信息
            Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<\"]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            String webCharSet = charSetMatch.Groups[2].Value;
            if (!String.IsNullOrEmpty(webCharSet))
            {
                try
                {
                    encoding = Encoding.GetEncoding(webCharSet);
                }
                catch (ArgumentException)
                {
                    if (!doNotThrowException) throw;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// html解码对于.NET20等旧版的调用HttpUtility.HtmlDecode进行解码；
        /// .net40后新版的调用WebUtility.HtmlDecode进行解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String HtmlDecode(String value)
        {
#if HAVE_WEBUTILITY
            return WebUtility.HtmlDecode(value);
#else
            return HttpUtility.HtmlDecode(value);
#endif
        }

        /// <summary>
        /// 是否已经进行了urlencode
        /// UrlDecode一下，再UrlDecode一下，前后一致即未Encode过，前后不一致即Encode过。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean HasUrlEncode(String value)
        {
            String decodeText = UrlDecode(value);
            String decodeText2 = UrlDecode(decodeText);
#if NETSTANDARD1_0
            return String.Compare(value, decodeText2, StringComparison.CurrentCultureIgnoreCase) != 0;
#else
            return String.Compare(value, decodeText2, true) != 0;
#endif
        }

        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="encodedValue"></param>
        /// <returns></returns>
        public static String UrlDecode(String encodedValue)
        {
#if HAVE_WEBUTILITY
            return WebUtility.UrlDecode(encodedValue);
#else
            return HttpUtility.UrlDecode(encodedValue);
#endif
        }

        /// <summary>
        /// 对文本进行UrlEncode（内部会判断是否经过了Encode的，所以可以多次调用此Encode方法都没关系）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="doNotCheckHasEncode">是否判断是否已经encode过一次了，true：不判定；false：判断，如果已经encode一次了，不会再encode了</param>
        /// <returns></returns>
        public static String UrlEncode(String value, Boolean doNotCheckHasEncode = true)
        {
            if (doNotCheckHasEncode) return UrlEncode(value);
            if (!HasUrlEncode(value)) return UrlEncode(value);
            return value;
        }
    }
}
