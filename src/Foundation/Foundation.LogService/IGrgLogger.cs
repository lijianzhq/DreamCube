using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DreamCube.Foundation.Log
{
    [Flags]
    public enum emLogLevel
    {
        Debug = 0,
        Info,
        Warn,
        Error,
        Fatal
    }

    [Flags]
    public enum LogMethodTag
    {
        Begin,
        End
    }

    public interface IGrgLogger
    {
#region method
        void Log( string message, emLogLevel level );

        void Log( string message, emLogLevel level, Exception exp );

        void Log( string message, string key, emLogLevel level );

        void Log( string message, string key, emLogLevel level, Exception exp);

        void LogHex( byte[] arrBuffer, int argSize );

        void LogHexFormat( string argFormat, byte[] arrBuffer, int argSize );

        void LogFormat( string format, emLogLevel level, params object[] args );

        void LogFormat(string format, string key, emLogLevel level, params object[] args);
#endregion

    }
}
