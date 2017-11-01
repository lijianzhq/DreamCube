using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Memory_INFO
    {
        /// <summary>
        /// 内存相关
        /// MEMORYSTATUS结构的大小，在调GlobalMemoryStatus函数前用sizeof()函数求得，用来供函数检测结构的版本。 
        /// </summary>
        public UInt32 dwLength;

        /// <summary>
        /// 内存使用率
        /// 返回一个介于0～100之间的值，用来指示当前系统内存的使用率。 
        /// </summary>　　
        public UInt32 dwMemoryLoad;

        /// <summary>
        /// 物理内存的总大小
        /// 返回总的物理内存大小，以字节(byte)为单位。 
        /// </summary>
        public UInt32 dwTotalPhys;

        /// <summary>
        /// 返回可用的物理内存大小，以字节(byte)为单位。 
        /// </summary>
        public UInt32 dwAvailPhys;

        /// <summary>
        /// 显示可以存在页面文件中的字节数。注意这个数值并不表示在页面文件在磁盘上的真实物理大小。 
        /// </summary>
        public UInt32 dwTotalPageFile;

        /// <summary>
        /// 返回可用的页面文件大小，以字节(byte)为单位。 
        /// </summary>
        public UInt32 dwAvailPageFile;

        /// <summary>
        /// 返回调用进程的用户模式部分的全部可用虚拟地址空间，以字节(byte)为单位。 
        /// </summary>
        public UInt32 dwTotalVirtual;

        /// <summary>
        /// 返回调用进程的用户模式部分的实际自由可用的虚拟地址空间，以字节(byte)为单位。 
        /// </summary>
        public UInt32 dwAvailVirtual;
    }
}
