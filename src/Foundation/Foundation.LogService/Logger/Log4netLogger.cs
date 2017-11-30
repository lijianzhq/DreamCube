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

using DreamCube.Foundation.TraceService;

namespace DreamCube.Foundation.LogService.Logger
{
    public class Log4netLogger : ILogger
    {
        #region constructor

        static Log4netLogger()
        {
            try
            {
                //先读取自定义的字段
                string cfgFile = GetLogConfigFile();
                XmlDocument doc = GetLogConfigXmlDoc(cfgFile);
                XmlNode encryptNode = doc.DocumentElement.SelectSingleNode(s_encryptNode);
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

                //配置log4net模块
                FileInfo cfgInfo = new FileInfo(cfgFile);
                XmlConfigurator.Configure(cfgInfo);
                m_bIsOpen = true;
                doc.RemoveAll();
                doc = null;
            }
            catch (System.Exception ex)
            {
                MyTrace.TraceError("Failed to startup the log4net service with error {0}", ex.Message);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iLog">log4net对象实例</param>
        /// <param name="argName">logger的name值</param>
        private Log4netLogger(ILog iLog, String argName)
        {
            Debug.Assert(null != iLog && Opened && !string.IsNullOrEmpty(argName));
            m_iLog = iLog;
            try
            {
                //收集filters
                XmlDocument doc = GetLogConfigXmlDoc();
                XmlNode refNode = doc.DocumentElement.SelectSingleNode(string.Format(s_appendRefFormat, argName));

                if (null != refNode)
                {
                    XmlAttribute refAttri = refNode.Attributes[s_refAttri];
                    if (null != refAttri && !string.IsNullOrEmpty(refAttri.Value))
                    {
                        XmlNodeList listFilterNodes = doc.DocumentElement.SelectNodes(string.Format(s_filterFormat, refAttri.Value));
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
                                    MyTrace.TraceWarning("The filter [{0}] is duplicate,ex:{1}", valueAttri.Value, ex.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MyTrace.TraceError("Failed to startup the log4net service with error {0}", ex.Message);
                return;
            }
        }

        #endregion

        #region public method

        public static ILogger Create(String name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name) && Opened);
            return new Log4netLogger(log4net.LogManager.GetLogger(name), name) as ILogger;
        }

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
        private static String GetLogConfigFile()
        {
            try
            {
                String cfgFilePath = ConfigurationManager.AppSettings["LogConfigFilePath"];
                cfgFilePath = String.IsNullOrEmpty(cfgFilePath) ? @"config\logconfig.xml" : cfgFilePath;
                String cfgFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFilePath);
                return cfgFileFullPath;
            }
            catch (Exception ex)
            {
                MyTrace.TraceError("GetLogConfigFile() error:{0}", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 获取日志配置信息
        /// </summary>
        /// <returns></returns>
        private static XmlDocument GetLogConfigXmlDoc(string cfgFilePath = "")
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (String.IsNullOrEmpty(cfgFilePath))
                    cfgFilePath = GetLogConfigFile();
                //doc.Load(GetLogConfigFile());
                doc.Load(cfgFilePath);
                return doc;
            }
            catch (Exception ex)
            {
                MyTrace.TraceError("GetLogConfigXmlDoc() error:{0}", ex.Message);
            }
            return null;
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
