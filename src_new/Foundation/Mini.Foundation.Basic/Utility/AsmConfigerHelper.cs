#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;
using System.Reflection;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// </summary>
    public static class AsmConfigerHelper
    {
        //static AssemblyConfiger _asmConfiger = null;
        //static Object _locker = new Object();

        //internal static AssemblyConfiger AsmConfiger
        //{
        //    get
        //    {
        //        if (_asmConfiger == null)
        //        {
        //            lock (_locker)
        //            {
        //                if (_asmConfiger == null)
        //                    _asmConfiger = new AssemblyConfiger();
        //            }
        //        }
        //        return _asmConfiger;
        //    }
        //}

        /// <summary>
        /// 传入当前的程序集，以获取程序集配置对象
        /// </summary>
        /// <param name="callingAssembly"></param>
        /// <returns></returns>
        public static AssemblyConfiger GetConfiger(Assembly callingAssembly)
        {
            return new AssemblyConfiger(callingAssembly);
        }
    }
}
#endif
