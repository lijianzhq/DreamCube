using System;
using System.Reflection;

using Mini.Foundation.Basic.Utility;
using Mini.Framework.Database;
using Mini.Framework.EFCommon;

namespace Mini.Framework.Datagrid
{
    public static class Helper
    {
        public const String RESOURCE_NS = @"\Mini.Framework.Datagrid.xml";

        static IDBConnectionProvider _connectionProvider = null;
        internal static IDBConnectionProvider ConnectionProvider
        {
            get
            {
                if (_connectionProvider == null)
                {
                    _connectionProvider = CreateInstance<IDBConnectionProvider>("DBConnectionProvider");
                }
                return _connectionProvider;
            }
        }

        static IDBCharacterProviderFactory _dbCharacterProviderFactory = null;
        internal static IDBCharacterProviderFactory DBCharacterProviderFactory
        {
            get
            {
                if (_dbCharacterProviderFactory == null)
                {
                    _dbCharacterProviderFactory = CreateInstance<IDBCharacterProviderFactory>("DBCharacterProviderFactory");
                }
                return _dbCharacterProviderFactory;
            }
        }

        internal static T CreateInstance<T>(String appSettingKey)
        {
            var typeName = AsmConfigerHelper.GetConfiger().ConfigFileReader.AppSettings(appSettingKey);
            if (!String.IsNullOrEmpty(typeName))
            {
                var type = Type.GetType(typeName);
                return (T)type.Assembly.CreateInstance(type.FullName);
            }
            return default(T);
        }

        public static DBService.DB CreateEFDB(Boolean autoCloseConn = true)
        {
            if (ConnectionProvider != null)
                return new DBService.DB(ConnectionProvider.CreateConnection(false), autoCloseConn);
            else
                return new DBService.DB();
        }

        internal static DB CreateDBObj()
        {
            return new DB(Helper.DBCharacterProviderFactory.Create());
        }
    }
}
