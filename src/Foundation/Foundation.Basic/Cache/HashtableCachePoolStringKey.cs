using System;
using System.Collections;

//自定义命名空间
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Foundation.Basic.Cache
{
    public class HashtableCachePoolStringKey
    {
        #region "私有字段"

        /// <summary>
        /// 内部的缓冲区
        /// </summary>
        private Hashtable innerCache = null;

        #endregion

        #region "公共方法"

        /// <summary>
        /// 根据键值比较委托，构建对象
        /// </summary>
        /// <param name="defaultKeyComparer">key值比较器</param>
        /// <param name="needSynchronized">是否需要线程安全的包装，默认为true；支持线程安全</param>
        public HashtableCachePoolStringKey( IEqualityComparer defaultKeyComparer = null, Boolean needSynchronized = true)
        {
            if (defaultKeyComparer == null)
                defaultKeyComparer = new StringEqualityIgnoreCaseComparer();
            if (needSynchronized)
                innerCache = Hashtable.Synchronized(new Hashtable(defaultKeyComparer));
            else
                innerCache = new Hashtable(defaultKeyComparer);
        }

        /// <summary>
        /// 移除所有记录项
        /// </summary>
        public void Clear()
        {
            this.innerCache.Clear();
        }

        /// <summary>
        /// 或者或者设置值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Object this[Object index]
        {
            get
            {
                return innerCache[index];
            }
            set
            {
                innerCache[index] = value;
            }
        }

        #endregion
    }
}
