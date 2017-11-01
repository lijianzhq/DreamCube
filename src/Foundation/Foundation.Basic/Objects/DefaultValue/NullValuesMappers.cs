using System;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Objects.DefaultValue
{
    /// <summary>
    /// 默认值对象
    /// </summary>
    public class NullValuesMappers
    {
        /// <summary>
        /// 默认值链表
        /// </summary>
        private Dictionary<String, NullValues> defaultValueList = new Dictionary<String, NullValues>();

        /// <summary>
        /// 添加一个默认值映射关系
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <param name="value">类型为NULL或者为DBNull时的默认值</param>
        /// <param name="addOper">添加操作；默认的操作是：存在同类型的转换器时，则不添加</param>
        public void AddDefaultValue(Type type, NullValues value, Enums.CollectionsAddOper addOper = Enums.CollectionsAddOper.IgnoreIfExist)
        {
            String typeString = type.ToString();
            MyDictionary.TryAdd(defaultValueList, typeString, value, addOper);
        }

        /// <summary>
        /// 或者指定类型类型为NULL或者为DBNull时的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public NullValues GetDefaultValue(Type type)
        {
            String typeString = type.ToString();
            NullValues value = null;
            MyDictionary.TryGetValue(defaultValueList, typeString, out value, Enums.CollectionsGetOper.DefaultValueIfNotExist, null);
            return value;
        }
    }
}
