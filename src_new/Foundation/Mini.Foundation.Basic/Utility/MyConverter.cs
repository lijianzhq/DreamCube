using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mini.Foundation.Basic.Utility
{
    public static class MyConverter
    {
        public static T ChangeType<T>(Object value)
        {
            return (T)ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object ChangeType(Object value, Type type)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_3
            return Convert.ChangeType(value, type);
#else
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = Convert.ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
#endif
        }

#if !(NETSTANDARD1_0 || NETSTANDARD1_3)

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        public class VersionConverter : TypeConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                string strvalue = value as string;
                if (strvalue != null)
                {
                    return new Version(strvalue);
                }
                else
                {
                    return new Version();
                }
            }
        }
#endif
    }
}
