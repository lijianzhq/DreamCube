using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Cache.Interface
{
    public interface IQueueCachePool<T>
    {
        /// <summary>
        /// 往队列中增加一项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void Enqueue(T item);

        /// <summary>
        /// 从队列中删除一项
        /// </summary>
        /// <returns></returns>
        T Dequeue();

        /// <summary>
        /// 清除所有缓存项
        /// 如果在控制之外的代码获得了写锁，则此方法内部不再做同步锁的处理
        /// </summary>
        void Clear();
    }
}
