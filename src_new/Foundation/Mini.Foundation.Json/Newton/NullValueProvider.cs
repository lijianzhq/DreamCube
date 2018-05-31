using System;
using System.Reflection;
using System.Collections;
using System.Globalization;

using Newtonsoft.Json.Serialization;

namespace Mini.Foundation.Json.Newton
{
    /// <summary>
    /// </summary>
    public class NullValueProvider : IValueProvider
    {
        protected readonly MemberInfo _memberInfo;
#if !(PORTABLE40 || PORTABLE || DOTNET || NETSTANDARD2_0)
        protected DynamicValueProvider _innerValueProvider;
#elif !(PORTABLE40)
        protected ExpressionValueProvider _innerValueProvider;
#else
        protected ExpressionValueProvider _innerValueProvider;
#endif
        public NullValueProvider(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
#if !(PORTABLE40 || PORTABLE || DOTNET || NETSTANDARD2_0)
            _innerValueProvider = new DynamicValueProvider(memberInfo);
#elif !(PORTABLE40)
            _innerValueProvider = new ExpressionValueProvider(memberInfo);
#else
            _innerValueProvider = new ExpressionValueProvider(memberInfo);
#endif
        }

        public Object GetValue(object target)
        {        //在这里可以做很多处理，可以在序列化的时候对敏感数据进行屏蔽等等
            Object value = _innerValueProvider.GetValue(target);
            if (value == null)
            {
                PropertyInfo property = _memberInfo as PropertyInfo;
                FieldInfo field = property == null ? _memberInfo as FieldInfo : null;
                Type memberType = property == null ? field.FieldType : property.PropertyType;
                if (memberType == typeof(String))
                    return "";
#if !(NETSTANDARD1_0 || NETSTANDARD1_3)
                else if (typeof(IEnumerable).IsAssignableFrom(memberType))
                    return new object[] { };
                else if (memberType.IsClass)
                    return new object();
#endif
            }
            return value;
        }

        public void SetValue(object target, object value)
        {
            _innerValueProvider.SetValue(target, value);
        }
    }
}
