using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;

namespace Mini.Framework.WebUploader
{
    /// <summary>
    /// 保存文件
    /// </summary>
    public class FileSave : IHttpHandler
    {
        /// <summary>
        /// 您将需要在网站的 Web.config 文件中配置此处理程序 
        /// 并向 IIS 注册它，然后才能使用它。有关详细信息，
        /// 请参阅以下链接: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var result = new ResponseParams()
            {
                Status = true,
                Message = ""
            };
            context.Response.ContentType = "application/json";
            try
            {
                Int32 chunk = Convert.ToInt32(context.Request.Form["chunk"]);
                String fileName = Helper.GetFileName(context.Request["guid"],
                                        context.Request["id"],
                                        context.Request.Files[0].FileName,
                                        Convert.ToInt32(context.Request.Form["chunks"]),
                                        chunk);
                result.Chunked = chunk > 0;
                context.Request.Files[0].SaveAs(fileName);
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
                result.Status = false;
                result.Message = ex.Message;
            }
            finally
            {
                context.Response.Write(MyJson.Serialize(result));
                context.Response.End();
            }
        }

        ///// <summary>
        ///// 保存分片的文件 
        ///// （这里不能根据分片序号来作为最后合并文件的依据，客户端可以采用多进程来上传，这样不确保最后一个分片是最后上传的）
        ///// </summary>
        ///// <param name="context"></param>
        //protected virtual void SaveChunkFile(HttpContext context)
        //{
        //    String fileFullPath = Helper.GetFileName(context);
        //    using (var fs = new FileStream(fileFullPath, FileMode.Append, FileAccess.Write))
        //    {
        //        using (BinaryWriter bw = new BinaryWriter(fs))
        //        {
        //            //获得上传的分片数据流
        //            HttpPostedFile file = context.Request.Files[0];
        //            Stream stream = file.InputStream;
        //            BinaryReader br = new BinaryReader(stream);
        //            //将上传的分片追加到临时文件末尾
        //            bw.Write(br.ReadBytes((Int32)stream.Length));
        //        }
        //    }
        //}

        #endregion
    }
}
