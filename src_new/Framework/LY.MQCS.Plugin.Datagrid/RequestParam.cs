using System;
using System.Web;

namespace LY.MQCS.Plugin.Datagrid
{
    public class RequestParam
    {
        public String OpType { get; set; }
        public String GridCode { get; set; }
        public String FieldCODE { get; set; }

        public RequestParam(HttpContext context)
        {
            OpType = context.Request.Params["OpType"];
            GridCode = context.Request.Params["GridCode"];
            FieldCODE = context.Request.Params["FieldCODE"];
        }
    }
}
