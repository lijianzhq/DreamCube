using System;

namespace DreamCube.Foundation.Basic.Cache.Interface
{
    public interface IListCachePool<T>
    {
        /// <summary>
        /// 索引属性
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        T this[Int32 i]
        {
            get;
        }

        /// <summary>
        /// 向缓冲区添加缓存项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void Add(T item);

        /// <summary>
        /// 根据序号移除项
        /// </summary>
        /// <param name="index"></param>
        void RemoveAt(Int32 index);

        /// <summary>
        /// 清除所有缓存项
        /// 如果在控制之外的代码获得了写锁，则此方法内部不再做同步锁的处理
        /// </summary>
        void Clear();

        /// <summary>
        /// 判断是否包含某项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Boolean Contains(T item);

        /// <summary>
        /// 异常指定项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void Remove(T item);
    }
}
