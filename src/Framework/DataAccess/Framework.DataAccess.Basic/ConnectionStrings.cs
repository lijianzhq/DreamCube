using System;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 连接字符串类
    /// </summary>
    public class ConnectionStrings
    {
        private Dictionary<String, ConnectionString> innerDictionary = new Dictionary<String, ConnectionString>();

        internal void Add(String key, ConnectionString value)
        {
            MyDictionary.TryAdd(innerDictionary, key, value, Foundation.Basic.Enums.CollectionsAddOper.ReplaceIfExist);
        }

        public ConnectionString this[String key]
        {
            get { return MyDictionary.GetValue(innerDictionary, key, Foundation.Basic.Enums.CollectionsGetOper.DefaultValueIfNotExist, null); }
        }

        public ConnectionString this[Int32 index]
        {
            get { return MyDictionary.GetValue(innerDictionary, index, Foundation.Basic.Enums.CollectionsGetOper.DefaultValueIfNotExist, null); }
        }
    }
}
