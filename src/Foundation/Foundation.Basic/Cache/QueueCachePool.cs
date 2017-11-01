using System;
using System.Threading;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// 队列缓冲区；线程安全
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueCachePool<T> : Interface.IQueueCachePool<T>
    {
        #region "私有字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private Interface.IQueueCachePool<T> innerQueue = null;

        #endregion

        #region "构造方法"

        public QueueCachePool()
        {
#if NET20 || NET35 || NET30
            innerQueue = new QueueCachePool_Net20<T>();
#elif NET40
            innerQueue = new QueueCachePool_Net40<T>();
#endif
        }

        #endregion

        #region "公共方法"

        public void Enqueue(T item)
        {
            innerQueue.Enqueue(item);
        }

        public T Dequeue()
        {
            return innerQueue.Dequeue();
        }

        public void Clear()
        {
            innerQueue.Clear();
        }

        #endregion
    }
}
