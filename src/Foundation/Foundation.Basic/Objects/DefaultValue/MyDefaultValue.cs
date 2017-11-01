using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Objects.DefaultValue
{
    public static class MyDefaultValue
    {
        /// <summary>
        /// 获取系统类型的默认值
        /// </summary>
        /// <param name="systemType"></param>
        /// <returns></returns>
        public static Object GetSystemTypeDefaultValue(Type systemType)
        {
            if (systemType == null) return null;
            //不是值类型，默认值肯定是NULL
            if (!systemType.IsValueType) return null;
            String typeString = systemType.ToString();
            switch (typeString)
            {
                case "System.String": return default(System.String);
                case "System.Char": return default(System.Char);
                case "System.Single": return default(System.Single);
                case "System.Int32": return default(System.Int32);
                case "System.Int64": return default(System.Int64);
                case "System.Int16": return default(System.Int16);
                case "System.Double": return default(System.Double);
                case "System.Boolean": return default(System.Boolean);
                case "System.UInt16": return default(System.UInt16);
                case "System.UInt32": return default(System.UInt32);
                case "System.UInt64": return default(System.UInt64);
                case "System.DateTime": return default(System.DateTime);
                case "System.Decimal": return default(System.Decimal);
                default:
                    return null;
            }
        }
    }
}
