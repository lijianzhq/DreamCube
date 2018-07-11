#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;
using System.Reflection;
using System.Collections.Generic;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// </summary>
    public static class AsmConfigerHelper
    {
        static Dictionary<String, AssemblyConfiger> _cache = new Dictionary<String, AssemblyConfiger>();

        /// <summary>
        /// 获取指定程序集配置对象
        /// </summary>
        /// <param name="asm">不传入，默认获取CallingAssembly的配置对象</param>
        /// <returns></returns>
        public static AssemblyConfiger GetConfiger(Assembly asm = null)
        {
            if (asm == null) asm = Assembly.GetCallingAssembly();
            if (!_cache.ContainsKey(asm.FullName))
            {
                _cache[asm.FullName] = new AssemblyConfiger(asm);
            }
            return _cache[asm.FullName];
        }
    }
}
#endif
