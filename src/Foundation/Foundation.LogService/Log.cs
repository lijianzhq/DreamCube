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

        public static LoggerWrapper Root
        {
            get { return GetLogger("root"); }
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
