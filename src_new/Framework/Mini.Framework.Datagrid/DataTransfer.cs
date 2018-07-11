﻿using System;
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
using Mini.Framework.ResourceCommon;
using Mini.Framework.Database;
using Mini.Framework.Database.Oracle;
using Mini.Framework.Database.DefaultProviders;

namespace Mini.Framework.Datagrid
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
                rspParam.OpResult = false;
                rspParam.Message = ExceptionHelper.FormatException(ex);
                Log.Root.LogError(rspParam.Message, ex);
            }
            finally
            {
                //if (!rspParam.OpResult) context.Response.StatusCode = 500;
                if (context.Response.IsClientConnected && context.Response.OutputStream.CanWrite)
                    context.Response.Write(MyJson.Serialize(rspParam, MyJson.DefaultJsonSettings));
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
            var path = rqParam.Context.Server.MapPath(AsmConfigerHelper.GetConfiger().ConfigFileReader.AppSettings("tempFileCachePath"));
            Directory.CreateDirectory(path);
            var fileName = $"{Guid.NewGuid().ToString("N")}.xlsx";

            var fileFullPath = Path.Combine(path, fileName);
            DataTable table = null;
            Int32 recordCount = 0;

            //找不到对应的表格
            var grid = GetGrid(rqParam);
            if (grid == null)
            {
                rspParam.OpResult = false;
                rspParam.Message = StrResourceManager.Current.GetString("ConfigError", Helper.RESOURCE_NS);
                return;
            }

            //到处列为空
            var exportCols = grid.Columns.Where(it => it.Export);
            if (exportCols == null && exportCols.Count() == 0)
            {
                rspParam.OpResult = false;
                rspParam.Message = StrResourceManager.Current.GetString("NoExportColumn", Helper.RESOURCE_NS);
                return;
            }
            var fieldNameList = new List<String>();
            foreach (var col in exportCols)
            {
                fieldNameList.Add(GetFieldName(col.Config));
            }
            var newSql = $"select {MyEnumerable.JoinEx(fieldNameList, ",")} from ({grid.SQL})";
            LoadDataTable(rqParam, ref rspParam, ref recordCount, ref table, rqParam.ExportDataType == "1", newSql);
            //查询无数据
            if (table == null)
            {
                rspParam.OpResult = false;
                rspParam.Message = StrResourceManager.Current.GetString("NoData", Helper.RESOURCE_NS);
                return;
            }
            var excel = new ExcelWrapper(fileFullPath);
            excel.SetSheetData(table);
            excel.Save();
            if (rqParam.FileDownloadType == "1")
            {
                rspParam.OpData = MyString.RightOf(fileFullPath, rqParam.Context.Server.MapPath("~"));
                rspParam.OpResult = true;
            }
            else
            {
                MyWebUtility.WriteFileToClient(rqParam.Context, fileFullPath);
            }
        }

        /// <summary>
        /// 根据配置的json字符串，获取fieldname值
        /// 例如：{field:"name",title:"姓名"}，返回name值
        /// </summary>
        /// <param name="colConfigString"></param>
        /// <returns></returns>
        protected String GetFieldName(String colConfigString)
        {
            var reader = new JsonPropertyReader(colConfigString);
            String field = String.Empty;
            var dic = reader.Read();
            if (dic.TryGetValue("field", out field))
            {
                //去掉单引号或者是双引号
                field = field.Substring(1, field.Length - 2);
                //然后重命名列名
                String title = String.Empty;
                dic.TryGetValue("title", out title);
                if (!String.IsNullOrEmpty(title))
                {
                    title = title.Substring(1, title.Length - 2);
                    field = $"{field} as {title}";
                }
            }
            return field;
        }

        /// <summary>
        /// </summary>
        /// <param name="rqParam"></param>
        /// <param name="rspParam"></param>
        /// <param name="recordCount"></param>
        /// <param name="table"></param>
        /// <param name="loadAllData"></param>
        /// <param name="querySql">查询语句，如果指定了，则使用这个指定的sql，否则就直接用datagrid配置的sql</param>
        /// <returns></returns>
        protected void LoadDataTable(RequestParam rqParam, ref ExecResult rspParam, ref Int32 recordCount, ref DataTable table, Boolean loadAllData = true, String querySql = "")
        {
            table = null;
            recordCount = 0;
            var grid = GetGrid(rqParam);
            if (grid == null) return;
            if (String.IsNullOrWhiteSpace(querySql))
                querySql = grid.SQL;
            var db = Helper.CreateDBObj();
            using (var ctx = db.BeginExecuteContext(true))
            {
                if (!loadAllData && rqParam.PageNumber > 0 && rqParam.PageSize > 0)
                {
                    table = ctx.GetDataTableBySqlTemplate(querySql, rqParam.PageSize, rqParam.PageNumber, rqParam.QueryParamList);
                }
                else
                {
                    table = ctx.GetDataTableBySqlTemplate(querySql, rqParam.QueryParamList);
                }
                recordCount = ctx.GetRecordCountBySqlTemplate(querySql, rqParam.QueryParamList);
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
            rspParam.OpResult = table != null;
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
                if (grid.Columns[i].Config.IndexOf($"'{rqParam.FieldCODE}'", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    sql = grid.Columns[i].EditDataSQL;
                    break;
                }
            }
            if (String.IsNullOrEmpty(sql)) return;

            var db = Helper.CreateDBObj();
            DataTable table = null;
            using (var ctx = db.BeginExecuteContext(true))
            {
                table = ctx.GetDataTableBySqlTemplate(sql, rqParam.QueryParamList);
            }
            rspParam.OpData = table;
            rspParam.OpResult = table != null;
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

        protected DBService.Datagrid GetGrid(RequestParam rqParam)
        {
            using (var db = Helper.CreateEFDB())
            {
                //var grid = db.Datagrids.Include("Columns").Where(it => it.CODE == rqParam.GridCode && it.IsEnable == true).SingleOrDefault();
                //if (grid != null && grid.Columns != null)
                //    grid.Columns = grid.Columns.OrderBy(it => it.OrderNO).ToList();
                //return grid;

                //Where查询条件中用到的关联实体不需要Include
                var cols = db.DatagridCols.Include("Datagrid")
                                          .Where(it => it.GridCODE == rqParam.GridCode && it.IsEnable == true && it.Datagrid.IsEnable == true)
                                 .OrderBy(it => it.OrderNO)
                                 .ToList();
                if (cols != null && cols.Count > 0)
                {
                    var grid = cols[0].Datagrid;
                    grid.Columns = cols;
                    return grid;
                }
                return null;
            }
        }

        #endregion
    }
}
