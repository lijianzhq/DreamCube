using System;
using System.Threading;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyInterlock
    {
        /// <summary>
        /// 求两个数的最大值，并返回最大值的数，并且把最大值的数替换target值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 Maximun(ref Int32 target, Int32 value)
        {
            Int32 currentVal = target;
            Int32 startVal; //比较时的起始值
            Int32 desiredVal; //两个值之间的最大值
            do
            {
                startVal = currentVal;
                desiredVal = Math.Max(startVal, value);
                //比较 原始值target 和 比较开始时的值startVal 是否相等（target没有被任何线程更改过）；
                //如果相等，则用最大值替换target值，并且返回target的原始值
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
            } while (currentVal != startVal);
            return desiredVal;
        }

        /// <summary>
        /// 求两个数的最小值，并返回最小值的数，并且把最小值的数替换target值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 Minimum(ref Int32 target, Int32 value)
        {
            Int32 currentVal = target;
            Int32 startVal; //比较时的起始值
            Int32 desiredVal; //两个值之间的最大值
            do
            {
                startVal = currentVal;
                desiredVal = Math.Min(startVal, value);
                //比较 原始值target 和 比较开始时的值startVal 是否相等（target没有被任何线程更改过）；
                //如果相等，则用最大值替换target值，并且返回target的原始值
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
            } while (currentVal != startVal);
            return desiredVal;
        }
    }
}
