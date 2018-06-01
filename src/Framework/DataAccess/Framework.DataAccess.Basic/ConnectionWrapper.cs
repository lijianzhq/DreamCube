using System;
using System.Data.Common;

namespace DreamCube.Framework.DataAccess.Basic
{
    public class ConnectionWrapper
    {
        #region "字段"

        /// <summary>
        /// 连接对象
        /// </summary>
        private DbConnection conn = null;

        /// <summary>
        /// 事务对象
        /// </summary>
        private DbTransaction trans = null;

        /// <summary>
        /// 标志当前连接是否在事务过程中，以防客户端多次启动事务
        /// </summary>
        private Boolean isInTrans = false;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private String connectionString = String.Empty;

        #endregion

        #region "属性"

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public String ConnectionString
        {
            get { return connectionString; }
        }

        /// <summary>
        /// 封装的连接对象
        /// </summary>
        internal DbConnection Connection
        {
            get { return this.conn; }
        }

        #endregion

        #region "构造函数"

        /// <summary>
        /// 根据连接字符串创建一个连接对象
        /// </summary>
        /// <param name="connectionStr"></param>
        internal ConnectionWrapper(DbConnection conn, String connectionStr)
        {
            this.conn = conn;
            this.connectionString = connectionStr;
            this.conn.ConnectionString = connectionStr;
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            if (this.conn.State != System.Data.ConnectionState.Open)
                this.conn.Open();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (this.conn.State != System.Data.ConnectionState.Closed)
                this.conn.Close();
        }

        /// <summary>
        /// 启动事务
        /// </summary>
        public void BeginTrans()
        {
            if (this.isInTrans) return;
            this.isInTrans = true;
            this.trans = this.conn.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTrans()
        {
            if (!this.isInTrans) return;
            this.isInTrans = false;
            this.trans.Commit();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTrans()
        {
            if (!this.isInTrans) return;
            this.isInTrans = false;
            this.trans.Rollback();
        }

        #endregion
    }
}
