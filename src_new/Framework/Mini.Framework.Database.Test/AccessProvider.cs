using System;
using System.Data.OleDb;
using System.Data.Common;

using Mini.Framework.Database;

namespace Mini.Framework.Database.Test
{
    class AccessProvider : DBCharacterProvider
    {
        protected String _filePath = String.Empty;
        public AccessProvider(String filePath)
        {
            this._filePath = filePath;
        }

        public override DbProviderFactory DbProviderFactory => OleDbFactory.Instance;

        public override string GetConnectionString()
        {
            return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_filePath};Persist Security Info=False";
        }
    }
}
