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
    internal abstract class BasicWeakDictionaryCachePool<TKey, TValue> : Interface.IWeakDictionaryCachePool<TKey, TValue>
            where TValue : class
    {
        #region "字段"

        /// <summary>
        /// 内部的缓冲池
        /// </summary>
        protected DictionaryCachePool<TKey, WeakReference<TValue>> innerPool = null;
         
        /// <summary>
        /// 标识当前是否是强引用
        /// </summary>
        private Boolean isStrongReference;

        #endregion 

        #region "构造方法"

        /// <summary>
        /// 传入默认的键值比较委托来创建一个缓冲区
        /// </summary>
        /// <param name="defaultKeyComparer"></param>
        public BasicWeakDictionaryCachePool(IEqualityComparer<TKey> defaultKeyComparer = null)
        {
            innerPool = new DictionaryCachePool<TKey, WeakReference<TValue>>(defaultKeyComparer);
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 把缓冲区升级为强引用
        /// （注意：对于已经在集合中的元素，如果当前弱对象已经不存在了，
        /// 则升级为强引用也没用的，所以建议一般都是此集合还没有添加任何对象之前调用此方法，使用完之后，再降为弱引用，对数据进行缓存）
        /// </summary>
        public void UpToStrongReference()
        {
            this.isStrongReference = true;
            foreach (KeyValuePair<TKey, WeakReference<TValue>> item in innerPool)
                item.Value.UpToStrongReference();
        }

        /// <summary>
        /// 把缓冲区降级为弱引用
        /// </summary>
        public void DownToWeakReference()
        {
            this.isStrongReference = false;
            foreach (KeyValuePair<TKey, WeakReference<TValue>> item in innerPool)
                item.Value.DownToWeakReference();
        }

        /// <summary>
        /// 移除指定项，移除成功返回true；移除失败或者不存在key值返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">移除的项的值</param>
        /// <param name="keyComparer">比较key值的委托，判断key值是否相等</param>
        /// <returns></returns>
        public virtual Boolean Remove(TKey key)
        {
            TValue value;
            return Remove(key, out value);
        }

        /// <summary>
        /// 移除指定项，移除成功返回true；移除失败或者不存在key值返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">移除的项的值</param>
        /// <param name="keyComparer">比较key值的委托，判断key值是否相等</param>
        /// <returns></returns>
        public virtual Boolean Remove(TKey key, out TValue value)
        {
            value = default(TValue);
            WeakReference<TValue> weakRefVal;
            Boolean result = false;
            if ((result = innerPool.Remove(key, out weakRefVal)))
                value = weakRefVal.Target;
            return result;
        }

        /// <summary>
        /// 获取与指定的键相关联的值；
        /// 由于保存的是弱引用，只有当获取到目标值，此方法才返回真，
        /// 如果不存在指定的键值或者指定键值对应的目标对象已经被回收，则返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Boolean TryGetValue(TKey key,
                                           out TValue value,
                                           CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                           TValue defaultValue = default(TValue))
        {
            value = defaultValue;
            WeakReference<TValue> weakRefVal;
            Boolean result = innerPool.TryGetValue(key, out weakRefVal, CollectionsGetOper.DefaultValueIfNotExist);
            if (result) value = weakRefVal.Target;
            return result && (value != null);
        }

        /// <summary>
        /// 判断是否包含某Key值（此方法不考虑线程安全问题，因为考虑线程是否安全问题意义不大）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Boolean ContainsKey(TKey key)
        {
            return innerPool.ContainsKey(key);
        }

        /// <summary>
        /// 判断是否包含某Value值（此方法不考虑线程安全问题，因为考虑线程是否安全问题意义不大）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Boolean ContainsValue(TValue value);
        public abstract Boolean ContainsValue(TValue value, Func<TValue, TValue, Boolean> valueComparer);
        public abstract Boolean ContainsValue(TValue value, out TKey key, Func<TValue, TValue, Boolean> valueComparer = null);

        /// <summary>
        /// 清除所有缓存项
        /// </summary>
        public void Clear()
        {
            innerPool.Clear();
        }

        /// <summary>
        /// 向缓冲区添加缓存项；添加成功返回true，添加失败返回false；
        /// 默认的操作：如果在集合中已经存在了指定key值的项，则忽略添加操作，直接返回true
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="addOper"></param>
        /// <returns></returns>
        public Boolean TryAdd(TKey key, TValue value)
        {
            WeakReference<TValue> weakRefVal = new WeakReference<TValue>(value, this.isStrongReference);
            //这里必须选择ReplaceIfExist，因为对应的值为弱引用来的；即使存在了对应的key值，也要刷新一下对应的Value值域
            return innerPool.TryAdd(key, weakRefVal, CollectionsAddOper.ReplaceIfExist);
        }

        /// <summary>
        /// 向缓冲区添加缓存项；添加成功返回true，添加失败返回false；
        /// 默认的操作：如果在集合中已经存在了指定key值的项，则忽略添加操作，直接返回true
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="addOper"></param>
        /// <returns></returns>
        public Boolean TryAdd(TKey key, TValue value, CollectionsAddOper addOper = CollectionsAddOper.IgnoreIfExist)
        {
            WeakReference<TValue> weakRefVal = new WeakReference<TValue>(value, this.isStrongReference);
            //这里必须选择ReplaceIfExist，因为对应的值为弱引用来的；即使存在了对应的key值，也要刷新一下对应的Value值域
            return innerPool.TryAdd(key, weakRefVal, addOper);
        }

        public IEnumerator GetEnumerator()
        {
            return innerPool.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)innerPool).GetEnumerator();
        }

        #endregion

    }
}
