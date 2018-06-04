using System;
using System.Collections.Generic;
using System.Text;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// url的帮助类
    /// </summary>
    public static class MyUrl
    {
        /// <summary>
        /// 组合两个Url片段，处理斜杠符号
        /// 如果 part2以斜杠符号开始，则代表是相对根目录的路径;否则则代表是相对当前页面的路径
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        public static String Combine(String part1, String part2)
        {
            if (!String.IsNullOrEmpty(part1) && !String.IsNullOrEmpty(part2))
            {
                var uri = new Uri(part1);
                part1 = part1.Replace("\\", "/");
                part2 = part2.Replace("\\", "/");
                if (part2 == "/") return part1;
                if (part1.EndsWith("/")) part1 = part1.Substring(0, part1.Length - 1);
                if (part2.StartsWith("/"))
                    return String.Format("{0}://{1}{2}{3}", uri.Scheme, uri.Host, uri.IsDefaultPort ? "" : ":" + uri.Port.ToString(), part2);
                else if (part1.EndsWith(uri.Host))
                    return String.Format("{0}/{1}", part1, part2);
                return String.Format("{0}/{1}", MyString.LastLeftOf(part1, "/"), part2);
            }
            return part1 + part2;
        }
    }
}
