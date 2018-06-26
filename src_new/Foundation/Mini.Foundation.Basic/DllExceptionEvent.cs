using System;
using System.Reflection;

namespace Mini.Foundation.Basic
{
    public static class DllExceptionEvent
    {
        /// <summary>
        /// 异常回调的事件
        /// 参数：
        /// in Assembly callingAssembly：调用的assembly
        /// in Type sourceType：发生异常的类型
        /// in Exception ex：异常对象
        /// out Boolean：如果返回true，则不再调用委托连注册的方法（这种方案避免了很多dll都注册了这个事件，一个异常被多个调用方处理）
        /// </summary>
        public static event Func<Assembly, Type, Exception, Boolean> ExceptionEvent;

        public static Boolean TryFireExceptionEvent(Assembly callingAssembly, Type sourceType, Exception ex)
        {
            if (ExceptionEvent != null)
            {
                foreach (Delegate dg in ExceptionEvent.GetInvocationList())
                {
                    Boolean result = (dg as Func<Assembly, Type, Exception, Boolean>)(callingAssembly, sourceType, ex);
                    if (result) break;
                }
                return true;
            }
            return false;
        }
    }
}
