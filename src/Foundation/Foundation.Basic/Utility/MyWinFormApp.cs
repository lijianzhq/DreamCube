using System;
using System.Threading;
using System.Windows.Forms;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 对于winform的线程异常和未处理异常的统一处理方法
    /// </summary>
    public static class MyWinFormApp
    {
        /// <summary>
        /// 绑定应用程序的公共事件
        /// </summary>
        public static void BindAppCommonEvent()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        /// <summary>
        /// 线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Application_ThreadException(Object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MyLog.MakeLog(e.Exception);
        }

        /// <summary>
        /// 程序域异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MyLog.MakeLog("程序发生未处理异常:" + e.ExceptionObject.ToString());
        }
    }
}
