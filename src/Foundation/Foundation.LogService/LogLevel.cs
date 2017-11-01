using System;

namespace DreamCube.Foundation.Log
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
