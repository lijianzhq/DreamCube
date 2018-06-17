using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.LogService;

namespace Mini.Framework.WebUploader
{
    public class MergeFile : IHttpHandler
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
            String guid = context.Request["guid"];
            String id = context.Request["id"];
            String fileName = context.Request["fileName"];
            Log.Root.LogInfo($"start merge file [guid:{guid}],[id:{id}],[fileName:{fileName}]!");
            try
            {
                String filePath = context.Server.MapPath(Helper.AsmConfiger.AppSettings("File_SavePath"));
                String folderName = Helper.GetFileName(guid, id, fileName);
                if (Directory.Exists(folderName))
                {
                    String fileFullName = $"{folderName}{Path.GetExtension(fileName)}";
                    using (var file = File.Create(fileFullName))
                    {
                        var files = Directory.GetFiles(folderName).OrderBy(it => Convert.ToInt32(Path.GetFileNameWithoutExtension(it)));
                        foreach (String tempFile in files)
                        {
                            var bytes = File.ReadAllBytes(tempFile);
                            file.Write(bytes, 0, bytes.Length);
                        }
                        file.Flush();
                    }
                    Directory.Delete(folderName, true);
                }
                else
                {
                    Log.Root.LogInfo($"merge file [guid:{guid}],[id:{id}],[fileName:{fileName}] faild, directory can not find!");
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
            }
        }

        #endregion
    }
}
