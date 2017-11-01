using System;
using System.Collections;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Objects;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET20 || NET35 || NET30

    /// <summary>
    /// 弱引用的字典缓冲区
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class WeakDictionaryCachePool_Net20<TKey, TValue> : BasicWeakDictionaryCachePool<TKey, TValue>
            where TValue : class
    {
        #region "字段"

        private Object locker = new Object();

        #endregion

        #region "构造方法"

        public WeakDictionaryCachePool_Net20(IEqualityComparer<TKey> defaultKeyComparer = null) :
            base(defaultKeyComparer)
        { }

        #endregion

        #region "方法"

        /// <summary>
        /// 判断集合中是否存在指定的项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean ContainsValue(TValue value)
        {
            foreach (KeyValuePair<TKey, WeakReference<TValue>> item in innerPool)
            {
                TValue tempValue;
                tempValue = item.Value.Target;
                if (EqualityComparer<TValue>.Default.Equals(tempValue, value))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否包含某Value值（此方法不考虑线程安全问题，因为考虑线程是否安全问题意义不大）
        /// </summary>
        /// <param name="value">指定的值</param>
        /// <returns></returns>
        public override Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer)
        {
            TKey key;
            return ContainsValue(value, out key, valueComparer);
        }

        /// <summary>
        /// 判断是否包含某Value值（此方法不考虑线程安全问题，因为考虑线程是否安全问题意义不大）
        /// </summary>
        /// <param name="value">指定的值</param>
        /// <param name="key">值对应的键值</param>
        /// <param name="valueComparer">客户端可以自己指定比较值的方法</param>
        /// <returns></returns>
        public override Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer = null)
        {
            key = default(TKey);
            foreach (KeyValuePair<TKey, WeakReference<TValue>> item in innerPool)
            {
                TValue tempValue;
                tempValue = item.Value.Target;
                if (tempValue != null)
                {
                    if (valueComparer == null)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(tempValue, value))
                        {
                            key = item.Key;
                            return true;
                        }
                    }
                    else
                    {
                        if (valueComparer(tempValue, value))
                        {
                            key = item.Key;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion
    }

#endif
}
