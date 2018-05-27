using System;
using System.Net;
using System.Text;
#if !NETSTANDARD1_0 && !NETSTANDARD1_3
using System.Web;
#endif

namespace Mini.Foundation.Basic.Utility
{
    public static class MyWebUtility
    {
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
        /// <param name="text"></param>
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
