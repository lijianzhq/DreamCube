using System;
using System.Collections;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Objects;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// 弱引用的字典缓冲区
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class WeakDictionaryCachePool<TKey, TValue> : Interface.IWeakDictionaryCachePool<TKey, TValue>
            where TValue : class
    {
        #region "字段"

        private Interface.IWeakDictionaryCachePool<TKey, TValue> innerCache = null;

        #endregion

        #region "构造方法"

        public WeakDictionaryCachePool(IEqualityComparer<TKey> defaultKeyComparer = null)
        {
#if NET20 || NET35 || NET30
            innerCache = new WeakDictionaryCachePool_Net20<TKey, TValue>(defaultKeyComparer);
#elif NET40 
            innerCache = new WeakDictionaryCachePool_Net40<TKey, TValue>(defaultKeyComparer);
#endif
        }

        #endregion

        #region "公共方法"

        public Boolean ContainsValue(TValue value)
        {
            return innerCache.ContainsValue(value);
        }

        public Boolean ContainsValue(TValue value, out TKey key)
        {
            return innerCache.ContainsValue(value, out key);
        }

        public void UpToStrongReference()
        {
            innerCache.UpToStrongReference();
        }

        public void DownToWeakReference()
        {
            innerCache.DownToWeakReference();
        }

        public Boolean TryAdd(TKey key, TValue value, CollectionsAddOper addOper = CollectionsAddOper.IgnoreIfExist)
        {
            return innerCache.TryAdd(key, value, addOper);
        }

        public Boolean TryGetValue(TKey key, out TValue value, CollectionsGetOper getOper = CollectionsGetOper.DefaultValueIfNotExist, TValue defaultValue = default(TValue))
        {
            return innerCache.TryGetValue(key, out value, getOper, defaultValue);
        }

        public Boolean Remove(TKey key)
        {
            return innerCache.Remove(key);
        }

        public Boolean Remove(TKey key, out TValue value)
        {
            return innerCache.Remove(key, out value);
        }

        public Boolean ContainsKey(TKey key)
        {
            return innerCache.ContainsKey(key);
        }

        public void Clear()
        {
            innerCache.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return innerCache.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)innerCache).GetEnumerator();
        }

        public Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer = null)
        {
            return innerCache.ContainsValue(value, out key, valueComparer);
        }

        public Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer)
        {
            return innerCache.ContainsValue(value, valueComparer);
        }

        #endregion
    }
}
