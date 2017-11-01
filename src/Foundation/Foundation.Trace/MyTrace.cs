using System;
using D = System.Diagnostics;

namespace DreamCube.Foundation.Trace
{
    /// <summary>
    /// 系统的trace封装类
    /// </summary>
    public static class MyTrace
    {
        #region field

        private static D.TraceSwitch tSwitch = new D.TraceSwitch("traceSwitch", "DreamCube.Foundation.Trace.traceSwitch");

        #endregion

        #region method

        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入错误消息。traceSwitch>0
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void TraceError(string format, params Object[] args)
        {
            if (tSwitch.TraceError)
                D.Trace.TraceError(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入错误消息。 traceSwitch>0
        /// </summary>
        /// <param name="message"></param>
        public static void TraceError(string message)
        {
            if (tSwitch.TraceError)
                D.Trace.TraceError(message);
        }


        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入警告信息。traceSwitch>1
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void TraceWarning(string format, params Object[] args)
        {
            if (tSwitch.TraceWarning)
                D.Trace.TraceWarning(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入警告消息。  traceSwitch>1
        /// </summary>
        /// <param name="message"></param>
        public static void TraceWarning(string message)
        {
            if (tSwitch.TraceWarning)
                D.Trace.TraceInformation(message);
        }

        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入信息性消息。traceSwitch>2
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void TraceInformation(string format, params Object[] args)
        {
            if (tSwitch.TraceInfo)
                D.Trace.TraceInformation(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入信息性消息。 traceSwitch>2
        /// </summary>
        /// <param name="message"></param>
        public static void TraceInformation(string message)
        {
            if (tSwitch.TraceInfo)
                D.Trace.TraceInformation(message);
        }

        /// <summary>
        /// 将类别名称和消息写入 Listeners 集合中的跟踪侦听器。 traceSwitch>3
        /// </summary>
        /// <param name="message">要写入的消息。</param>
        /// <param name="category">用于组织输出的类别名称。</param>
        public static void WriteLine(string message, string category)
        {
            if (tSwitch.TraceVerbose)
                D.Trace.WriteLine(message, category);
        }

        /// <summary>
        /// 将消息写入 Listeners 集合中的跟踪侦听器。  traceSwitch>3
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {
            if (tSwitch.TraceVerbose)
                D.Trace.WriteLine(message);
        }

        #endregion
    }
}
