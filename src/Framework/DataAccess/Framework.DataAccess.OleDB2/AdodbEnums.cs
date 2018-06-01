
namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// 数据库类型 
    /// 注意: 1-10留给Sql Server的各个版本; 11-20留给Oracle的各个版本, 21-30留给Access, 31-40留给MySQL 
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
    /// 数据库版本范围 
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