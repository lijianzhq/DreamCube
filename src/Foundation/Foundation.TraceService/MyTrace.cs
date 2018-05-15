using System;
using D = System.Diagnostics;

namespace DreamCube.Foundation.TraceService
{
    /// <summary>
    /// 系统的trace封装类
    /// </summary>
    public class Tracer
    {
        #region field

        private D.TraceSwitch _tSwitch = null;
        private static Tracer _instance = new Tracer();
        private const String _description = "DreamCube.Foundation.TraceService.traceSwitch";

        #endregion

        #region property

        public static Tracer Instance
        {
            get
            {
                return _instance;
            }
        }

        protected virtual String SwitchName
        {
            get
            {
                return "traceSwitch";
            }
        }

        protected D.TraceSwitch TraceSwitch
        {
            get
            {
                if (_tSwitch == null) _tSwitch = new D.TraceSwitch(SwitchName, _description);
                return _tSwitch;
            }
        }

        #endregion

        #region method

        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入错误消息。traceSwitch>0
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public virtual void TraceError(String format, params Object[] args)
        {
            if (TraceSwitch.TraceError)
                D.Trace.TraceError(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入错误消息。 traceSwitch>0
        /// </summary>
        /// <param name="message"></param>
        public virtual void TraceError(String message)
        {
            if (TraceSwitch.TraceError)
                D.Trace.TraceError(message);
        }

        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入警告信息。traceSwitch>1
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public virtual void TraceWarning(String format, params Object[] args)
        {
            if (TraceSwitch.TraceWarning)
                D.Trace.TraceWarning(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入警告消息。  traceSwitch>1
        /// </summary>
        /// <param name="message"></param>
        public virtual void TraceWarning(String message)
        {
            if (TraceSwitch.TraceWarning)
                D.Trace.TraceInformation(message);
        }

        /// <summary>
        /// 使用指定的对象数组和格式设置信息向 Listeners 集合中的跟踪侦听器中写入信息性消息。traceSwitch>2
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public virtual void TraceInformation(String format, params Object[] args)
        {
            if (TraceSwitch.TraceInfo)
                D.Trace.TraceInformation(format, args);
        }

        /// <summary>
        /// 使用指定的消息向 Listeners 集合中的跟踪侦听器中写入信息性消息。 traceSwitch>2
        /// </summary>
        /// <param name="message"></param>
        public virtual void TraceInformation(String message)
        {
            if (TraceSwitch.TraceInfo)
                D.Trace.TraceInformation(message);
        }

        /// <summary>
        /// 将类别名称和消息写入 Listeners 集合中的跟踪侦听器。 traceSwitch>3
        /// </summary>
        /// <param name="message">要写入的消息。</param>
        /// <param name="category">用于组织输出的类别名称。</param>
        public virtual void WriteLine(String message, String category)
        {
            if (TraceSwitch.TraceVerbose)
                D.Trace.WriteLine(message, category);
        }

        /// <summary>
        /// 将消息写入 Listeners 集合中的跟踪侦听器。  traceSwitch>3
        /// </summary>
        /// <param name="message"></param>
        public virtual void WriteLine(String message)
        {
            if (TraceSwitch.TraceVerbose)
                D.Trace.WriteLine(message);
        }

        #endregion
    }
}
