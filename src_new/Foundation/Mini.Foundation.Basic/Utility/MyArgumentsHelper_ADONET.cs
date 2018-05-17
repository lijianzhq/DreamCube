#if HAVE_ADO_NET

using System;
using System.Data;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 参数帮助类
    /// </summary>
    public static partial class MyArgumentsHelper
    {
        /// <summary>
        /// 如果table为null，或者没有任何行则抛出异常
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

        /// <summary>
        /// 判断table是否有记录
        /// </summary>
        /// <param name="table"></param>
        /// <returns>true:table有记录；false:table没有任何行</returns>
        public static Boolean TestIfNullOrNoRows(DataTable table)
        {
            return table != null && table.Rows.Count > 0;
        }
    }
}

#endif