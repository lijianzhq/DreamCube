using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Mini.Foundation.LogService;
using Mini.Framework.EFCommon;

namespace Mini.Framework.WebUploader.DBService
{
    public class DB : BasicDb
    {
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
