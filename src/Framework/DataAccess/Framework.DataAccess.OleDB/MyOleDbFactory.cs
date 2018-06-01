using System;
using System.Data.Common;
using System.Data.OleDb;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.OleDb
{
    /// <summary>
    /// sqlserver数据的工厂类
    /// </summary>
    internal class MyOleDBFactory : DBFactory
    {
        public override Database CreateDB(String connectionString)
        {
            return new MyOleDB(connectionString, OleDbFactory.Instance);
        }
    }
}
