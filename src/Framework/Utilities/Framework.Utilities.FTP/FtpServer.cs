using System;
using System.IO;
using System.Net;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.FTP
{
    public class FtpServer
    {
        #region "私有字段"

        /// <summary>
        /// 文件的缓冲区大小
        /// </summary>
        private Int32 fileBufferLength = 4096;

        /// <summary>
        /// 用户名
        /// </summary>
        private String userName = String.Empty;

        /// <summary>
        /// 用户密码
        /// </summary>
        private String userPassword = String.Empty;

        /// <summary>
        /// ftp服务器的IP地址获取机器名
        /// </summary>
        private String hostAddr = String.Empty;

        /// <summary>
        /// ftp服务器通信端口号
        /// </summary>
        private Int32 port = 21;

        #endregion

        #region "属性"

        /// <summary>
        /// ftp服务器通信端口号
        /// </summary>
        public String HostAddr
        {
            get { return this.hostAddr; }
        }

        /// <summary>
        /// ftp服务器的IP地址获取机器名
        /// </summary>
        public Int32 Port
        {
            get { return this.port; }
        }

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public Int32 FileBufferLength
        {
            get { return this.fileBufferLength; }
            set { this.fileBufferLength = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName
        {
            get { return this.userName; }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        public String UserPassword
        {
            get { return this.userPassword; }
        }

        #endregion

        #region "构造函数"

        public FtpServer(String userName, String userPassword, String hostAddr = "127.0.0.1", Int32 port = 21)
        {
            this.userName = userName;
            this.userPassword = userPassword;
            this.hostAddr = hostAddr;
            this.port = port;
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public String CreateDir(String dirName)
        {
            FtpWebRequest ftpServer = this.CreateFtpWebRequest(dirName);
            if (ftpServer != null)
            {
                ftpServer.Credentials = new NetworkCredential(this.userName, this.userPassword);
                ftpServer.EnableSsl = false;
                ftpServer.KeepAlive = false;
                ftpServer.UseBinary = true;
                ftpServer.Method = WebRequestMethods.Ftp.MakeDirectory;
                try
                {
                    WebResponse response = ftpServer.GetResponse();
                    response.Close();
                }
                catch (Exception ex)
                {
                    MyLog.MakeLog(ex);
                }
            }
            return ftpServer.RequestUri.ToString();
        }

        /// <summary>
        /// 附件数据到ftp服务器上的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public String AppendFile(String fileName, Stream fileData)
        {
            FtpWebRequest ftpServer = this.CreateFtpWebRequest(fileName);
            if (ftpServer != null)
            {
                ftpServer.Credentials = new NetworkCredential(this.userName, this.userPassword);
                ftpServer.EnableSsl = false;
                ftpServer.KeepAlive = false;
                ftpServer.UseBinary = true;
                ftpServer.Method = WebRequestMethods.Ftp.AppendFile;
                ftpServer.ContentLength = fileData.Length;

                //读取字节流
                Stream ftpRequestStream = ftpServer.GetRequestStream();
                Byte[] buffer = new Byte[fileBufferLength];
                Int32 readDataLength = fileData.Read(buffer, 0, fileBufferLength);
                try
                {
                    while (readDataLength > 0)
                    {
                        ftpRequestStream.Write(buffer, 0, readDataLength);
                        readDataLength = fileData.Read(buffer, 0, fileBufferLength);
                    }
                }
                catch (Exception ex)
                {
                    MyLog.MakeLog(ex);
                }
                finally
                {
                    if (fileData != null)
                        fileData.Close();
                    if (ftpRequestStream != null)
                        ftpRequestStream.Close();
                }
            }
            return ftpServer.RequestUri.ToString();
        }

        /// <summary>
        /// 保存文件到Ftp服务器上
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns>返回该文件的ftp；uri路径</returns>
        public String SaveFile(String fileName, Byte[] fileData)
        {
            FtpWebRequest ftpServer = this.CreateFtpWebRequest(fileName);
            if (ftpServer != null)
            {
                ftpServer.Credentials = new NetworkCredential(this.userName, this.userPassword);
                ftpServer.EnableSsl = false;
                ftpServer.KeepAlive = false;
                ftpServer.UseBinary = true;
                ftpServer.Method = WebRequestMethods.Ftp.UploadFile;
                ftpServer.ContentLength = fileData.Length;
                //读取字节流
                Stream ftpRequestStream = ftpServer.GetRequestStream();
                try
                {
                    ftpRequestStream.Write(fileData, 0, fileData.Length);
                }
                catch (Exception ex)
                {
                    MyLog.MakeLog(ex);
                }
                finally
                {
                    if (ftpRequestStream != null)
                        ftpRequestStream.Close();
                }
            }
            return ftpServer.RequestUri.ToString();
        }

        /// <summary>
        /// 保存文件到Ftp服务器上
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns>返回该文件的ftp；uri路径</returns>
        public String SaveFile(String fileName, Stream fileData)
        {
            FtpWebRequest ftpServer = this.CreateFtpWebRequest(fileName);
            if (ftpServer != null)
            {
                ftpServer.Credentials = new NetworkCredential(this.userName, this.userPassword);
                ftpServer.EnableSsl = false;
                ftpServer.KeepAlive = false;
                ftpServer.UseBinary = true;
                ftpServer.Method = WebRequestMethods.Ftp.UploadFile;
                ftpServer.ContentLength = fileData.Length;

                //读取字节流
                Stream ftpRequestStream = ftpServer.GetRequestStream();
                Byte[] buffer = new Byte[fileBufferLength];
                Int32 readDataLength = fileData.Read(buffer, 0, fileBufferLength);
                try
                {
                    while (readDataLength > 0)
                    {
                        ftpRequestStream.Write(buffer, 0, readDataLength);
                        readDataLength = fileData.Read(buffer, 0, fileBufferLength);
                    }
                }
                catch (Exception ex)
                {
                    MyLog.MakeLog(ex);
                }
                finally
                {
                    if (fileData != null)
                        fileData.Close();
                    if (ftpRequestStream != null)
                        ftpRequestStream.Close();
                }
            }
            return ftpServer.RequestUri.ToString();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">
        /// 注意：此参数是指对应此ftp目录下的路径；例如：请求文件：ftp://127.0.0.1/files/aa.txt；则此参数代表”aa.txt“
        /// </param>
        /// <returns></returns>
        public Byte[] DownLoadFile(String filePath)
        {
            FtpWebRequest ftp = this.CreateFtpWebRequest(filePath);
            ftp.Method = WebRequestMethods.Ftp.DownloadFile;
            WebResponse response = ftp.GetResponse();
            using (Stream s = response.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Int32 readCount = 0;
                    Byte[] buffer = new Byte[fileBufferLength];
                    do
                    {
                        readCount = s.Read(buffer, readCount, fileBufferLength);
                        if (readCount > 0)
                            s.Write(buffer, 0, readCount);
                    } while (readCount > 0);
                    Byte[] fileData = new Byte[s.Length];
                    s.Seek(0, SeekOrigin.Begin);
                    s.Read(fileData, 0, (Int32)s.Length);
                    return fileData;
                }
            }
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 创建一个ftp请求
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private FtpWebRequest CreateFtpWebRequest(String path)
        {
            Uri uri = new Uri(String.Format("ftp://{0}:{1}/{2}", this.hostAddr, this.port, path));
            FtpWebRequest request = FtpWebRequest.Create(uri) as FtpWebRequest;
            if (request != null)
            {
                request.Credentials = new NetworkCredential(this.userName, this.userPassword);
                request.EnableSsl = false;
                request.KeepAlive = false;
                request.UseBinary = true;
            }
            return request;
        }

        #endregion
    }
}
