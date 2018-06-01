using System;
using System.Data.Common;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

//oracle
using Oracle.DataAccess.Client;

namespace DreamCube.Framework.DataAccess.Oracle2
{
    /// <summary>
    /// oracle数据库访问类
    /// </summary>
    public class OracleDb : Database
    {
        #region "构造方法"

        internal OracleDb(String connectionString, OracleClientFactory providerFactory)
            : base(connectionString, providerFactory)
        {

        }

        #endregion
    }
}
