using System;
using System.Data.Common;
using System.Data.Entity;
using Mini.Framework.EFCommon;

namespace Mini.Framework.Datagrid.DBService
{
    public class DB : BasicDb<DB>
    {
        public DB(String nameOrConnectionString = "Mini.Framework.Datagrid.ConnectionStr")
            : base(nameOrConnectionString)
        { }

        public DB(DbConnection existingConnection, Boolean contextOwnsConnection = true)
            : base(existingConnection, contextOwnsConnection)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datagrid>().Map(it =>
            {
                it.ToTable(Helper.AsmConfiger.ConfigFileReader.AppSettings("Datagrid_TableName"));
            });

            modelBuilder.Entity<DatagridCol>().Map(it =>
            {
                it.ToTable(Helper.AsmConfiger.ConfigFileReader.AppSettings("DatagridCol_TableName"));
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Datagrid> Datagrids { get; set; }
    }
}
