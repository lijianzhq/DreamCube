using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Mini.Framework.Database
{
    internal class LogHelper
    {
        public static void Log(String sql, IEnumerable<DbParameter> paramList, Action<String> logCallback)
        {
            var sb = new StringBuilder();
            sb.Append(sql);
            sb.AppendLine();
            if (paramList != null)
            {
                foreach (var p in paramList)
                {
                    sb.Append($"{p.ParameterName},{p.Value}");
                    sb.AppendLine();
                }
            }
            logCallback(sb.ToString());
        }
    }
}
