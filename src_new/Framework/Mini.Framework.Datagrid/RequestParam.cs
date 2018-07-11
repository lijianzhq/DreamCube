using System;
using System.Web;
using System.Collections.Generic;

using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;
using Mini.Framework.Sdmap.Extension;

namespace Mini.Framework.Datagrid
{
    public class RequestParam
    {
        public String OpType { get; set; }
        public String GridCode { get; set; }
        public String FieldCODE { get; set; }
        public String ExportDataType { get; set; } //导出数据的方案，1：全部数据；2：当前页数据
        public String FileDownloadType { get; set; } //文件下载方式，1：返回路径下载；2：直接输出文件流下载
        public Int32 PageNumber { get; set; } = 1;//从1开始
        public Int32 PageSize { get; set; } = 1;//页面大小，最小为1
        public HttpContext Context { get; }

        public List<QueryParam> QueryParamList { get; set; } //查询参数

        public RequestParam(HttpContext context)
        {
            Context = context;
            OpType = context.Request.Params["OpType"];
            GridCode = context.Request.Params["GridCode"];
            FieldCODE = context.Request.Params["FieldCODE"];
            PageNumber = MyConvert.ToInt32(context.Request.Params["pageNumber"], -1);
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = MyConvert.ToInt32(context.Request.Params["pageSize"], -1);
            ExportDataType = context.Request.Params["ExportDataType"] ?? "1";
            FileDownloadType = context.Request.Params["FileDownloadType"] ?? "1";
            var queryParamStr = context.Request.Params["QueryParam"];
            if (!String.IsNullOrEmpty(queryParamStr))
            {
                //查询参数
                QueryParamList = MyJson.Deserialize<List<QueryParam>>(queryParamStr);
            }
        }
    }
}
