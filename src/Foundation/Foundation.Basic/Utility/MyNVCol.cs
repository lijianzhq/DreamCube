using System;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 针对NameValueCollection的相关辅助类
    /// </summary>
    public static class MyNVCol
    {
        /// <summary>
        /// 根据指定的键获取对应的值，它包含与指定键关联的值的列表（用逗号分隔）；否则为 null。 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="name">键</param>
        /// <param name="ignoreCase">匹配键的时候是否忽略大小写</param>
        /// <returns></returns>
        public static String GetValue(NameValueCollection col, String name, Boolean ignoreCase = false)
        {
            if (ignoreCase)
            {
                String[] allKeys = col.AllKeys;
                if (allKeys != null && allKeys.Length > 0)
                {
                    for (Int32 i = 0; i < allKeys.Length; i++)
                    {
                        if (String.Compare(allKeys[i], name) == 0) return col[allKeys[i]];
                    }
                }
            }
            else return col[name];
            return null;
        }
    }
}
