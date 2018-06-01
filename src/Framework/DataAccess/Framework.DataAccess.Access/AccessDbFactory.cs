using System;
using System.Data.OleDb;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.Access
{
    public class AccessDbFactory : DBFactory
    {
        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public override Database CreateDB(String connectionString)
        {
            return new AccessDb(connectionString, OleDbFactory.Instance);
        }
    }
}
