using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.Sqlserver
{
    /// <summary>
    /// sqlserver数据库
    /// </summary>
    public class SqlserverDb : Database
    {
        #region "构造方法"

        internal SqlserverDb(String connectionString, SqlClientFactory providerFactory)
            : base(connectionString, providerFactory)
        { }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 格式化参数名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override String FormatParameterName(String name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                if (!name.StartsWith("@"))
                    name = "@" + name;
                return name;
            }
            return base.FormatParameterName(name);
        }

        #endregion
    }
}
