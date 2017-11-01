using System;
using System.Threading;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache
{
#if NET40

    internal class ListCachePool_Net40<T>:Interface.IListCachePool<T>
    {
        #region "字段"

        /// <summary>
        /// 缓冲区
        /// </summary>
        private List<T> cacheBlock = new List<T>();

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

        #region "公共方法"

        public T this[int i]
        {
            get
            {
                if (rwl.TryEnterReadLock(readLockTimeout))
                {
                    try
                    {
                        return cacheBlock[i];
                    }
                    finally { rwl.ExitReadLock(); }
                }
                else
                {
                    throw new TimeoutException(Properties.Resources.ExceptionGetReadLockTimeout);
                }
            }
        }

        /// <summary>
        /// 在List中添加一项
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                {
                    cacheBlock.Add(item);
                }
                finally { rwl.ExitWriteLock(); }
            }
        }

        /// <summary>
        /// 移除指定位置的项
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (rwl.TryEnterReadLock(readLockTimeout))
            {
                try
                {
                    cacheBlock.RemoveAt(index);
                }
                finally { rwl.ExitReadLock(); }
            }
        }

        /// <summary>
        /// 清除List集合中的项
        /// </summary>
        public void Clear()
        {
            if (cacheBlock.Count > 0 && rwl.TryEnterWriteLock(writeLockTimeout))
            {
                try
                { cacheBlock.Clear(); }
                finally { rwl.ExitWriteLock(); }
            }
        }

        #endregion
    }

#endif
}
