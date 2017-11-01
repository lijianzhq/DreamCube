using System;
using System.Threading;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET40

    internal class DictionaryCachePool_Net40<TKey, TValue> : Interface.IDictionaryCachePool<TKey, TValue>
    {
        #region "私有字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private Dictionary<TKey, TValue> cacheBlock = new Dictionary<TKey, TValue>();

        /// <summary>
        /// 读写锁，支持递归进入读锁
        /// </summary>
        private ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// 读锁获取超时时间
        /// </summary>
        private TimeSpan readLockTimeout = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// 写锁获取尝试时间
        /// </summary>
        private TimeSpan writeLockTimeout = TimeSpan.FromMilliseconds(500);

        #endregion

        #region "构造方法"

        /// <summary>
        /// 传入默认的键值比较委托来创建一个缓冲区
        /// </summary>
        /// <param name="defaultKeyComparer"></param>
        public DictionaryCachePool_Net40(IEqualityComparer<TKey> defaultKeyComparer = null)
        {
            if (defaultKeyComparer == null) cacheBlock = new Dictionary<TKey, TValue>();
            else cacheBlock = new Dictionary<TKey, TValue>(defaultKeyComparer);
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 根据键值移除指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean Remove(TKey key)
        {
            TValue value;
            return Remove(key, out value);
        }

        /// <summary>
        /// 根据键值移除指定的项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean Remove(TKey key, out TValue value)
        {
            value = default(TValue);
            if (rwl.TryEnterWriteLock(readLockTimeout))
            {
                try
                {
                    //如果底层的锁不允许递归获取，则此处会报错
                    if (cacheBlock.TryGetValue(key, out value, CollectionsGetOper.DefaultValueIfNotExist))
                    {
                        cacheBlock.Remove(key);
                        return true;
                    }
                    return false;
                }
                finally { rwl.ExitWriteLock(); }
            }
            return false;
        }

        /// <summary>
        /// 判断是否包含了指定的键值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(TKey key)
        {
            return cacheBlock.ContainsKey(key);
        }

        /// <summary>
        /// 判断集合中是否包含了指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value)
        {
            return cacheBlock.ContainsValue(value);
        }

        /// <summary>
        /// 判断集合中是否包含指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value">需要查询的值</param>
        /// <param name="key">对应该值的键</param>
        /// <param name="valueComparer">客户端可以自己传入值比较的委托方法</param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, bool> valueComparer = null)
        {
            key = default(TKey);
            return MyDictionary.ContainsValue<TKey, TValue>(cacheBlock, value, out key, valueComparer);
        }

        /// <summary>
        /// 判断集合中是否包含指定的值（此方法不做线程安全控制，感觉没什么意义）
        /// </summary>
        /// <param name="value">需要查询的值</param>
        /// <param name="valueComparer">客户端可以自己传入值比较的委托方法</param>
        /// <returns></returns>
        public Boolean ContainsValue(TValue value, Func<TValue, TValue, bool> valueComparer)
        {
            return MyDictionary.ContainsValue<TKey, TValue>(cacheBlock, value, valueComparer);
        }

        /// <summary>
        /// 清楚所有项
        /// </summary>
        public void Clear()
        {
            if (cacheBlock.Count > 0 && rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                {
                    cacheBlock.Clear();
                }
                finally { rwl.ExitWriteLock(); }
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return cacheBlock.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>>  IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)cacheBlock).GetEnumerator();
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
            if (rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                {
                    return MyDictionary.TryAdd<TKey, TValue>(cacheBlock, key, value, addOper);
                }
                finally { rwl.ExitWriteLock(); }
            }
            return false;
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
            value = default(TValue);
            if (rwl.TryEnterReadLock(readLockTimeout))
            {
                try
                {
                    return MyDictionary.TryGetValue<TKey, TValue>(cacheBlock, key, out value, type);
                }
                finally { rwl.ExitReadLock(); }
            }
            return false;
        }

        #endregion
    }

#endif
}
