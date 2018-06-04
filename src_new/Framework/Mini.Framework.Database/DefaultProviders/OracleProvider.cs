using System;
using System.Data.Common;
#if !NETSTANDARD2_0
using System.Data.OracleClient;
#endif

namespace Mini.Framework.Database.DefaultProviders
{
#if !NETSTANDARD2_0
    public class OracleProvider : DBCharacterProvider
    {
        protected String _connStr = String.Empty;
        public OracleProvider(String connectionString)
        {
            this._connStr = connectionString;
        }

        public override DbProviderFactory DbProviderFactory => OracleClientFactory.Instance;

        public override string GetConnectionString()
        {
            return _connStr;
        }
    }
#endif
}
