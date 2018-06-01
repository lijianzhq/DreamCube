using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Globalization;
using System.Transactions;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 数据库基础对象
    /// </summary>
    public abstract partial class Database
    {
        #region "字段"

        /// <summary>
        /// 查询空表、为了获取表定义的sql语句模板
        /// 默认为：select * from {0} where 1=0
        /// </summary>
        protected virtual String SelectEmptyDatableSqlTemplate
        {
            get { return "select * from {0} where 1=0"; }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected String connectionStringStr;
        /// <summary>
        /// 连接字符串对象
        /// </summary>
        private ConnectionString connectionStringObject = null;
        readonly DbProviderFactory dbProviderFactory;

        /// <summary>
        /// 表定义缓冲区
        /// </summary>
        private DictionaryCachePool<String, DataTable> dataTableCachePool =
            new DictionaryCachePool<String, DataTable>(new StringEqualityIgnoreCaseComparerGeneric());

        #endregion

        #region "属性"

        public ConnectionString ConnectionString
        {
            get
            {
                if (connectionStringObject == null)
                {
                    connectionStringObject = new ConnectionString(connectionStringStr,
                                                                  Properties.Resources.UserNameToken,
                                                                  Properties.Resources.PasswordToken,
                                                                  Properties.Resources.DataSourceToken);
                }
                return connectionStringObject;
            }
        }

        /// <summary>
        /// 数据库提供程序
        /// </summary>
        public DbProviderFactory DbProviderFactory
        {
            get { return dbProviderFactory; }
        }

        /// <summary>
        /// 是否支持发现参数；根据存储过程名，获得存储过程的参数
        /// </summary>
        public virtual Boolean SupportsParemeterDiscovery
        {
            get { return false; }
        }

        #endregion

        #region "构造方法"

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbProviderFactory">数据库工厂对象</param>
        protected Database(String connectionString, DbProviderFactory dbProviderFactory)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "connectionString");
            if (dbProviderFactory == null) throw new ArgumentNullException("dbProviderFactory");
            this.connectionStringStr = connectionString;
            this.dbProviderFactory = dbProviderFactory;
        }

        #endregion

        #region "CommandText"

        /// <summary>
        /// 执行命令返回首行首列的数据，忽略其他的行和列（命令无需配置连接对象）
        /// 需要调用方手动释放连接对象
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令文本的类型（存储过程名；或者是sql语句）</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(String commandText, CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            return ExecuteReader(command);
        }

        /// <summary>
        /// 执行命令，并返回Datatable对象
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令文本的类型（存储过程名；或者是sql语句）</param>
        /// <returns></returns>
        public DataTable ExecuteTable(String commandText, CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            return ExecuteTable(command);
        }

        /// <summary>
        /// 执行命令返回首行首列的数据，忽略其他的行和列（命令无需配置连接对象）
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令文本的类型（存储过程名；或者是sql语句）</param>
        /// <returns></returns>
        public Object ExecuteScalar(String commandText, CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            return ExecuteScalar(command);
        }

        /// <summary>
        /// 执行命令并返回影响的行数
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令文本的类型（存储过程名；或者是sql语句）</param>
        /// <returns></returns>
        public Int32 ExecuteNonQuery(String commandText, CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            return ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行命令，并把数据加载到指定的dataset对象中
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令文本的类型（存储过程名；或者是sql语句）</param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public void LoadDataSet(String commandText, CommandType commandType, DataSet dataSet, String[] tableNames)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            LoadDataSet(command, dataSet, tableNames);
        }

        #endregion

        #region "CommandObj(命令对象可配置连接对象，可不配置连接对象)"

        /// <summary>
        /// 执行命令，并返回Datatable对象（命令无需配置连接对象）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable ExecuteTable(DbCommand command)
        {
            InitalCommand(command);
            using (DbDataAdapter adapter = this.dbProviderFactory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                DataTable data = new DataTable();
                adapter.Fill(data);
                return data;
            }
        }

        /// <summary>
        /// 执行命令返回首行首列的数据，忽略其他的行和列（命令无需配置连接对象）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Object ExecuteScalar(IDbCommand command)
        {
            InitalCommand(command);
            Object returnValue = command.ExecuteScalar();
            return returnValue;
        }

        /// <summary>
        /// 执行命令返回首行首列的数据，忽略其他的行和列（命令无需配置连接对象）
        /// 需要调用方手动释放连接对象
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(IDbCommand command)
        {
            InitalCommand(command);
            return command.ExecuteReader();
        }

        /// <summary>
        /// 执行命令并返回一个DataReader对象
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(DbCommand command, CommandBehavior behavior = CommandBehavior.Default)
        {
            InitalCommand(command);
            IDataReader realReader = command.ExecuteReader(behavior);
            return realReader;
        }

        /// <summary>
        /// 执行命令并返回影响的行数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Int32 ExecuteNonQuery(IDbCommand command)
        {
            InitalCommand(command);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行命令，并把数据加载到指定的dataset对象中
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public void LoadDataSet(IDbCommand command, DataSet dataSet, String[] tableNames)
        {
            if (tableNames == null) throw new ArgumentNullException("tableNames");
            if (tableNames.Length == 0)
                throw new ArgumentException(Properties.Resources.ExceptionTableNameArrayEmpty, "tableNames");

            for (Int32 i = 0; i < tableNames.Length; i++)
            {
                if (String.IsNullOrEmpty(tableNames[i]))
                    throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString,
                        String.Concat("tableNames[", i, "]"));
            }
            InitalCommand(command);
            using (DbDataAdapter adapter = this.dbProviderFactory.CreateDataAdapter())
            {
                ((IDbDataAdapter)adapter).SelectCommand = command;
                DateTime startTime = DateTime.Now;
                String systemCreatedTableNameRoot = "Table";
                for (Int32 i = 0; i < tableNames.Length; i++)
                {
                    String systemCreatedTableName = (i == 0)
                                                        ? systemCreatedTableNameRoot
                                                        : systemCreatedTableNameRoot + i;

                    adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                }
                adapter.Fill(dataSet);
            }
        }

        #endregion

        #region "创建对象的方法"

        /// <summary>
        /// 创建一个数据库连接对象
        /// </summary>
        /// <param name="open">标志是否打开连接；true打开；false不打开</param>
        /// <returns></returns>
        protected DbConnection CreateConnection(Boolean open)
        {
            return ConnectionMgr.CreateConnection(this.ConnectionString.ToString(), this.dbProviderFactory, open).Connection;
        }

        /// <summary>
        /// 创建一个命令对象
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public DbCommand CreateCommand(String commandText = "", CommandType commandType = CommandType.Text)
        {
            DbCommand command = this.DbProviderFactory.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        #endregion

        #region "命令参数相关"

        /// <summary>
        /// 向命令对象添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddInParameter(DbCommand command, String name)
        {
            AddParameter(command, name, null, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 向命令对象添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddInParameter(DbCommand command, String name, DbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 向命令对象添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInParameter(DbCommand command, String name, DbType dbType, Object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// 向命令对象添加输入参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        public void AddInParameter(DbCommand command,
                                   String name,
                                   DbType dbType,
                                   String sourceColumn,
                                   DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// 向命令对象添加输出参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddOutParameter(DbCommand command, String name, DbType dbType)
        {
            AddParameter(command, name, dbType, null, ParameterDirection.Output, true, 0, 0,
                String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// 向命令对象添加输出参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size">设置列中数据的最大大小（以字节为单位）。</param>
        public void AddOutParameter(DbCommand command, String name, DbType dbType, Int32 size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0,
                String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// 向命令对象添加参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable"></param>
        /// <param name="precision">Precision 属性由 SqlDbType 为 Decimal 的参数使用。 </param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public virtual void AddParameter(DbCommand command,
                                         String name,
                                         DbType? dbType,
                                         Int32? size,
                                         ParameterDirection direction,
                                         Boolean nullable,
                                         Byte precision,
                                         Byte scale,
                                         String sourceColumn,
                                         DataRowVersion sourceVersion,
                                         Object value)
        {
            if (command == null) throw new ArgumentNullException("command");
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision,
                                        scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// 向命令对象添加参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public void AddParameter(DbCommand command,
                                 String name,
                                 DbType? dbType,
                                 ParameterDirection direction,
                                 String sourceColumn,
                                 DataRowVersion sourceVersion,
                                 Object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// 向命令对象添加输入参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value">参数值</param>
        /// <param name="size"></param>
        /// <param name="direction">参数方向</param>
        /// <returns></returns>
        public DbParameter CreateParameter(String name, DbType dbType,
                                            Object value = null,
                                            Int32 size = 0,
                                            ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateParameter(name, dbType, size, direction, true, 0, 0,
                                   String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 创建一个命令参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable">该值指示参数是否接受 null 值</param>
        /// <param name="precision">
        /// 获取或设置用来表示 Value 属性的最大位数。 
        /// Precision 属性由 SqlDbType 为 Decimal 的参数使用。 
        /// 不需要为输入参数指定 Precision 和 Scale 属性的值，因为可以从参数值推断它们的值。 
        /// 输出参数以及在您需要指定参数的完整元数据而不指示值时，
        /// 需要 Precision 和 Scale，例如指定一个具有特定精度和小数位数的空值。 
        /// </param>
        /// <param name="scale">获取或设置 Value 解析为的小数位数。 </param>
        /// <param name="sourceColumn">获取或设置源列的名称，该源列映射到 DataSet 并用于加载或返回 Value </param>
        /// <param name="sourceVersion">获取或设置在加载 Value 时要使用的 DataRowVersion。</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        protected DbParameter CreateParameter(String name,
                                              DbType? dbType,
                                              Int32? size,
                                              ParameterDirection direction,
                                              Boolean nullable,
                                              Byte precision,
                                              Byte scale,
                                              String sourceColumn,
                                              DataRowVersion sourceVersion,
                                              Object value)
        {
            DbParameter param = CreateParameter(name);
            if (dbType != null) param.DbType = dbType.Value;
            if (size.HasValue) param.Size = size.Value;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
            return param;
        }

        /// <summary>
        /// 创建一个命令参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected DbParameter CreateParameter(String name)
        {
            DbParameter param = dbProviderFactory.CreateParameter();
            param.ParameterName = this.FormatParameterName(name);
            return param;
        }

        #endregion

        #region "其他方法"

        /// <summary>
        /// 关闭所有连接对象
        /// </summary>
        public void CloseAllConnection()
        {
            ConnectionMgr.CloseAllConnection();
        }

        /// <summary>
        /// 格式化参数名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual String FormatParameterName(String name)
        {
            return name;
        }

        /// <summary>
        /// 根据表名获取数据表的架构信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual DataTable GetTableSchema(String tableName)
        {
            DataTable table = null;
            if (dataTableCachePool.TryGetValue(tableName, out table, CollectionsGetOper.DefaultValueIfNotExist, null))
                return table;

            using (DbCommand comm = this.CreateCommand (String.Format(this.SelectEmptyDatableSqlTemplate, tableName)))
            {
                //指定：CommandBehavior.SchemaOnly无法读取到主键的信息
                //using (IDataReader reader = this.ExecuteReader (comm, CommandBehavior.SchemaOnly))
                using (IDataReader reader = this.ExecuteReader(comm, CommandBehavior.KeyInfo))
                {
                    table = reader.GetSchemaTable();
                    dataTableCachePool.TryAdd(tableName, table, CollectionsAddOper.IgnoreIfExist);
                    return table;
                }
            }
        }

        /// <summary>
        /// 初始化命令对象（配置连接对象）
        /// </summary>
        /// <param name="command"></param>
        protected void InitalCommand(IDbCommand command)
        {
            if (command.Connection == null)
                command.Connection = this.CreateConnection(true);
        }

        /// <summary>
        /// 返回命令参数的起始序号
        /// </summary>
        /// <returns></returns>
        protected virtual Int32 UserParametersStartIndex()
        {
            return 0;
        }

        #endregion
    }
}
