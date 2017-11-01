using System;
using System.Collections;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Foundation.Basic.Cache.Interface
{
    /// <summary>
    /// 字段缓存区的接口
    /// </summary>
    public interface IDictionaryCachePool<TKey, TValue> : IEnumerable, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Boolean TryAdd(TKey key, TValue value, CollectionsAddOper addOper = CollectionsAddOper.IgnoreIfExist);

        Boolean TryGetValue(TKey key,
                            out TValue value,
                            CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                            TValue defaultValue = default(TValue));

        Boolean Remove(TKey key);

        Boolean Remove(TKey key, out TValue value);

        Boolean ContainsKey(TKey key);

        Boolean ContainsValue(TValue value);
        Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer);
        Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer = null);

        void Clear();
    }
}
