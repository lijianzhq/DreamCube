using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.LogService
{
    public static class Log
    {
        #region field

        private static Dictionary<String, LoggerWrapper> logDic = new Dictionary<String, LoggerWrapper>();
        private static Object locker = new Object();

        #endregion

        #region property

        private static LoggerWrapper s_rootLogger = null;
        public static LoggerWrapper Root
        {
            get
            {
                if (s_rootLogger == null)
                    s_rootLogger = GetLogger("root");
                return s_rootLogger;
            }
        }

        #endregion

        #region method

        /// <summary>
        /// 根据logger文件中配置的名字获取对应的log对象
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static LoggerWrapper GetLogger(String loggerName)
        {
            return LoggerWrapper.Create(loggerName);
        }

        #endregion
    }
}
