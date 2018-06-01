using System;

namespace DreamCube.Framework.DataAccess.Basic
{
    public enum DBProviderType
    {
        /// <summary>
        /// sqlserver数据库
        /// </summary>
        SqlClient,

        /// <summary>
        /// 连接sqlce数据库
        /// </summary>
        SqlCe,

        /// <summary>
        /// System.Data.OracleClient
        /// </summary>
        Oracle,

        /// <summary>
        /// Oracle.DataAccess.OracleClient2
        /// </summary>
        Oracle2,

        /// <summary>
        /// 连接MySql
        /// </summary>
        MySQL,

        /// <summary>
        /// 使用OleDbProvider连接数据库
        /// </summary>
        OleDb
    }
}
