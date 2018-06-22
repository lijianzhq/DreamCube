using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;

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
            //在此处写入您的处理程序实现。
            var result = new FileSaveRespParams()
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
            try
            {
                if (optype == "save") //保存文件数据
                {
                    result = SaveFile(context);
                }
                else if (optype == "merge") //合并文件
                {
                    result = MergeFile(context);
                }

                //保存数据库记录
                if (result.Status && (!result.Chunked || optype == "merge"))
                    result.FileCode = SaveDBRecord(context, result.FileSavePath);
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

        String SaveDBRecord(HttpContext context, String fileFullName)
        {
            var rqParam = new RequestParams(context);
            var file = new DBService.UploadFile()
            {
                RefTableCode = rqParam.RefTableCode,
                RefTableName = rqParam.RefTableName,
                BarCode = rqParam.BarCode,
                SavePath = MyString.RightOf(fileFullName.Replace("/", "\\"), context.Server.MapPath("~").Replace("/", "\\")),
                CODE = DBService.DB.GetGuid(),
                FileName = rqParam.fileName,
                CreateBy = rqParam.userid,
                LastUpdateBy = rqParam.userid
            };
            DBService.DB.SaveUploadFileRecord(file);
            return file.CODE;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        FileSaveRespParams MergeFile(HttpContext context)
        {
            //获得参数
            var rqParam = new RequestParams(context);
            var result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            String folderName = MyString.LastLeftOf(rqParam.fileSavePath.Replace("\\", "/"), "/");
            if (!String.IsNullOrEmpty(folderName) && Directory.Exists(folderName))
            {
                String fileFullName = $"{folderName}{Path.GetExtension(rqParam.fileName)}";
                using (var file = File.Create(fileFullName))
                {
                    var files = Directory.GetFiles(folderName).OrderBy(it => Convert.ToInt32(Path.GetFileNameWithoutExtension(it)));
                    //校验分片数是否一致
                    if (files.Count() != rqParam.chunks) return result;
                    foreach (String tempFile in files)
                    {
                        var bytes = File.ReadAllBytes(tempFile);
                        file.Write(bytes, 0, bytes.Length);
                    }
                    file.Flush();
                }
                result.FileSavePath = fileFullName;
                Directory.Delete(folderName, true);
                result.Status = true;
            }
            else
            {
                String msg = $"merge file params:[{MyJson.Serialize(rqParam)}] faild, directory can not find!";
                Log.Root.LogInfo(msg);
                result.Message = msg;
            }
            return result;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        FileSaveRespParams SaveFile(HttpContext context)
        {
            var result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            //获得参数
            var rqParam = new RequestParams(context);
            String fileFullName = Helper.GetFileName(rqParam.guid, rqParam.fileid, context.Request.Files[0].FileName, rqParam.chunks, rqParam.chunk);
            result.Chunked = rqParam.chunks > 0;
            //返回文件存放路径
            result.FileSavePath = fileFullName;
            context.Request.Files[0].SaveAs(fileFullName);
            result.Status = true;
            return result;
        }

        #endregion
    }
}
