using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyUrl
    {
        /// <summary>
        /// 组合两个Url片段，处理斜杠符号
        /// </summary>
        /// <param name="url"></param>
        /// <param name="urlParam"></param>
        /// <returns></returns>
        public static String Combine(String url, String urlParam)
        {
            if (!String.IsNullOrEmpty(url) && !String.IsNullOrEmpty(urlParam))
            {
                url = url.Replace("\\", "/");
                urlParam = urlParam.Replace("\\", "/");
                if (url.EndsWith("/")) url = url.Substring(0, url.Length - 1);
                if (urlParam.StartsWith("/")) urlParam = urlParam.Substring(1);
                return url + "/" + urlParam;
            }
            return url + urlParam;
        }
    }
}
