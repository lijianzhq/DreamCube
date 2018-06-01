using System;
using System.Data;
using System.Data.OleDb;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.OleDb
{
    public class MyOleDB : Database
    {
        #region "构造方法"

        internal MyOleDB(String connectionString, OleDbFactory providerFactory)
            : base(connectionString, providerFactory)
        { }

        #endregion
    }
}
