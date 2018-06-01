using System;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;
//oracle
using Oracle.DataAccess.Client;

namespace DreamCube.Framework.DataAccess.Oracle2
{
    public class OracleDbFactory : DBFactory
    {
        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public override Database CreateDB(String connectionString)
        { 
            return new OracleDb(connectionString, OracleClientFactory.Instance);
        }
    }
}
