using System;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Foundation.Basic.Cache
{
    /// <summary>
    /// Key值为String类型的；默认的比较方式为忽略字符串的大小写匹配
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryCachePoolStringKey<TValue> : DictionaryCachePool<String, TValue>
    {
        public DictionaryCachePoolStringKey(IEqualityComparer<String> keyComparer = null)
            : base(keyComparer == null ? (new StringEqualityIgnoreCaseComparerGeneric()) : keyComparer)
        { }
    }
}
