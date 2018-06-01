using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Data.Common;
using System.Xml;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 数据库管理类；
    /// 数据库管理类加载数据库对象时，
    /// 都是从应用程序的config文件的connectionString节点获得连接字符串来创建的
    /// </summary>
    public static class DBManager
    {
        #region "字段"

        /// <summary>
        /// 缓存数据库的实例
        /// </summary>
        private static Dictionary<String, Database> dbList =  new Dictionary<String, Database>(new StringEqualityIgnoreCaseComparerGeneric());

        #endregion

        #region "公共方法"

        /// <summary>
        /// 根据连接字符串对象获取Database对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Database GetDBByConnectionString(ConnectionString connectionString)
        {
            if (connectionString != null)
                return GetDBByConnectionString(connectionString.ToString(), connectionString.DBProviderType);
            return null;
        }

        /// <summary>
        /// 根据链接字符串获得Database对象
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static Database GetDBByConnectionString(String connectionStr, DBProviderType providerType)
        {
            return GetDBByConnectionString(connectionStr, providerType.ToString());
        }

        /// <summary>
        /// 根据链接字符串获得Database对象
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static Database GetDBByConnectionString(String connectionStr, String providerType = "SqlClient")
        {
            if (String.IsNullOrEmpty(connectionStr)) throw new ArgumentNullException("connectionStr");
            Database db = null;
            if (dbList.TryGetValue(connectionStr, out db)) return db;
            Config.DBFactoryElement factoryElement = DBFactoryHelper.GetFactoryElementByProviderType(providerType);
            DBFactory factory = null;
            if (MyReflection.TryCreateNewInstanceByTypeFullName(factoryElement.DBFactoryType, out factory, true))
                db = factory.CreateDB(connectionStr);
            else
                return null;
            //把数据库实例添加到缓存中
            AddDBToCache(db, connectionStr);
            return db;
        }

        /// <summary>
        /// 获取默认的数据库名
        /// 也就是获取配置文件中，ConnectionStrings的第一个ConnectionString节点内容
        /// </summary>
        /// <returns></returns>
        public static String GetDefaultDBName()
        {
            if (ConfigurationManager.ConnectionStrings.Count > 0)
                return ConfigurationManager.ConnectionStrings[0].Name;
            return String.Empty;
        }

        /// <summary>
        /// 根据app.config(web.config)中的ConnectionStrings配置的连接字符串name值，获取对应的数据库实例；
        /// 此方法启用了缓存机制，并且支持单例模式，一个数据库名字只创建一个数据库实例
        /// </summary>
        /// <param name="name">如果不传入数据库名，则默认获取配置文件中的第一个配置连接对象，如果没有配置任何的数据库连接信息，则抛出异常</param>
        /// <returns></returns>
        public static Database GetDBByName(String name = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                    name = ConfigurationManager.ConnectionStrings[0].Name;
                else
                    throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(name)) return null;
            Database db = null;
            if (dbList.TryGetValue(name, out db)) return db;
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (String.IsNullOrEmpty(connectionStringSettings.ProviderName))
                throw new ArgumentNullException("name", Properties.Resources.ExceptionNotSetProviderName);

            Config.DBFactoryElement factoryElement = DBFactoryHelper.GetFactoryElementByProviderName(connectionStringSettings.ProviderName);
            DBFactory factory = null;
            if (MyReflection.TryCreateNewInstanceByTypeFullName(factoryElement.DBFactoryType, out factory, true))
                db = factory.CreateDB(connectionStringSettings.ConnectionString);
            else
                return null;
            //把数据库实例添加到缓存中
            AddDBToCache(db, name);
            //返回缓冲区中的数据库对象实例，因为有可能多线程创建了多个实例了
            return dbList[name];
        }

        /// <summary>
        /// 根据配置文件获取数据库实例
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="dbName">数据库名字(如果不传入，则默认获取第一个数据库)</param>
        /// <returns></returns>
        public static Database GetDBFromConfigFile(String configFilePath, String dbName = "")
        {
            String connectionStrs = String.Empty;
            String providerName = String.Empty;
            if (!String.IsNullOrEmpty(configFilePath))
            {
                XmlNodeList nodes = MyXml.GetXmlNodes(configFilePath, "/ConnectionStrings/ConnectionString");
                if (nodes != null)
                {
                    Int32 length = nodes.Count;
                    for (var i = 0; i < length; i++)
                    {
                        String name = nodes[i].Attributes["name"].Value;
                        if (dbName == name)
                        {
                            connectionStrs = nodes[i].Attributes["value"].Value;
                            providerName = nodes[i].Attributes["providerType"].Value;
                            break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("读取数据库配置文件出错！");
                }
            }
            if (String.IsNullOrEmpty(dbName)) return null;
            Database db = null;
            if (dbList.TryGetValue(dbName, out db)) return db;
            if (String.IsNullOrEmpty(providerName))
                throw new ArgumentNullException("name", Properties.Resources.ExceptionNotSetProviderName);

            Config.DBFactoryElement factoryElement = DBFactoryHelper.GetFactoryElementByProviderName(providerName);
            DBFactory factory = null;
            if (MyReflection.TryCreateNewInstanceByTypeFullName(factoryElement.DBFactoryType, out factory, true))
                db = factory.CreateDB(connectionStrs);
            else
                return null;
            //把数据库实例添加到缓存中
            AddDBToCache(db, dbName);
            //返回缓冲区中的数据库对象实例，因为有可能多线程创建了多个实例了
            return dbList[dbName];
        }

        /// <summary>
        /// 根据app.config(web.config)中的ConnectionStrings配置的连接字符串name值，获取对应的数据库实例；
        /// 此方法启用了缓存机制，并且支持单例模式，一个数据库名字只创建一个数据库实例
        /// </summary>
        /// <param name="name">如果不传入数据库名，则默认获取配置文件中的第一个配置连接对象，如果没有配置任何的数据库连接信息，则抛出异常</param>
        /// <returns></returns>
        public static T GetDBByName<T>(String name = "") where T : Database
        {
            Database db = GetDBByName(name);
            if (db != null) return db as T;
            return null;
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 把数据库实例添加到缓存中
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        private static void AddDBToCache(Database db, String key)
        {
            try
            {
                //捕获重复键值的异常
                dbList.Add(key, db);
                //MyDictionary.TryAdd(dbList, key, db, Foundation.Basic.Enums.CollectionsAddOper.IgnoreIfExist);
            }
            catch (ArgumentException)
            { 
                //忽略重复键值的异常
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        #endregion
    }
}
