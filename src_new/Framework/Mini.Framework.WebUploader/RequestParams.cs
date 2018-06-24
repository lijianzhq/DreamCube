using System;
using System.Web;
using System.Collections.Generic;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.Json;

namespace Mini.Framework.WebUploader
{
    class RequestParams
    {
        public String guid { get; set; }
        public String fileid { get; set; }
        public String fileName { get; set; }
        public String fileSavePath { get; set; }
        public String userid { get; set; }
        public String optype { get; set; }
        public Int32 chunks { get; set; }
        public Int32 chunk { get; set; }
        public String FileCode { get; set; } //对应文件数据库中的code值（用于下载的）
        public String RefTableName { get; set; }
        public String RefTableCode { get; set; }
        public String BarCode { get; set; }
        public String RmFCodes { get; set; }//移除的文件code值

        public Dictionary<String, String> SavePathParams { get; set; } //存放路径的参数模板

        public RequestParams(HttpContext context)
        {
            //获得参数
            guid = context.Request["guid"];
            fileid = context.Request["id"];
            fileName = context.Request["name"];
            fileSavePath = context.Request["fileSavePath"];
            userid = context.Request["uid"];
            optype = context.Request["optype"];//操作类型
            RefTableName = context.Request["RefTableName"];
            RefTableCode = context.Request["RefTableCode"];
            BarCode = context.Request["BarCode"];
            RmFCodes = context.Request["RmFCodes"];
            FileCode = context.Request["FileCode"];
            chunks = MyConvert.ToInt32(context.Request.Form["chunks"]);
            chunk = MyConvert.ToInt32(context.Request.Form["chunk"]);
            String savePathParamStr = context.Request["savePathParam"];
            if (!String.IsNullOrWhiteSpace(savePathParamStr))
                SavePathParams = MyJson.Deserialize<Dictionary<String, String>>(savePathParamStr);
        }
    }
}
