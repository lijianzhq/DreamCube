using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET20 || NET35 || NET30

    internal class ListCachePool_Net20<T> : Interface.IListCachePool<T>
    {
        #region "字段"

        private List<T> innerCache = new List<T>();

        private Object locker = new Object();

        #endregion

        #region "方法"

        public T this[int i]
        {
            get
            {
                lock (locker)
                {
                    return innerCache[i];
                }
            }
        }

        public void Add(T item)
        {
            lock (locker)
            {
                innerCache.Add(item);
            }
        }

        public void RemoveAt(Int32 index)
        {
            lock (locker)
            {
                innerCache.RemoveAt(index);
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                innerCache.Clear();
            }
        }

        public Boolean Contains(T item)
        {
            //lock (locker)
            //{
                return innerCache.Contains(item);
            //}
        }

        public void Remove(T item)
        {
            lock (locker)
            {
                innerCache.Remove(item);
            }
        }

        #endregion
    }

#endif
}
