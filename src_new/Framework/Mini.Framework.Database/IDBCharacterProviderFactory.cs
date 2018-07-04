using System;

namespace Mini.Framework.Database
{
    public interface IDBCharacterProviderFactory
    {
        DBCharacterProvider Create();
    }
}
