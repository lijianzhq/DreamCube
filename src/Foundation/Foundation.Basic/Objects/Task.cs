using System;
using System.Threading;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 任务类
    /// </summary>
    public class Task<T>
    {
        /// <summary>
        /// 任务的操作
        /// </summary>
        private Delegate taskWork = null;
        private TimeSpan? timeOutTimeSpan = null;
        /// <summary>
        /// 标志任务是否开始了
        /// </summary>
        private Boolean start = false;
        /// <summary>
        /// 标志任务完成
        /// </summary>
        private Boolean done = false;
        /// <summary>
        /// 任务开始的时间
        /// </summary>
        private DateTime taskStartTime = DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="taskWork">需要执行在的任务的回调方法</param>
        /// <param name="timeOutTimeSpan">指定任务的超时时间（如果不指定，无线等待）</param>
        public Task(Action<T> taskWork, TimeSpan? timeOutTimeSpan = null)
        {
            this.taskWork = taskWork;
            this.timeOutTimeSpan = timeOutTimeSpan;
        }

        /// <summary>
        /// 开始任务（会阻塞等待任务完成）
        /// </summary>
        public void Start(T args)
        {
            Action<T, Task<T>> innerAction = null;
            IAsyncResult actionResult = null;
            Action<T, Task<T>> a = new Action<T, Task<T>>((inputArgs, inputTask) =>
            {
                try
                {
                    innerAction = new Action<T, Task<T>>((inputArgs2, inputTask2) =>
                    {
                        try
                        {
                            inputTask2.taskStartTime = DateTime.Now;//记录任务开始时间
                            inputTask2.start = true;                //标志任务开始执行了
                            inputTask2.taskWork.DynamicInvoke(inputArgs2);
                            //如果任务结束了，也可以直接退出方法了，不需要等到超时
                            inputTask2.done = true;
                        }
                        catch (Exception ex)
                        {
                            MyLog.MakeLog(ex);
                        }
                    });
                }
                catch (Exception ex) //捕获超时强制退出的异常
                {
                    MyLog.MakeLog(ex);
                }
                //开始任务
                actionResult = innerAction.BeginInvoke(inputArgs, inputTask, null, null);
            });
            a.BeginInvoke(args, this, new AsyncCallback((o) =>
            {
                Task<T> t = null;
                if (o != null && o.AsyncState != null)
                    t = o.AsyncState as Task<T>;
                while (t != null && !t.done)
                {
                    if (t != null)
                    {
                        if (t.start && t.timeOutTimeSpan != null)
                        {
                            //如果任务指定的时间等于或者大于指定的超时时间，则退出任务执行了
                            if ((DateTime.Now - t.taskStartTime).TotalMilliseconds >= t.timeOutTimeSpan.Value.TotalMilliseconds)
                            {
                                t.done = true;
                                MyUtility.CatchEx(delegate
                                {
                                    innerAction = null;
                                });
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }), this);
            //阻塞等待
            while (!this.done) System.Threading.Thread.Sleep(1000);
        }
    }
}
