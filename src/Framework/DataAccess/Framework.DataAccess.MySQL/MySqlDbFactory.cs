using System;
using System.Data.Common;
using System.Data.SqlClient;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.MySQL
{
    /// <summary>
    /// sqlserver数据的工厂类
    /// </summary>
    internal class MySqlDbFactory : DBFactory
    {
        public override Database CreateDB(String connectionString)
        {
            return new MySqlDb(connectionString, MySql.Data.MySqlClient.MySqlClientFactory.Instance);
        }
    }
}
