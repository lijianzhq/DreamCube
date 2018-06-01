using System;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// MySql 的连接参数类 
    /// </summary> 
    public class MySqlConnectParam : ConnectParam
    {
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="sHost"></param> 
        /// <param name="sDbName"></param> 
        /// <param name="sUser"></param> 
        /// <param name="sPassword"></param> 
        public MySqlConnectParam(int iDbType, string sHost, string sDbName, string sUser, string sPassword)
        {
            m_DatabaseType = iDbType;
            m_Host = sHost;
            m_Database = sDbName;
            m_User = sUser;
            m_Password = sPassword;
        }

        /// <summary> 
        /// 生成连接字符串 
        /// </summary> 
        /// <returns></returns> 
        public override string MakeConnectString()
        {
            return "Data Source=" + m_Host + ";Initial Catalog=" + m_Database + ";User ID=" + m_User + ";Password=" + m_Password;
        }
    }
}
