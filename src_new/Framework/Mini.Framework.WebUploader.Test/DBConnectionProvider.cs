using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

using Mini.Framework.EFCommon;
using Oracle.ManagedDataAccess.Client;

namespace Mini.Framework.WebUploader.Test
{
    public class DBConnectionProvider : IDBConnectionProvider
    {
        public DbConnection CreateConnection(bool open = true)
        {
            return new OracleConnection("User Id=test;Password=guanliyuan;Data Source=orcl");
        }
    }
}