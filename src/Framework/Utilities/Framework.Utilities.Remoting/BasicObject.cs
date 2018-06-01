using System;
using System.Net.Sockets;
using System.Reflection;

//自定义
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.Remoting
{
    /// <summary>
    /// Remoting的基础对象；所有Remoting对象继承于此对象，而不要直接继承于MarshalByRefObject对象
    /// </summary>
    public class BasicObject : MarshalByRefObject
    {
        /// <summary>
        /// 服务器端调用此方法来触发事件(异步调用，适合不需要参数调用返回值和广播的方式）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        public void FireEventAsynch(Delegate method, params Object[] args)
        {
            if (method == null) return;
            //服务器端触发某个事件后，遍历所有此委托以通知关联的客户端
            foreach (Delegate item in method.GetInvocationList())
            {
                try
                {
                    Action a = new Action(() =>
                    {
                        try
                        {
                            item.DynamicInvoke(args);
                        }
                        catch (MemberAccessException)
                        {
                            Delegate.Remove(method, item);
                        }
                        catch (TargetInvocationException ex)
                        {
                            Delegate.Remove(method, item);
                        }
                        catch (SocketException)
                        {
                            Delegate.Remove(method, item);
                        }
                    });
                    a.BeginInvoke(null, null);
                }
                catch (MemberAccessException)
                {
                    Delegate.Remove(method, item);
                }
                catch (TargetInvocationException)
                {
                    Delegate.Remove(method, item);
                }
                catch (SocketException)
                {
                    Delegate.Remove(method, item);
                }
            }
        }

        /// <summary>
        /// 服务器端调用此方法来触发事件(调用方法成功返回true，调用方法失败返回false）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        public Boolean FireEventSynch(Delegate method, params Object[] args)
        {
            if (method == null) return false;
            //服务器端触发某个事件后，遍历所有此委托以通知关联的客户端
            foreach (Delegate item in method.GetInvocationList())
            {
                try
                {
                    item.DynamicInvoke(args);
                    return true;
                }
                catch (MemberAccessException ex)
                {
                    Delegate.Remove(method, item);
                    return false;
                }
                catch (TargetInvocationException ex)
                {
                    Delegate.Remove(method, item);
                    return false;
                }
                catch (SocketException ex)
                {
                    Delegate.Remove(method, item);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 重写对象生命周期的方法；由于有事件的触发，所以确保对象永不过期
        /// </summary>
        /// <returns></returns>
        public override Object InitializeLifetimeService()
        {
            return null;
        }
    }
}
