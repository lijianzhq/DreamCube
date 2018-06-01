using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using System.Reflection;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.SqlCe
{
    public class SqlCeDb : Database
    {
        #region "构造方法"

        internal SqlCeDb(String connectionString, SqlCeProviderFactory factory)
            : base(connectionString, factory)
        { }

        #endregion

        #region "公共方法"

        public override string FormatParameterName(string name)
        {
            if (name.StartsWith("@")) return name;
            return "@" + name.TrimStart();
        }

        #endregion
    }
}
