using System;

namespace DreamCube.Framework.Utilities.Remoting
{

    /// <summary>
    /// Remoting的事件包装类(事件只有一个参数的)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventWrapper<T> : MarshalByRefObject
    {
        public event Action<T> TargetEvent;

        /// <summary>
        /// 触发事件的方法
        /// </summary>
        /// <param name="eventInValue"></param>
        public void FireEvent(T eventInValue)
        {
            if (TargetEvent != null)
                TargetEvent(eventInValue);
        }

        /// <summary>
        /// 控制此实例的生存周期策略
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            return null;
        }
    }

    /// <summary>
    /// Remoting的事件包装类(事件有两个参数的)
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class EventWrapper<T1, T2> : MarshalByRefObject
    {
        public event Action<T1, T2> TargetEvent;

        /// <summary>
        /// 触发事件的方法
        /// </summary>
        /// <param name="eventInValue"></param>
        public void FireEvent(T1 eventInValue1, T2 eventInValue2)
        {
            if (TargetEvent != null)
                TargetEvent(eventInValue1, eventInValue2);
        }

        /// <summary>
        /// 控制此实例的生存周期策略
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            return null;
        }
    }
}
