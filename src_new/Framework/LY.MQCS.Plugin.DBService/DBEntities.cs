using System;

using Mini.Framework.EFCommon;

namespace LY.MQCS.Plugin.DBService
{
    public class DBEntities : BasicDb<DBEntities>
    {
        internal DBEntities(String nameOrConnectionString)
            : base(nameOrConnectionString)
        { }
    }
}
