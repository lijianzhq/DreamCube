using System;
using System.Web;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyWebUtility
    {
        /// <summary>
        /// 是否已经进行了urlencode
        /// UrlDecode一下，再UrlDecode一下，前后一致即未Encode过，前后不一致即Encode过。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Boolean HasUrlEncode(String text)
        {
            String decodeText = HttpUtility.UrlDecode(text);
            String decodeText2 = HttpUtility.UrlDecode(decodeText);
            return String.Compare(text, decodeText2, true) != 0;
        }

        /// <summary>
        /// 对文本进行UrlEncode（内部会判断是否经过了Encode的，所以可以多次调用此Encode方法都没关系）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String UrlEncode(String text)
        {
            if (!HasUrlEncode(text)) return HttpUtility.UrlEncode(text);
            return text;
        }
    }
}
