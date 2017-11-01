using System;

namespace DreamCube.Foundation.LogService
{
    [Flags]
    public enum LogLevel
    {
        Debug = 0,
        Info,
        Warn,
        Error,
        Fatal
    }
}
