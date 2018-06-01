
namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// ���ݿ����� 
    /// ע��: 1-10����Sql Server�ĸ����汾; 11-20����Oracle�ĸ����汾, 21-30����Access, 31-40����MySQL 
    /// </summary> 
    public enum DatabaseType
    {
        SqlServer2000 = 1,
        SqlServer2005 = 2,
        Oracle9i = 11,
        Oracle10g = 12,
        Oracle11g = 13,
        Access = 21,
        MySQL = 31
    }

    /// <summary> 
    /// ���ݿ�汾��Χ 
    /// </summary> 
    public enum DatabaseVersionRange
    {
        SqlServer_Min = 1,
        SqlServer_Max = 10,
        Oracle_Min = 11,
        Oracle_Max = 20,
        Access_Min = 21,
        Access_Max = 30,
        MySQL_Min = 31,
        MySQL_Max = 40
    }
}