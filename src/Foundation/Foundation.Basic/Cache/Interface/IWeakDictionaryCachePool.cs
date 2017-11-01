using System;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Foundation.Basic.Cache.Interface
{
    public interface IWeakDictionaryCachePool<TKey, TValue> : IDictionaryCachePool<TKey, TValue>
    {
        /// <summary>
        /// 把缓冲区升级为强引用（注意：必须在使用前升级为强引用才有用）
        /// 在遍历的时候，先升级为强引用，会大大提高效率
        /// </summary>
        void UpToStrongReference();

        /// <summary>
        /// 把缓冲区降级为弱引用
        /// </summary>
        void DownToWeakReference();
    }
}
