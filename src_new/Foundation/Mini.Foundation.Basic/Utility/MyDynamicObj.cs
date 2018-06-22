using System;
using System.Reflection;
using System.Collections.Generic;

namespace Mini.Foundation.Basic.Utility
{
    public static class MyDynamicObj
    {
#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NET20)
        /// <summary>
        /// 根据对象，创建 一个动态对象；返回一个键值对，键为对象的属性名，值为属性值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="include">包含的属性</param>
        /// <param name="exclude">排除的属性</param>
        /// <returns></returns>
        public static Dictionary<String, Object> GetDynamicObj(Object o, Func<PropertyInfo, Boolean> include = null, Func<PropertyInfo, Boolean> exclude = null)
        {
            if (o == null) return null;
            var type = o.GetType();
            if (type.IsValueType)
                return new Dictionary<string, object> { { o.ToString(), o } };
            var dic = new Dictionary<String, Object>();
            foreach (var p in o.GetType().GetProperties())
            {
                if ((include != null && include(p)))
                {
                    dic.Add(p.Name, p.GetValue(o, null));
                }
                else if (exclude != null)
                {
                    if (!exclude(p))
                        dic.Add(p.Name, p.GetValue(o, null));
                }
                else
                {
                    dic.Add(p.Name, p.GetValue(o, null));
                }
            }
            return dic;
        }

#endif
    }
}
