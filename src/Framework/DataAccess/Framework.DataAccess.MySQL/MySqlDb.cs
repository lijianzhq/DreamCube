using System;
using System.Data;
using System.Data.Common;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;
using MySql.Data.MySqlClient;

namespace DreamCube.Framework.DataAccess.MySQL
{
    public class MySqlDb : Database
    {
        #region "构造方法"

        internal MySqlDb(String connectionString, MySql.Data.MySqlClient.MySqlClientFactory providerFactory)
            : base(connectionString, providerFactory)
        { }

        #endregion
    }
}
