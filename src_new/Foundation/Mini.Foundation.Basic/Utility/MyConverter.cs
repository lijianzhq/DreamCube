using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// </summary>
    public static class MyConvert
    {
        /// <summary>
        /// 把字符串转换为整型，如果转换失败，则返回默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败返回值</param>
        /// <returns></returns>
        public static Int32 ToInt32<T>(T value, Int32 defaultValue = 0)
        {
            Int32 result = 0;
            if (Int32.TryParse(Convert.ToString(value), out result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// 泛型版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 向系统注册TypeConverter，当调用convert方法的时候，对应的类型转换器会自动被调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TC"></typeparam>
        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

#endif
    }
}
