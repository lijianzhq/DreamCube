using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    public class FileTransfer : IHttpHandler
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
            RespParams result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
                FileCode = ""
            };
            //设置响应内容格式
            context.Response.ContentType = "application/json";

            //获得参数
            String optype = context.Request["optype"];//操作类型
            String savepath = context.Request["savepath"];//保存目录
            try
            {
                if (optype == "test")
                {
                    result.Status = true;
                    result.Message = "It is a message for test!";
                }
                else
                {
                    savepath = String.IsNullOrWhiteSpace(savepath) ? Helper.AsmConfiger.ConfigFileReader.AppSettings("File_SavePath") : savepath;
                    var worker = Helper.GetFileWorker(savepath);
                    result = worker.ProcessRequest(savepath, context);
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
                result.Status = false;
                result.Message = ex.Message;
                if (ex.InnerException != null)
                    result.Message += ex.InnerException.Message;
            }
            finally
            {
                if (result != null && !result.Status) context.Response.StatusCode = 500;
                if (context.Response.IsClientConnected && context.Response.OutputStream.CanWrite)
                {
                    if (result != null)
                        context.Response.Write(MyJson.Serialize(result));
                    context.Response.End();
                }
            }
        }

        #endregion
    }
}
