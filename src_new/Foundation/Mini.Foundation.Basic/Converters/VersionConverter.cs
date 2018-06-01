using System;
using System.ComponentModel;
using System.Globalization;

namespace Mini.Foundation.Basic.Converters
{
#if !(NETSTANDARD1_0 || NETSTANDARD1_3)

    /// <summary>
    /// 版本和字符串之间的转换器
    /// </summary>
    public class VersionConverter : TypeConverter
    {
        /// <summary>
        /// 重写基类方法，实现从字符串转换version对象的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            string strValue = value as String;
            if (!Utility.MyString.IsInvisibleString(strValue))
            {
                return new Version(strValue);
            }
            else
            {
                return new Version();
            }
        }
    }

#endif
}
