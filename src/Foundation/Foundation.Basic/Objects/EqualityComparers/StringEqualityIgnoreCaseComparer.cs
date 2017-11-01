using System;
using System.Collections;

namespace DreamCube.Foundation.Basic.Objects.EqualityComparers
{
    /// <summary>
    /// 指定字符串比较器
    /// </summary>
    public class StringEqualityIgnoreCaseComparer : IEqualityComparer
    {
        public new Boolean Equals(Object x, Object y)
        {
            Type strType = typeof(String);
            return x.GetType() == strType &&
                   y.GetType() == strType &&
                   String.Compare(x.ToString(), y.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        public Int32 GetHashCode(Object obj)
        {
            return obj.GetHashCode();
        }
    }
}
