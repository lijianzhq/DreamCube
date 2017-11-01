using System;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// 字典缓存类；线程安全；比微软封装的ConcurrentDictionary快很多
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryCachePool<TKey, TValue> : Interface.IDictionaryCachePool<TKey, TValue>
    {
        #region "字段"

        private Interface.IDictionaryCachePool<TKey, TValue> innerCache = null;

        #endregion

        #region "构造方法"

        /// <summary>
        /// 传入默认的键值比较委托来创建一个缓冲区
        /// </summary>
        /// <param name="defaultKeyComparer"></param>
        public DictionaryCachePool(IEqualityComparer<TKey> defaultKeyComparer = null)
        {
#if NET20 || NET35 || NET30
            innerCache = new DictionaryCachePool_Net20<TKey, TValue>(defaultKeyComparer);
#elif NET40
            innerCache = new DictionaryCachePool_Net40<TKey, TValue>(defaultKeyComparer);
#endif
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 根据键值获取对应给定键的value值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value = default(TValue);
                this.TryGetValue(key, out value);
                return value;
            }
            set
            {
                this.TryAdd(key, value);
            }
        }

        /// <summary>
        /// 移除指定键值项
        /// 如果成功找到并移除该元素，则为 true；否则为 false。 如果在 Dictionary<TKey, TValue>中没有找到 key，此方法则返回 false。 
        /// </summary>
        /// <param name="key">要移除的元素的键</param>
        public Boolean Remove(TKey key)
        {
            return innerCache.Remove(key);
        }

        /// <summary>
        /// 移除指定键值项
        /// 如果成功找到并移除该元素，则为 true；否则为 false。 如果在 Dictionary<TKey, TValue>中没有找到 key，此方法则返回 false。 
        /// </summary>
        /// <param name="key">要移除的元素的键</param>
        /// <param name="value">返回移除的项值</param>
        public Boolean Remove(TKey key, out TValue value)
        {
            return innerCache.Remove(key, out value);
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
            return innerCache.TryAdd(key, value, addOper);
        }

        /// <summary>
        /// 根据键值从集合中获取对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue">当集合中不存在指定的键项时，则返回此默认值</param>
        /// <returns></returns>
        public Boolean TryGetValue(TKey key, out TValue value, CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist, TValue defaultValue = default(TValue))
        {
            return innerCache.TryGetValue(key, out value, type, defaultValue);
        }

        /// <summary>
        /// 判断是否包含了指定的键值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(TKey key)
        {
            return innerCache.ContainsKey(key);
        }

        /// <summary>
        /// 判断集合中是否包含了指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value)
        {
            return innerCache.ContainsValue(value);
        }

        /// <summary>
        /// 判断集合中是否包含指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value">需要查询的值</param>
        /// <param name="key">对应该值的键</param>
        /// <param name="valueComparer">客户端可以自己传入值比较的委托方法</param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer)
        {
            return innerCache.ContainsValue(value, out key, valueComparer);
        }

        /// <summary>
        /// 判断集合中是否包含指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value">需要查询的值</param>
        /// <param name="valueComparer">客户端可以自己传入值比较的委托方法</param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer)
        {
            return innerCache.ContainsValue(value, valueComparer);
        }

        /// <summary>
        /// 清除缓存中所有项
        /// </summary>
        public void Clear()
        {
            innerCache.Clear();
        }

        /// <summary>
        /// 实现对象的foreach
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return innerCache.GetEnumerator();
        }

        /// <summary>
        /// 实现对象的foreach
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)innerCache).GetEnumerator();
        }

        #endregion
    }
}
