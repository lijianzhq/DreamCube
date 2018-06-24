using System;

namespace Mini.Foundation.Basic
{
    public static class DllExceptionEvent
    {
        /// <summary>
        /// 异常回调的事件
        /// </summary>
        public static event Action<Type, Exception> ExceptionEvent;

        public static Boolean TryFireExceptionEvent(Type sourceType, Exception ex)
        {
            if (ExceptionEvent != null)
            {
                ExceptionEvent(sourceType, ex);
                return true;
            }
            return false;
        }
    }
}
