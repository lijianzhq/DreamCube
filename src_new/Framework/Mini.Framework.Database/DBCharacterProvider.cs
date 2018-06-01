using System;
using System.Data;
using System.Data.Common;

namespace Mini.Framework.Database
{
    public abstract class DBCharacterProvider
    {
        public abstract String GetConnectionString();

        public abstract DbProviderFactory DbProviderFactory { get; }

        public virtual String FormatParameterName(String name)
        {
            return name;
        }
    }
}
