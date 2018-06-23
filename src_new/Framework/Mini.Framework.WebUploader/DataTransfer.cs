using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            RespParams result = new DataTransferRespParams()
            {
                Status = false,
                Message = "",
            };
            //设置响应内容格式
            context.Response.ContentType = "application/json";

            //获得参数
            String optype = context.Request["optype"];//操作类型
            try
            {
                if (optype == "test")
                {
                    result.Status = true;
                    result.Message = "It is a message for test!";
                }
                if (optype == "loadFile") //加载文件列表
                {
                    result = LoadFiles(context);
                }
                else if (optype == "deleteFile") //删除文件
                {
                    result = DeleteFiles(context);
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

        RespParams DeleteFiles(HttpContext context)
        {
            var rqParam = new RequestParams(context);
            var result = new DataTransferRespParams()
            {
                Status = true,
                Message = "",
            };
            String[] codes = MyJson.Deserialize<String[]>(rqParam.RmFCodes);
            using (var db = new DBService.DB())
            {
                foreach (var code in codes)
                {
                    //不能做级联删除操作（操作历史表不能删除）
                    //var f = db.UploadFiles.Include(nameof(DBService.UploadFile.OpHistory))
                    //                      .Where(it => it.CODE == code).SingleOrDefault();
                    //if (f != null) db.UploadFiles.Remove(f);
                    var f = db.UploadFiles.Include(nameof(DBService.UploadFile.OpHistory))
                                          .Where(it => it.CODE == code).SingleOrDefault();
                    if (f != null)
                    {
                        f.Status = DBService.FileStatus.Delete;
                        f.OpHistory = f.OpHistory == null ? new List<DBService.UploadFileOpHistory>() : f.OpHistory;
                        f.OpHistory.Add(new DBService.UploadFileOpHistory()
                        {
                            OpType = DBService.FileOpType.Delete
                        });
                    }
                }
                db.SaveChanges();
            }
            return result;
        }

        RespParams LoadFiles(HttpContext context)
        {
            var rqParam = new RequestParams(context);
            var result = new DataTransferRespParams()
            {
                Status = true,
                Message = "",
            };
            using (var db = new DBService.DB())
            {
                var emptyQ = db.UploadFiles.Where(it => it.Status == DBService.FileStatus.Normal);
                var whereQ = emptyQ;
                if (!String.IsNullOrWhiteSpace(rqParam.RefTableName))
                    whereQ = whereQ.Where(it => it.RefTableName == rqParam.RefTableName);

                if (!String.IsNullOrWhiteSpace(rqParam.RefTableCode))
                    whereQ = whereQ.Where(it => it.RefTableCode == rqParam.RefTableCode);

                if (!String.IsNullOrWhiteSpace(rqParam.BarCode))
                    whereQ = whereQ.Where(it => it.BarCode == rqParam.BarCode);
                if (whereQ != emptyQ)
                {
                    //需要排除导航属性
                    result.Result = whereQ.ToList()
                        .Select(it => MyDynamicObj.GetDynamicObj(it, null, p => p.Name == nameof(it.OpHistory)))
                        .ToList();
                }
            }
            return result;
        }

        #endregion


    }
}
