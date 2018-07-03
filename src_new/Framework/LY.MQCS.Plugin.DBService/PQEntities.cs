using System;
using System.Data.Common;
using System.Data.Entity;

using Mini.Framework.EFCommon;

namespace LY.MQCS.Plugin.DBService
{
    public class PQEntities : BasicDb<PQEntities>
    {
        internal PQEntities(String nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        internal PQEntities(DbConnection conn, Boolean autoCloseConn)
            : base(conn, autoCloseConn)
        {
        }

        public DbSet<PQ.T_PQ_BU_PROD_GROUP_CM> T_PQ_BU_PROD_GROUP_CM { get; set; }
    }
}
