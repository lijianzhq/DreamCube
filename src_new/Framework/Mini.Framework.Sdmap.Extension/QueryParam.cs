using System;
using System.Collections.Generic;
using System.Text;

namespace Mini.Framework.Sdmap.Extension
{
    public enum QueryParamType
    {
        SqlParam = 0,
        ValueParam = 1
    }

    public class QueryParam
    {
        public String Name { get; set; }

        public Object Value { get; set; }

        /// <summary>
        /// 0=Parameter查询参数；1=Value匹配参数
        /// </summary>
        public QueryParamType Type { get; set; } = QueryParamType.SqlParam;
    }
}
