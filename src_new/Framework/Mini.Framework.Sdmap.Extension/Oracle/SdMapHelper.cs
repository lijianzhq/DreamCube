using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using sdmap.Compiler;

namespace Mini.Framework.Sdmap.Extension.Oracle
{
    public static partial class SdMapHelper
    {
        const String SQLCODE_FOR_GETDATATABLEBYSQLTEMPLATE = "F2EA1EB1BDB249FDBA0EF53F705367C8";
        const String SQLCODE_FOR_GETRECORDCOUNTBYSQLTEMPLATE = "F2EA1EB1BDB249FDBA0EF53F705367C9";

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
            var newSql = $"select * from(select t1.*,rownum rn from ({commandTextTemplate}) t1) where rn>=:STARTINDEX and rn<=:ENDINDEX";
            var pageDBParam = new DbParameter[] { ctx.DB.CreateParameter("STARTINDEX", start), ctx.DB.CreateParameter("ENDINDEX", end) };
            newSql = $"sql {SQLCODE_FOR_GETDATATABLEBYSQLTEMPLATE}{{{newSql}}}";
            compiler.AddSourceCode(newSql);

            var templateParams = new Dictionary<String, Object>();
            var dbParams = new List<DbParameter>();
            if (inputParamList != null)
            {
                foreach (var p in inputParamList)
                {
                    if (p.Type == QueryParamType.SqlParam)
                        dbParams.Add(ctx.DB.CreateParameter(p.Name, p.Value));
                    templateParams.Add(p.Name, p.Value);
                }
            }
            var sql = compiler.Emit(SQLCODE_FOR_GETDATATABLEBYSQLTEMPLATE, templateParams);
            dbParams.AddRange(pageDBParam);

            return ctx.GetDataTable(sql, dbParams.ToArray(), commandType);
        }

        public static Int32 GetRecordCountBySqlTemplate(this IExecute ctx, String commandTextTemplate, IList<QueryParam> inputParamList, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandTextTemplate, nameof(commandTextTemplate));
            var newSql = $"select count(*) from ({commandTextTemplate}) t1";
            newSql = $"sql {SQLCODE_FOR_GETRECORDCOUNTBYSQLTEMPLATE}{{{newSql}}}";

            var templateParams = new Dictionary<String, Object>();
            var dbParams = new List<DbParameter>();
            if (inputParamList != null)
            {
                foreach (var p in inputParamList)
                {
                    if (p.Type == QueryParamType.SqlParam)
                        dbParams.Add(ctx.DB.CreateParameter(p.Name, p.Value));
                    templateParams.Add(p.Name, p.Value);
                }
            }
            var compiler = SdmapCompiler.Instance;
            compiler.AddSourceCode(newSql);
            var sql = compiler.Emit(SQLCODE_FOR_GETRECORDCOUNTBYSQLTEMPLATE, templateParams);

            return MyConvert.ToInt32(ctx.ExecuteScalar(sql, dbParams.ToArray(), commandType), 0);
        }
    }
}
