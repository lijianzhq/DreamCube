using System;
using System.Data.Common;

using DreamCube.Foundation.Basic.Objects;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 数据库连接对象管理类
    /// </summary>
    public static class ConnectionMgr
    {
        #region "字段"

        private static String connectionCacheKey = "DreamCube.Framework.DataAccess.Basic.ConnectionMgr";

        #endregion

        #region "方法"

        /// <summary>
        /// 创建一个连接对象
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="factory">数据库提供程序</param>
        /// <param name="open">是否打开连接；true：打开连接；false：关闭连接</param>
        /// <returns></returns>
        public static ConnectionWrapper CreateConnection(String connStr, DbProviderFactory factory, Boolean open = false)
        {
            //因为CurrentContext是线程安全的，线程唯一性，所以Key值是可以一致的，不需要不同的连接字符串作为key值
            ConnectionWrapper conn = CurrentContext.GetCacheItem<ConnectionWrapper>(connectionCacheKey);
            if (conn == null || conn.ConnectionString != connStr)
            {
                DbConnection tempConn = factory.CreateConnection();
                conn = new ConnectionWrapper(tempConn, connStr);
                CurrentContext.TryCacheItem(connectionCacheKey, conn);
            }
            if (open && conn.Connection.State != System.Data.ConnectionState.Open)
                conn.Open();
            return conn;
        }

        /// <summary>
        /// 删除当前上下文的所有数据库连接
        /// </summary>
        public static void CloseAllConnection()
        {
            try
            {
                ConnectionWrapper conn = CurrentContext.GetCacheItem<ConnectionWrapper>(connectionCacheKey);
                if (conn != null) conn.Close();
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        #endregion
    }
}
