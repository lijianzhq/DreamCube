using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using sdmap.Compiler;

namespace Mini.Framework.Sdmap.Extension
{
    public static class SdMapHelper
    {
        /// <summary>
        /// 根据sql语句模板查询（不支持分页）
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="commandTextTemplate"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataTable GetDataTableBySqlTemplate(this IExecute ctx, String commandTextTemplate, IList<QueryParam> inputParamList, CommandType commandType = CommandType.Text)
        {
            MyArgumentsHelper.ThrowsIfNull(ctx, nameof(ctx));
            MyArgumentsHelper.ThrowsIfNullOrEmpty(commandTextTemplate, nameof(commandTextTemplate));
            String sql;
            List<DbParameter> dbParams;
            SdMapHelper.EmitSql(ctx, commandTextTemplate, inputParamList, out sql, out dbParams);
            return ctx.GetDataTable(sql, dbParams.ToArray(), commandType);
        }

        internal static void EmitSql(this IExecute ctx, String sql, IList<QueryParam> inputParamList, out String newsql, out List<DbParameter> dbParams)
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
