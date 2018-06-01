using System;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// Access数据库的连接参数类 
    /// </summary> 
    public class AccessConnectParam : ConnectParam
    {
        public AccessConnectParam(int iDbType, string sFilePath, string sUser, string sPassword)
        {
            m_DatabaseType = iDbType;
            m_FilePath = sFilePath;
            m_User = sUser;
            m_Password = sPassword;
        }

        /// <summary> 
        /// 生成连接字符串 
        /// </summary> 
        /// <returns></returns> 
        public override string MakeConnectString()
        {
            if (m_User == "")
            {
                return "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + m_FilePath;
            }
            else
            {
                return "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + m_FilePath + "; User ID=" + m_User + "; Password=" + m_Password;
            }
        }
    }
}
