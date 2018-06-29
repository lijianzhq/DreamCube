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
    public class Initializer0 : IDatabaseInitializer<BasicDb>
    {
        public void InitializeDatabase(BasicDb context)
        {
            return;
        }
    }

    /// <summary>
    /// CreateDatabaseIfNotExists
    /// </summary>
    public class Initializer1 : CreateDatabaseIfNotExists<BasicDb>
    {
    }

    /// <summary>
    /// DropCreateDatabaseIfModelChanges
    /// </summary>
    public class Initializer2 : DropCreateDatabaseIfModelChanges<BasicDb>
    {
    }

    /// <summary>
    /// DropCreateDatabaseAlways
    /// </summary>
    public class Initializer3 : DropCreateDatabaseAlways<BasicDb>
    {
    }

    public class BasicDb : DbContext
    {
        static BasicDb()
        {
            String type = ConfigurationManager.AppSettings["DBInitializer"];
            if (String.IsNullOrEmpty(type))
                type = Helper.AsmConfiger.AppSettings("DBInitializer");
            if (String.IsNullOrEmpty(type)) type = "0";
            switch (type)
            {
                case "0":
                    Database.SetInitializer(new Initializer0());
                    break;
                case "1":
                    Database.SetInitializer(new Initializer1());
                    break;
                case "2":
                    Database.SetInitializer(new Initializer2());
                    break;
                case "3":
                    Database.SetInitializer(new Initializer3());
                    break;
                default:
                    break;
            }
        }

        public BasicDb()
        { }

        public BasicDb(String nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public BasicDb(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        { }

        public BasicDb(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        { }

        public BasicDb(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        { }
        public BasicDb(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        { }

        /// <summary>
        /// 外部还可以传入策略
        /// </summary>
        /// <param name="strategy"></param>
        public static void Init(IDatabaseInitializer<BasicDb> strategy)
        {
            Database.SetInitializer(strategy);
            using (var db = new BasicDb())
            {
                Log.Root.LogInfo($"database ServerVersion:{db.Database.Connection.ServerVersion}");
                db.Database.Initialize(false);
            }
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

        public static String GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
