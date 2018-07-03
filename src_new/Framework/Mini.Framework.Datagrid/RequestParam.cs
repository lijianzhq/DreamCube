using System;
using System.Web;

using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.Datagrid
{
    public class RequestParam
    {
        public String OpType { get; set; }
        public String GridCode { get; set; }
        public String FieldCODE { get; set; }
        public Int32 PageNumber { get; set; } = 1;//从1开始
        public Int32 PageSize { get; set; } = 1;//页面大小，最小为1

        public RequestParam(HttpContext context)
        {
            OpType = context.Request.Params["OpType"];
            GridCode = context.Request.Params["GridCode"];
            FieldCODE = context.Request.Params["FieldCODE"];
            PageNumber = MyConvert.ToInt32(context.Request.Params["pageNumber"], -1);
            PageSize = MyConvert.ToInt32(context.Request.Params["pageSize"], -1);
        }
    }
}
