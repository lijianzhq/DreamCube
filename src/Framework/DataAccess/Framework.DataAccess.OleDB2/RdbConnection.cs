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
    /// ���Ӷ����װ
    /// </summary> 
    public class RdbConnection
    {
        #region "�ֶ�"

        internal OleDbConnection m_OledbConn = null;

        #endregion

        #region "����"

        /// <summary>
        /// ���Ӷ����״̬
        /// </summary>
        public ConnectionState State
        {
            get
            {
                return m_OledbConn.State;
            }
        }

        #endregion

        #region "����"

        public RdbConnection(string sConnString)
        {
            this.m_OledbConn = new OleDbConnection();
            this.m_OledbConn.ConnectionString = sConnString;
            this.m_OledbConn.Open();
        }

        /// <summary>
        /// ������
        /// </summary>
        public void Open()
        {
            if (m_OledbConn.State != ConnectionState.Open)
                m_OledbConn.Open();
        }

        /// <summary>
        /// �ر�����
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