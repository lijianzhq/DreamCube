using System;
using System.IO;
using System.Net;

namespace DreamCube.Framework.Utilities.FTP
{
    /// <summary>
    /// Ftp服务器相关的帮助类
    /// </summary>
    public static class FtpHelper
    {
        /// <summary>
        /// 创建一个Ftp请求
        /// </summary>
        /// <param name="hostAddr"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FtpWebRequest CreateFtpRequest(String hostAddr, Int32 port, String userName, String password, String path)
        {
            Uri uri = new Uri(String.Format("ftp://{0}:{1}/{2}", hostAddr, port, path));
            FtpWebRequest request = WebRequest.Create(uri) as FtpWebRequest;
            request.KeepAlive = false;
            request.UseBinary = true;
            request.Credentials = new NetworkCredential(userName, password);
            return request;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hostAddr"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="path"></param>
        /// <param name="buffer"></param>
        /// <param name="append">是否追加，默认为false</param>
        /// <returns></returns>
        public static Boolean UploadFile(String hostAddr, Int32 port, String userName, String password, String path, Byte[] buffer, Boolean append = false)
        {
            FtpWebRequest request = null;
            request = CreateFtpRequest(hostAddr, port, userName, password, path);
            request.Method = append ? WebRequestMethods.Ftp.AppendFile : WebRequestMethods.Ftp.UploadFile;
            throw new NotImplementedException();
        }
    }
}
