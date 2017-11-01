using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 简单的日志记录功能
    /// </summary>
    [Obsolete("此方法已经过时了，不要再使用，直接使用Foundation.Log里面的方法")]
    public class MyLog
    {
        #region "静态/实例共享的字段"

        /// <summary>
        /// 当前写日志的模式（同步/异步）
        /// </summary>
        public static WriteModel WriteLogModelType = WriteModel.SynchrWrite;

        /// <summary>
        /// 是否保存调用方法的参数，当发生异常的时候，记录调用方法的参数，可以快速定位问题（只有同步记录日志，该参数才有效）
        /// </summary>
        public static Boolean SaveFuncCallParamData = true;

        /// <summary>
        /// 获取写锁的超时时间
        /// </summary>
        private static Int32 writeLockTime = 500;

        #endregion

        #region "静态相关"

        private static WriteSystemLog writeSystemLog = null;
        private static WriteTextFileLog writeTextFileLog = null;

        /// <summary>
        /// 类的静态方法
        /// </summary>
        static MyLog()
        {
            //创建两个日志对象
            writeTextFileLog = new WriteTextFileLog();
            writeSystemLog = new WriteSystemLog();
            writeSystemLog.WriteModelType = WriteModel.SynchrWrite;
            writeTextFileLog.WriteModelType = WriteModel.SynchrWrite;
        }

        /// <summary>
        /// 根据指定的日志文件夹，获取最新的日志文件的日志内容
        /// </summary>
        /// <param name="logPath"></param>
        /// <returns></returns>
        public static String GetLogFileText(String logPath)
        {
            WriteTextFileLog log = new WriteTextFileLog(logPath);
            return log.GetLogFileText();
        }

        /// <summary>
        /// 根据指定的日志文件夹，清空日志内容
        /// </summary>
        /// <param name="logPath"></param>
        public static void ClearLog(String logPath)
        {
            WriteTextFileLog log = new WriteTextFileLog(logPath);
            log.ClearLog();
        }

        /// <summary>
        /// 获取当前日志文件的文本
        /// </summary>
        public static String GetCurrentSysLogFileText()
        {
            return writeTextFileLog.GetLogFileText();
        }

        /// <summary>
        /// 清除当前日志文件中的内容
        /// </summary>
        public static void ClearCurrentSysLog()
        {
            writeTextFileLog.ClearLog();
        }

        /// <summary>
        /// 写入日志文件，加入了写锁控制
        /// </summary>
        /// <param name="logText"></param>
        /// <param name="logType">记录在日志类型（调试/异常）</param>
        /// <param name="logType">记录日志的目标；可以记录在记事本中，也可以记录在系统的日志中</param>
        public static void MakeLog(String logText, LogType logType = LogType.Exception, LogTarget logTarget = LogTarget.TextFile)
        {
            try
            {
                if (logTarget == LogTarget.TextFile)
                    writeTextFileLog.WriteLog(logText, logType);
                if (logTarget == LogTarget.SystemLog)
                    writeSystemLog.WriteLog(logText, logType);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 根据异常对象的信息，写入日志文件
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logType">记录日志的目标；可以记录在记事本中，也可以记录在系统的日志中</param>
        public static void MakeLog(Exception exception, LogTarget logTarget = LogTarget.TextFile)
        {
            MyLog.MakeLog(exception, logTarget, null);
        }

        /// <summary>
        /// 根据异常对象的信息，写入日志文件
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logType">记录日志的目标；可以记录在记事本中，也可以记录在系统的日志中</param>
        /// <param name="callFuncParams">调用方法的参数</param>
        public static void MakeLog(Exception exception, LogTarget logTarget, params Object[] callFuncParams)
        {
            try
            {
                if (logTarget == LogTarget.TextFile)
                    writeTextFileLog.WriteLog(exception, callFuncParams);
                if (logTarget == LogTarget.SystemLog)
                    writeSystemLog.WriteLog(exception);
            }
            catch (Exception)
            { }
        }

        #endregion

        #region "内部类"

        /// <summary>
        /// 日志的基础类
        /// </summary>
        private abstract class Log
        {
            #region "属性/字段"

            /// <summary>
            /// 当前写日志的模式（同步/异步）
            /// </summary>
            protected WriteModel writeModelType = WriteModel.SynchrWrite;

            /// <summary>
            /// 写日志的模式；同步/异步
            /// </summary>
            public WriteModel WriteModelType
            {
                get { return writeModelType; }
                set { writeModelType = value; }
            }

            #endregion

            #region "公共方法"

            /// <summary>
            /// 写入日志文件，加入了写锁控制
            /// </summary>
            /// <param name="logText"></param>
            /// <param name="logType">记录在日志类型（调试/异常）</param>
            /// <param name="callFuncParams">调用方法的参数</param>
            public abstract void WriteLog(String logText, LogType logType = LogType.Exception, params Object[] callFuncParams);

            /// <summary>
            /// 写入日志文件，加入了写锁控制
            /// </summary>
            /// <param name="logText"></param>
            public void WriteLog(String logText, LogType logType = LogType.Exception)
            {
                WriteLog(logText, logType, null);
            }


            /// <summary>
            /// 根据异常对象的信息，写入日志文件
            /// </summary>
            /// <param name="exception"></param>
            /// <param name="callFuncParams">调用方法的参数</param>
            public void WriteLog(Exception exception, params Object[] callFuncParams)
            {
                if (WriteModelType == WriteModel.SynchrWrite)
                {
                    //同步
                    WriteLog(FormatExceptionText(exception, callFuncParams), LogType.Exception, callFuncParams);
                }
                else
                {
                    //异步
                    WriteLogAsync(FormatExceptionText(exception), LogType.Exception);
                }
            }

            #endregion

            #region "受保护方法"

            /// <summary>
            /// 获取日志内容的前缀提示文本
            /// </summary>
            /// <param name="logType">记录在日志类型（调试/异常）</param>
            /// <returns></returns>
            protected String GetTipString(LogType logType)
            {
                switch (logType)
                {
                    case LogType.Exception: return "异常：";
                    case LogType.Debug: return "调试：";
                }
                return String.Empty;
            }

            /// <summary>
            /// 根据异常对象格式化成异常信息字符串
            /// </summary>
            /// <param name="exception"></param>
            /// <returns></returns>
            protected String FormatExceptionText(Exception exception, params Object[] callFuncParams)
            {
                String exceptionString = DoFormatExceptionText(exception, callFuncParams);
                TargetInvocationException tie = exception as TargetInvocationException;
                if (tie != null && tie.InnerException != null)
                    exceptionString += Environment.NewLine + "目标发生的异常信息为：" + Environment.NewLine + DoFormatExceptionText(tie.InnerException, callFuncParams);

                exceptionString += Environment.NewLine + Environment.NewLine;
                return exceptionString;
            }

            /// <summary>
            /// 真正执行格式化异常操作的方法
            /// </summary>
            /// <param name="exception"></param>
            /// <param name="callFuncParams">调用方法的参数</param>
            /// <returns></returns>
            protected String DoFormatExceptionText(Exception exception, params Object[] callFuncParams)
            {
                String stackTrace = exception.StackTrace;
                String[] stackTraceStrings = MyString.SplitEx(stackTrace, "位置", StringSplitOptions.None);
                String exceptionString = String.Empty;
                if (callFuncParams != null && callFuncParams.Length > 0)
                {
                    exceptionString += "调用方法的参数分别为：" + Environment.NewLine;
                    for (var i = 0; i < callFuncParams.Length; i++)
                    {
                        exceptionString += Convert.ToString(callFuncParams[i]) + Environment.NewLine;
                    }
                }
                exceptionString += "异常类型：" + exception.GetType().ToString() + ";" +
                                        Environment.NewLine + "异常源：" + Convert.ToString(exception.Source) + ";" +
                                        Environment.NewLine + "异常信息：" + Convert.ToString(exception.Message) + ";" +
                                        Environment.NewLine + "异常堆栈信息：" + (stackTraceStrings == null ? "" : MyEnumerable.JoinEx(stackTraceStrings, Environment.NewLine + "       位置", false));
                return exceptionString;
            }

            /// <summary>
            /// 格式化异常信息字符串
            /// </summary>
            /// <param name="exception"></param>
            /// <param name="logType"></param>
            /// <param name="callFuncParams">调用方法的参数</param>
            /// <returns></returns>
            protected String FormatExceptionText(String exceptionText, LogType logType = LogType.Exception, params Object[] callFuncParams)
            {
                String title = "";
                if (HttpContext.Current != null) title = String.Format(" [{0}] {1}", HttpContext.Current.Request.Path, GetTipString(logType));
                else title = String.Format(" {0}", GetTipString(logType));
                String exceptionString = MyDatetime.FormatToSecond(DateTime.Now) + title + Environment.NewLine + exceptionText + Environment.NewLine + Environment.NewLine;
                return exceptionString;
            }

            /// <summary>
            /// 异步记录日志
            /// </summary>
            /// <param name="logText"></param>
            /// <param name="logType">记录在日志类型（调试/异常）</param>
            protected void WriteLogAsync(String logText, LogType logType = LogType.Exception)
            {
                Action<String, LogType> writeTextAction = new Action<String, LogType>(WriteLog);
                writeTextAction.BeginInvoke(logText, logType, null, null);
            }

            #endregion
        }

        /// <summary>
        /// 记录日志到操作系统的类
        /// </summary>
        private class WriteSystemLog : Log
        {
            public override void WriteLog(String logText, LogType logType = LogType.Exception, params Object[] callFuncParams)
            {
                logText = FormatExceptionText(logText, logType);
                EventLog.WriteEntry(AppDomain.CurrentDomain.BaseDirectory, logText);
            }
        }

        /// <summary>
        /// 记录日志到记事本的类
        /// </summary>
        private class WriteTextFileLog : Log
        {
            #region "私有字段"

            /// <summary>
            /// 当前正在写的日志文件序号
            /// </summary>
            private Int32 currentFileIndex = 0;

            /// <summary>
            /// 写文件的锁
            /// </summary>
            private ReaderWriterLock writeFileLocker = new ReaderWriterLock();

            private String logFilePath = String.Empty;
            private String logFileFullName = String.Empty;

            /// <summary>
            /// 记录当前的日志目录的日期，当跳过凌晨12点时，重新创建一个日志目录
            /// </summary>
            private String logFilePathDate = MyDatetime.FormatToDay(DateTime.Now);

            /// <summary>
            /// 当前日志文件的大小/用于判断日志文件不要超过10M
            /// </summary>
            private Double currentFileSize = 0.0;

            /// <summary>
            /// 控制每一个日志文件的大小（MB为单位）
            /// </summary>
            private Int32 logFileSize = 10;

            #endregion

            #region "属性"

            /// <summary>
            /// 当前的日志文件目录（不包含文件名）
            /// </summary>
            public String LogFilePath
            {
                get { return logFilePath; }
                private set { logFilePath = value; }
            }

            /// <summary>
            /// 当前的日志文件完整路径
            /// </summary>
            public String LogFileFullName
            {
                get { return logFileFullName; }
                private set { logFileFullName = value; }
            }

            #endregion

            #region "公开方法"

            /// <summary>
            /// 获取当前日志文件的文本
            /// </summary>
            public String GetLogFileText()
            {
                writeFileLocker.AcquireReaderLock(writeLockTime);
                try
                {
                    return MyIO.ReadText(logFileFullName);
                }
                catch (Exception) //忽略写日志发生的异常
                { }
                finally
                {
                    writeFileLocker.ReleaseReaderLock();
                }
                return String.Empty;
            }

            /// <summary>
            /// 清除当前日志的内容
            /// </summary>
            public void ClearLog()
            {
                writeFileLocker.AcquireWriterLock(writeLockTime);
                try
                {
                    currentFileSize = 0;
                    MyIO.Write(logFileFullName, "", false, null, false);
                }
                catch (Exception) //忽略写日志发生的异常
                { }
                finally
                {
                    writeFileLocker.ReleaseWriterLock();
                }
            }

            /// <summary>
            /// 写入日志文件，加入了写锁控制
            /// </summary>
            /// <param name="logText"></param>
            /// <param name="logType">记录在日志类型（调试/异常）</param>
            /// <param name="callFuncParams">调用方法的参数</param>
            public override void WriteLog(String logText, LogType logType = LogType.Exception, params Object[] callFuncParams)
            {
                writeFileLocker.AcquireWriterLock(writeLockTime);
                try
                {
                    //叠加文件长度
                    //logText = doNotFormat ? logText : FormatExceptionText(logText, logType);
                    logText = FormatExceptionText(logText, logType, callFuncParams);
                    Double valueSize = Encoding.Default.GetBytes(logText).Length;
                    if (valueSize + currentFileSize > logFileSize * 1024 * 1024)
                        CreateNewLogFile();
                    currentFileSize += valueSize;
                    MyIO.Write(logFileFullName, logText, true, null, false);
                }
                catch (Exception) //忽略写日志发生的异常
                { }
                finally
                {
                    writeFileLocker.ReleaseWriterLock();
                }
            }

            #endregion

            #region "构造函数”

            /// <summary>
            /// 静态构造函数，类每次初始化时会初始化参数
            /// </summary>
            public WriteTextFileLog()
                : this(String.Empty)
            { }

            /// <summary>
            /// 根据Log目录路径创建一个Log对象
            /// </summary>
            /// <param name="logFilePath"></param>
            public WriteTextFileLog(String logFilePath)
            {
                try
                {
                    this.logFilePath = logFilePath;
                    CreateNewLogFile();
                }
                catch (Exception) //忽略异常
                { }
            }

            #endregion

            #region "私有方法"

            /// <summary>
            /// 异步执行（2.0版本）已丢弃
            /// </summary>
            /// <param name="values"></param>
            private void WriteLogAsyncNET20(Object values)
            {
                Object[] valuesArray = values as Object[];
                if (valuesArray != null)
                {
                    WriteLog(valuesArray[0].ToString(), (LogType)valuesArray[1]);
                }
            }

            /// <summary>
            /// 创建日志文件
            /// 日志的目录如下：程序运行的目录\Log\年-月-日_1234.txt
            /// </summary>
            /// <returns></returns>
            private void CreateNewLogFile()
            {
                //Int32 count = 0;
                //日志的目录如下：Log\年-月-日.txt
                DateTime dateTime = DateTime.Now;
                logFilePathDate = MyDatetime.FormatToDay(dateTime);//保存当前文件的日期（用于比较用）
                //String logFilePath = Path.Combine(GetLogFilePath(), MyDatetime.FormatToMonth(dateTime));
                String logFilePath = GetLogFilePath();
                MyIO.EnsurePath(logFilePath);//确保目录存在

                String logFileName = String.Empty;
                Objects.FileSystemSize fileSizeObj = null;
                do
                {
                    currentFileIndex++;
                    logFileName = MyDatetime.FormatToDay(dateTime) + "_" + currentFileIndex.ToString() + ".log";
                    logFileFullName = Path.Combine(logFilePath, logFileName);//保存当前日志文件的路径
                    fileSizeObj = MyIO.GetFileSize(logFileFullName);
                    //判断文件大小是否超过10M，超过10M，则重新写另外一个文件
                } while (fileSizeObj.MB > (ulong)logFileSize);
                currentFileSize = fileSizeObj.B;//保存当前日志文件的大小
            }

            /// <summary>
            /// 派生类可以重写此方法，用于获取日志文件的目录名（不包含日志文件名）
            /// </summary>
            /// <returns></returns>
            private String GetLogFilePath()
            {
                if (String.IsNullOrEmpty(logFilePath))
                    logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log\\");
                return logFilePath;
            }

            #endregion
        }

        #endregion

        #region "内部枚举值"

        public enum LogType
        {
            /// <summary>
            /// 跟踪调试信息的日志
            /// </summary>
            Debug,

            /// <summary>
            /// 异常的日志
            /// </summary>
            Exception
        }

        /// <summary>
        /// 记录日志的目标
        /// </summary>
        public enum LogTarget
        {
            /// <summary>
            /// 记事本文件
            /// </summary>
            TextFile,
            /// <summary>
            /// 系统日志
            /// </summary>
            SystemLog
        }

        /// <summary>
        /// 写入日志的模式
        /// </summary>
        public enum WriteModel
        {
            /// <summary>
            /// 异步写入日志（在项目部署之后通常使用此模式写入日志，获得更大的伸缩性）
            /// </summary>
            AsyncWrite,

            /// <summary>
            /// 同步写入
            /// </summary>
            SynchrWrite
        }

        #endregion
    }
}
