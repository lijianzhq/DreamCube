using System;
using System.Collections.Generic;
using System.Text;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 对Enumerable的类的一些扩展方法
    /// </summary>
    public static class MyEnumerable
    {
        /// <summary>
        /// 判断一个集合中是否存在指定的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Boolean Contains<T>(IEnumerable<T> target, T item)
        {
            List<T> targetL = target as List<T>;
            if (targetL != null)
                return targetL.Contains(item);
            else
            {
                foreach (T t in target)
                {
                    if (EqualityComparer<T>.Default.Equals(item, t))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 组合字符串数组并返回；
        /// 要确保泛型T类型转换成字符串是有意义的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="separatorStr"></param>
        /// <param name="validate">
        /// 此参数表示：是否需要验证目标数组是否包含有指定的分隔符；
        /// 通常情况下，都不希望目标数组包含了分隔符的；此参数默认为false
        /// </param>
        /// <returns></returns>
#if NET20
        public static String JoinEx<TInput>(IEnumerable<TInput> target,
                                            String separatorStr,
                                            Boolean validate = true)
#else
        public static String JoinEx<TInput>(this IEnumerable<TInput> target,
                                            String separatorStr,
                                            Boolean validate = true,
                                            Func<TInput, String> parseItemToString = null)
#endif
        {
            StringBuilder builder = new StringBuilder();
            String tempStr = null;
            Int32 index = 0;
            foreach (TInput item in target)
            {
                if (item == null)
                    continue;
#if !NET20
                if (parseItemToString == null)
                    tempStr = Convert.ToString(item);
                else
                    tempStr = parseItemToString.Invoke(item);
#else
                tempStr = Convert.ToString(item);
#endif
                if (validate)
                {
                    if (tempStr.Contains(separatorStr))
                        //throw new ArgumentException(String.Format("目标数组的元素中包含了分隔符【{0}】，组合字符串后会导致混乱，无法还原为原来的数组.", separatorStr));
                        throw new ArgumentException(String.Format("the element[{0}] in the Enumerable target contains the separatorStr[{1}]", item, separatorStr));
                }

                if (String.IsNullOrEmpty(separatorStr))
                {
                    builder.Append(tempStr);
                }
                else
                {
                    if (index++ == 0)
                        builder.Append(tempStr);
                    else
                        builder.Append(separatorStr + tempStr);
                }
            }
            return builder.ToString();
        }
    }
}
