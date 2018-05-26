using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using log4net;
using log4net.Config;

using Mini.Foundation.TraceService;

namespace Mini.Foundation.LogService.Logger
{
    public class Log4netLogger : ILogger
    {
        ///// <summary>
        ///// 公开一个委托给外部定定制自己的config文件路径获取方案
        ///// 避免直接修改这个类（支持直接拷贝去用）
        ///// </summary>
        //public static Func<String> GetLogConfigFile;

        #region constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="argName">logger的name值</param>
        private Log4netLogger(String argName)
        {
            try
            {
                this.Initial(argName);
                //创建log4net的logger对象
                this.m_iLog = log4net.LogManager.GetLogger(argName);
            }
            catch (System.Exception ex)
            {
                Tracer.Instance.TraceError("Failed to startup the log4net service with error {0}", ex.Message);
                return;
            }
        }

        #endregion

        #region static method

        public static ILogger Create(String name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            return new Log4netLogger(name) as ILogger;
        }

        /// <summary>
        /// 外部提供获取日志文件的provider对象
        /// </summary>
        /// <param name="configFileProvider"></param>
        public static void RegisterLogConfigFileProvider(ILogerConfigFileProvider configFileProvider)
        {
            m_configFileProvider = configFileProvider;
        }

        #endregion

        #region ILogger method

        public void LogDebug(string strMsg, Exception objExp = null)
        {
            Debug.Assert(null != m_iLog);
            if (m_iLog.IsDebugEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Debug(strMsg);
                }
                else
                {
                    m_iLog.Debug(strMsg, objExp);
                }
            }
        }

        public void LogDebugFormat(string strFormat, params object[] arrObjs)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsDebugEnabled)
            {
                m_iLog.DebugFormat(strFormat, arrObjs);
            }
        }

        public void LogInfo(string strMsg, Exception objExp = null)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsInfoEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Info(strMsg);
                }
                else
                {
                    m_iLog.Info(strMsg, objExp);
                }
            }
        }

        public void LogInfo(string strMsg, Exception objExp, bool argDate)
        {
            Debug.Assert(null != m_iLog);

            if (argDate)
            {
                DateTime date = DateTime.Now;
                string strDateTime = string.Format(" {0:dd/MM/yyyy HH:mm:ss} ", date);
                strDateTime = strDateTime.Replace("-", "/");
                strMsg = strDateTime + strMsg;
            }

            if (m_iLog.IsInfoEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Info(strMsg);
                }
                else
                {
                    m_iLog.Info(strMsg, objExp);
                }
            }
        }

        public void LogInfoFormat(string strFormat, params object[] arrObjs)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsInfoEnabled)
            {
                m_iLog.InfoFormat(strFormat, arrObjs);
            }
        }

        public void LogWarn(string strMsg, Exception objExp = null)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsWarnEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Warn(strMsg);
                }
                else
                {
                    m_iLog.Warn(strMsg, objExp);
                }
            }
        }

        public void LogWarnFormat(string strFormat, params object[] arrObjs)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsWarnEnabled)
            {
                m_iLog.WarnFormat(strFormat, arrObjs);
            }
        }

        public void LogError(string strMsg, Exception objExp = null)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsErrorEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Error(strMsg);
                }
                else
                {
                    m_iLog.Error(strMsg, objExp);
                }
            }
        }

        public void LogErrorFormat(string strFormat, params object[] arrObjs)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsErrorEnabled)
            {
                m_iLog.ErrorFormat(strFormat, arrObjs);
            }
        }

        public void LogFatal(string strMsg, Exception objExp = null)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsFatalEnabled)
            {
                if (null == objExp)
                {
                    m_iLog.Fatal(strMsg);
                }
                else
                {
                    m_iLog.Fatal(strMsg, objExp);
                }
            }
        }

        public void LogFataFormat(string strFormat, params object[] arrObjs)
        {
            Debug.Assert(null != m_iLog);

            if (m_iLog.IsFatalEnabled)
            {
                m_iLog.FatalFormat(strFormat, arrObjs);
            }
        }

        #endregion
        
        #region private method

        /// <summary>
        /// 获取日志配置信息
        /// </summary>
        /// <returns></returns>
        protected XmlDocument GetLogConfigXmlDoc(string cfgFilePath = "")
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cfgFilePath);
                return doc;
            }
            catch (Exception ex)
            {
                Tracer.Instance.TraceError("GetLogConfigXmlDoc() error:{0}", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="argName"></param>
        protected void Initial(String argName)
        {
            try
            {
                //先读取自定义的字段
                string cfgFile = ConfigFileProvider.GetLogConfigFile();
                XmlDocument doc = GetLogConfigXmlDoc(cfgFile);
                InitEncryptConfig(doc);
                InitFiltersConfig(doc, argName);
                InitLog4netConfig(cfgFile);
                //配置log4net模块
                doc.RemoveAll();
                doc = null;
                m_bIsOpen = true;
            }
            catch (System.Exception ex)
            {
                Tracer.Instance.TraceError("Failed to startup the log4net service with error {0}", ex.Message);
            }
        }

        /// <summary>
        /// 初始化log4net
        /// </summary>
        /// <param name="configFile"></param>
        protected void InitLog4netConfig(String configFile)
        {
            FileInfo cfgInfo = new FileInfo(configFile);
            XmlConfigurator.Configure(cfgInfo);
        }

        /// <summary>
        /// 处理加密的配置
        /// </summary>
        /// <param name="configDoc"></param>
        protected void InitEncryptConfig(XmlDocument configDoc)
        {
            //处理加密的情况
            XmlNode encryptNode = configDoc.DocumentElement.SelectSingleNode(s_encryptNode);
            if (null != encryptNode)
            {
                XmlAttribute valueAttri = encryptNode.Attributes[s_valueAttri];
                if (null != valueAttri && !string.IsNullOrEmpty(valueAttri.Value))
                {
                    LoggerKeyHelper.EncryptLoggerAppenders = valueAttri.Value.Split(
                        new[] { '|' },
                        StringSplitOptions.RemoveEmptyEntries
                    );
                }
            }
        }

        /// <summary>
        /// 处理Filter的配置
        /// </summary>
        /// <param name="configDoc"></param>
        /// <param name="argName"></param>
        protected void InitFiltersConfig(XmlDocument configDoc, String argName)
        {
            XmlNode refNode = configDoc.DocumentElement.SelectSingleNode(string.Format(s_appendRefFormat, argName));
            if (null != refNode)
            {
                XmlAttribute refAttri = refNode.Attributes[s_refAttri];
                if (null != refAttri && !string.IsNullOrEmpty(refAttri.Value))
                {
                    XmlNodeList listFilterNodes = configDoc.DocumentElement.SelectNodes(string.Format(s_filterFormat, refAttri.Value));
                    if (0 != listFilterNodes.Count)
                    {
                        XmlAttribute valueAttri = null;
                        XmlAttribute enableAttri = null;
                        int temp = 0;
                        foreach (XmlNode filterNode in listFilterNodes)
                        {
                            valueAttri = filterNode.Attributes[s_valueAttri];
                            enableAttri = filterNode.Attributes[s_enableAttri];
                            if (null == valueAttri ||
                                string.IsNullOrEmpty(valueAttri.Value) ||
                                null == enableAttri ||
                                string.IsNullOrEmpty(enableAttri.Value))
                            {
                                continue;
                            }

                            if (!int.TryParse(enableAttri.Value, out temp))
                            {
                                continue;
                            }

                            try
                            {
                                Filters.Add(valueAttri.Value, 0 == temp ? false : true);
                            }
                            catch (System.Exception ex)
                            {
                                Tracer.Instance.TraceWarning("The filter [{0}] is duplicate,ex:{1}", valueAttri.Value, ex.Message);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region property

        /// <summary>
        /// 标识日志模块是否正确被打开
        /// </summary>
        public static bool Opened
        {
            get
            {
                return m_bIsOpen;
            }
        }

        /// <summary>
        /// 配置文件provider对象
        /// </summary>
        public static ILogerConfigFileProvider ConfigFileProvider
        {
            get
            {
                if (m_configFileProvider == null)
                    m_configFileProvider = new DefaultLogerConfigFileProvider();
                return m_configFileProvider;
            }
        }

        /// <summary>
        /// 自定义的过滤器
        /// </summary>
        public Dictionary<string, bool> Filters
        {
            get
            {
                if (null == m_dicFilter)
                {
                    m_dicFilter = new Dictionary<string, bool>();
                }
                return m_dicFilter;
            }
        }

        #endregion

        #region field

        private static ILogerConfigFileProvider m_configFileProvider = null;

        private ILog m_iLog = null;

        /// <summary>
        /// 标识日志模块是否正确被打开（创建对象的时候没有出错）
        /// </summary>
        private static bool m_bIsOpen = false;

        private object m_synLocker = new object();

        /// <summary>
        /// 自定义的过滤器
        /// </summary>
        private Dictionary<string, bool> m_dicFilter = null;

        /// <summary>
        /// 从app.config或者web.config文件的appsetting节点读取日志文件的路径（注意：是相对程序运行的路径）
        /// </summary>
        private const String s_appsettingKey = "LogConfigFilePath";

        /// <summary>
        /// 日志模块的配置文件名
        /// </summary>
        private const String s_configFileName = "logconfig.xml";

        private const string s_appendRefFormat = "logger[@name='{0}']/appender-ref";

        private const string s_filterFormat = "appender[@name='{0}']/filter/key";

        private const string s_refAttri = "ref";

        private const string s_valueAttri = "value";

        private const string s_enableAttri = "enable";

        private const string s_encryptNode = "EncryptLog";

        #endregion
    }
}
