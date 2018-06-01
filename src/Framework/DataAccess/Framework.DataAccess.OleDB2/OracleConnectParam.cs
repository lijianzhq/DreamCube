using System;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// Oracle的连接参数类 
    /// </summary> 
    public class OracleConnectParam : ConnectParam
    {
        public OracleConnectParam(string sHost, string sDbName, string sUser, string sPassword)
            : this(12, sHost, "1521", sDbName, sUser, sPassword)
        { }

        public OracleConnectParam(string sHost, string sPort, string sDbName, string sUser, string sPassword)
            : this(12, sHost, sPort, sDbName, sUser, sPassword)
        { }

        public OracleConnectParam(int iDbType, string sHost, string sPort, string sDbName, string sUser, string sPassword)
        {
            m_DatabaseType = iDbType;
            m_Host = sHost;
            m_Port = sPort;
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
            string sDataSource = "\"(DESCRIPTION = (SDU = 2048)(ADDRESS = (PROTOCOL = tcp)(HOST = " + m_Host + ")(PORT = " + m_Port + "))(CONNECT_DATA = (SERVICE_NAME = " + m_Database + ")))\"";
            return "Provider=OraOLEDB.Oracle.1;Persist Security Info=True;User ID=" + m_User + ";Password=" + m_Password + ";Data Source=" + sDataSource;
        }
    }
}
