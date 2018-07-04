using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Mini.Foundation.LogService;
using Mini.Framework.EFCommon;

namespace Mini.Framework.WebUploader.DBService
{
    public class DB : BasicDb<DB>
    {
        public DB(String nameOrConnectionString = "Mini.Framework.WebUploader.ConnectionStr")
            : base(nameOrConnectionString)
        { }

        public DB(DbConnection existingConnection, Boolean contextOwnsConnection = true)
            : base(existingConnection, contextOwnsConnection)
        { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UploadFile>().Map(it =>
            {
                it.ToTable(Helper.AsmConfiger.ConfigFileReader.AppSettings("UploadFile_TableName"));
            });

            modelBuilder.Entity<UploadFileOpHistory>().Map(it =>
            {
                it.ToTable(Helper.AsmConfiger.ConfigFileReader.AppSettings("UploadFileOpHistory_TableName"));
            });
            base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UploadFile> UploadFiles { get; set; }

        public DbSet<UploadFileOpHistory> UploadFileOpHistorys { get; set; }

        public static String GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static void SaveUploadFileRecord(UploadFile filedata)
        {
            using (var db = Helper.CreateEFDB())
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
