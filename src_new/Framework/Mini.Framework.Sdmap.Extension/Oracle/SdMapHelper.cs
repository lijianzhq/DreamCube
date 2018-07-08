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
            var start = (pageIndex - 1) * pageSize + 1;
            var startParamName = "STARTINDEX";
            var endParamName = "ENDINDEX";
            var end = pageIndex * pageSize;
            var newSql = $"select * from(select t1.*,rownum rn from ({commandTextTemplate}) t1) where rn>={ctx.DB.DBCharacterProvider.FormatParameterName(startParamName)} and rn<={ctx.DB.DBCharacterProvider.FormatParameterName(endParamName)}";
            var pageDBParam = new DbParameter[] { ctx.DB.CreateParameter(startParamName, start), ctx.DB.CreateParameter(endParamName, end) };
            //newSql = $"sql {SQLCODE_FOR_GETDATATABLEBYSQLTEMPLATE}{{{newSql}}}";

            String sql;
            List<DbParameter> dbParams;
            EmitSql(ctx, newSql, inputParamList, out sql, out dbParams);

            dbParams.AddRange(pageDBParam);
            return ctx.GetDataTable(sql, dbParams.ToArray(), commandType);
        }

        public static Int32 GetRecordCountBySqlTemplate(this IExecute ctx, String commandTextTemplate, IList<QueryParam> inputParamList, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandTextTemplate, nameof(commandTextTemplate));
            var newSql = $"select count(*) from ({commandTextTemplate}) t1";
            //newSql = $"sql {SQLCODE_FOR_GETRECORDCOUNTBYSQLTEMPLATE}{{{newSql}}}";
            String sql;
            List<DbParameter> dbParams;
            EmitSql(ctx, newSql, inputParamList, out sql, out dbParams);
            return MyConvert.ToInt32(ctx.ExecuteScalar(sql, dbParams.ToArray(), commandType), 0);
        }

        private static void EmitSql(this IExecute ctx, String sql, IList<QueryParam> inputParamList, out String newsql, out List<DbParameter> dbParams)
        {
            var sqlTemplate = $"sql v1{{{sql}}}";
            var templateParams = new Dictionary<String, Object>();
            if (inputParamList != null)
            {
                foreach (var p in inputParamList)
                {
                    templateParams.Add(p.Name, p.Value);
                }
            }
            var compiler = SdmapCompiler.Instance;
            compiler.AddSourceCode(sqlTemplate);
            List<String> paramsNameList = null;//返回来的，构造sql需要用到的参数名称列表
            newsql = compiler.Emit("v1", templateParams, out paramsNameList);

            //构造sql的参数（根据sql语句具体使用了哪个参数，然后构造sqlparameter查询参数）
            dbParams = new List<DbParameter>();
            if (inputParamList != null && inputParamList.Count > 0
                && paramsNameList != null && paramsNameList.Count > 0)
            {
                foreach (var p in inputParamList)
                {
                    if (p.Type == QueryParamType.SqlParam)
                    {
                        var paramName = ctx.DB.DBCharacterProvider.FormatParameterName(p.Name);
                        //再判断sql中是否已经使用了这个参数变量
                        if (sqlTemplate.IndexOf(paramName, StringComparison.CurrentCultureIgnoreCase) >= 0 && paramsNameList.Contains(p.Name))
                            dbParams.Add(ctx.DB.CreateParameter(p.Name, p.Value, p.DbType));
                    }
                }
            }
        }
    }
}
