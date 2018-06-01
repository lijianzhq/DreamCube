using System;
using System.Data;
using System.Data.Common;

namespace Mini.Framework.Database
{
    /// <summary>
    /// </summary>
    public class DB
    {
        protected DBCharacterProvider _characterProvider = null;

        public DbProviderFactory DbProviderFactory => _characterProvider.DbProviderFactory;

        public DB(DBCharacterProvider characterProvider)
        {
            this._characterProvider = characterProvider;
        }

        public virtual IExecute BeginExecuteContext()
        {
            return new ExecuteContext(this);
        }

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="open"></param>
        /// <returns></returns>
        public virtual DbConnection GetConnection(Boolean open = false)
        {
            var conn = DbProviderFactory.CreateConnection();
            conn.ConnectionString = _characterProvider.GetConnectionString();
            if (open) conn.Open();
            return conn;
        }

        /// <summary>
        /// 创建一个命令对象
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="dbParams">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public virtual DbCommand CreateCommand(String commandText, DbParameter[] dbParams = null, CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.DbProviderFactory.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if(dbParams!=null)
            {
                foreach (var p in dbParams)
                    command.Parameters.Add(p);
            }
            return command;
        }

        /// <summary>
        /// 创建变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual DbParameter CreateParameter(String name,
                                                   Object value,
                                                   DbType? dbType = null,
                                                   Int32? size = null,
                                                   ParameterDirection direction = ParameterDirection.Input)
        {
            DbParameter param = DbProviderFactory.CreateParameter();
            param.ParameterName = _characterProvider.FormatParameterName(name);
            if (dbType != null) param.DbType = dbType.Value;
            if (size.HasValue) param.Size = size.Value;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            return param;
        }
    }
}
