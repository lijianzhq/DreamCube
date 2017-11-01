using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyInt32
    {
        /// <summary>
        /// 把ASCII码转成对应的字符
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String ToASCII(Int32 target)
#else 
        public static String ToASCII(this Int32 target)
#endif
        {
            if (target >= 0 && target <= 255)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)target };
                String strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            return null;
        }

        /// <summary>
        /// 把整型转换成布尔型；
        /// 处于false边界内的数据表示假（包括上下边界的两个数字）；除此之外的数字表示表示真；
        /// 当不传入此两个参数时，则默认逻辑认为：0为false，非0为true
        /// </summary>
        /// <param name="target"></param>
        /// <param name="falseLowerLimit">表示false的下边界整型</param>
        /// <param name="falseUpperLimit">表示false的上边界整型</param>
        /// <param name="includeLowerBound">是否包含下边界</param>
        /// <param name="includeUpperBound">是否包含上边界</param>
        /// <returns></returns>
#if NET20
        public static Boolean ToBoolean3(Int32 target,
                                         Int32 falseLowerLimit = default(Int32),
                                         Int32 falseUpperLimit = default(Int32),
                                         Boolean includeLowerBound = true,
                                         Boolean includeUpperBound = true)
#else 
         public static Boolean ToBoolean3(this Int32 target,
                                         Int32 falseLowerLimit = default(Int32), 
                                         Int32 falseUpperLimit = default(Int32),
                                         Boolean includeLowerBound = true, 
                                         Boolean includeUpperBound = true)
#endif 
        {
            return !((target > falseLowerLimit || (includeLowerBound && target == falseLowerLimit))
                    && ((includeUpperBound && target == falseUpperLimit) || target < falseUpperLimit));
        }

        /// <summary>
        /// 把整型转换成布尔型；
        /// 处于true边界内的数据表示真（包括上下边界的两个数字）；除此之外的数字表示表示假；
        /// 当不传入此两个参数时，则默认逻辑认为：0为true，非0为false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trueLowerLimit">表示true的下边界整型</param>
        /// <param name="trueUpperLimit">表示true的上边界整型</param>
        /// <param name="includeLowerBound">是否包含下边界</param>
        /// <param name="includeUpperBound">是否包含上边界</param>
        /// <returns></returns>
#if NET20
        public static Boolean ToBoolean2(Int32 target,
                                         Int32 trueLowerLimit = default(Int32), 
                                         Int32 trueUpperLimit = default(Int32),
                                         Boolean includeLowerBound = true, 
                                         Boolean includeUpperBound = true)
#else 
        public static Boolean ToBoolean2(this Int32 target,
                                         Int32 trueLowerLimit = default(Int32), 
                                         Int32 trueUpperLimit = default(Int32),
                                         Boolean includeLowerBound = true, 
                                         Boolean includeUpperBound = true)
#endif
        {
            return !((target >= trueLowerLimit || (target == trueLowerLimit && includeLowerBound))
                    && target <= trueUpperLimit);
        }

#if NET20
        /// <summary>
        /// 0为false，非0为true
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean ToBoolean1(Int32 target)
        {
            return target != 0;
        }
#else 
        /// <summary>
        /// 转换成布尔型
        /// </summary>
        /// <param name="target"></param>
        /// <param name="convertToBoolean">把整数转换成布尔型的委托，默认不传入的话，则0为false，非0为true</param>
        /// <returns></returns>
        public static Boolean ToBoolean1(this Int32 target, Func<Int32, Boolean> convertToBoolean = null)
        {
            if (convertToBoolean != null)
                return convertToBoolean(target);
            return target != 0;
        }
#endif

    }
}
