using System;
using System.Threading;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 由于.NET4.0内置了一个更高效率的线程访问锁对象，ReaderWriterLockSlim，此对象就是为了封装.NET2.0和.NET4.0之间的适配
    /// </summary>
    public class ThreadLocker
    {
#if NET40
        private ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
#else 
        private ReaderWriterLock locker = new ReaderWriterLock();
#endif

        /// <summary>
        /// 进入读取模式锁定状态
        /// </summary>
        public void EnterReadLock()
        {
#if NET40
            rwl.EnterReadLock();
#else 
            locker.AcquireReaderLock(-1);
#endif
        }

        /// <summary>
        /// 进入写入模式锁定状态
        /// </summary>
        public void EnterWriteLock()
        {
#if NET40
            rwl.EnterWriteLock();
#else
            locker.AcquireWriterLock(-1);
#endif
        }

        /// <summary>
        /// 减少读取模式的递归计数，并在生成计数为0的时候，自动退出读取模式
        /// </summary>
        public void ExitReadLock()
        {
#if NET40
            rwl.ExitReadLock();
#else
            if (locker.IsReaderLockHeld) locker.ReleaseReaderLock();
#endif
        }

        /// <summary>
        /// 减少写入模式的递归技术，并在生成技术为0的时候，自动退出写入模式
        /// </summary>
        public void ExitWriteLock()
        {
#if NET40
            rwl.ExitWriteLock();
#else
            if(locker.IsWriterLockHeld) locker.ReleaseWriterLock();
#endif
        }
        
        /// <summary>
        /// 尝试进入读取模式锁定状态
        /// </summary>
        /// <param name="millisecondsTimeout">整数超时时间</param>
        /// <returns></returns>
        public Boolean TryEnterReadLock(Int32 millisecondsTimeout)
        {
#if NET40
            return rwl.TryEnterReadLock(millisecondsTimeout);
#else
            if (locker.IsReaderLockHeld) return true;
            locker.AcquireReaderLock(millisecondsTimeout);
            return locker.IsReaderLockHeld;
#endif
        }

        /// <summary>
        /// 尝试进入写入模式锁定状态
        /// </summary>
        /// <param name="millisecondsTimeout">整数超时时间</param>
        /// <returns></returns>
        public Boolean TryEnterWriteLock(Int32 millisecondsTimeout)
        {
#if NET40
            return rwl.TryEnterWriteLock(millisecondsTimeout);
#else
            if (locker.IsWriterLockHeld) return true;
            locker.AcquireWriterLock(millisecondsTimeout); ;
            return locker.IsWriterLockHeld;
#endif
        }
    }
}
