using System;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// 以字符串作为键值，键值的比较是忽略大小写的
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class WeakDictionaryCachePoolStringKey<TValue> : WeakDictionaryCachePool<String, TValue> where TValue : class
    {
        public WeakDictionaryCachePoolStringKey(IEqualityComparer<String> keyComparer = null)
            : base(keyComparer == null ? (new StringEqualityIgnoreCaseComparerGeneric()) : keyComparer)
        { }
    }
}
