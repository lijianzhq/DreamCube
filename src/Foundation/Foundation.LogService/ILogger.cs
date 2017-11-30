using System;

namespace DreamCube.Foundation.LogService
{
    public interface ILogger
    {
        void LogDebug(string strMsg, Exception objExp = null);
        void LogDebugFormat(string strFormat, params object[] arrObjs);
        void LogError(string strMsg, Exception objExp = null);
        void LogErrorFormat(string strFormat, params object[] arrObjs);
        void LogFataFormat(string strFormat, params object[] arrObjs);
        void LogFatal(string strMsg, Exception objExp = null);
        void LogInfo(string strMsg, Exception objExp = null);
        void LogInfo(string strMsg, Exception objExp, bool argDate);
        void LogInfoFormat(string strFormat, params object[] arrObjs);
        void LogWarn(string strMsg, Exception objExp = null);
        void LogWarnFormat(string strFormat, params object[] arrObjs);
    }
}
