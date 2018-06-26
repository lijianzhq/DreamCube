using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.LogService;

namespace Mini.Framework.EFCommon
{
    static class Helper
    {
        private static AssemblyConfiger _asmConfiger = null;
        public static AssemblyConfiger AsmConfiger
        {
            get
            {
                if (_asmConfiger == null)
                {
                    Mini.Foundation.Basic.DllExceptionEvent.ExceptionEvent += DllExceptionEvent_ExceptionEvent; ;
                    _asmConfiger = new AssemblyConfiger();
                }
                return _asmConfiger;
            }
        }

        private static Boolean DllExceptionEvent_ExceptionEvent(Assembly callingAsembly, Type arg1, Exception arg2)
        {
            if (callingAsembly == typeof(Helper).Assembly)
            {
                Log.Root.LogError($"callingAsembly:{callingAsembly.ToString()},ex type[{arg1.FullName}]ex:", arg2);
                return true;
            }
            return false;
        }
    }
}
