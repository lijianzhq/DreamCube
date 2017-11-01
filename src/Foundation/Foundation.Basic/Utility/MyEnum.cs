using System;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 枚举值
    /// </summary>
    public static class MyEnum
    {
        #region "字段"

        /// <summary>
        /// 增加缓冲区
        /// </summary>
        private static Cache.DictionaryCachePool<String, Object> cacheBlock = new Cache.DictionaryCachePool<String, Object>();
        private static Dictionary<String, Object> enumItemsCache = new Dictionary<String, Object>();

        #endregion

        /// <summary>
        /// 转换成枚举类型（吞并异常信息）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
#if NET20
        public static T ToEnum<T>(String enumString, Boolean ignoreCase = true) where T : struct
        {
            T tempValue = default(T);

            try
            {
                tempValue = (T)Enum.Parse(typeof(T), enumString, ignoreCase);
            }
            catch (Exception)
            { }
            return tempValue;
        }
#else 
        public static T ToEnum<T>(this String enumString, Boolean ignoreCase = true) where T : struct
        {
            T tempValue = default(T);
            Enum.TryParse<T>(enumString, ignoreCase, out tempValue);
            return tempValue;
        }
#endif

        /// <summary>
        /// 转换成枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString">枚举项的字符串</param>
        /// <param name="enumValue">返回枚举值</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryToEnum<T>(String enumString, out T enumValue, Boolean ignoreCase = true) where T : struct
        {
            enumValue = default(T);
            Boolean result = false;
            try
            {
                enumValue = (T)Enum.Parse(typeof(T), enumString, ignoreCase);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
#else
        public static Boolean TryToEnum<T>(this String enumString, out T enumValue, Boolean ignoreCase = true) where T : struct
        {
            enumValue = default(T);
            Boolean result = false;
            result = Enum.TryParse<T>(enumString, ignoreCase, out enumValue);
            return result;
        }
#endif

        /// <summary>
        /// 根据枚举值的描述文本，获取对应的枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptionText"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static Object GetEnumItemByDescriptionText<T>(String descriptionText, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase) where T : struct
        {
            Type t = typeof(T);
            String typeName = t.ToString();
            if (!enumItemsCache.ContainsKey(typeName))
                enumItemsCache.Add(typeName, GetEnumItems<T>());

            if (enumItemsCache.ContainsKey(typeName))
            {
                List<Objects.EnumItem<T>> enumItems = (List<Objects.EnumItem<T>>)enumItemsCache[typeName];
                for (var i = 0; i < enumItems.Count; i++)
                {
                    if (String.Equals(descriptionText, enumItems[i].Description, comparison)) return (T)enumItems[i].EnumValue;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取枚举项的Description值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static String GetEnumItemDescription<T>(String enumString) where T : struct
        {
            T enumItem = ToEnum<T>(enumString, true);
            Type t = typeof(T);
            String typeName = t.ToString();
            if (!enumItemsCache.ContainsKey(typeName)) 
                enumItemsCache.Add(typeName, GetEnumItems<T>());

            if (enumItemsCache.ContainsKey(typeName))
            {
                List<Objects.EnumItem<T>> enumItems = (List<Objects.EnumItem<T>>)enumItemsCache[typeName];
                for (var i = 0; i < enumItems.Count; i++)
                {
                    if (enumItems[i].IdentityValue == enumString) return enumItems[i].Description;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取枚举项的Description值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumItem"></param>
        /// <returns></returns>
        public static String GetEnumItemDescription<T>(T enumItem) where T : struct
        {
            String enumItemType = enumItem.ToString();
            Type t = typeof(T);
            String typeName = t.ToString();
            if (!enumItemsCache.ContainsKey(typeName))
                enumItemsCache.Add(typeName, GetEnumItems<T>());

            if (enumItemsCache.ContainsKey(typeName))
            {
                List<Objects.EnumItem<T>> enumItems = (List<Objects.EnumItem<T>>)enumItemsCache[typeName];
                for (var i = 0; i < enumItems.Count; i++)
                {
                    if (enumItems[i].IdentityValue == enumItemType) return enumItems[i].Description;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取枚举项对应的基础值，默认是Int32值
        /// </summary>
        /// <param name="enumItem">枚举项</param>
        /// <returns></returns>
        public static Object GetEnumUnderlyingValue(Object enumItem)
        {
            Type t = Enum.GetUnderlyingType(enumItem.GetType());
            return MyObject.To(enumItem, t);
        }

        /// <summary>
        /// 获取枚举类型的所有枚举项
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<Objects.EnumItem> GetEnumItems(Type enumType)
        {
            String typeStr = enumType.ToString();
            //先从缓存中查找
            if (cacheBlock.ContainsKey(typeStr))
            {
                Object enumItems = null;
                if (cacheBlock.TryGetValue(typeStr, out enumItems))
                    return (List<Objects.EnumItem>)enumItems;
            }
            String[] names = Enum.GetNames(enumType);
            List<Objects.EnumItem> items = new List<Objects.EnumItem>();
            for (Int32 i = 0; i < names.Length; i++)
            {
                Objects.EnumItem item = new Objects.EnumItem();
                item.EnumValue = Enum.Parse(enumType, names[i]);
                item.IdentityValue = names[i];
                item.Description = MyType.GetFieldDescription(enumType, names[i]);
                item.UnderlyingType = Enum.GetUnderlyingType(enumType);
                item.UnderlyingValue = MyEnum.GetEnumUnderlyingValue(item.EnumValue);
                items.Add(item);
            }
            //添加到缓存中
            cacheBlock.TryAdd(typeStr, items);
            return items;
        }

        /// <summary>
        /// 获取枚举类型的所有枚举项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Objects.EnumItem<T>> GetEnumItems<T>() where T : struct
        {
            Type t = typeof(T);
            String typeStr = t.ToString();
            //先从缓存中查找
            if (cacheBlock.ContainsKey(typeStr))
            {
                Object enumItems = null;
                if (cacheBlock.TryGetValue(typeStr, out enumItems))
                    return (List<Objects.EnumItem<T>>)enumItems;
            }
            String[] names = Enum.GetNames(t);
            Array values = Enum.GetValues(t);
            List<Objects.EnumItem<T>> items = new List<Objects.EnumItem<T>>();
            for (Int32 i = 0; i < names.Length; i++)
            {
                Objects.EnumItem<T> item = new Objects.EnumItem<T>();
                item.EnumValue = Enum.Parse(t, names[i]);
                item.IdentityValue = names[i];
                item.Description = MyType.GetFieldDescription(t, names[i]);
                item.UnderlyingType = Enum.GetUnderlyingType(t);
                item.UnderlyingValue = values.GetValue(i);
                items.Add(item);
            }
            //添加到缓存中
            cacheBlock.TryAdd(typeStr, items);
            return items;
        }
    }
}
