using System;
using System.Collections.Generic;
using System.Text;

namespace Mini.Foundation.Basic.Utility
{
    public static class MyUrl
    {
        /// <summary>
        /// 组合两个Url片段，处理斜杠符号
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        public static String Combine(String part1, String part2)
        {
            if (!String.IsNullOrEmpty(part1) && !String.IsNullOrEmpty(part2))
            {
                part1 = part1.Replace("\\", "/");
                part2 = part2.Replace("\\", "/");
                if (part1.EndsWith("/")) part1 = part1.Substring(0, part1.Length - 1);
                if (part2.StartsWith("/")) part2 = part2.Substring(1);
                return part1 + "/" + part2;
            }
            return part1 + part2;
        }
    }
}
