using System;
using System.Collections;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyList
    {
        /// <summary>
        /// 把整个List项的类型转换成别的数据类型
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<Tout> ConvertItemToTargetType<Tin, Tout>(List<Tin> target)
        {
            if (target == null) return null;
            List<Tout> tOutList = new List<Tout>();
            foreach (Tin item in target)
            {
                tOutList.Add((Tout)((Object)item));
            }
            return tOutList;
        }

        /// <summary>
        /// 添加一项到列表中，可以控制当项纯在时不添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="item"></param>
        /// <param name="addOper">传入IgnoreIfExist：当纯在相同的项，则不执行插入操作；传入其他的任何值，都是直接插入值</param>
        public static void AddItem<T>(IList<T> target, T item, Enums.CollectionsAddOper addOper = Enums.CollectionsAddOper.IgnoreIfExist)
        {
            if (target == null) return;
            if (addOper == Enums.CollectionsAddOper.IgnoreIfExist)
                if (target.Contains(item)) return;
            target.Add(item);
        }

        /// <summary>
        /// 合并两个List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
#if NET20
        public static void AddList<T>(IList<T> target, IList<T> value)
#else
        public static void AddList<T>(this IList<T> target, IList<T> value)
#endif
        {
            if (target == null || value == null)
                return;
            if (target.Count > value.Count)
            {
                for (Int32 i = 0; i < value.Count; i++)
                    target.Add(value[i]);
            }
            else
            {
                for (Int32 i = 0; i < target.Count; i++)
                    value.Add(target[i]);
            }
        }

        /// <summary>
        /// 移除重复的行
        /// 复杂度 O(n)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">要检查的列表对象</param>
        /// <param name="result">返回的结果对象</param>
#if NET20
        public static void RemoveRepeatItems(IList target, ref IList result)
#else
        public static void RemoveRepeatItems(this IList target, ref IList result)
#endif
        {
            for (Int32 i = target.Count - 1; i >= 0; i--)
            {
                if (!result.Contains(target[i]))
                    result.Add(target[i]);
            }
        }

        /// <summary>
        /// 移除重复的行
        /// 复杂度 O(n)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">要检查的列表对象</param>
        /// <param name="result">返回的结果对象</param>
#if NET20
        public static void RemoveRepeatItems<T>(IList<T> target, ref IList<T> result)
#else
        public static void RemoveRepeatItems<T>(this IList<T> target, ref IList<T> result)
#endif
        {
            for (Int32 i = target.Count - 1; i >= 0; i--)
            {
                if (!result.Contains(target[i]))
                    result.Add(target[i]);
            }
        }

        /// <summary>
        /// 移除重复的行
        /// 复杂度 O(n)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">要检查的列表对象</param>
        /// <param name="result">返回的结果对象</param>
        public static List<T> RemoveNullItem<T>(IList<T> target) where T : class
        {
            List<T> result = new List<T>();
            for (Int32 i = target.Count - 1; i >= 0; i--)
            {
                if (target[i] != null)
                    result.Add(target[i]);
            }
            return result;
        }

        /// <summary>
        /// 移除重复的行
        /// 复杂度 O(n)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">要检查的列表对象</param>
        /// <param name="result">返回的结果对象</param>
        public static List<T> RemoveRepeatItems<T>(IList<T> target)
        {
            List<T> result = new List<T>();
            for (Int32 i = target.Count - 1; i >= 0; i--)
            {
                if (!result.Contains(target[i]))
                    result.Add(target[i]);
            }
            return result;
        }

        /// <summary>
        /// 随机获取一个链表的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static T GetRandomItem<T>(IList target)
#else
        public static T GetRandomItem<T>(this IList target)
#endif
        {
            if (target == null)
                return default(T);
            Int32 maxValue = target.Count;
            Int32 tempRandomNumber = MyRand.Random.Next(0, maxValue);
            return (T)target[tempRandomNumber];
        }
    }
}
