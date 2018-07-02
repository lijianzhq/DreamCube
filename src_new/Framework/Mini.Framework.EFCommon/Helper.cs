using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.LogService;
using System.Data.Entity.Validation;

namespace Mini.Framework.EFCommon
{
    public static class Helper
    {
        private static AssemblyConfiger _asmConfiger = null;
        internal static AssemblyConfiger AsmConfiger
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

        /// <summary>
        /// 收集ef实体校验错误信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String GetEntityValidationErrorsMsg(DbEntityValidationException ex, Func<DbEntityValidationResult, String> format = null)
        {
            var sb = new StringBuilder();
            foreach (var e in ex.EntityValidationErrors)
            {
                if (format != null) sb.Append(format(e));
                else
                {
                    foreach (var dbError in e.ValidationErrors)
                    {
                        sb.Append($"[{dbError.PropertyName}]:{dbError.ErrorMessage}");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static String GetGuidForDBCODE()
        {
            return Guid.NewGuid().ToString("N");
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
