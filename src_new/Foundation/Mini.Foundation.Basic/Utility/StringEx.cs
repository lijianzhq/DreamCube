using System;

namespace Mini.Foundation.Basic.Utility
{
    public static class StringEx
    {
        /// <summary>
        /// net20,net35调用String.IsNullOrEmpty，其他类库调用String.IsNullOrWhiteSpace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsInvisibleString(String value)
        {
#if NET20 || NET35
            return String.IsNullOrEmpty(value);
#else
            return String.IsNullOrWhiteSpace(value);
#endif
        }
    }
}
