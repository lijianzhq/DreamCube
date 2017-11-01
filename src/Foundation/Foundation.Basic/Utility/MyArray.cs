using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyArray
    {
        /// <summary>
        /// 把整个数组项的类型转换成别的数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static T[] ConvertItemToTargetType<T>(Array target) where T : class
#else 
        public static T[] ConvertItemToTargetType<T>(this Array target) where T : class
#endif
        {
            if (target == null) return null;
            T[] result = new T[target.Length];
            for (Int32 i = 0; i < target.Length; i++)
                result[i] = target.GetValue(i) as T;
            return result;
        }

        /// <summary>
        /// 把数组整体转换成List数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static List<T> ConverToList<T>( Array target)
#else 
        public static List<T> ConverToList<T>(this Array target)
#endif
        {
            if (target == null) return null;
            List<T> data = new List<T>();
            Type expectType = typeof(T);
            for (Int32 i = 0; i < target.Length; i++)
                data.Add((T)Convert.ChangeType(target.GetValue(i), expectType));
            return data;
        }

        /// <summary>
        /// 判断数组是否包含某个元素；
        /// 如果目标数据为NULL或者在数组中找不到指定的值，则返回false；否则返回true
        /// </summary>
        /// <param name="target">目标数组</param>
        /// <param name="targetValue">目标值</param>
        /// <returns></returns>
#if NET20
        public static Boolean Contains(Array target, Object targetValue)
#else
        public static Boolean Contains(this Array target, Object targetValue)
#endif
        {
            if (target == null) return false;
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (targetValue == target.GetValue(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断两个数组是否有相等的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="compareArray"></param>
        /// <returns></returns>
#if NET20
        public static Boolean HasEqualItem<T>(T[] target, T[] compareArray)
#else
        public static Boolean HasEqualItem<T>(this T[] target, T[] compareArray)
#endif
        {
            return GetEqualItems<T>(target, compareArray).Length > 0;
        }

        /// <summary>
        /// 获取数组的相等项并以数组形式返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="findRecordCount">表示获取相等项的个数</param>
        /// <returns></returns>
#if NET20
        public static T[] GetEqualItems<T>(T[] target, T[] compareArray, Int32 findRecordCount = 1)
#else
        public static T[] GetEqualItems<T>(this T[] target, T[] compareArray, Int32 findRecordCount = 1)
#endif
        {
            Int32 compareArrayLength = compareArray.Length;
            Int32 targetArrayLength = target.Length;
            List<T> equalItems = new List<T>();
            Int32 equalItemCount = 0;
            for (Int32 i = 0; i < targetArrayLength; i++)
            {
                for (Int32 j = 0; j < compareArrayLength; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(target[i], compareArray[j]))
                    {
                        equalItems.Add(target[i]);
                        equalItemCount++;
                    }
                    if (equalItemCount == findRecordCount)
                        break;
                }
            }
            return equalItems.ToArray();
        }
    }
}
