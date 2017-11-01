using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Enums
{
    /// <summary>
    /// http头的值
    /// </summary>
    public enum HttpContentType
    {
        /// <summary>
        ///  ".txt"="text/plain"
        /// </summary>
        text_plain
    }

    public static class HttpContentTypeHelper
    {
        /// <summary>
        /// 文件后缀与contenttype的对应关系
        /// </summary>
        private static Dictionary<String, String> fileContentTypeMapper = new Dictionary<String, String>(new Objects.EqualityComparers.StringEqualityIgnoreCaseComparerGeneric());

        static HttpContentTypeHelper()
        {
            fileContentTypeMapper.Add(".ico", "image/x-icon");
            fileContentTypeMapper.Add(".jpeg", "image/jpeg");
            fileContentTypeMapper.Add(".jpg", "image/jpeg");
        }

        /// <summary>
        /// 根据文件后缀名获取contenttype
        /// </summary>
        /// <param name="fileExtension">前面必须加上点符号，例如：.jpg</param>
        /// <returns></returns>
        public static String GetContentTypeByFileExtension(String fileExtension)
        {
            return fileContentTypeMapper[fileExtension];
        }

        /// <summary>
        /// 根据枚举值获取ContentType实际的值
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static String GetContentTypeByEnmu(HttpContentType contentType)
        {
            return contentType.ToString().Replace("_", "/");
        }
    }
}
