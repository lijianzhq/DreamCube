using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net.Appender;

namespace DreamCube.Foundation.Log
{
    public class EncryptRollingFileAppender : RollingFileAppender
    {
        /// <summary>
        /// Sets the quiet writer being used.
        /// </summary>
        /// <remarks>
        /// This method can be overridden by sub classes.
        /// </remarks>
        /// <param name="writer">the writer to set</param>
        override protected void SetQWForFiles(TextWriter writer)
        {
            if (LoggerKeyHelper.EncryptLoggerAppenders == null ||
                LoggerKeyHelper.EncryptLoggerAppenders.All(t => !String.Equals(t, base.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                base.SetQWForFiles(writer);
            }
            else
            {
                QuietWriter = new EncryptCountingQuietTextWriter(
                    LoggerKeyHelper.Key,
                    writer,
                    ErrorHandler
                );
            }
        }
    }
}
