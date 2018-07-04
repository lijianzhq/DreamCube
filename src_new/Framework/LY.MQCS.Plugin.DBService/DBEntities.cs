using System;
using System.Data.Common;
using System.Data.Entity;

using Mini.Framework.EFCommon;

namespace LY.MQCS.Plugin.DBService
{
    public class DBEntities : BasicDb<DBEntities>
    {
        internal DBEntities(String nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        internal DBEntities(DbConnection conn, Boolean autoCloseConn)
            : base(conn, autoCloseConn)
        { }

        public DbSet<PQ.T_PQ_BU_PROD_GROUP_CM> T_PQ_BU_PROD_GROUP_CM { get; set; }

        public DbSet<MQCSBUS.T_PQ_BU_DATAGRID> T_PQ_BU_DATAGRID { get; set; }
    }
}
