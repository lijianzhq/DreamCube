#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 这个类不是给外部调用的，是给外部拷贝到各自的类库去使用的。
    /// 建议不要修改命名空间，什么都不要改，直接拷贝到自己的类库用。
    /// </summary>
    static class AsmConfigerHelper
    {
        static AssemblyConfiger _asmConfiger = null;
        static Object _locker = new Object();

        internal static AssemblyConfiger AsmConfiger
        {
            get
            {
                if (_asmConfiger == null)
                {
                    lock (_locker)
                    {
                        if (_asmConfiger == null)
                            _asmConfiger = new AssemblyConfiger();
                    }
                }
                return _asmConfiger;
            }
        }
    }
}
#endif
