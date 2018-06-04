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
        protected DbTransaction _trans = null;

        public ExecuteContext(DB db)
        {
            this._db = db;
        }

        ~ExecuteContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Open)
                        _trans.Commit();
                    _connection.Dispose();
                }
            }
        }

        public virtual void Commit()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _trans.Commit();
        }

        public virtual void Rollback()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _trans.Rollback();
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

        public virtual T ExecuteScalar<T>(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            return (T)ExecuteScalar(commandText, dbParams, commandType);
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
            {
                _connection = _db.GetConnection(true);
                _trans = _connection.BeginTransaction();
            }
            command.Connection = _connection;
            command.Transaction = _trans;
        }
    }
}
