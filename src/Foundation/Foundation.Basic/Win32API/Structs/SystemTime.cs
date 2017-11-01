using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        /// <summary>
        /// 年
        /// </summary>
        public UInt16 wYear;
        /// <summary>
        /// 月
        /// </summary>
        public UInt16 wMonth;
        /// <summary>
        /// 星期
        /// </summary>
        public UInt16 wDayOfWeek;
        /// <summary>
        /// 天
        /// </summary>
        public UInt16 wDay;
        /// <summary>
        /// 小时
        /// </summary>
        public UInt16 wHour;
        /// <summary>
        /// 分钟
        /// </summary>
        public UInt16 wMinute;
        /// <summary>
        /// 秒
        /// </summary>
        public UInt16 wSecond;
        /// <summary>
        /// 毫秒
        /// </summary>
        public UInt16 wMilliseconds;
    }
}
