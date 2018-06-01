using System;
using System.IO;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// 链接对象封装
    /// </summary> 
    public class RdbConnection
    {
        #region "字段"

        internal OleDbConnection m_OledbConn = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 连接对象的状态
        /// </summary>
        public ConnectionState State
        {
            get
            {
                return m_OledbConn.State;
            }
        }

        #endregion

        #region "方法"

        public RdbConnection(string sConnString)
        {
            this.m_OledbConn = new OleDbConnection();
            this.m_OledbConn.ConnectionString = sConnString;
            this.m_OledbConn.Open();
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public void Open()
        {
            if (m_OledbConn.State != ConnectionState.Open)
                m_OledbConn.Open();
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public void Close()
        {
            if (m_OledbConn.State == ConnectionState.Open)
            {
                m_OledbConn.Dispose();
                m_OledbConn.Close();
                m_OledbConn = null;
            }
        }

        #endregion
    }
}