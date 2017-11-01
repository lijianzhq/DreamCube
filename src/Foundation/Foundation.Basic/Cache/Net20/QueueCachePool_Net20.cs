using System;
using System.Threading;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET20 || NET35 || NET30

    /// <summary>
    /// 队列缓冲区；线程安全
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class QueueCachePool_Net20<T> : Interface.IQueueCachePool<T>
    {
        #region "私有字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private Queue<T> innerCache = new Queue<T>();

        private Object locker = new Object();

        #endregion

        #region "方法"

        public void Enqueue(T item)
        {
            lock (locker)
            {
                innerCache.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (locker)
            {
                return innerCache.Dequeue();
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                innerCache.Clear();
            }
        }

        #endregion
    }

#endif
}
