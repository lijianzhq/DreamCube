using System;

//自定义命名空间
using DreamCube.Foundation.Basic.Win32API.API;
using DreamCube.Foundation.Basic.Win32API.Structs;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyMemory
    {
        /// <summary>
        /// 获取已经使用的内存值MB
        /// </summary>
        /// <returns></returns>
        public static Single GetMemoryUsed()
        {
            Memory_INFO memoryInfo = new Memory_INFO();
            kernel32.GlobalMemoryStatus(ref memoryInfo);

            Single avaliableMB = Convert.ToInt64(memoryInfo.dwAvailPageFile.ToString()) / 1024 / 1024;
            Single totalMB = Convert.ToInt64(memoryInfo.dwTotalPageFile.ToString()) / 1024 / 1024;
            return totalMB - avaliableMB;
        }

        /// <summary>
        /// 获取可用的内存值MB
        /// </summary>
        /// <returns></returns>
        public static Single GetAvaliableMemory()
        {
            Memory_INFO memoryInfo = new Memory_INFO();
            kernel32.GlobalMemoryStatus(ref memoryInfo);

            Single avaliableMB = Convert.ToInt64(memoryInfo.dwAvailPageFile.ToString()) / 1024 / 1024;
            return avaliableMB;
        }

        /// <summary>
        /// 获取总共的内存值MB
        /// </summary>
        /// <returns></returns>
        public static Single GetTotalMemory()
        {
            Memory_INFO memoryInfo = new Memory_INFO();
            kernel32.GlobalMemoryStatus(ref memoryInfo);
            Single totalMB = Convert.ToInt64(memoryInfo.dwTotalPageFile.ToString()) / 1024 / 1024;
            return totalMB;
        }
    }
}
