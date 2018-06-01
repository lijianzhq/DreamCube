using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

//自定义
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.Remoting
{
    /// <summary>
    /// Remoting服务器端的类
    /// </summary>
    public class Server
    {
        #region "字段"

        private static Dictionary<String, MarshalByRefObject> remotingObjs = new Dictionary<String, MarshalByRefObject>();

        /// <summary>
        /// 当前服务器端注册的信道对象
        /// </summary>
        private TcpChannel channel = null;

        /// <summary>
        /// 获取当前Remoting服务器对象实例
        /// </summary>
        private static Server instance = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 服务器侦听的端口号
        /// </summary>
        public Int32 Port
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前的服务器对象实例
        /// </summary>
        public static Server Instance
        {
            get
            {
                if (instance != null) return instance;
                Server c = new Server();
                Interlocked.CompareExchange(ref instance, c, null);
                return instance;
            }
        }

        #endregion

        private Server() { }

        /// <summary>
        /// 根据远程对象的类型注册远程通信对象
        /// 注意：通过此方法注册远程对象，注册端无法获取实际远程对象的实例引用，无法触发事件了
        /// </summary>
        /// <param name="remotingObjType"></param>
        /// <param name="objectUri">为remoting对象指定一个唯一的URI访问接口；如果不传入此参数，则默认选择第一个参数的类型的类名</param>
        /// <param name="mode"></param>
        public void RegisterRemotingObjByType(Type remotingObjType,
                                              String objectUri,
                                              WellKnownObjectMode mode = WellKnownObjectMode.Singleton)
        {
            if (String.IsNullOrEmpty(objectUri))
                objectUri = MyString.RightOfLast(remotingObjType.ToString(), ".");
            RemotingConfiguration.RegisterWellKnownServiceType(remotingObjType, objectUri, mode);
        }

        /// <summary>
        /// 根据远程对象的类型注册远程通信对象
        /// </summary>
        /// <param name="remotingObjType"></param>
        /// <param name="objectUri">为remoting对象指定一个唯一的URI访问接口；如果不传入此参数，则默认选择第一个参数的类型的类名</param>
        /// <param name="mode"></param>
        public void RegisterRemotingObjByInstance(MarshalByRefObject remoteObj,
                                                  String objectUri = null,
                                                  WellKnownObjectMode mode = WellKnownObjectMode.Singleton)
        {
            if (String.IsNullOrEmpty(objectUri))
                objectUri = MyString.RightOfLast(remoteObj.GetType().ToString(), ".");
            RemotingServices.Marshal(remoteObj, objectUri);
        }

        /// <summary>
        /// 初始化服务器端对象
        /// </summary>
        /// <param name="port">remoting服务的端口号</param>
        /// <param name="channelName">remoting服务的通道名</param>
        public void InitialServer(Int32 port, String channelName)
        {
            Hashtable properties = new Hashtable();
            properties["port"] = port;
            properties["name"] = channelName;
            this.Port = port;
            this.InitialServer(properties);
        }

        /// <summary>
        /// 初始化服务器端对象
        /// </summary>
        /// <param name="chanelProperties"></param>
        public void InitialServer(IDictionary chanelProperties)
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            channel = new TcpChannel(chanelProperties, new BinaryClientFormatterSinkProvider(), serverProvider);
            Int32 port = 0;
            if (MyObject.TryToInt32(chanelProperties["port"], out port)) this.Port = port;
            ChannelServices.RegisterChannel(channel, false);

            //本地接受完整的异常信息
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        /// <summary>
        /// 创建一个远程对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objUri"></param>
        /// <param name="protocol"></param>
        /// <param name="ip"></param>
        /// <param name="autoCache">是否自动把远程对象缓存起来</param>
        /// <returns></returns>
        public T CreateRemotingObj<T>(Int32 port, String protocol = "tcp", String ip = "127.0.0.1", Boolean autoCache = true)
            where T : MarshalByRefObject
        {
            Type t = typeof(T);
            if (remotingObjs.ContainsKey(t.FullName)) return remotingObjs[t.FullName] as T;
            String uriTemplate = "{0}://{1}:{2}/{3}";

            //GetObject() 是采用服务器端激活
            //CreateInstance() 采用客户端激活方式激活
            T obj = Activator.GetObject(t, String.Format(uriTemplate, protocol, ip, port, MyString.RightOfLast(t.ToString(), "."))) as T;
            if (autoCache) MyDictionary.TryAdd(remotingObjs, t.FullName, obj);
            return obj;
        }

        /// <summary>
        /// 创建一个远程对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objUri"></param>
        /// <returns></returns>
        public T CreateRemotingObj<T>(String objUri, Boolean autoCache = true)
            where T : MarshalByRefObject
        {
            Type t = typeof(T);
            if (remotingObjs.ContainsKey(t.FullName)) return remotingObjs[t.FullName] as T;
            T obj = Activator.GetObject(t, objUri) as T;
            if (autoCache) MyDictionary.TryAdd(remotingObjs, t.FullName, obj);
            return obj;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Stop()
        {
            if (channel != null)
            {
                channel.StopListening(null);
                ChannelServices.UnregisterChannel(channel);
            }
        }
    }
}
