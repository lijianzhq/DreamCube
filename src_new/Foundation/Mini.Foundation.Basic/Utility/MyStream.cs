using System;
using System.IO;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 流的相关公共方法
    /// </summary>
    public static class MyStream
    {
        /// <summary>
        /// 把不可查找的流读取成内存流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static MemoryStream ReadToMemoryStream(Stream stream)
        {
            if (stream == null) return null;
            var buffer = new Byte[1024 * 100];
            var read = 0;
            var ms = new MemoryStream();
            do
            {
                read = stream.Read(buffer, 0, buffer.Length);
                if (read > 0) ms.Write(buffer, 0, read);
            } while (read > 0);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
