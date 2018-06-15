using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;

namespace Mini.Framework.WebUploader
{
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
            try
            {
                context.Response.ContentType = "application/json";
                //如果进行了分片
                if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
                {
                    //取得chunk和chunks
                    int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                    int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                    Log.Root.LogDebug($"chunks:{chunks}");
                    //根据GUID创建用该GUID命名的临时文件夹
                    string folder = context.Server.MapPath("~/1/" + context.Request["guid"] + "/");
                    string path = folder + chunk;

                    //建立临时传输文件夹
                    if (!Directory.Exists(Path.GetDirectoryName(folder)))
                        Directory.CreateDirectory(folder);

                    FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                    BinaryWriter AddWriter = new BinaryWriter(addFile);
                    //获得上传的分片数据流
                    HttpPostedFile file = context.Request.Files[0];
                    Stream stream = file.InputStream;

                    BinaryReader TempReader = new BinaryReader(stream);
                    //将上传的分片追加到临时文件末尾
                    AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                    //关闭BinaryReader文件阅读器
                    TempReader.Close();
                    stream.Close();
                    AddWriter.Close();
                    addFile.Close();

                    TempReader.Dispose();
                    stream.Dispose();
                    AddWriter.Dispose();
                    addFile.Dispose();

                    context.Response.Write("{\"chunked\" : true, \"hasError\" : true, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
                }
                else
                {
                    //没有分片直接保存
                    //String filePath = 
                    context.Request.Files[0].SaveAs(context.Server.MapPath("~/1/" + DateTime.Now.ToFileTime() + Path.GetExtension(context.Request.Files[0].FileName)));
                    context.Response.Write("{\"chunked\" : false, \"hasError\" : false}");
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
            }
            finally
            {
                context.Response.End();
            }
        }

        #endregion
    }
}
