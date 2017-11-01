using System;
using System.Collections;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyQueue
    {
        /// <summary>
        /// 向队列添加一个集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="collection"></param>
        /// <returns>添加集合后的队列对象</returns>
#if NET20
        public static Queue<T> EnqueueRange<T>(Queue<T> target, IEnumerable collection)
#else
        public static Queue<T> EnqueueRange<T>(this Queue<T> target, IEnumerable collection)
#endif
        {
            foreach (T item in collection)
                target.Enqueue(item);
            return target;
        }
    }
}
