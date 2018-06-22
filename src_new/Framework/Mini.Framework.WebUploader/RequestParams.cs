using Mini.Foundation.Basic.Utility;
using System;
using System.Web;

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
        public String RefTableName { get; set; }
        public String RefTableCode { get; set; }
        public String BarCode { get; set; }
        public String RmFCodes { get; set; }//移除的文件code值

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
            chunks = MyConvert.ToInt32(context.Request.Form["chunks"]);
            chunk = MyConvert.ToInt32(context.Request.Form["chunk"]);
        }
    }
}
