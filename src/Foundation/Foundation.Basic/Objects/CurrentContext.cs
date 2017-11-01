using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 当前上下文对象（支持windows和web）
    /// </summary>
    public static class CurrentContext
    {
        #region "字段"

        /// <summary>
        /// 静态字段在每一个线程中都是唯一的
        /// </summary>
        [ThreadStatic]
        private static Dictionary<String,Object> innerCache = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        private static Dictionary<String, Object> InnerCache
        {
            get
            {
                if (innerCache == null)
                {
                    innerCache = new Dictionary<String, Object>();
                }
                return innerCache;
            }
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 尝试在当前上下文中缓存对象；
        /// 如果指定的键值存在，则替换对应的值，否则添加值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Boolean TryCacheItem(String name, Object item)
        {
            return Utility.MyDictionary.TryAdd(InnerCache, name, item, Enums.CollectionsAddOper.ReplaceIfExist);
        }

        /// <summary>
        /// 从缓冲区中获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Object GetCacheItem(String name)
        {
            Object value;
            Utility.MyDictionary.TryGetValue(InnerCache, name, out value, Enums.CollectionsGetOper.DefaultValueIfNotExist, null);
            return value;
        }

        /// <summary>
        /// 从缓冲区中获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetCacheItem<T>(String name)
        {
            Object value;
            Utility.MyDictionary.TryGetValue<String, Object>(InnerCache, name, out value, Enums.CollectionsGetOper.DefaultValueIfNotExist, null);
            return (T)value;
        }

        #endregion
    }
}
