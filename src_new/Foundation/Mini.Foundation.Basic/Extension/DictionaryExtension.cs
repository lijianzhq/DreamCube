#if !NET20
using System;
using System.Collections.Generic;

namespace Mini.Foundation.Basic.Extension
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 把key值对应的value值++，如果key值不存在，则设置为defaultValue+1
        /// </summary>
        /// <param name="target"></param>
        public static void ValueIncrement(this Dictionary<String, Int32> target, String key, Int32 defaultValue = 0)
        {
            if (target == null) return;
            if (target.ContainsKey(key)) target[key]++;
            else target[key] = defaultValue + 1;
        }
    }
}
#endif
