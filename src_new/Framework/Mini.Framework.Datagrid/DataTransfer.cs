using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;

using Mini.Foundation.Json;
using Mini.Foundation.LogService;
using Mini.Foundation.Basic.CommonObj;
using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using Mini.Framework.Database.Oracle;
using LY.MQCS.Plugin.DBService;
using LY.MQCS.Plugin.DBService.PQ;

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
        /// 加载数据
        /// </summary>
        /// <param name="rqParam">请求</param>
        /// <param name="rspParam">响应参数</param>
        /// <returns></returns>
        protected void LoadData(RequestParam rqParam, ref ExecResult rspParam)
        {
            //where rn > @STARTINDEX and rn <= @ENDINDEX
            var grid = GetGrid(rqParam);
            if (grid == null) return;
            //var table = LYDBCommon.ExecuteDataTable(grid.SQL);
            DataTable table = null;
            if (rqParam.PageNumber > 0 && rqParam.PageSize > 0)
            {
                table
            }
            rspParam.OpData = new { rows = table, recordCount = 100 }; ;
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
            var grid = new EasyUIDataGrid()
            {
                SQL = "select * from V1_ALL_QUES",
                LoadDataAfterInital = true,
                Columns = new List<EasyUIDataGridColumn>() {
                        new EasyUIDataGridColumn(){
                              Config = "{ field: 'DISTRI_GROUP_CODE', title: '分发目标班组', width: 180 }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = @"{ field: 'EFFECT_CONF_SCHE', title: '效果确认方案', width: 180,
                                            formatter: function(value, row) {
                                                    return row.DISTRI_GROUP_CODE;
                                            },
                                            editor:{
                                                    type: 'combobox',
                                                    options:{
                                                        valueField: 'EFFECT_CONF_SCHE',
                                                        textField: 'NAME',
                                                        required: true
                                                     }
                                             }
                                         }",
                               EditDataSQL = "select LOOKUP_VALUE_CODE EFFECT_CONF_SCHE,LOOKUP_VALUE_NAME EFFECT_CONF_SCHE_NAME from T_DB_DB_LOOKUP_VALUE where LOOKUP_TYPE_CODE='118_EFFECT_CONF_SCHE'"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '问题点', title: '问题点', width: 180,sortable:true }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '发生日期', title: '发生日期', width: 60, align: 'right' }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '品情来源', title: '品情来源', width: 80 }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '车型', title: '车型', width: 80 }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '车号', title: '车号', width: 80 }"
                        },
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '不良区分', title: '不良区分', width: 80 }"
                        }
                        ,
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '发生工站', title: '发生工站', width: 80 }"
                        }
                        ,
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '责任人', title: '责任人', width: 80 }"
                        }
                        ,
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '不良件数', title: '不良件数', width: 80 }"
                        }
                        ,
                        new EasyUIDataGridColumn(){
                              Config = "{ field: '是否再发', title: '是否再发', width: 80 }"
                        }
                    }
            };
            return grid;
            //using (var db = new DB_PQ())
            //{

            //}
        }

        #endregion
    }
}
