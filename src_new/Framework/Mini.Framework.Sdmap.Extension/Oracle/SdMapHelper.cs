using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using sdmap.Compiler;

namespace Mini.Framework.Sdmap.Extension.Oracle
{
    public static class SdMapHelper
    {
        /// <summary>
        /// 根据sql语句模板查询
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="commandTextTemplate"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataTable GetDataTableBySqlTemplate(this IExecute ctx, String commandTextTemplate, Int32 pageSize, Int32 pageIndex, IList<QueryParam> inputParamList, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandTextTemplate, nameof(commandTextTemplate));
            var compiler = SdmapCompiler.Instance;
            var start = (pageIndex - 1) * pageSize + 1;
            var end = pageIndex * pageSize;
            var pageSql = $"select * from(select t1.*,rownum rn from ({commandTextTemplate}) t1) where rn>={start} and rn<={end}";

            pageSql = $"sql v1{{{pageSql}}}";
            compiler.AddSourceCode(pageSql);

            var templateParams = new Dictionary<String, Object>();
            var dbParams = new List<DbParameter>();
            foreach (var p in inputParamList)
            {
                if (p.Type == QueryParamType.SqlParam)
                    dbParams.Add(ctx.DB.CreateParameter(p.Name, p.Value));
                templateParams.Add(p.Name, p.Value);
            }
            var sql = compiler.Emit("v1", templateParams);

            return ctx.GetDataTable(sql, dbParams.ToArray(), commandType);
        }
    }
}
