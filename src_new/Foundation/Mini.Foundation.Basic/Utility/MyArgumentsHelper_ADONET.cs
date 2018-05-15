#if HAVE_ADO_NET

using System;
using System.Data;

namespace Mini.Foundation.Basic.Utility
{
    public static partial class MyArgumentsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="parameterName"></param>
        /// <param name="messageTemplate">{0}=parameterName</param>
        public static void ThrowsIfNullOrNoRows(DataTable table, String parameterName, String messageTemplate = "table [{0}] does not contains one row!")
        {
            if (table == null)
                throw new ArgumentNullException(parameterName);
            if (table.Rows.Count == 0)
                throw new ArgumentException(String.Format(messageTemplate, parameterName));
        }

        public static Boolean TryIfNullOrNoRows(DataTable table)
        {
            return table != null && table.Rows.Count > 0;
        }
    }
}

#endif