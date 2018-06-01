using System;
using System.IO;
using System.Data.SqlServerCe;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.DataAccess.SqlCe
{
    public class SqlCeDbFactory : DBFactory
    {
        public enum CreateType
        {
            /// <summary>
            /// 创建一个全新的数据库实例
            /// </summary>
            NewInstance,

            /// <summary>
            /// 打开存在的数据库
            /// </summary>
            OpenExist
        }

        /// <summary>
        /// 获取数据库对象实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public override Database CreateDB(String connectionString)
        {
            return CreateDatabase(connectionString, CreateType.OpenExist);
        }

        /// <summary>
        /// 创建一个新的sqlce数据库，如果数据库已经存在，则删除原来的数据库（传入数据库路径）
        /// 不验证数据库指定的数据库名是否已经存在
        /// </summary>
        /// <param name="dbSource"></param>
        public static Database CreateDatabase(String connectionString, CreateType createType = CreateType.OpenExist)
        {
            Database db = new SqlCeDb(connectionString, SqlCeProviderFactory.Instance);
            ConnectionString connectionStringObj = db.ConnectionString;
            String dbPath = connectionStringObj.DataSource;
            if (File.Exists(dbPath))
            {
                if (createType == CreateType.NewInstance)
                {
                    File.Delete(dbPath);
                    SqlCeEngine engine = new SqlCeEngine(connectionString);
                    engine.CreateDatabase();
                }
            }
            else
            {
                SqlCeEngine engine = new SqlCeEngine(connectionString);
                engine.CreateDatabase();
            }
            return db;
        }
    }
}
