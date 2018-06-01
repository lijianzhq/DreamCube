
namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// 连接参数基础类 
    /// </summary> 
    public abstract class ConnectParam
    {
        public string m_User = "sa";
        public string m_Password = "sa";
        public string m_Host = "127.0.0.1";
        public string m_Port = "1521";
        public string m_Database;
        public int m_DatabaseType;
        public string m_FilePath;

        public abstract string MakeConnectString();

        public int DatabaseType
        {
            get
            {
                return m_DatabaseType;
            }
        }
    }
}