using System;
using System.Collections.Generic;
using System.Threading;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// 链表缓存类；线程安全
    /// 如果在对象外部申请读写锁，可能会造成线程不安全（存在bug）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListCachePool<T> : Interface.IListCachePool<T>
    {
        #region "私有字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private Interface.IListCachePool<T> cacheBlock = null;

        #endregion

        #region "方法"

        public ListCachePool()
        {
#if NET20 || NET35 || NET30
            cacheBlock = new ListCachePool_Net20<T>();
#elif NET40
            cacheBlock = new ListCachePool_Net40<T>();
#endif
        }

        public T this[int i]
        {
            get { return cacheBlock[i]; }
        }

        public void Add(T item)
        {
            cacheBlock.Add(item);
        }

        public void RemoveAt(Int32 index)
        {
            cacheBlock.RemoveAt(index);
        }

        public void Clear()
        {
            cacheBlock.Clear();
        }

        public Boolean Contains(T item)
        {
            return cacheBlock.Contains(item);
        }

        public void Remove(T item)
        {
            cacheBlock.Remove(item);
        }

        #endregion
    }
}
