using System;
using System.Data;
using System.Data.Common;

namespace Mini.Framework.Database
{
    /// <summary>
    /// 执行上下文
    /// </summary>
    public class ExecuteContext : IExecute
    {
        protected DB _db = null;
        protected DbConnection _connection = null;

        public ExecuteContext(DB db)
        {
            this._db = db;
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        public virtual Int32 ExecuteNonQuery(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            var command = _db.CreateCommand(commandText, dbParams, commandType);
            SetConnection(command);
            return command.ExecuteNonQuery();
        }

        public virtual Object ExecuteScalar(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            var command = _db.CreateCommand(commandText, dbParams, commandType);
            SetConnection(command);
            return command.ExecuteScalar();
        }

        public virtual DbDataReader ExecuteReader(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            var command = _db.CreateCommand(commandText, dbParams, commandType);
            SetConnection(command);
            return command.ExecuteReader();
        }

        public virtual DataTable GetDataTable(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            var command = _db.CreateCommand(commandText, dbParams, commandType);
            SetConnection(command);
            var adapter = _db.DbProviderFactory.CreateDataAdapter();
            adapter.SelectCommand = command;
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        protected virtual void SetConnection(DbCommand command)
        {
            if (_connection == null)
                _connection = _db.GetConnection(true);
            command.Connection = _connection;
        }
    }
}
