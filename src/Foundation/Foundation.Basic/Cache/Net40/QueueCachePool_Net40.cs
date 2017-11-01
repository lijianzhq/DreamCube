using System;
using System.Threading;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET40

    /// <summary>
    /// 队列缓冲区；线程安全
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class QueueCachePool_Net40<T> : Interface.IQueueCachePool<T>
    {
        #region "私有字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private Queue<T> innerQueue = new Queue<T>();

        /// <summary>
        /// 读写锁，支持递归进入读锁
        /// </summary>
        private ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// 写锁获取尝试时间
        /// 队列，读和写都是一样要全部锁定的
        /// </summary>
        private TimeSpan writeLockTimeout = TimeSpan.FromMilliseconds(500);

        #endregion

        #region "公共方法"

        /// <summary>
        /// 往队列中增加一项
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            if (rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                {
                    innerQueue.Enqueue(item);
                }
                finally { rwl.ExitWriteLock(); }
            }
        }

        /// <summary>
        /// 从队列中移除一项
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                {
                    return innerQueue.Dequeue();
                }
                finally { rwl.ExitWriteLock(); }
            }
            else
            {
                throw new TimeoutException(Properties.Resources.ExceptionGetReadLockTimeout);
            }
        }

        /// <summary>
        /// 清除队列中所有的项
        /// </summary>
        public void Clear()
        {
            if (innerQueue.Count > 0 && rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                { innerQueue.Clear(); }
                finally { rwl.ExitWriteLock(); }
            }
        }

        #endregion
    }

#endif
}
