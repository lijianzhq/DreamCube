using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Cache.Interface;
using DreamCube.Foundation.Basic.Objects.Converters;
using DreamCube.Foundation.Basic.Objects.DefaultValue;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyObject
    {
        #region "字段"

        private static IDictionaryCachePool<String, MemberInfo[]> reflectionCache = new DictionaryCachePool<String, MemberInfo[]>();

        #endregion

        #region "公共静态方法;反射相关的方法"

        /// <summary>
        /// 设置对象的成员的值(属性或者字段)
        /// </summary>
        /// <param name="targetObj">设置的目标对象</param>
        /// <param name="memberName">指定对象的成员名称</param>
        /// <param name="value">成员的对象值</param>
        /// <param name="index">针对索引属性的 索引值</param>
#if NET20
        public static void SetMemberValue(Object targetObj, String memberName, Object value, Object[] index)
#else
        public static void SetMemberValue(this Object targetObj, String memberName, Object value, Object[] index)
#endif
        {
            if (targetObj == null) return;
            Type targetType = targetObj.GetType();
            String targetTypeString = MyObject.ToStringEx(targetType);
            //先获取所有的字段和属性成员，并缓存起来
            MemberInfo[] propertiesFields = null;
            if (!reflectionCache.TryGetValue(targetTypeString, out propertiesFields))
            {
                propertiesFields = targetType.GetMembers(BindingFlags.SetProperty | BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public);
                reflectionCache.TryAdd(targetTypeString, propertiesFields, Enums.CollectionsAddOper.ReplaceIfExist);
            }
            if (propertiesFields == null || propertiesFields.Length <= 0) return;
            for (Int32 i = 0; i < propertiesFields.Length; i++)
            {
                if (propertiesFields[i].Name == memberName)
                {
                    PropertyInfo propertyInfo = propertiesFields[i] as PropertyInfo;
                    if (propertyInfo != null && propertyInfo.PropertyType == value.GetType())
                    {
                        propertyInfo.SetValue(targetObj, value, null);
                        break;
                    }
                    else
                    {
                        FieldInfo fieldInfo = propertiesFields[i] as FieldInfo;
                        if (fieldInfo != null && fieldInfo.FieldType == value.GetType())
                        {
                            fieldInfo.SetValue(targetObj, value);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 把一个对象的值复制到另外一个对象上（根据同名属性（字段）进行复制）
        /// </summary>
        /// <param name="valueObj">值对象</param>
        /// <param name="targetObj">目标对象</param>
#if NET20
        public static void CopyValueTo(Object valueObj, Object targetObj)
#else
        public static void CopyValueTo(this Object valueObj, Object targetObj)
#endif
        {
            if (valueObj == null || targetObj == null) return;
            FieldInfo[] fields = MyType.GetFieldsEx(valueObj);
            PropertyInfo[] properties = MyType.GetPropertiesEx(valueObj);

            //先设置字段值
            if (fields != null && fields.Length > 0)
            {
                for (Int32 i = 0; i < fields.Length; i++)
                {
                    MyObject.SetMemberValue(targetObj, fields[i].Name, fields[i].GetValue(valueObj), null);
                }
            }
            if (properties != null && properties.Length > 0)
            {
                for (Int32 i = 0; i < properties.Length; i++)
                {
                    MyObject.SetMemberValue(targetObj, properties[i].Name, properties[i].GetValue(valueObj, null), null);
                }
            }
        }

        /// <summary>
        /// 获取一个对象的克隆体（深度复制对象）
        /// 注意：对象必须是可序列化的
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static T DeepClone<T>(T target)
#else
        public static T DeepClone<T>(this T target)
#endif
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //序列化格式器
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                //把对象序列化到流中
                formatter.Serialize(ms, target);
                //反序列化时，先定位流到起始位置
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 获取指定列的值，如果指定的列名存在，则返回true；如果指定的列名不存在，则返回false
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="sourceRow"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetColumnValue(DataRow sourceRow, String columnName, out Object columnValue)
#else
        public static Boolean TryGetColumnValue(this DataRow sourceRow, String columnName, out Object columnValue)
#endif
        {
            columnValue = null;
            if (sourceRow.Table.Columns.Contains(columnName))
            {
                columnValue = sourceRow[columnName];
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载xml节点的属性数据到对象属性上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="targetObj"></param>
#if NET20
        public static T LoadPropertyValueFromXmlNode<T>(XmlNode node, T targetObj)
#else
        public static T LoadPropertyValueFromXmlNode<T>(this XmlNode node, T targetObj)
#endif
        {
            if (node == null) return targetObj;
            PropertyInfo[] properties = MyType.GetPropertiesEx(targetObj);
            if (properties != null)
            {
                Type stringType = typeof(String);
                for (Int32 i = 0; i < properties.Length; i++)
                {
                    String attrValue = String.Empty;
                    if (properties[i].CanWrite && MyXml.TryGetNodeAttributeValue(node, properties[i].Name, out attrValue, true, MyXml.XmlValueType.InnerText))
                    {
                        if (!String.IsNullOrEmpty(attrValue))
                        {
                            Type propertyType = properties[i].PropertyType;
                            if (stringType == propertyType) properties[i].SetValue(targetObj, attrValue, null);
                            else
                            {
                                Object resultValue = Utility.MyConverter.Convert(attrValue, propertyType);
                                if (resultValue != null)
                                    properties[i].SetValue(targetObj, resultValue, null);
                            }
                        }
                    }
                }
            }
            return targetObj;
        }

        /// <summary>
        /// 加载数据行的所有数据到指定对象的属性，设置对应列名的属性值
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="targetObj"></param>
        /// <param name="columnNamePrefix">指定的列名前缀，从datarow寻找列名为： 列名前缀+属性名  的值作为对象对应的属性值</param>
        public static void LoadPropertyValueFromDataRow(DataRow sourceRow, Object targetObj, String columnNamePrefix = "")
        {
            LoadPropertyValueFromDataRow(sourceRow, targetObj, null, null, columnNamePrefix);
        }

        /// <summary>
        /// 加载数据行的所有数据到指定对象的属性，设置对应列名的属性值
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="targetObj"></param>
        /// <param name="nullDefaultValues"></param>
        /// <param name="converters"></param>
        /// <param name="columnNamePrefix">指定的列名前缀，从datarow寻找列名为： 列名前缀+属性名  的值作为对象对应的属性值</param>
        public static Object LoadPropertyValueFromDataRow(DataRow sourceRow,
                                                          Object targetObj,
                                                          NullValuesMappers nullDefaultValues,
                                                          ConvertersMapper converters,
                                                          String columnNamePrefix = "")
        {
            if (sourceRow == null) return targetObj;
            PropertyInfo[] properties = MyType.GetPropertiesEx(targetObj);
            if (properties != null)
            {
                Object columnValue = null;
                for (Int32 i = 0; i < properties.Length; i++)
                {
                    if (properties[i].CanWrite)
                    {
                        String dbColumnName = columnNamePrefix + properties[i].Name;
                        //如果打了标签，则会根据标签的值去获取
                        Object[] attributes = MyType.GetPropertyAttributes(targetObj, properties[i].Name, typeof(Objects.Attributes.DataColumnAttribute), true);
                        if (attributes != null && attributes.Length > 0)
                        {
                            Objects.Attributes.DataColumnAttribute attribute = attributes[0] as Objects.Attributes.DataColumnAttribute;
                            if (attribute != null) dbColumnName = attribute.DBColumnName;
                        }
                        if (MyObject.TryGetColumnValue(sourceRow, dbColumnName, out columnValue))
                        {
                            Type propertyType = properties[i].PropertyType;
                            Object resultValue = null;
                            if (Utility.MyConverter.TryConvert(columnValue, propertyType, out resultValue, nullDefaultValues, converters))
                                properties[i].SetValue(targetObj, resultValue, null);
                        }
                    }
                }
            }
            return targetObj;
        }

        /// <summary>
        /// 加载数据行的所有数据到指定对象的属性，设置对应列名的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceRow">数据行对象</param>
        /// <param name="targetObj">目标对象</param>
        /// <param name="nullDefaultValues">value为空时，返回的默认值（当数据行的数据位DBNull时，设置对应的属性的默认值）</param>
        /// <param name="converters">类型转换方法表（当数据行对象的数据类型与同名的对象属性的类型不一致时，通过此参数进行类型转换）</param>
        public static T LoadPropertyValueFromDataRow<T>(DataRow sourceRow, T targetObj, NullValuesMappers nullDefaultValues = null, ConvertersMapper converters = null)
        {
            return (T)LoadPropertyValueFromDataRow(sourceRow, targetObj, nullDefaultValues, converters, "");
        }

        /// <summary>
        /// 加载数据行的所有数据到指定对象的字段，设置对应列名的字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceRow"></param>
        /// <param name="targetObj"></param>
        /// <param name="defaultValue">指定的默认值映射表</param>
        public static T LoadFieldValueFromDataRow<T>(DataRow sourceRow, T targetObj, NullValuesMappers nullDefaultValues = null, ConvertersMapper converters = null)
        {
            if (sourceRow == null) return targetObj;
            FieldInfo[] fields = MyType.GetFieldsEx(targetObj);
            if (fields != null)
            {
                Object columnValue = null;
                for (Int32 i = 0; i < fields.Length; i++)
                {
                    if (MyObject.TryGetColumnValue(sourceRow, fields[i].Name, out columnValue))
                    {
                        Type propertyType = fields[i].FieldType;
                        Object resultValue = null;
                        if (Utility.MyConverter.TryConvert(columnValue, propertyType, out resultValue, nullDefaultValues, converters))
                            fields[i].SetValue(targetObj, resultValue);
                    }
                }
            }
            return targetObj;
        }

        #endregion

        #region "数据格式转换的相关方法"

        /// <summary>
        /// 把对象转换成字符串，如果对象为空，则返回String.Empty，不会抛出异常
        /// </summary>
        /// <param name="target"></param>
        /// <param name="defaultStrWhenNULLOrDBNull">当对象为NULL或者为DBNull.Value时，返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String ToStringEx(Object target, String defaultStrWhenNULLOrDBNull = "")
#else
        public static String ToStringEx(this Object target, String defaultStrWhenNULLOrDBNull = "")
#endif
        {
            if (target == null || target == System.DBNull.Value) return defaultStrWhenNULLOrDBNull;
            return target.ToString();
        }

        /// <summary>
        /// 把对象转换成boolean型
        /// 如果传入的对象为NULL,FALSE,0都是返回false；
        /// 否则返回true；
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(Object target)
        {
            if (target == null) return false;
            String val = target.ToString();
            return (String.Compare(val, "true", true) == 0) || String.Compare(val, "1") == 0 || String.Compare(val, "on", true) == 0;
        }

        /// <summary>
        /// 把对象转换成整型
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">当枚举值为转换失败的时候仍然返回，则在转换失败的时候会返回此值</param>
        /// <returns></returns>
#if NET20
        public static Int32 ToInt32(Object target, Int32 returnValueWhenParseError = default(Int32))
#else
        public static Int32 ToInt32(this Object target, Int32 returnValueWhenParseError = default(Int32))
#endif
        {
            Int32 result = returnValueWhenParseError;
            try
            {
                result = Convert.ToInt32(target);
                return result;
            }
            catch (Exception)
            {
                return returnValueWhenParseError;
            }
        }

        /// <summary>
        /// 把对象转换成整型(null-->0 返回true)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="returnValueWhenParseError">当枚举值为转换失败的时候仍然返回，则在转换失败的时候会返回此值</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryToInt32(Object target, out Int32 result, Int32 returnValueWhenParseError = default(Int32))
#else
            public static Boolean TryToInt32(this Object target,
                                         out Int32 result,
                                         Int32 returnValueWhenParseError = default(Int32))
#endif
        {
            result = returnValueWhenParseError;
            try
            {
                result = Convert.ToInt32(target);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 把对象转换成整型(null-->0 返回true)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="result"></param>
        /// <param name="returnValueWhenParseError">当枚举值为转换失败的时候仍然返回，则在转换失败的时候会返回此值</param>
        /// <returns></returns>
        public static Boolean TryToDouble(Object target, out Double result, Double returnValueWhenParseError = default(Double))
        {
            result = returnValueWhenParseError;
            try
            {
                result = Convert.ToDouble(target);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 把对象转换成目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="convertFailReturnValue">转换失败的时候返回的默认值</param>
        /// <returns></returns>
        public static T TryTo<T>(Object target, T convertFailReturnValue = default(T))
        {
            try
            {
                return To<T>(target);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
                return convertFailReturnValue;
            }
        }

        /// <summary>
        /// 把数据转换成目标的类型
        /// </summary>
        /// <param name="target">数据。（支持枚举项转换成枚举的基础值，例如枚举项转换为整型）</param>
        /// <param name="resultType">目标类型</param>
        /// <param name="nullDefaultValues">null的时候，返回的默认值</param>
        /// <param name="dbNullReturnValue">dbnull的时候，返回的默认值</param>
        /// <returns></returns>
        public static Object To(Object target, Type resultType, Object nullDefaultValues = null, Object dbNullReturnValue = null)
        {
            //底层已经写了更加强大的方法
            return MyConverter.Convert(target, resultType, nullDefaultValues, dbNullReturnValue);
            //枚举值
            //if (resultType.IsEnum)
            //{
            //    //Type underlyingType = Enum.GetUnderlyingType(resultType);
            //    ////如果传入的值与枚举项对应的基础值的类型是一致的，则支持直接转换
            //    //if (target.GetType() == underlyingType)
            //    //{
            //    //    List<Objects.EnumItem> enumItems = MyEnum.GetEnumItems(resultType);
            //    //    for (Int32 i = 0, j = enumItems.Count; i < j; i++)
            //    //    {
            //    //        //if (enumItems[i].UnderlyingValue.Equals(target) || enumItems[i].UnderlyingValue == target)
            //    //        //    return enumItems[i].EnumValue;
            //    //    }
            //    //}
            //    //如果上面的预测转换失败，则尝试下面这个转换
            //    //枚举项字符串转换为对应的枚举值
            //    String itemStr = Convert.ToString(target);
            //    return Enum.Parse(resultType, itemStr, true);
            //}
            //else
            //{
            //    if (resultType == typeof(UInt16))
            //    {
            //        return Convert.ToUInt16(target);
            //    }
            //    else if (resultType == typeof(UInt32))
            //    {
            //        return Convert.ToUInt32(target);
            //    }
            //    else if (resultType == typeof(UInt64))
            //    {
            //        return Convert.ToUInt64(target);
            //    }
            //    else if (resultType == typeof(Int16))
            //    {
            //        return Convert.ToInt16(target);
            //    }
            //    else if (resultType == typeof(Int32))
            //    {
            //        return Convert.ToInt32(target);
            //    }
            //    else if (resultType == typeof(Int64))
            //    {
            //        return Convert.ToInt64(target);
            //    }
            //    else if (resultType == typeof(Single))
            //    {
            //        return Convert.ToSingle(target);
            //    }
            //    else if (resultType == typeof(String))
            //    {
            //        return Convert.ToString(target);
            //    }
            //    else if (resultType == typeof(Boolean))
            //    {
            //        return Convert.ToBoolean(target);
            //    }
            //    else if (resultType == typeof(Byte))
            //    {
            //        return (Object)Convert.ToByte(target);
            //    }
            //    else if (resultType == typeof(Char))
            //    {
            //        return Convert.ToChar(target);
            //    }
            //    else if (resultType == typeof(DateTime))
            //    {
            //        return Convert.ToDateTime(target);
            //    }
            //    else if (resultType == typeof(Decimal))
            //    {
            //        return Convert.ToDecimal(target);
            //    }
            //    else if (resultType == typeof(Double))
            //    {
            //        return Convert.ToDouble(target);
            //    }
            //    else if (resultType == typeof(SByte))
            //    {
            //        return Convert.ToSByte(target);
            //    }
            //    else
            //    {
            //        return target;
            //    }
            //}
        }

        /// <summary>
        /// 把对象转换成目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T To<T>(Object target)
        {
            Type resultType = typeof(T);
            return (T)To(target, resultType);
        }

        #endregion

        #region "数据验证的相关方法"

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsNumber(Object value)
        {
            if (value == null) return false;
            try
            {
                Double result = 0.0;
                return Double.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsDouble(Object value)
        {
            if (value == null) return false;
            try
            {
                Double result = 0.0;
                return Double.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否为整型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsInt(Object value)
        {
            if (value == null) return false;
            if (value.GetType() == typeof(Int32)) return true;
            //还可以通过TryParse去校验
            return Regex.IsMatch(Convert.ToString(value), @"^[+-]?\d*$");
        }

        #endregion
    }
}
