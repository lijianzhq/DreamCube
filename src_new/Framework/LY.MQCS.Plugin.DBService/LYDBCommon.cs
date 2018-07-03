using System;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Security.Cryptography;

using Ora = Oracle.ManagedDataAccess.Client;
using LY.TEC.Data.Data2;
using LY.TEC.Data.Common;
using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using Mini.Framework.Database.DefaultProviders;

namespace LY.MQCS.Plugin.DBService
{
    /// <summary>
    /// add by lj
    /// oracleclient库已经Obsolete
    /// 查看了一下公司封装的数据库访问库，公司是采用微软的oracleclient库，
    /// 所以这里也采用这个库，所有部署情况应该是统一的，不会有什么特殊问题。
    /// </summary>
    public static partial class LYDBCommon
    {
        private static String connectionStr = String.Empty;
        private static DB _db = null;

        /// <summary>
        /// 获取Gateway（appservice 类库里面的写法不支持多线程，所以在这里重新做了一个实现）
        /// </summary>
        /// <returns></returns>
        public static Gateway GetGateway()
        {
            return Gateway.Default;
        }

        public static DB GetDB()
        {
            if (_db == null)
            {
                _db = new DB(new OracleProvider(GetConnectionString()));
            }
            return _db;
        }

        /// <summary>
        /// 获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(String sql)
        {
            using (var conn = CreateConnection(true))
            {
                var command = CreateCommand(sql, conn);
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = command;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static OracleCommand CreateCommand(String sql, OracleConnection conn = null, OracleTransaction trans = null)
        {
            OracleCommand command = new OracleCommand()
            {
                CommandText = sql,
                Connection = conn,
                Transaction = trans,
            };
            return command;
        }

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="open"></param>
        /// <returns></returns>
        public static OracleConnection CreateConnection(Boolean open = true)
        {
            Gateway gateway = GetGateway();
            OracleConnection conn = new OracleConnection(gateway.Db.ConnectionString);
            if (open && conn.State != ConnectionState.Open) conn.Open();
            return conn;
        }

        /// <summary>
        /// 获取pq数据库
        /// </summary>
        /// <returns></returns>
        public static PQEntities GetDB_PQ()
        {
            var conn = new Ora.OracleConnection(GetConnectionString());
            return new PQEntities(conn, true);
        }

        /// <summary>
        /// 读取连接字符串，直接读取配置文件的
        /// </summary>
        /// <returns></returns>
        public static String GetConnectionString()
        {
            //return "User Id=MQCSBUS;Password=MQCSBUS;Data Source=172.26.136.162/KFMQCS;Unicode=True";
            return "User Id=test;Password=guanliyuan;Data Source=127.0.0.1/orcl";
            if (!String.IsNullOrEmpty(connectionStr)) return connectionStr;
            var asmConfiger = new AssemblyConfiger();
            var type = asmConfiger.ConfigFileReader.AppSettings("readConnectionStringType");
            if (type == "1")
            {
                var configFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, asmConfiger.ConfigFileReader.AppSettings("dbConfigFilePath"));
                if (File.Exists(configFileFullPath))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(configFileFullPath);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("configuration/connectionStrings/add");

                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        string name = xmlNodeList[xmlNodeList.Count - 1].Attributes["name"].Value;
                        if (String.Equals(name, asmConfiger.ConfigFileReader.AppSettings("connectionName"), StringComparison.CurrentCultureIgnoreCase))
                        {
                            connectionStr = DecrpytConnectionString(name.ToLower(), xmlNodeList[xmlNodeList.Count - 1].Attributes["connectionString"].Value);
                            break;
                        }
                    }
                }
            }
            else
            {
                connectionStr = GetGateway().Db.ConnectionString;
            }
            connectionStr = new ConnectionString(connectionStr).ToOracle();
            return connectionStr;
        }

        internal static string DecrpytConnectionString(string name, string connectionString)
        {
            string result = connectionString;
            if (!string.IsNullOrEmpty(connectionString) && connectionString.Length > 0)
            {
                result = new CryptographyManager().SymmetricDecrpyt(connectionString, Rijndael.Create(), name);
            }
            return result;
        }

        ///// <summary>
        ///// 创建一个命令
        ///// </summary>
        ///// <param name="conn"></param>
        ///// <param name="trans"></param>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //public static OracleCommand CreateCommand(OracleConnection conn, OracleTransaction trans, String sql)
        //{
        //    OracleCommand command = new OracleCommand()
        //    {
        //        CommandText = sql,
        //        Connection = conn,
        //        Transaction = trans
        //    };
        //    return command;
        //}

        ///// <summary>
        ///// 创建一个连接
        ///// </summary>
        ///// <param name="gateway"></param>
        ///// <param name="open"></param>
        ///// <returns></returns>
        //public static OracleConnection CreateConnection(Gateway gateway, Boolean open = true)
        //{
        //    //OracleConnection conn = new OracleConnection(gateway.Db.ConnectionString);
        //    //OracleCommand command = new OracleCommand()
        //    //{
        //    //    CommandText = sql,
        //    //    Connection = conn
        //    //};
        //    OracleConnection connection = new OracleConnection(gateway.Db.ConnectionString);
        //    if (open && connection.State != ConnectionState.Open)
        //        connection.Open();
        //    return connection;
        //}

        //public static OracleTransaction BeginTransaction2(this OracleCommand command)
        //{
        //    if (command.Connection == null)
        //        throw new ArgumentException("command");
        //    if (command.Connection.State != ConnectionState.Open)
        //        command.Connection.Open();
        //    OracleTransaction trans = command.Connection.BeginTransaction();
        //    command.Transaction = trans;
        //    return trans;
        //}

        //public static OracleParameter AddInputParameter(this OracleCommand command, String name, DbType dbtype, Object value)
        //{
        //    OracleParameter param = new OracleParameter()
        //    {
        //        DbType = dbtype,
        //        ParameterName = name,
        //        Value = value
        //    };
        //    command.Parameters.Add(param);
        //    return param;
        //}

        //public static OracleParameter AddOutputParameter(this OracleCommand command, String name, DbType dbtype, Int32 size, Object value = null)
        //{
        //    OracleParameter param = new OracleParameter()
        //    {
        //        DbType = dbtype,
        //        ParameterName = name,
        //        Value = value,
        //        Size = size,
        //        Direction = ParameterDirection.Output
        //    };
        //    command.Parameters.Add(param);
        //    return param;
        //}

        //public static OracleParameter AddInputParameter(this OracleCommand command, String name, Object value)
        //{
        //    OracleParameter param = new OracleParameter()
        //    {
        //        ParameterName = name,
        //        Value = value
        //    };
        //    command.Parameters.Add(param);
        //    return param;
        //}

        //public static OracleParameter AddParameter(this OracleCommand command, String name, DbType dbtype, Int32 size, Object value, ParameterDirection direction)
        //{
        //    OracleParameter param = new OracleParameter()
        //    {
        //        DbType = dbtype,
        //        ParameterName = name,
        //        Value = value,
        //        Direction = direction,
        //        Size = size
        //    };
        //    command.Parameters.Add(param);
        //    return param;
        //}

        //private static void OpenConn(this OracleCommand command)
        //{
        //    if (command.Connection == null)
        //        throw new ArgumentException("command");
        //    if (command.Connection.State != ConnectionState.Open)
        //        command.Connection.Open();
        //}

        //public static Int32 ExecuteNonQuery2(this OracleCommand command)
        //{
        //    using (command.Connection)
        //    {
        //        command.OpenConn();
        //        return command.ExecuteNonQuery();
        //    }
        //}

        //public static Int32 ExecuteNonQuery2WithoutUsing(this OracleCommand command)
        //{
        //    //不能using，using导致了事务执行的时候被释放了
        //    //using (command.Connection)
        //    {
        //        command.OpenConn();
        //        return command.ExecuteNonQuery();
        //    }
        //}

        //public static DataTable ExecuteDatatable(this OracleCommand command)
        //{
        //    using (command.Connection)
        //    {
        //        OracleDataAdapter adapter = new OracleDataAdapter();
        //        command.OpenConn();
        //        adapter.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        adapter.Fill(ds);
        //        return ds.Tables[0];
        //    }
        //}

        //public static Object ExecuteScalar2(this OracleCommand command)
        //{
        //    using (command.Connection)
        //    {
        //        command.OpenConn();
        //        return command.ExecuteScalar();
        //    }
        //}

        //public static DataTable ExecuteDatatable(this OracleCommand command, DataTable table)
        //{
        //    using (command.Connection)
        //    {
        //        OracleDataAdapter adapter = new OracleDataAdapter();
        //        command.OpenConn();
        //        adapter.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        ds.Tables.Add(table);
        //        adapter.Fill(ds);
        //        return ds.Tables[0];
        //    }
        //}
    }
}
