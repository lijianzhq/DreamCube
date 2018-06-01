using System;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DreamCube.Framework.Utilities.Remoting
{
    /// <summary>
    /// Remoting的客户端类
    /// </summary>
    public sealed class Client
    {
        #region "私有字段"

        /// <summary>
        /// 当前客户端注册的信道对象
        /// </summary>
        private TcpChannel channel = null;

        /// <summary>
        /// 判断是否已经初始化了(0未初始化，1初始化）
        /// </summary>
        private Int32 hasInitial = 0;

        /// <summary>
        /// 单例模式，保存此对象的一个引用
        /// </summary>
        private static Client instance = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 获取当前的客户端对象实例引用
        /// </summary>
        public static Client Instance
        {
            get
            {
                if (instance != null) return instance;
                Client c = new Client();
                Interlocked.CompareExchange(ref instance, c, null);
                return instance;
            }
        }

        #endregion

        private Client()
        { }

        #region "公共实例方法"

        /// <summary>
        /// 初始化客户端
        /// </summary>
        public void InitialClient(Boolean throwException = false)
        {
            try
            {
                //应付多线程的情况
                if (Interlocked.CompareExchange(ref this.hasInitial, 1, 0) == 0)
                {
                    BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                    serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                    IDictionary properties = new Hashtable();
                    properties.Add("port", 0);
                    channel = new TcpChannel(properties, new BinaryClientFormatterSinkProvider(), serverProvider);
                    ChannelServices.RegisterChannel(channel, false);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
        }

        #endregion

        public void Dispose()
        {
            if (channel != null)
            {
                channel.StopListening(null);
                ChannelServices.UnregisterChannel(channel);
            }
        }
    }
}
