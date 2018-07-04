using System;
using System.Data.Common;

namespace Mini.Framework.EFCommon
{
    /// <summary>
    /// 数据库连接对象提供程序接口
    /// </summary>
    public interface IDBConnectionProvider
    {
        DbConnection CreateConnection(Boolean open = true);
    }
}
