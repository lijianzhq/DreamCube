using System;
using System.Collections.Generic;

namespace Mini.Foundation.LogService
{
    /// <summary>
    /// 日志密钥帮助类
    /// </summary>
    public static class LoggerKeyHelper
    {
        private static string _key;

        /// <summary>
        /// 加密LoggerAppender列表
        /// </summary>
        public static IEnumerable<string> EncryptLoggerAppenders { get; set; }

        /// <summary>
        /// 密钥(长度必须8位以上)
        /// </summary>
        public static string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_key))
                {
                    _key = "1a2b3c4d";
                }
                return _key;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 8)
                {
                    throw new Exception("The Key min length is 8.");
                }
                _key = value;
            }
        }
    }
}
