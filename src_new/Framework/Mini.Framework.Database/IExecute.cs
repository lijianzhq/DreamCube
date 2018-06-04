using System;
using System.Data;
using System.Data.Common;

namespace Mini.Framework.Database
{
    public interface IExecute : IDisposable
    {
        Int32 ExecuteNonQuery(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        DbDataReader ExecuteReader(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        Object ExecuteScalar(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        T ExecuteScalar<T>(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);

        DataTable GetDataTable(string commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text);
    }
}
