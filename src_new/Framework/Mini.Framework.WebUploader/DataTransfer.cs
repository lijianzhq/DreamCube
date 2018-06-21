using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    public class DataTransfer : IHttpHandler
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
            //在此处写入您的处理程序实现。
            var result = new ResponseParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            //设置响应内容格式
            context.Response.ContentType = "application/json";

            //获得参数
            String guid = context.Request["guid"];
            String id = context.Request["id"];
            String fileName = context.Request["fileName"];
            String fileSavePath = context.Request["fileSavePath"];
            String userid = context.Request["uid"];
            String optype = context.Request["optype"];//操作类型
            Int32 chunks = MyConvert.ToInt32(context.Request.Form["chunks"]);
            Int32 chunk = MyConvert.ToInt32(context.Request.Form["chunk"]);
            try
            {
                String fileFullName = String.Empty;
                if (optype == "save") //保存文件数据
                {
                    fileFullName = Helper.GetFileName(guid, id, context.Request.Files[0].FileName, chunks, chunk);
                    result.Chunked = chunk > 0;
                    //返回文件存放路径
                    result.FileSavePath = fileFullName;
                    context.Request.Files[0].SaveAs(fileFullName);
                    result.Status = true;
                }
                else if (optype == "merge") //合并文件
                {
                    String folderName = MyString.LastLeftOf(fileSavePath.Replace("\\", "/"), "/");
                    if (!String.IsNullOrEmpty(folderName) && Directory.Exists(folderName))
                    {
                        fileFullName = $"{folderName}{Path.GetExtension(fileName)}";
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
                        result.Status = true;
                    }
                    else
                    {
                        String msg = $"merge file [guid:{guid}],[id:{id}],[fileSavePath:{fileSavePath}] faild, directory can not find!";
                        Log.Root.LogInfo(msg);
                        result.Message = msg;
                    }
                }

                //保存数据库记录
                if (result.Status && (!result.Chunked || optype == "merge"))
                {
                    DBService.DB.SaveUploadFileRecord(new DBService.UploadFile()
                    {
                        SavePath = MyString.RightOf(fileFullName.Replace("/", "\\"), context.Server.MapPath("~").Replace("/", "\\")),
                        CODE = DBService.DB.GetGuid(),
                        FileName = MyString.LastRightOf(fileFullName.Replace("\\", "/"), "/"),
                        CreateBy = userid,
                        LastUpdateBy = userid
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
                result.Status = false;
                result.Message = ex.Message;
            }
            finally
            {
                if (!result.Status) context.Response.StatusCode = 500;
                context.Response.Write(MyJson.Serialize(result));
                context.Response.End();
            }
        }

        #endregion
    }
}
