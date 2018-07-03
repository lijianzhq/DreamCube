using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;
using System.Linq;
using System.IO;

using Mini.Foundation.Json;
using Mini.Foundation.LogService;
using Mini.Foundation.Basic.CommonObj;
using Mini.Foundation.Basic.Utility;
using Mini.Foundation.Office;
using Mini.Framework.Sdmap.Extension;
using Mini.Framework.Sdmap.Extension.Oracle;
using Mini.Framework.Database;
using Mini.Framework.Database.Oracle;
using LY.MQCS.Plugin.DBService;
using LY.MQCS.Plugin.DBService.PQ;

namespace Mini.Framework.Datagrid
{
    public class DataTransfer : IHttpHandler
    {
        static AssemblyConfiger _asmConfiger = null;
        static AssemblyConfiger AsmConfiger
        {
            get
            {
                if (_asmConfiger == null) _asmConfiger = new AssemblyConfiger();
                return _asmConfiger;
            }
        }

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
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            context.Response.Charset = "utf-8";

            var rspParam = new ExecResult()
            {
                OpResult = true,
                OpData = null,
            };
            try
            {
                var rqParam = new RequestParam(context);
                if (rqParam.OpType == "init")
                {
                    InitDatagrid(rqParam, ref rspParam);
                }
                else if (rqParam.OpType == "loadData")
                {
                    LoadData(rqParam, ref rspParam);
                }
                else if (rqParam.OpType == "loadColumnEditData")
                {
                    LoadColumnEditData(rqParam, ref rspParam);
                }
                else if (rqParam.OpType == "exportData")
                {
                    ExportData(rqParam, ref rspParam);
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("", ex);
                rspParam.OpResult = false;
                rspParam.Message = ExceptionHelper.FormatException(ex);
            }
            finally
            {
                if (!rspParam.OpResult) context.Response.StatusCode = 500;
                if (context.Response.IsClientConnected && context.Response.OutputStream.CanWrite)
                    context.Response.Write(MyJson.Serialize(rspParam));
                context.Response.End();
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="rqParam"></param>
        /// <param name="rspParam"></param>
        protected void ExportData(RequestParam rqParam, ref ExecResult rspParam)
        {
            var path = AsmConfiger.ConfigFileReader.AppSettings("tempFileCachePath");
            var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.xlsx";
            var fileFullPath = Path.Combine(rqParam.FieldCODE);
            DataTable table = null;
            Int32 recordCount = 0;
            LoadDataTable(rqParam, ref rspParam, ref recordCount, ref table, true);
            if (table == null)
            {
                rspParam.OpResult = false;
                rspParam.Message = "查询无数据！";
                return;
            }
            var excel = new ExcelWrapper(fileFullPath);
            excel.AddSheet(table);
            excel.Save();
        }

        /// <summary>
        /// </summary>
        /// <param name="rqParam"></param>
        /// <param name="rspParam"></param>
        /// <param name="recordCount"></param>
        /// <param name="table"></param>
        /// <param name="loadAllData"></param>
        /// <returns></returns>
        protected void LoadDataTable(RequestParam rqParam, ref ExecResult rspParam, ref Int32 recordCount, ref DataTable table, Boolean loadAllData = true)
        {
            table = null;
            recordCount = 0;
            var grid = GetGrid(rqParam);
            if (grid == null) return;
            var db = LYDBCommon.GetDB();
            using (var ctx = db.BeginExecuteContext())
            {
                if (!loadAllData && rqParam.PageNumber > 0 && rqParam.PageSize > 0)
                {
                    table = ctx.GetDataTableBySqlTemplate(grid.SQL, rqParam.PageSize, rqParam.PageNumber, rqParam.QueryParamList);
                }
                else
                {
                    table = ctx.GetDataTableBySqlTemplate(grid.SQL, rqParam.QueryParamList);
                }
                recordCount = ctx.GetRecordCountBySqlTemplate(grid.SQL, rqParam.QueryParamList);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="rqParam">请求</param>
        /// <param name="rspParam">响应参数</param>
        /// <returns></returns>
        protected void LoadData(RequestParam rqParam, ref ExecResult rspParam)
        {
            DataTable table = null;
            Int32 recordCount = 0;
            LoadDataTable(rqParam, ref rspParam, ref recordCount, ref table, false);
            rspParam.OpData = new { rows = table, recordCount }; ;
            rspParam.OpResult = table != null && table.Rows.Count > 0;
        }

        /// <summary>
        /// 加载列编辑数据（下拉框的数据）
        /// </summary>
        /// <param name="rqParam">请求</param>
        /// <param name="rspParam">响应参数</param>
        /// <returns></returns>
        protected void LoadColumnEditData(RequestParam rqParam, ref ExecResult rspParam)
        {
            if (String.IsNullOrWhiteSpace(rqParam.FieldCODE)) return;
            var grid = GetGrid(rqParam);
            if (grid == null || grid.Columns == null || grid.Columns.Count == 0) return;
            var sql = String.Empty;
            for (var i = 0; i < grid.Columns.Count; i++)
            {
                //var dic = MyJson.Deserialize<Dictionary<String, String>>(grid.Columns[i].Config);
                //if (dic.ContainsKey(rqParam.FieldCODE)) sql = grid.Columns[i].EditDataSQL;
                if (grid.Columns[i].Config.IndexOf($"'{rqParam.FieldCODE}'", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    sql = grid.Columns[i].EditDataSQL;
                    break;
                }
            }
            if (String.IsNullOrEmpty(sql)) return;
            var table = LYDBCommon.ExecuteDataTable(sql);
            rspParam.OpData = table;
            rspParam.OpResult = table != null && table.Rows.Count > 0;
        }

        /// <summary>
        /// 首次加载datagridtable的时候调用此方法
        /// </summary>
        /// <param name="rqParam">请求</param>
        /// <param name="rspParam">响应参数</param>
        /// <returns></returns>
        protected void InitDatagrid(RequestParam rqParam, ref ExecResult rspParam)
        {
            //在此处写入您的处理程序实现。
            var grid = GetGrid(rqParam);
            rspParam.OpData = grid;
            rspParam.OpResult = true;
        }

        protected EasyUIDataGrid GetGrid(RequestParam rqParam)
        {
            using (var db = LYDBCommon.GetDB_PQ())
            {
                return db.EasyUIDataGrid.Include("Columns").Where(it => it.CODE == rqParam.GridCode).SingleOrDefault();
            }
        }

        #endregion
    }
}
