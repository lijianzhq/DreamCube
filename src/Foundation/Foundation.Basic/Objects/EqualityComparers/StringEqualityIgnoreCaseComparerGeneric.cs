using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Objects.EqualityComparers
{
    /// <summary>
    /// 默认的字符串的比较器；不区分大小写
    /// </summary>
    public class StringEqualityIgnoreCaseComparerGeneric : IEqualityComparer<String>
    {
        public Boolean Equals(String x, String y)
        {
            return String.Compare(x, y, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        public Int32 GetHashCode(String obj)
        {
            return obj.GetHashCode();
        }
    }
}
