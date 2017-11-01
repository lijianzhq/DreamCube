using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Cache.Interface;
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyType
    {
        #region "字段"

        private static IDictionaryCachePool<String, Object[]> attributeCache = new DictionaryCachePool<String, Object[]>();

        private static IDictionaryCachePool<String, MemberInfo[]> reflectionCache = new DictionaryCachePool<String, MemberInfo[]>();

        #endregion

        #region "属性"

        public static readonly BindingFlags AllFlag = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
        public static readonly BindingFlags StaticFlag = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
        public static readonly BindingFlags InstanceFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        public static readonly BindingFlags InstancePublic = BindingFlags.Public | BindingFlags.Instance;

        #endregion

        #region "针对目标对象为Type的公共方法"

        #region "获取Attribute的方法"

        /// <summary>
        /// 获取类型指定字段的指定标签（attribute）的值
        /// 必须传入Type对象实例，如果是直接操作目标对象，则调用ObjectExtension_Reflection类的相关方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="attributeType">attribute类型</param>
        /// <returns></returns>
#if NET20
        public static Object[] GetFieldAttributes(Type type,
                                                  String fieldName,
                                                  Type attributeType,
                                                  Boolean inherit = true)
#else
        public static Object[] GetFieldAttributes(this Type type,
                                                  String fieldName,
                                                  Type attributeType,
                                                  Boolean inherit = true)
#endif
        {
            Object[] values = null;
            String key = CreateAttributeCacheKey(type.Name, attributeType.Name, fieldName);
            if (attributeCache.TryGetValue(key, out values))
                return values;
            FieldInfo info = MyType.GetFieldEx(type, fieldName);
            if (info != null) values = info.GetCustomAttributes(attributeType, inherit);
            attributeCache.TryAdd(key, values);
            return values;
        }

        /// <summary>
        /// 获取一个类型所有属性的TAttribute标签，以及与其对应的属性的匹配关系
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="targetType"></param>
        /// <param name="inherit">只是是否在此对象的祖先中查找此标志，true表示查找祖先</param>
        /// <returns></returns>
#if NET20
        public static Dictionary<PropertyInfo, TAttribute> 
            GetAttributesOfProperties<TAttribute>(Type targetType, Boolean inherit = true) 
                where TAttribute : Attribute
#else
        public static Dictionary<PropertyInfo, TAttribute>
            GetAttributesOfProperties<TAttribute>(this Type targetType, Boolean inherit = true)
                where TAttribute : Attribute
#endif
        {
            Dictionary<PropertyInfo, TAttribute> attrList = new Dictionary<PropertyInfo, TAttribute>();
            PropertyInfo[] properties = MyType.GetPropertiesEx(targetType);
            if (properties == null || properties.Length == 0) return null;
            for (Int32 i = 0; i < properties.Length; i++)
            {
                PropertyInfo info = properties[i];
                TAttribute attr = Attribute.GetCustomAttribute(info, typeof(TAttribute), inherit)
                                                as TAttribute;
                if (attr != null)
                    attrList.Add(info, attr);
            }
            return attrList;
        }

        /// <summary>
        /// 获取类型指定字段的指定标签（attribute）的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <param name="attributeType">attribute类型</param>
        /// <returns></returns>
#if NET20
        public static Object[] GetPropertyAttributes(Type type,
                                                    String propertyName,
                                                    Type attributeType,
                                                    Boolean inherit = true)
#else
        public static Object[] GetPropertyAttributes(this Type type,
                                                    String propertyName,
                                                    Type attributeType,
                                                    Boolean inherit = true)
#endif
        {
            Object[] values = null;
            String key = CreateAttributeCacheKey(type.Name, attributeType.Name, propertyName);
            if (attributeCache.TryGetValue(key, out values))
                return values;
            PropertyInfo info = MyType.GetPropertyEx(type, propertyName);
            if (info != null)
                values = info.GetCustomAttributes(attributeType, inherit);
            attributeCache.TryAdd(key, values);
            return values;
        }

        /// <summary>
        /// 获取指定类型的所有标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit">指定是否搜索继承链以查找此特性</param>
        /// <returns></returns>
#if NET20
        public static T[] GetAllAttributes<T>(Type type, Boolean inherit = true) where T : class
#else
        public static T[] GetAllAttributes<T>(this Type type, Boolean inherit = true) where T : class
#endif
        {
            Object[] attributes = type.GetCustomAttributes(typeof(T), inherit);
            T[] result = MyArray.ConvertItemToTargetType<T>(attributes);
            return result;
        }

        /// <summary>
        /// 获取类型指定属性的DescriptionAttribute属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
#if NET20
        public static String GetPropertyDescription(Type type, String propertyName)
#else
        public static String GetPropertyDescription(this Type type, String propertyName)
#endif
        {
            Object[] attrs = MyType.GetPropertyAttributes(type, propertyName, typeof(DescriptionAttribute));
            return (attrs != null && attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : String.Empty;
        }

        /// <summary>
        /// 获取类型指定字段的DescriptionAttribute属性值
        /// 获取枚举项的Description值，请使用此方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
#if NET20
        public static String GetFieldDescription(Type type, String fieldName)
#else
        public static String GetFieldDescription(this Type type, String fieldName)
#endif
        {
            Object[] attrs = MyType.GetFieldAttributes(type, fieldName, typeof(DescriptionAttribute));
            return (attrs != null && attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : String.Empty;
        }

        #endregion

        #region "创建对象的方法"

        /// <summary>
        /// 创建Remoting对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objUrl"></param>
        /// <returns></returns>
#if NET20
        public static T CreateRemotingObject<T>(String objUrl) where T : MarshalByRefObject
#else
        public static T CreateRemotingObject<T>(String objUrl) where T : MarshalByRefObject
#endif
        {
            return Activator.GetObject(typeof(T), objUrl) as T;
        }

        /// <summary>
        /// 创建指定类型的一个实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
#if NET20
        public static Object CreateInstance(Type type)
#else
        public static Object CreateInstance(this Type type)
#endif
        {
            if (type == null) return null;
            Assembly assembly = type.Assembly;
            return assembly.CreateInstance(type.FullName);
        }

        /// <summary>
        /// 调用类型的构造函数创建类型实例
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="os">构造函数需要的参数</param>
        /// <returns></returns>
        public static Object CreateInstance(Type t, params Object[] os)
        {
            var ts = GetTypesByObjs(os);
            var ci = t.GetConstructor(InstanceFlag, null, ts, null);
            return ci.Invoke(os);
        }

        /// <summary>
        /// 创建指定类型的一个实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
#if NET20
        public static T CreateInstance<T>(Type type)
#else
        public static T CreateInstance<T>(this Type type)
#endif
        {
            if (type == null) return default(T);
            Assembly assembly = type.Assembly;
            return (T)assembly.CreateInstance(type.FullName);
        }

        /// <summary>
        /// 创建指定类型的一个实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="operType">执行方法的响应动作</param>
        /// <param name="defaultValue">如果执行失败仍然返回，则返回此默认值</param>
        /// <returns></returns>
#if NET20
        public static T CreateInstance<T>(Type type, T defaultValue = default(T))
#else
        public static T CreateInstance<T>(this Type type, T defaultValue = default(T))
#endif
        {
            if (type == null) return defaultValue;
            Assembly assembly = type.Assembly;
            return (T)assembly.CreateInstance(type.FullName);
        }

        #endregion

        #region "获取字段、属性的方法"

        /// <summary>
        /// 获取类型所有的属性；
        /// 可以通过排除类型或者包括类型来获取指定类型type的属性，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
#if NET20
        public static PropertyInfo[] GetPropertiesEx(Type type,
                                                    BindingFlags[] unbindingFlags = null,
                                                    BindingFlags[] bindingFlags = null)
#else
       public static PropertyInfo[] GetPropertiesEx(this Type type,
                                                    BindingFlags[] unbindingFlags = null,
                                                    BindingFlags[] bindingFlags = null)
#endif
        {
            MemberInfo[] infos = null;
            Boolean b = unbindingFlags == null && bindingFlags == null;
            String typeName = type.ToString();
            String cacheKey = CreateCacheKey(typeName, CacheKeyType.property);
            if (b && reflectionCache.TryGetValue(cacheKey, out infos))
                return infos as PropertyInfo[];
            //获取属性
            infos = type.GetProperties(InitialBindingFlags(unbindingFlags, bindingFlags));
            if (b) reflectionCache.TryAdd(cacheKey, infos);
            return infos as PropertyInfo[];
        }

        /// <summary>
        /// 获取类型指定名称的属性；
        /// 可以通过排除类型或者包括类型来获取指定类型type的属性，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
#if NET20
        public static PropertyInfo GetPropertyEx(Type type,
                                               String name,
                                               BindingFlags[] unbindingFlags = null,
                                               BindingFlags[] bindingFlags = null)
#else
        public static PropertyInfo GetPropertyEx(this Type type,
                                            String name,
                                            BindingFlags[] unbindingFlags = null,
                                            BindingFlags[] bindingFlags = null)
#endif
        {
            Boolean b = unbindingFlags == null && bindingFlags == null;
            if (b)
            {
                MemberInfo[] infos = null;
                String typeName = type.ToString();
                String cacheKey = CreateCacheKey(typeName, CacheKeyType.property);
                if (!reflectionCache.TryGetValue(cacheKey, out infos))
                    infos = GetPropertiesEx(type);
                if (infos != null)
                {
                    for (Int32 i = 0, j = infos.Length; i < j; i++)
                        if (String.Compare(name, infos[i].Name, true) == 0) return infos[i] as PropertyInfo;
                }
            }
            return type.GetProperty(name, InitialBindingFlags(unbindingFlags, bindingFlags));
        }

        /// <summary>
        /// 获取类型所有的字段；
        /// 可以通过排除类型或者包括类型来获取指定类型type的字段，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unbindingFlags">排除以外类型的字段</param>
        /// <param name="bindingFlags">包括在内的类型字段</param>
        /// <returns></returns>
#if NET20
        public static FieldInfo[] GetFieldsEx(Type type,
                                              BindingFlags[] unbindingFlags = null,
                                              BindingFlags[] bindingFlags = null)
#else
        public static FieldInfo[] GetFieldsEx(this Type type,
                                              BindingFlags[] unbindingFlags = null,
                                              BindingFlags[] bindingFlags = null)
#endif
        {
            MemberInfo[] infos = null;
            Boolean b = unbindingFlags == null && bindingFlags == null;
            String typeName = type.ToString();
            String cacheKey = CreateCacheKey(typeName, CacheKeyType.field);
            if (b && reflectionCache.TryGetValue(cacheKey, out infos))
                return infos as FieldInfo[];
            //获取字段
            infos = type.GetFields(InitialBindingFlags(unbindingFlags, bindingFlags));
            if (b) reflectionCache.TryAdd(cacheKey, infos);
            return infos as FieldInfo[];
        }

        /// <summary>
        /// 获取类型指定名称的字段；
        /// 可以通过排除类型或者包括类型来获取指定类型type的字段，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="unbindingFlags"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        
#if NET20
        public static FieldInfo GetFieldEx(Type type,
                                           String name,
                                           BindingFlags[] unbindingFlags = null,
                                           BindingFlags[] bindingFlags = null)
#else
        public static FieldInfo GetFieldEx(this Type type,
                                           String name,
                                           BindingFlags[] unbindingFlags = null,
                                           BindingFlags[] bindingFlags = null)
#endif
        {
            Boolean b = unbindingFlags == null && bindingFlags == null;
            if (b)
            {
                MemberInfo[] infos = null;
                String typeName = type.ToString();
                String cacheKey = CreateCacheKey(typeName, CacheKeyType.field);
                if (!reflectionCache.TryGetValue(cacheKey, out infos))
                    infos = GetFieldsEx(type);
                if (infos != null)
                {
                    for (Int32 i = 0, j = infos.Length; i < j; i++)
                        if (String.Compare(name, infos[i].Name, true) == 0) return infos[i] as FieldInfo;
                }
            }
            return type.GetField(name, InitialBindingFlags(unbindingFlags, bindingFlags));
        }

        #endregion

        #endregion

        #region "针对目标对象为Object的公共方法（这些方法是对象实例调用的，调用的时候切勿传入了对象的Type类型来调用）"

        #region "获取对象属性（Attribute）相关操作"

        /// <summary>
        /// 获取一个对象所有属性的TAttribute标签以及其对应属性的关系
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="target"></param>
        /// <param name="inherit">只是是否在此对象的祖先中查找此标志，true表示查找祖先</param>
        /// <returns></returns>
#if NET20
        public static Dictionary<PropertyInfo, TAttribute>
            GetAttributesOfProperties<TAttribute>(Object target, Boolean inherit = true)
                where TAttribute : Attribute
#else
        public static Dictionary<PropertyInfo, TAttribute>
            GetAttributesOfProperties<TAttribute>(this Object target, Boolean inherit = true)
                where TAttribute : Attribute
#endif
        {
            Type targetType = target.GetType();
            return MyType.GetAttributesOfProperties<TAttribute>(targetType, inherit);
        }

        /// <summary>
        /// 获取类型指定字段的所有指定类型的标签
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="attributeType">attribute类型</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>应用于此字段的自定义属性的数组；如果未应用任何属性，则为包含零 (0) 个元素的数组。</returns>
#if NET20
        public static Object[] GetFieldAttributes(Object obj,
                                                  String fieldName,
                                                  Type attributeType,
                                                  Boolean inherit = true)
#else
            public static Object[] GetFieldAttributes(this Object obj,
                                                  String fieldName,
                                                  Type attributeType,
                                                  Boolean inherit = true)
#endif
        {
            Type targetType = obj.GetType();
            return MyType.GetFieldAttributes(targetType, fieldName, attributeType, inherit);
        }

        /// <summary>
        /// 获取类型指定属性的所有指定类型的标签
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="attributeType">attribute类型</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>应用于此属性的自定义属性的数组；如果未应用任何属性，则为包含零 (0) 个元素的数组</returns>
        
#if NET20
        public static Object[] GetPropertyAttributes(Object obj,
                                                     String propertyName,
                                                     Type attributeType,
                                                     Boolean inherit = true)
#else
        public static Object[] GetPropertyAttributes(this Object obj,
                                                     String propertyName,
                                                     Type attributeType,
                                                     Boolean inherit = true)
#endif
        {
            Type targetType = obj.GetType();
            return MyType.GetPropertyAttributes(targetType, propertyName, attributeType, inherit);
        }

        /// <summary>
        /// 获取指定类型的所有标签
        /// </summary>
        /// <typeparam name="T">执行的Attribute类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="inherit">指定是否搜索继承链以查找此特性</param>
        /// <returns></returns>
#if NET20
        public static T[] GetAttributes<T>(Object obj, Boolean inherit = true) where T : Attribute
#else
        public static T[] GetAttributes<T>(this Object obj, Boolean inherit = true) where T : Attribute
#endif
        {
            Type targetType = obj.GetType();
            return MyType.GetAllAttributes<T>(targetType, inherit);
        }

        /// <summary>
        /// 获取类型指定属性的DescriptionAttribute属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
#if NET20
        public static String GetPropertyDescription(Object obj, String propertyName)
#else
        public static String GetPropertyDescription(this Object obj, String propertyName)
#endif
        {
            Type targetType = obj.GetType();
            return MyType.GetPropertyDescription(targetType, propertyName);
        }

        /// <summary>
        /// 获取类型指定字段的DescriptionAttribute属性值
        /// 获取枚举项的Description值，请使用此方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
#if NET20
        public static String GetFieldDescription(Object obj, String fieldName)
#else
        public static String GetFieldDescription(this Object obj, String fieldName)
#endif
        {
            Type targetType = obj.GetType();
            return MyType.GetFieldDescription(targetType, fieldName);
        }

        #endregion

        #region "获取字段、属性"

        /// <summary>
        /// 获取对象的所有属性；（注意：直接用对象实例调用此方法，不要用Type对象实例调用此方法）
        /// 可以通过排除类型或者包括类型来获取指定类型type的属性，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
#if NET20
        public static PropertyInfo[] GetPropertiesEx(Object obj,
                                                     BindingFlags[] unbindingFlags = null,
                                                     BindingFlags[] bindingFlags = null)
#else
        public static PropertyInfo[] GetPropertiesEx(this Object obj,
                                                     BindingFlags[] unbindingFlags = null,
                                                     BindingFlags[] bindingFlags = null)
#endif
        {
            if (obj == null) return null;
            Type type = obj.GetType().UnderlyingSystemType; //实际的对象类型
            return MyType.GetPropertiesEx(type, unbindingFlags, bindingFlags);
        }

        /// <summary>
        /// 获取对象指定名称的属性；（注意：直接用对象实例调用此方法，不要用Type对象实例调用此方法）
        /// 可以通过排除类型或者包括类型来获取指定类型type的属性，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
#if NET20
        public static PropertyInfo GetPropertyEx(Object obj,
                                                 String name,
                                                 BindingFlags[] unbindingFlags = null,
                                                 BindingFlags[] bindingFlags = null)
#else
        public static PropertyInfo GetPropertyEx(Object obj,
                                                 String name,
                                                 BindingFlags[] unbindingFlags = null,
                                                 BindingFlags[] bindingFlags = null)
#endif
        {
            if (obj == null) return null;
            Type type = obj.GetType().UnderlyingSystemType;
            return MyType.GetPropertyEx(type, name, unbindingFlags, bindingFlags);
        }

        /// <summary>
        /// 获取对象所有的字段；（注意：直接用对象实例调用此方法，不要用Type对象实例调用此方法）
        /// 可以通过排除类型或者包括类型来获取指定类型type的字段，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="unbindingFlags">排除以外类型的字段</param>
        /// <param name="bindingFlags">包括在内的类型字段</param>
        /// <returns></returns>
        
#if NET20
        public static FieldInfo[] GetFieldsEx(Object obj,
                                             BindingFlags[] unbindingFlags = null,
                                             BindingFlags[] bindingFlags = null)
#else
        public static FieldInfo[] GetFieldsEx(this Object obj,
                                             BindingFlags[] unbindingFlags = null,
                                             BindingFlags[] bindingFlags = null)
#endif
        {
            if (obj == null) return null;
            Type type = obj.GetType().UnderlyingSystemType;
            return MyType.GetFieldsEx(type, unbindingFlags, bindingFlags);
        }

        /// <summary>
        /// 获取对象指定名称的字段；（注意：直接用对象实例调用此方法，不要用Type对象实例调用此方法）
        /// 可以通过排除类型或者包括类型来获取指定类型type的字段，默认不传入排除类型和包括类型时，搜索范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// 如果指定某类型既是排除类型又是包括类型时，则会抛出ArgumentException异常
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="unbindingFlags"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
#if NET20
        public static FieldInfo GetFieldEx(Object obj,
                                            String name,
                                            BindingFlags[] unbindingFlags = null,
                                            BindingFlags[] bindingFlags = null)
#else
            public static FieldInfo GetFieldEx(this Object obj,
                                            String name,
                                            BindingFlags[] unbindingFlags = null,
                                            BindingFlags[] bindingFlags = null)
#endif
        {
            if (obj == null) return null;
            Type type = obj.GetType().UnderlyingSystemType;
            return MyType.GetFieldEx(type, name, unbindingFlags, bindingFlags);
        }

        #endregion

        #endregion

        #region "针对目标对象为String的共用方法"

        /// <summary>
        /// 获取参数的类型数组（按参数数组顺序返回类型数组）
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <returns></returns>
        public static List<Type> GetParameterInfoType(ParameterInfo[] parameterInfos)
        {
            if (parameterInfos == null || parameterInfos.Length  == 0) return null;
            List<Type> types = new List<Type>();
            for (Int32 i = 0; i < parameterInfos.Length; i++)
                types.Add(parameterInfos[i].ParameterType);
            return types;
        }

        /// <summary>
        /// 把inputs数组的值，转换成指定的目标类型；
        /// 类型数组必须和值数组一一对应
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Object[] FormatToTargetType(Object[] inputs, Type[] types)
        {
            if (inputs == null) return null;
            if (inputs.Length != types.Length)
                throw new Exception(String.Format(Properties.Resources.ExceptionParamsCountNotEqual, "值数组inputs", "类型数组types"));
            List<Object> outputs = new List<Object>();
            for (Int32 i = 0, j = inputs.Length; i < j; i++)
                outputs.Add(MyType.FormatToTargetType(inputs[i], types[i]));
            return outputs.ToArray();
        }

        /// <summary>
        /// 把inputs数组的值，转换成指定的目标类型；
        /// 类型数组必须和值数组一一对应
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="targetTypes"></param>
        /// <returns></returns>
        public static Object[] FormatToTargetType(Object[] inputs, String[] targetTypes)
        {
            if (inputs == null) return null;
            if (inputs.Length != targetTypes.Length)
                throw new Exception(String.Format(Properties.Resources.ExceptionParamsCountNotEqual, "值数组inputs", "类型数组targetTypes"));
            List<Object> outputs = new List<Object>();
            for (Int32 i = 0, j = inputs.Length; i < j; i++)
                outputs.Add(MyType.FormatToTargetType(inputs[i], targetTypes[i]));
            return outputs.ToArray();
        }

        /// <summary>
        /// 把数据转换成目标的类型
        /// 仅支持：string;char;single/float;int32/int;int64;int16;double;bool/boolean;uint16;uint32;uint64;datetime;decimal;object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object FormatToTargetType(Object input, String targetType)
        {
            return FormatToTargetType(input, MyType.GetType(targetType));
        }

        /// <summary>
        /// 获取对象数组的类型，按顺序返回类型数组
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        public static Type[] GetTypesByObjs(params Object[] os)
        {
            var ts = new Type[os.Length];
            for (int i = 0; i < os.Length; i++)
                ts[i] = os[i].GetType();
            return ts;
        }

        /// <summary>
        /// 把数据转换成目标的类型
        /// 仅支持：string;char;single/float;int32/int;int64;int16;double;bool/boolean;uint16;uint32;uint64;datetime;decimal;object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object FormatToTargetType(Object input, Type targetType)
        {
            if(input == null)return null;
            if (targetType == typeof(Boolean))
                return MyString.ToBoolean(input.ToString());
            else if (targetType == typeof(String))
                return input;
            else if (targetType == typeof(float))
                return Convert.ToSingle(input);
            else if (targetType == typeof(Int32))
                return Convert.ToInt32(input);
            else if (targetType == typeof(Int16))
                return Convert.ToInt16(input);
            else if (targetType == typeof(Int64))
                return Convert.ToInt64(input);
            else if (targetType == typeof(Single))
                return Convert.ToSingle(input);
            else if (targetType == typeof(Char))
                return Convert.ToChar(input);
            else if (targetType == typeof(Double))
                return Convert.ToDouble(input);
            else if (targetType == typeof(DateTime))
                return Convert.ToDateTime(input);
            else if (targetType == typeof(UInt16))
                return Convert.ToUInt16(input);
            else if (targetType == typeof(UInt32))
                return Convert.ToUInt32(input);
            else if (targetType == typeof(UInt64))
                return Convert.ToUInt64(input);
            else if(targetType == typeof(Decimal))
                return Convert.ToDecimal(input);
            return input;
        }

        /// <summary>
        /// 根据类型字符串获取.NET自带的数据类型Type对象
        /// 仅支持：string;char;single/float;int32/int;int64;int16;double;bool/boolean;uint16;uint32;uint64;datetime;decimal;object
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
#if NET20
        public static Type GetType(String typeStr)
#else 
        public static Type GetType(this String typeStr)
#endif
        {
            if (String.IsNullOrEmpty(typeStr)) return null;
            typeStr = typeStr.Trim().ToLower();
            switch (typeStr)
            {
                case "string": return typeof(String);
                case "char": return typeof(Char);
                case "single":
                case "float":
                    return typeof(Single);
                case "int32":
                case "int":
                    return typeof(Int32);
                case "int64":
                    return typeof(Int64);
                case "int16":
                    return typeof(Int16);
                case "double": return typeof(double);
                case "object": return typeof(object);
                case "bool":
                case "boolean":
                    return typeof(Boolean);
                case "uint16":
                    return typeof(UInt16);
                case "uint32":
                    return typeof(UInt32);
                case "uint64":
                    return typeof(UInt64);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(Decimal);
            }
            return null;
        }

        #endregion

        #region "私有辅助方法"

        /// <summary>
        /// 创建反射获取MemberInfo的key值
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        private static String CreateCacheKey(String typeName, CacheKeyType keyType)
        {
            return String.Format("{0}_{1}", typeName, keyType.ToString());
        }

        /// <summary>
        /// 创建attribute缓存池中的key值
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="itemName"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        private static String CreateAttributeCacheKey(String typeName,
                                                    String attributeTypeName,
                                                    String itemName)
        {
            return String.Format("{0}_{1}_{2}", typeName, attributeTypeName, itemName);
        }

        /// <summary>
        /// 获取反射是的绑定绑定搜索范围
        /// 默认搜索的范围为：实例级别；类级别；忽略大小写；public、protected、private
        /// </summary>
        /// <param name="unbindingFlags"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        private static BindingFlags InitialBindingFlags(BindingFlags[] unbindingFlags = null, BindingFlags[] bindingFlags = null)
        {
            //验证排除类型数组与包含类型数组是否有相交，相矛盾
            if (unbindingFlags != null && bindingFlags != null)
                if (MyArray.HasEqualItem<BindingFlags>(unbindingFlags,bindingFlags))
                    throw new ArgumentException(String.Format(
                                                Properties.Resources.ExceptionMismatchParams,
                                                "unbindingFlags", "bindingFlags"));

            //默认的绑定方式
            BindingFlags defaultFlag = BindingFlags.Instance |
                                        BindingFlags.IgnoreCase |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Public |
                                        BindingFlags.Static;

            //计算反射搜索字段的标志
            BindingFlags bindingFlag = defaultFlag;
            if (bindingFlags != null)
            {
                for (Int32 i = 0, j = bindingFlags.Length; i < j; i++)
                    bindingFlag = bindingFlag | bindingFlags[i];
            }
            if (unbindingFlags != null)
            {
                for (Int32 i = 0, j = unbindingFlags.Length; i < j; i++)
                    bindingFlag = bindingFlag & (~unbindingFlags[i]);
            }
            return bindingFlag;
        }

        #endregion

        #region "私有内部枚举值"

        private enum CacheKeyType
        {
            /// <summary>
            /// 属性类型
            /// </summary>
            property,

            /// <summary>
            /// 字段类型
            /// </summary>
            field
        }

        #endregion
    }
}
