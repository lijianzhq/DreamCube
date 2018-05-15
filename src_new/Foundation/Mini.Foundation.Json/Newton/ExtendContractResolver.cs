using System;
using System.Reflection;

//第三方的命名空间
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mini.Foundation.Json.Newton
{
    public class ExtendContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 方案一：重新此方法，根据类型返回自己的合约，然后再通过合约去构造不同的converter
        /// Creates a <see cref="JsonPrimitiveContract"/> for the given type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>A <see cref="JsonPrimitiveContract"/> for the given type.</returns>
        //protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        //{
        //    if (objectType == typeof(DBNull))
        //    {
        //        return new DBNullValueContract(objectType);
        //    }
        //    return base.CreatePrimitiveContract(objectType);
        //}

        /// <summary>
        /// 方案二：重写此方法，直接根据类型返回指定的converter
        /// Resolves the default <see cref="JsonConverter" /> for the contract.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The contract's default <see cref="JsonConverter" />.</returns>
        //protected override JsonConverter ResolveContractConverter(Type objectType)
        //{
        //    return JsonTypeReflector.GetJsonConverter(objectType, objectType);
        //}

        /// <summary>
        /// 重写此方法，重新构造valueprovider对象，通过这个方式可以实现空值处理，属性值加密，隐藏等需求
        /// Creates the <see cref="IValueProvider"/> used by the serializer to get and set values from a member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The <see cref="IValueProvider"/> used by the serializer to get and set values from a member.</returns>
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            object[] attrs = member.GetCustomAttributes(typeof(NullValueProviderAttribute), true);
            if (attrs != null && attrs.Length > 0)
                return new NullValueProvider(member);
            // warning - this method use to cause errors with Intellitrace. Retest in VS Ultimate after changes
            return base.CreateMemberValueProvider(member);
        }
    }
}
