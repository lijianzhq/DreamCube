using System;
#if !NETSTANDARD2_0
using System.Data.OleDb;
#endif
using System.Data.Common;

namespace Mini.Framework.Database.DefaultProviders
{
#if !NETSTANDARD2_0
    public class AccessProvider : DBCharacterProvider
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
#endif
}
