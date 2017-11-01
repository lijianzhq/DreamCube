using System;
using System.Collections;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET20 || NET35 || NET30

    /// <summary>
    /// 基于2.0的缓冲区实现方法
    /// </summary>
    internal class DictionaryCachePool_Net20<TKey, TValue> : Interface.IDictionaryCachePool<TKey, TValue>
    {
        #region "字段"

        private Dictionary<TKey, TValue> innerCache = null;

        private Object locker = new Object();

        #endregion

        #region "构造方法"

        public DictionaryCachePool_Net20(IEqualityComparer<TKey> defaultKeyComparer = null)
        {
            if (defaultKeyComparer == null)
                innerCache = new Dictionary<TKey, TValue>();
            else
                innerCache = new Dictionary<TKey, TValue>(defaultKeyComparer);
        }

        #endregion

        #region IDictionaryCachePool<TKey,TValue> 成员

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            lock (locker)
            {
                innerCache.Clear();
            }
        }

        /// <summary>
        /// 移除指定键值项
        /// 如果成功找到并移除该元素，则为 true；否则为 false。 如果在 Dictionary<TKey, TValue>中没有找到 key，此方法则返回 false。 
        /// </summary>
        /// <param name="key">要移除的元素的键</param>
        public Boolean Remove(TKey key)
        {
            lock (locker)
            {
                return innerCache.Remove(key);
            }
        }

        /// <summary>
        /// 移除指定键值项
        /// 如果成功找到并移除该元素，则为 true；否则为 false。 如果在 Dictionary<TKey, TValue>中没有找到 key，此方法则返回 false。 
        /// </summary>
        /// <param name="key">要移除的元素的键</param>
        /// <param name="value">返回移除的项值</param>
        public Boolean Remove(TKey key, out TValue value)
        {
            lock (locker)
            {
                value = default(TValue);
                if (innerCache.ContainsKey(key))
                {
                    value = innerCache[key];
                    return innerCache.Remove(key);
                }
                return false;
            }
        }

        /// <summary>
        /// 往集合中添加一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="addOper"></param>
        /// <returns></returns>
        public Boolean TryAdd(TKey key, TValue value, CollectionsAddOper addOper = CollectionsAddOper.IgnoreIfExist)
        {
            lock (locker)
            {
                return MyDictionary.TryAdd(innerCache, key, value, addOper);
            }
        }

        /// <summary>
        /// 根据键获取对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="getOper"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Boolean TryGetValue(TKey key,
                                   out TValue value,
                                   CollectionsGetOper getOper = CollectionsGetOper.DefaultValueIfNotExist,
                                   TValue defaultValue = default(TValue))
        {
            lock (locker)
            {
                return MyDictionary.TryGetValue(innerCache, key, out value, getOper, defaultValue);
            }
        }

        /// <summary>
        /// 判断集合中是否存在指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key">包含改值对应的键</param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, out TKey key)
        {
            return MyDictionary.ContainsValue(innerCache, value, out key);
        }

        /// <summary>
        /// 判断集合中是否存在指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value)
        {
            return innerCache.ContainsValue(value);
        }

        /// <summary>
        /// 判断集合中是否存在指定的键（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(TKey key)
        {
            return innerCache.ContainsKey(key);
        }

        /// <summary>
        /// 判断集合中是否存在指定的项值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer)
        {
            return MyDictionary.ContainsValue<TKey, TValue>(innerCache, value, valueComparer);
        }

        /// <summary>
        /// 判断集合中是否存在指定的项值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer = null)
        {
            key = default(TKey);
            return MyDictionary.ContainsValue<TKey, TValue>(innerCache, value, out key, valueComparer);
        }

        #endregion

        #region "可遍历的接口方法"

        public IEnumerator GetEnumerator()
        {
            return innerCache.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return innerCache.GetEnumerator();
        }

        #endregion
    }

#endif
}
