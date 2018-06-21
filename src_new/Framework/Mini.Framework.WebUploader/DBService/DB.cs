using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Mini.Framework.WebUploader.DBService
{
    public class Initializer : CreateDatabaseIfNotExists<DB>
    { }

    public class DB : DbContext
    {
        static DB()
        {
            Database.SetInitializer(new Initializer());
        }

        public static void Init(IDatabaseInitializer<DB> strategy)
        {
            Database.SetInitializer(strategy);
            using (var db = new DB())
            {
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
            base.OnModelCreating(modelBuilder);
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
