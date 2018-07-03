using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

using Mini.Foundation.LogService;

namespace Mini.Framework.EFCommon
{
    /// <summary>
    /// do nothing
    /// </summary>
    public class Initializer0<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public void InitializeDatabase(T context)
        {
            return;
        }
    }

    /// <summary>
    /// CreateDatabaseIfNotExists
    /// </summary>
    public class Initializer1<T> : CreateDatabaseIfNotExists<T> where T : DbContext
    {
    }

    /// <summary>
    /// DropCreateDatabaseIfModelChanges
    /// </summary>
    public class Initializer2<T> : DropCreateDatabaseIfModelChanges<T> where T : DbContext
    {
    }

    /// <summary>
    /// DropCreateDatabaseAlways
    /// </summary>
    public class Initializer3<T> : DropCreateDatabaseAlways<T> where T : DbContext
    {
    }

    public class BasicDb<T> : DbContext where T : DbContext
    {
        protected void Init()
        {
            String type = ConfigurationManager.AppSettings["DBInitializer"];
            if (String.IsNullOrEmpty(type))
                type = Helper.AsmConfiger.ConfigFileReader.AppSettings("DBInitializer");
            if (String.IsNullOrEmpty(type)) type = "0";
            switch (type)
            {
                case "0":
                    Database.SetInitializer<T>(null);
                    break;
                case "1":
                    Database.SetInitializer(new Initializer1<T>());
                    break;
                case "2":
                    Database.SetInitializer(new Initializer2<T>());
                    break;
                case "3":
                    Database.SetInitializer(new Initializer3<T>());
                    break;
                default:
                    break;
            }
        }

        public BasicDb()
        {
            Init();
        }

        public BasicDb(String nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Init();
        }

        public BasicDb(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            Init();
        }

        public BasicDb(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            Init();
        }

        public BasicDb(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            Init();
        }

        public BasicDb(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            Init();
        }

        /// <summary>
        /// 外部还可以传入策略
        /// </summary>
        /// <param name="strategy"></param>
        public static void Init(IDatabaseInitializer<T> strategy)
        {
            Database.SetInitializer<T>(strategy);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //解决EF动态建库数据库表名变为复数问题  
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //解决oracle的默认架构的问题
            var schema = GetDefaultSchema();
            if (!String.IsNullOrEmpty(schema))
                modelBuilder.HasDefaultSchema(schema);
            base.OnModelCreating(modelBuilder);
        }

        protected virtual String GetDefaultSchema()
        {
            String connStr = Database.Connection.ConnectionString;
            String[] connParts = connStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < connParts.Length; i++)
            {
                var configParams = connParts[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (configParams.Length == 2
                    && configParams[0].StartsWith("user", StringComparison.CurrentCultureIgnoreCase)
                    && configParams[0].EndsWith("ID", StringComparison.CurrentCultureIgnoreCase))
                    return configParams[1].ToUpper();
            }
            return String.Empty;
        }
    }
}
