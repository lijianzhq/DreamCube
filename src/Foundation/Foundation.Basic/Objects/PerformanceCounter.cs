using System;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 效率计算器
    /// </summary>
    public class PerformanceCounter
    {
        #region "字段"

        long startTick = 0;
        long endTick = 0;
        long freq = 0;

        #endregion

        #region "属性"

        /// <summary>
        /// 所耗的秒数
        /// </summary>
        public Double Seconds
        {
            get;
            private set;
        }

        /// <summary>
        /// 所耗的毫秒数
        /// </summary>
        public Double Milliseconds
        {
            get
            {
                return Seconds * 1000;
            }
        }

        #endregion

        public void Start()
        {
            Win32API.API.kernel32.QueryPerformanceFrequency(ref freq);
            Win32API.API.kernel32.QueryPerformanceCounter(ref startTick);
        }

        public void Stop()
        {
            Win32API.API.kernel32.QueryPerformanceCounter(ref endTick);
            long ticks = endTick - startTick;
            Seconds = (double)(ticks) / (double)freq;
        }
    }
}
