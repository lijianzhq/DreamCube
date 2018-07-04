using FluentFTP;
using Mini.Foundation.Basic.Utility;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace Mini.Framework.WebUploader
{
    class FtpParam
    {
        public String UserName;
        public String Password;
        public String Host;
        public String FileUrl;

        public virtual String GetFtpFillFullPathWinthoutCredential()
        {
            return String.Format("ftp://{0}{1}", Host, FileUrl);
        }

        public virtual String GetFtpFillFullPath()
        {
            return String.Format("ftp://{0}{1}{2}",
                String.IsNullOrEmpty(UserName) ? "" : (UserName + (String.IsNullOrEmpty(Password) ? "" : ":" + Password) + "@"),
                Host,
                FileUrl);
        }
    }

    class FtpFileWorker : InWebFileWorker
    {
        protected override RespParams DoSaveFile(string savePath, HttpContext context)
        {
            var result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            //获得参数
            var rqParam = new RequestParams(context);
            result.Chunked = rqParam.chunks > 0;
            if (result.Chunked) //如果是分片，则先在缓存目录缓存下来
            {
                var cacheSavePath = Helper.AsmConfiger.ConfigFileReader.AppSettings("CachePath");
                return Helper.GetFileWorker(cacheSavePath).ProcessRequest(cacheSavePath, context);
            }
            else
            {
                //非分片传输，直接保存文件到ftp上
                String fileFullName = GetFileName(savePath, rqParam);
                result.FileSavePath = SaveFileToFTP(fileFullName, context);
            }
            result.Status = true;
            return result;
        }

        protected override Stream GetFileStreamBySavePath(HttpContext context, String fileSavePath)
        {
            var ftpParam = GetFtpParam(fileSavePath);
            FtpClient conn = null;
            Stream ostream = null;
            try
            {
                conn = new FtpClient(ftpParam.Host);
                conn.Credentials = new NetworkCredential(ftpParam.UserName, ftpParam.Password);
                conn.CreateDirectory(MyString.LastLeftOf(ftpParam.FileUrl, "/"));
                ostream = conn.OpenRead(ftpParam.FileUrl);
                return new FtpFileStream(conn, ostream);
            }
            catch (Exception)
            {
                if (ostream != null) ostream.Dispose();
                if (conn != null) conn.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 获取合并文件的流（新的文件流，以待写入文件数据）
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="fileFullName"></param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected override Stream GetMergeFileStream(String savePath, ref String fileFullName, RequestParams rqParam)
        {
            String tempFileFullName = fileFullName;
            String fileName = Path.GetFileName(tempFileFullName);
            String fileFullPath = FormatPath(savePath + "/" + fileName, rqParam);
            var ftpParam = GetFtpParam(fileFullPath);
            fileFullName = ftpParam.GetFtpFillFullPathWinthoutCredential();
            FtpClient conn = null;
            Stream ostream = null;
            try
            {
                conn = new FtpClient(ftpParam.Host);
                conn.Credentials = new NetworkCredential(ftpParam.UserName, ftpParam.Password);
                conn.CreateDirectory(MyString.LastLeftOf(ftpParam.FileUrl, "/"));
                ostream = conn.OpenWrite(ftpParam.FileUrl);
                return new FtpFileStream(conn, ostream);
            }
            catch (Exception)
            {
                if (ostream != null) ostream.Dispose();
                if (conn != null) conn.Dispose();
                throw;
            }
        }

        protected virtual String SaveFileToFTP(String fileFullName, HttpContext context)
        {
            var ftpParam = GetFtpParam(fileFullName);
            using (FtpClient conn = new FtpClient(ftpParam.Host))
            {
                conn.Credentials = new NetworkCredential(ftpParam.UserName, ftpParam.Password);
                String folder = MyString.LastLeftOf(ftpParam.FileUrl, "/");
                if (!String.IsNullOrWhiteSpace(folder))
                    conn.CreateDirectory(folder);
                using (Stream ostream = conn.OpenWrite(ftpParam.FileUrl))
                {
                    var buffer = new Byte[1024 * 1024];
                    Int32 read = 0;
                    do
                    {
                        read = context.Request.Files[0].InputStream.Read(buffer, 0, buffer.Length);
                        if (read > 0)
                            ostream.Write(buffer, 0, read);
                    } while (read > 0);
                }
            }
            return ftpParam.GetFtpFillFullPath();
        }

        /// <summary>
        /// 根据path，解析出相关的参数
        /// ftp://username:password@192.168.0.1:21/profile/11.txt
        /// Host:192.168.0.1:21
        /// UserName:username
        /// Password:password
        /// FileUrl:/profile/11.txt
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        protected virtual FtpParam GetFtpParam(String savePath)
        {
            var ftpParam = new FtpParam();
            String path = savePath.Substring("ftp://".Length);
            var pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            String[] serverDetails = pathParts[0].Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (serverDetails.Length == 2)
            {
                ftpParam.Host = serverDetails[1];
                String[] credentialDetails = serverDetails[0].Split(':');
                ftpParam.UserName = credentialDetails[0];
                if (credentialDetails.Length == 2)
                    ftpParam.Password = credentialDetails[1];
            }
            else
            {
                ftpParam.Host = serverDetails[0];
            }
            ftpParam.FileUrl = "/" + MyString.RightOf(path, "/").Replace("\\", "/");
            return ftpParam;
        }

        /// <summary>
        /// 负责解析保存路径模板的
        /// </summary>
        /// <param name="savePath">配置的保存路径模板</param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected override String FormatPath(String savePath, RequestParams rqParam)
        {
            String path = MyString.RightOf(savePath, "ftp://");
            var pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            String newPaths = String.Empty;
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) newPaths += "/";
                newPaths += FormatPathPartString(pathParts[i], rqParam);
            }
            return "ftp://" + newPaths;
        }

        protected override String FormatDBSaveFileFullPath(HttpContext context, String fileFullName)
        {
            return fileFullName;
        }
    }
}
