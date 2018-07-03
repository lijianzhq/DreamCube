using System;

using Mini.Framework.EFCommon;

namespace LY.MQCS.Plugin.DBService
{
    public class MQCSBUSEntities : BasicDb<MQCSBUSEntities>
    {
        internal MQCSBUSEntities(String nameOrConnectionString)
            : base(nameOrConnectionString)
        { }
    }
}
