using System;
using System.Data.OleDb;
using System.Data.Common;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.Access
{
    /// <summary>
    /// AccessDb数据库访问类
    /// </summary>
    public class AccessDb : Database
    {
        #region "构造方法"

        internal AccessDb(String connectionString, OleDbFactory providerFactory)
            : base(connectionString, providerFactory)
        { }

        #endregion
    }
}
