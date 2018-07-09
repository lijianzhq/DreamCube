using System;
using System.Data;
using System.Data.Common;

namespace Mini.Framework.Database
{
    /// <summary>
    /// 应该用AOP植入记录日志的功能的
    /// 留待以后完善。
    /// </summary>
    public interface IExecute : IDisposable
    {
        DB DB { get; }

        Int32 ExecuteNonQuery(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        DbDataReader ExecuteReader(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        Object ExecuteScalar(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        T ExecuteScalar<T>(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        DataTable GetDataTable(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);
    }
}
