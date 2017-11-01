using System;

namespace DreamCube.Foundation.LogService
{
    public interface ILogger
    {
        void Log(String message, LogLevel level);

        void Log(String message, LogLevel level, Exception exp);

        void Log(String message, String key, LogLevel level);

        void Log(String message, String key, LogLevel level, Exception exp);

        void LogFormat(String format, LogLevel level, params Object[] args);

        void LogFormat(String format, String key, LogLevel level, params Object[] args);
    }
}
