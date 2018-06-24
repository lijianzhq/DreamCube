using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Mini.Foundation.LogService;

namespace Mini.Framework.WebUploader.DBService
{
    public class Initializer0 : IDatabaseInitializer<DB>
    {
        public void InitializeDatabase(DB context)
        {
            return;
        }
    }

    public class Initializer1 : CreateDatabaseIfNotExists<DB>
    {
    }

    public class Initializer2 : DropCreateDatabaseIfModelChanges<DB>
    {
    }

    public class Initializer3 : DropCreateDatabaseAlways<DB>
    {
    }

    public class DB : DbContext
    {
        static DB()
        {
            String type = Helper.AsmConfiger.AppSettings("DBInitializer");
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

        /// <summary>
        /// 外部还可以传入策略
        /// </summary>
        /// <param name="strategy"></param>
        public static void Init(IDatabaseInitializer<DB> strategy)
        {
            Database.SetInitializer(strategy);
            using (var db = new DB())
            {
                Log.Root.LogInfo($"database ServerVersion:{db.Database.Connection.ServerVersion}");
                db.Database.Initialize(false);
            }
        }

        public DB(String nameOrConnectionString = "UploadFileDB")
            : base(nameOrConnectionString)
        { }

        public DB(DbConnection existingConnection, Boolean contextOwnsConnection = true)
            : base(existingConnection, contextOwnsConnection)
        { }

        public DbSet<UploadFile> UploadFiles { get; set; }

        public DbSet<UploadFileOpHistory> UploadFileOpHistorys { get; set; }

        public static String GetGuid()
        {
            return Guid.NewGuid().ToString("N");
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
                if (configParams.Length == 2 && configParams[0] == "User ID")
                    return configParams[1].ToUpper();
            }
            return String.Empty;
        }

        public static void SaveUploadFileRecord(UploadFile filedata)
        {
            using (var db = new DB())
            {
                filedata.OpHistory = new List<UploadFileOpHistory>() {
                     new UploadFileOpHistory()
                     {
                          CreateBy = filedata.CreateBy,
                          LastUpdateBy = filedata.LastUpdateBy,
                          OpType = FileOpType.Create,
                     }
                };
                db.UploadFiles.Add(filedata);
                db.SaveChanges();
            }
        }
    }
}
