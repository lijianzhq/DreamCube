using System;
using System.Data.Common;
using System.Data.OracleClient;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.Oracle
{
    /// <summary>
    /// oracle数据库访问类
    /// </summary>
    public class OracleDb : Database
    {
        #region "构造方法"

        internal OracleDb(String connectionString, OracleClientFactory providerFactory)
            : base(connectionString, providerFactory)
        { }

        #endregion
    }
}
