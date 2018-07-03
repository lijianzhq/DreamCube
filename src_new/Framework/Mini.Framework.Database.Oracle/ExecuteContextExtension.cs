using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.Database.Oracle
{
    /// <summary>
    /// 执行上下文
    /// </summary>
    public static class ExecuteContextExtension
    {
        public static DataTable GetDataTable(this IExecute ctx, String commandText, Int32 pageSize, Int32 pageIndex, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandText, nameof(commandText));
            var start = (pageIndex - 1) * pageSize + 1;
            var end = pageIndex * pageSize;
            var pageSql = $"select * from(select t1.*,rownum rn from ({commandText}) t1) where rn>={start} and rn<={end}";
            //var pageSql = $"select * from(select t1.*,rownum rn from ({commandText}) t1) where rn>=:STARTINDEX and rn<=:ENDINDEX";
            //var pageDBParam = new DbParameter[] { ctx.DB.CreateParameter("STARTINDEX", start), ctx.DB.CreateParameter("ENDINDEX", end) };
            //if (dbParams == null)
            //{
            //    dbParams = pageDBParam;
            //}
            //else
            //{
            //    var paramList = new List<DbParameter>();
            //    paramList.AddRange(dbParams);
            //    paramList.AddRange(pageDBParam);
            //    dbParams = paramList.ToArray();
            //}
            return ctx.GetDataTable(pageSql, dbParams, commandType);
        }

        public static Int32 GetRecordCount(this IExecute ctx, String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandText, nameof(commandText));
            var pageSql = $"select count(*) from ({commandText}) t1";
            return MyConvert.ToInt32(ctx.ExecuteScalar(pageSql, dbParams, commandType), 0);
        }
    }
}
