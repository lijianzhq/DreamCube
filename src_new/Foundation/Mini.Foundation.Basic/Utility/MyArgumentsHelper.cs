using System;
using System.Collections;
using System.IO;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 参数帮助类
    /// </summary>
    public static partial class MyArgumentsHelper
    {
        /// <summary>
        /// value为空，抛出空异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException">value为null</exception>
        public static void ThrowsIfNull(Object value, String parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// net20,net35调用String.IsNullOrEmpty，其他类库调用String.IsNullOrWhiteSpace，如果为空，则抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException">value为null或者为empty，.NET4.0以上会判断空白字符串，.net2.0只会判断空串""</exception>
        public static void ThrowsIfIsInvisibleString(String value, String parameterName)
        {
            if (MyString.IsInvisibleString(value))
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <exception cref="ArgumentNullException">value为null或者为空串""</exception>
        public static void ThrowsIfNullOrEmpty(String value, String parameterName)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="parameterName"></param>
        /// <param name="messageTemplate">{0}=parameterName</param>
        /// <exception cref="ArgumentNullException">collection参数为null</exception>
        /// <exception cref="ArgumentException">collection.count==0</exception>
        public static void ThrowsIfNullOrNoRecord(ICollection collection, String parameterName, String messageTemplate = "collection [{0}] does not contains one record!")
        {
            if (collection == null)
                throw new ArgumentNullException(parameterName);
            if (collection.Count == 0)
                throw new ArgumentException(String.Format(messageTemplate, parameterName));
        }

#if !NETSTANDARD1_0

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="messageTemplate">{0}=value</param>
        /// <exception cref="ArgumentNullException">value参数为null或者为空白串</exception>
        /// <exception cref="FileNotFoundException">value指定的文件不存在</exception>
        public static void ThrowsIfFileNotExist(String value, String parameterName, String messageTemplate = "file [{0}] does not exsit!")
        {
            if (MyString.IsInvisibleString(value))
                throw new ArgumentNullException(parameterName);
            if (!File.Exists(value))
                throw new FileNotFoundException(String.Format(messageTemplate, value));
        }
#endif
    }
}
