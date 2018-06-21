using System;
using System.Threading;

namespace Mini.Framework.WebUploader.DBService
{
    /// <summary>
    /// 【抛弃，还要处理线程上下文】
    /// 增加一些处理（缓存数据库实例，连接对象，当一定时间没有访问的时候，才释放连接）
    /// </summary>
    public class DBWrapper : IDisposable
    {
        private static DBService.DB _db = null;
        private static Timer _timer = null;
        private const Int32 _clearDBPeriod = 1000 * 30; //30秒检查一次，此参数也是作为清除空闲时间间隔用（数据库实例30秒没有被调用过，则马上清除）
        private static Int32 _clearLock = 0;//锁标识，用于确保不会在执行的时候，释放数据库对象（0表示没有在执行中，1表示正在执行，-1清理中）
        private static DateTime _lastCallDatetime = DateTime.Now;

        static DBWrapper()
        {
            _timer = new Timer(new TimerCallback(ClearDBObj), null, _clearDBPeriod, Timeout.Infinite);
        }

        static void ClearDBObj(Object obj)
        {
            try
            {
                //标记数据库实例进入清理状态
                if (Interlocked.CompareExchange(ref _clearLock, -1, 0) == 0)
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //改变状态
                Interlocked.CompareExchange(ref _clearLock, 0, 1);
                _timer.Change(_clearDBPeriod, Timeout.Infinite);
            }
        }

        public static DBService.DB GetDB()
        {
            //如果正在清理总，则自旋
            SpinWait.SpinUntil(() => Interlocked.CompareExchange(ref _clearLock, 1, -1) != -1);
            if (_db == null)
            {
                _db = new DBService.DB("UploadFileDB");
            }
            return _db;
        }

        public void Dispose()
        {
            Interlocked.CompareExchange(ref _clearLock, 0, 1);
        }
    }
}
