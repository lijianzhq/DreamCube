using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

#if NET40
using System.Linq;
#endif

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyDictionary
    {
        /// <summary>
        /// 尝试添加项到字典中
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="item">需要添加到字典的项</param>
        /// <param name="type">添加操作的类型</param>
#if NET20
        public static Boolean TryAdd<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                  KeyValuePair<TKey, TValue> item,
                                                  CollectionsAddOper type = CollectionsAddOper.NotSet)
#else
        public static Boolean TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> target,
                                                  KeyValuePair<TKey, TValue> item,
                                                  CollectionsAddOper type = CollectionsAddOper.NotSet)
#endif
        {
            return TryAdd<TKey, TValue>(target, item.Key, item.Value, type);
        }

        /// <summary>
        /// 尝试添加项到字典中；(此方法一样是不支持线程安全的)
        /// 添加成功返回true，添加失败返回false 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="item">需要添加到字典的项</param>
        /// <param name="type">添加操作的类型</param>
#if NET20
        public static Boolean TryAdd<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                   TKey key, TValue value,
                                                   CollectionsAddOper type = CollectionsAddOper.NotSet)
#else
        public static Boolean TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> target,
                                                   TKey key, TValue value,
                                                   CollectionsAddOper type = CollectionsAddOper.NotSet)
#endif
        {
            try
            {
                if (type == CollectionsAddOper.NotSet)
                {
                    target.Add(key, value);
                    return true;
                }
                if (target.ContainsKey(key))
                {
                    switch (type)
                    {
                        case CollectionsAddOper.IgnoreIfExist:
                            return false;
                        case CollectionsAddOper.ExceptionIfExist:
                            throw new ArgumentException(
                                    String.Format(Properties.Resources.ExceptionKeyExist, key));
                        case CollectionsAddOper.ReplaceIfExist:
                            target[key] = value;
                            return true;
                        default:
                            throw new ArgumentException(
                                        String.Format(Properties.Resources.ExceptionArgumentEnumError
                                                        , "type"
                                                        , type.ToString()));
                    }
                }
                else
                {
                    target.Add(key, value);
                    return true;
                }
            }
            catch (ArgumentException)
            {
                //多线程环境的时候，会发生重复添加key值的情况，所以直接忽略此情况
                return true;
            }
        }

        /// <summary>
        /// 把一个字典的项添加到目标字典中
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="data"></param>
        /// <param name="type">插入字典的操作类型：如果存在则忽略；如果存在则替换；如果存在则抛出异常</param>
        /// <returns></returns>
#if NET20
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                                     IDictionary<TKey, TValue> data,
                                                                     CollectionsAddOper type = CollectionsAddOper.ReplaceIfExist)
#else
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> target, 
                                                                     IDictionary<TKey, TValue> data, 
                                                                     CollectionsAddOper type = CollectionsAddOper.ReplaceIfExist)
#endif
        {
            ///程序遍历的时候选择项最少的集合来遍历
            if (target.Count > data.Count)
            {
                foreach (KeyValuePair<TKey, TValue> item in data)
                    TryAdd<TKey, TValue>(target, item, type);
                return target;
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> item in target)
                    TryAdd<TKey, TValue>(data, item, type);
                return data;
            }
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="TKey">Key值的类型</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                        TKey key,
                                                        out TValue value,
                                                        CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                        TValue defaultValue = default(TValue))
#else
        public static Boolean TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> target,
                                                        TKey key,
                                                        out TValue value,
                                                        CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                        TValue defaultValue = default(TValue))
#endif
        {
            value = defaultValue;
            if (target.ContainsKey(key)) ///判断是否还有指定的Key值
            {
                value = target[key];
                return true;
            }
            //如果找不到指定的Key值，应该执行哪种操作
            if (type == CollectionsGetOper.ExceptionIfNotExist)
                throw new ArgumentException(String.Format(Properties.Resources.ExceptionKeyNotFind, key));
            return false;
        }

        /// <summary>
        /// 根据序号获取值，如果小于0或者超出了项数，则返回NULL
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Boolean TryGetValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                        Int32 index,
                                                        out TValue value,
                                                        CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                        TValue defaultValue = default(TValue))
        {
            value = defaultValue;
            if (index < 0 || target.Count <= index) return false;
            Int32 i = 0;
            foreach (KeyValuePair<TKey, TValue> item in target)
            {
                if (index == i)
                {
                    value = item.Value;
                    return true;
                }
                i++;
            }
            //如果找不到指定的Key值，应该执行哪种操作
            if (type == CollectionsGetOper.ExceptionIfNotExist)
                throw new ArgumentException(String.Format(Properties.Resources.ExceptionIndexInvalid, index));
            return false;
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="TKey">Key值的类型</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
#if NET20
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                     TKey key,
                                                     CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                     TValue defaultValue = default(TValue))
#else
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> target,
                                                     TKey key,
                                                     CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                     TValue defaultValue = default(TValue))
#endif
        {
            return GetValue(target, key, type, defaultValue, null);
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="TKey">Key值的类型</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <param name="keyComparer">键值比较委托（通常不建议使用此方法，此参数可以在创建Dictionary对象的时候指定，效率会更高）</param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                     TKey key,
                                                     CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                     TValue defaultValue = default(TValue),
                                                     Func<TKey,TKey,Boolean > keyComparer = null)
        {
            if (keyComparer == null)
            {
                //判断是否还有指定的Key值
                if (target.ContainsKey(key)) return target[key];
                //如果找不到指定的Key值，应该执行哪种操作
                if (type == CollectionsGetOper.ExceptionIfNotExist)
                    throw new ArgumentException(String.Format(Properties.Resources.ExceptionKeyNotFind, key));
                return defaultValue;
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> item in target)
                    if (keyComparer(item.Key, key)) return item.Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 根据序号获取值，如果小于0或者超出了项数，则返回NULL
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                        Int32 index,
                                                        CollectionsGetOper type = CollectionsGetOper.DefaultValueIfNotExist,
                                                        TValue defaultValue = default(TValue))
        {
            if (index >= 0 && target.Count > index)
            {
                Int32 i = 0;
                foreach (KeyValuePair<TKey, TValue> item in target)
                {
                    if (index == i) return item.Value;
                    i++;
                }
            }
            //如果找不到指定的Key值，应该执行哪种操作
            if (type == CollectionsGetOper.ExceptionIfNotExist)
                throw new ArgumentException(String.Format(Properties.Resources.ExceptionIndexInvalid, index));
            return defaultValue;
        }

        /// <summary>
        /// 判断是否包含值(由于.Net不提供此接口，所有底层只能采用遍历的模式去实现，如果基于.NET4.0，对于大数据量效率会大大提高，因为.NET4.0采用并行遍历)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="key">找到对应值的key值</param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
#if NET20
        public static Boolean ContainsValue<TKey, TValue>(IDictionary<TKey, TValue> target,
                                                          TValue value,
                                                          out TKey key,
                                                          Func<TValue, TValue, Boolean> valueComparer = null)
        {
            key = default(TKey);
            if (target == null || target.Count == 0) return false;
            Boolean hasItem = false;
            if (valueComparer == null)
            {
                foreach (KeyValuePair<TKey, TValue> item in target)
                {
                    if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
                    {
                        key = item.Key;
                        hasItem = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> item in target)
                {
                    if (valueComparer(item.Value, value))
                    {
                        key = item.Key;
                        hasItem = true;
                        break;
                    }
                }
            }
            return hasItem;
        }
#else
        public static Boolean ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> target,
                                                          TValue value,
                                                          out TKey key,
                                                          Func<TValue, TValue, Boolean> valueComparer = null)

        {
            //并行处理寻找相匹配的项
            Boolean hasItem = false;
            key = default(TKey);
            ParallelQuery query = target.AsParallel();
            foreach (KeyValuePair<TKey, TValue> item in query)
            {
                if (valueComparer != null)
                {
                    if (valueComparer(item.Value, value))
                    {
                        key = item.Key;
                        hasItem = true;
                        break;
                    }
                }
                else
                {
                    if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
                    {
                        key = item.Key;
                        hasItem = true;
                        break;
                    }
                }
            }
            return hasItem;
        }
#endif

        /// <summary>
        /// 判断是否包含值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
#if NET20
        public static Boolean ContainsValue<TKey, TValue>(IDictionary<TKey, TValue> target, TValue value, Func<TValue, TValue, Boolean> valueComparer = null)
        {
            if (target == null || target.Count == 0) return false;
            Dictionary<TKey, TValue> tempDic = target as Dictionary<TKey, TValue>;
            if (valueComparer == null)
            {
                if (tempDic != null) return tempDic.ContainsValue(value);
            }
            else
            {
                TKey key;
                ContainsValue(target, value, out key, valueComparer);
            }
            return false;
        }
#else
        public static Boolean ContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> target, 
                                                          TValue value,
                                                          Func<TValue, TValue, Boolean> valueComparer = null)
        {
            if (valueComparer == null)
            {
                Dictionary<TKey, TValue> tempDic = target as Dictionary<TKey, TValue>;
                if (tempDic != null) return tempDic.ContainsValue(value);
            }
            TKey key;
            return ContainsValue<TKey, TValue>(target, value, out key, valueComparer);
        }
#endif

        /// <summary>
        /// 把Dictionary里面的记录串成字符串
        /// 如果不符合要求，也可以调用IEnumerableExtension类的JoinEx接口
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="targetDictionary"></param>
        /// <param name="sKeyValueDiv">键、值之间的分隔符，如果不传，默认则为“:”</param>
        /// <param name="sItemDiv">记录项之间的分隔符，如果不传，默认则为英文分号“;”</param>
        /// <returns></returns>
#if NET20
        public static String JoinEx<TKey, TValue>(Dictionary<TKey, TValue> targetDictionary, String keyValueDiv = ":", String itemDiv = ";")
#else
        public static String JoinEx<TKey, TValue>(this Dictionary<TKey, TValue> targetDictionary, String keyValueDiv = ":", String itemDiv = ";")
#endif
        {
            StringBuilder builder = new StringBuilder();
            Int32 i = 0;
            foreach (KeyValuePair<TKey, TValue> oItem in targetDictionary)
            {
                if (i > 0) builder.Append(itemDiv);
                builder.Append(oItem.Key + keyValueDiv + oItem.Value);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 返回一个泛型Dictionary的副本；没有线程安全
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="oTargetDictionary"></param>
        /// <returns></returns>
#if NET20
        public static Dictionary<TKey, TValue> Copy<TKey, TValue>(Dictionary<TKey, TValue> targetDictionary)
#else
        public static Dictionary<TKey, TValue> Copy<TKey, TValue>(this Dictionary<TKey, TValue> targetDictionary)
#endif
        {
            Dictionary<TKey, TValue> oNewDictionary = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> oItem in targetDictionary)
                TryAdd<TKey, TValue>(oNewDictionary, oItem.Key, oItem.Value);
            return oNewDictionary;
        }
    }
}
