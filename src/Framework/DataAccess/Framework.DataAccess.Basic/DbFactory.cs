using System;
using System.Data.Common;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 数据库提供工厂
    /// </summary>
    public abstract class DBFactory
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <returns></returns>
        public abstract Database CreateDB(String connectionString);
    }
}
