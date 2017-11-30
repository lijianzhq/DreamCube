using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.LogService
{
    public static class Log
    {
        #region field

        private static Dictionary<String, ILogger> logDic = new Dictionary<String, ILogger>();
        private static Object locker = new Object();

        #endregion

        #region property

        private static ILogger s_rootLogger = null;
        public static ILogger Root
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
        public static ILogger GetLogger(String loggerName)
        {
            //这里可以采用IOC容器来优化，以后再进一步优化
            return Logger.Log4netLogger.Create(loggerName);
            //return LoggerWrapper.Create(loggerName);
        }

        #endregion
    }
}
