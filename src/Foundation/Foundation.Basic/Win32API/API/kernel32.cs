using System;
using System.Runtime.InteropServices;

//自定义命名空间
using DreamCube.Foundation.Basic.Win32API.Structs;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    public class kernel32
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        /// <summary>
        /// 函数功能:此函数设置当前系统的时间和日期 
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern Boolean SetSystemTime(ref SystemTime sysTime);

        /// <summary>
        /// 在一个SYSTEMTIME中载入当前系统时间，这个时间采用的是“协同世界时间”（即UTC，也叫做GMT）格式 
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern Boolean GetSystemTime(ref SystemTime sysTime);

        /// <summary>
        /// 此函数用来获得当前可用的物理和虚拟内存信息
        /// </summary>
        /// <param name="memoryInfo"></param>
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref Memory_INFO memoryInfo);

        /// <summary>
        /// 如果安装的硬件支持高精度计时器,函数将返回非0值.
        /// 如果安装的硬件不支持高精度计时器,函数将返回0.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern Boolean QueryPerformanceCounter(ref long count);

        /// <summary>
        /// 作用：返回硬件支持的高精度计数器的频率。
        /// 返回值：非零，硬件支持高精度计数器；零，硬件不支持，读取失败。
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern Boolean QueryPerformanceFrequency(ref long count);   
    }
}
