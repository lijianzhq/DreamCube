using System;
using System.Data.Common;
using System.Data.SqlClient;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.Sqlserver
{
    /// <summary>
    /// sqlserver数据的工厂类
    /// </summary>
    internal class SqlserverDbFactory : DBFactory
    {
        /// <summary>
        /// 获取数据库对象实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public override Database CreateDB(String connectionString)
        {
            return new SqlserverDb(connectionString, SqlClientFactory.Instance);
        }
    }
}
