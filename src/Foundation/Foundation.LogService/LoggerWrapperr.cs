using System;
using System.Configuration;
using System.Diagnostics;

namespace DreamCube.Foundation.Log
{
    /// <summary>
    /// 自定义的logger包装器
    /// </summary>
    public class LoggerWrapper
    {
        #region constructor

        private LoggerWrapper(ILogger iLogger)
        {
            Debug.Assert(null != iLogger);
            m_iLogger = iLogger;
        }

        #endregion

        #region function for creating

        public static LoggerWrapper Create(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            return new LoggerWrapper(LogWrapper.Log4netWrapper.Create(name));
        }

        #endregion

        #region method

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message)
        {
            m_iLogger.Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void LogDebug(string message, string key)
        {
            m_iLogger.Log(message, key, LogLevel.Debug);
        }

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public void LogDebug(string message, Exception exp)
        {
            m_iLogger.Log(message, LogLevel.Debug, exp);
        }

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="exp"></param>
        public void LogDebug(Exception exp)
        {
            m_iLogger.Log("", LogLevel.Debug, exp);
        }

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="exp"></param>
        public void LogDebug(string message, string key, Exception exp)
        {
            m_iLogger.Log(message, key, LogLevel.Debug, exp);
        }

        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的。 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogDebugFormat(string format, params object[] args)
        {
            m_iLogger.LogFormat(format, LogLevel.Debug, args);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            m_iLogger.Log(message, LogLevel.Info);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void LogInfo(string message, string key)
        {
            m_iLogger.Log(message, key, LogLevel.Info);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public void LogInfo(string message, Exception exp)
        {
            m_iLogger.Log(message, LogLevel.Info, exp);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="exp"></param>
        public void LogInfo(Exception exp)
        {
            m_iLogger.Log("", LogLevel.Info, exp);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="exp"></param>
        public void LogInfo(string message, string key, Exception exp)
        {
            m_iLogger.Log(message, key, LogLevel.Info, exp);
        }

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程。 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogInfoFormat(string format, params object[] args)
        {
            m_iLogger.LogFormat(format, LogLevel.Info, args);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="message"></param>
        public void LogWarn(string message)
        {
            m_iLogger.Log(message, LogLevel.Warn);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void LogWarn(string message, string key)
        {
            m_iLogger.Log(message, key, LogLevel.Warn);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public void LogWarn(string message, Exception exp)
        {
            m_iLogger.Log(message, LogLevel.Warn, exp);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="exp"></param>
        public void LogWarn(Exception exp)
        {
            m_iLogger.Log("", LogLevel.Warn, exp);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="exp"></param>
        public void LogWarn(string message, string key, Exception exp)
        {
            m_iLogger.Log(message, key, LogLevel.Warn, exp);
        }

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogWarnFormat(string format, params object[] args)
        {
            m_iLogger.LogFormat(format, LogLevel.Warn, args);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            m_iLogger.Log(message, LogLevel.Error);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void LogError(string message, string key)
        {
            m_iLogger.Log(message, key, LogLevel.Error);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public void LogError(string message, Exception exp)
        {
            m_iLogger.Log(message, LogLevel.Error, exp);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="exp"></param>
        public void LogError(Exception exp)
        {
            m_iLogger.Log("", LogLevel.Error, exp);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="exp"></param>
        public void LogError(string message, string key, Exception exp)
        {
            m_iLogger.Log(message, key, LogLevel.Error, exp);
        }

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogErrorFormat(string format, params object[] args)
        {
            m_iLogger.LogFormat(format, LogLevel.Error, args);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="message"></param>
        public void LogFatal(string message)
        {
            m_iLogger.Log(message, LogLevel.Fatal);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public void LogFatal(string message, string key)
        {
            m_iLogger.Log(message, key, LogLevel.Fatal);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public void LogFatal(string message, Exception exp)
        {
            m_iLogger.Log(message, LogLevel.Fatal, exp);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="exp"></param>
        public void LogFatal(Exception exp)
        {
            m_iLogger.Log("", LogLevel.Fatal, exp);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="exp"></param>
        public void LogFatal(string message, string key, Exception exp)
        {
            m_iLogger.Log(message, key, LogLevel.Fatal, exp);
        }

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogFatalFormat(string format, params object[] args)
        {
            m_iLogger.LogFormat(format, LogLevel.Fatal, args);
        }

        #endregion

        #region field

        private ILogger m_iLogger = null;

        #endregion
    }
}
