using System;
using System.Collections.Generic;
using System.Data;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Objects.Converters;
using DreamCube.Foundation.Basic.Objects.DefaultValue;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyConverter
    {
        #region "字段"

        private static Dictionary<ConverterKey, Basic.Objects.Converters.IConverter> converters = null;

        private static Object locker = new Object();

        #endregion

        #region "属性"

        /// <summary>
        /// 属性内部会自动创建
        /// </summary>
        private static Dictionary<ConverterKey, Basic.Objects.Converters.IConverter> Converters
        {
            get
            {
                if (converters != null) return converters;
                lock (locker)
                {
                    if (converters != null) return converters;
                    converters = new Dictionary<ConverterKey, Objects.Converters.IConverter>();
                    Type[] assemblyAllTypes = typeof(MyConverter).Assembly.GetTypes();
                    for (Int32 i = 0; i < assemblyAllTypes.Length; i++)
                    {
                        Type tempType = assemblyAllTypes[i];
                        if (tempType.IsAbstract) continue;
                        if (tempType.GetInterface("IConverter") != null)
                        {
                            Objects.Converters.IConverter tempConverter = MyType.CreateInstance<Objects.Converters.IConverter>(tempType);
                            if (tempConverter != null)
                            {
                                ConverterKey key = new ConverterKey(tempConverter.InputType, tempConverter.ResultType);
                                converters.Add(key, tempConverter);
                            }
                        }
                    }
                }
                return converters;
            }
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 转换数据类型
        /// 注意：方法只会从传入的空值映射表（nullDefaultValues）中获取空值时的默认值，不会自动获取系统的默认值；
        /// 所以，当传入的value为空值（DBNull，或者Null），而空值映射表（nullDefaultValues）无法获取到用户指定的默认值时，则会返回false
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <param name="outputObject">转换后的对象</param>
        /// <param name="nullDefaultValues">value为空时，返回的默认值</param>
        /// <param name="converters">转换方法表</param>
        /// <param name="he">处理异常的方式</param>
        /// <returns></returns>
        public static Boolean TryConvert(Object value,
                                         Type targetType,
                                         out Object outputObject,
                                         NullValuesMappers nullDefaultValues = null,
                                         ConvertersMapper converters = null,
                                         Enums.HandleExceptionInTry he = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            outputObject = null;
            //空值时的处理方法
            try
            {
                outputObject = MyConverter.Convert(value, targetType, nullDefaultValues, converters);
                return true;
            }
            catch (Exception ex)
            {
                switch (he)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw ex;
                }
            }
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <param name="nullReturnValue">当需要转换的值为null时，返回的默认值</param>
        /// <param name="dbNullReturnValue">当需要转换的值为DBNull时，返回的默认值</param>
        /// <param name="converters">类型转换器，可以自己指定不同的数据类型的转换方法</param>
        /// <returns></returns>
        public static Object Convert(Object value, Type targetType, Object nullReturnValue, Object dbNullReturnValue, ConvertersMapper converters = null)
        {
            if (value == null || value == System.DBNull.Value)
                return value == null ? nullReturnValue : dbNullReturnValue;
            else
            {
                //类型相同的话就不用再转换了
                if (value.GetType() == targetType) return value;
                if (String.IsNullOrEmpty(System.Convert.ToString(value))) return null;
                //如果有converters，则先常使用converters里面的转换映射表来转换，没有找到对应的映射表，再用默认的方式的来转换
                if (converters != null)
                {
                    IConverter converter = converters.GetConverter(value.GetType(), targetType);
                    if (converter != null) return converter.Convert(value);
                }
                //默认的转换方式
                if (targetType.IsValueType && targetType.IsGenericType)
                {
                    Object o = Convert(value, targetType.GetGenericArguments()[0]);
                    return MyType.CreateInstance(targetType, o);
                }
                if (targetType.IsEnum)
                {
                    return value is String
                               ? Enum.Parse(targetType, (String)value)
                               : Enum.ToObject(targetType, value);
                }
                if (targetType == typeof(Boolean))
                    return MyObject.ToBoolean(value);
                ////hero 2016-4-21
                //if((targetType.Name.Contains("Decimal") || targetType.Name.Contains("Int"))&& value.ToString()=="")
                //    return System.Convert.ChangeType(0, targetType);
                return System.Convert.ChangeType(value, targetType);
            }
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <param name="nullReturnValue">当需要转换的值为null时，返回的默认值</param>
        /// <param name="dbNullReturnValue">当需要转换的值为DBNull时，返回的默认值</param>
        /// <returns></returns>
        public static Object Convert(Object value, Type targetType, Object nullReturnValue = null, Object dbNullReturnValue = null)
        {
            return Convert(value, targetType, nullReturnValue, dbNullReturnValue, null);
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">需要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <param name="nullDefaultValues">
        /// 可以指定空值的时候，返回的默认值的映射表（可以针对不同的目标类型的数据，指定不同的空值默认值）
        /// 例如：当目标类型是：Dictionary类型时，value值为null，可以指定返回一个 new Dictionary()对象；
        /// 而当目标类型是：List 类型时，value值为null，可以指定返回null
        /// 虽然：Dictionary和List都是引用类型，但是他们对于需要转换的值都为null时，他们的返回值可以不一样
        /// （这个需求比较少遇到）
        /// </param>
        /// <param name="converters">类型转换器，可以自己指定不同的数据类型的转换方法</param>
        /// <returns></returns>
        public static Object Convert(Object value,
                                     Type targetType,
                                     NullValuesMappers nullDefaultValues,
                                     ConvertersMapper converters)
        {
            if (value == null || value == System.DBNull.Value)
            {
                if (nullDefaultValues != null)
                {
                    NullValues nullValues = nullDefaultValues.GetDefaultValue(targetType);
                    if (nullValues != null) return value == null ? nullValues.NullValue : nullValues.DBNullValue;
                }
                return null;
            }
            else
            {
                //类型相同的话就不用再转换了
                if (value.GetType() == targetType) return value;
                if (converters != null)
                {
                    IConverter converter = converters.GetConverter(value.GetType(), targetType);
                    if (converter != null) return converter.Convert(value);
                }
                //如果转换方法表中没有找到转换的方法，则调用默认的转换方法
                return Convert(value, targetType);
            }
        }

        /// <summary>
        /// 按顺序转换格式
        /// </summary>
        /// <param name="values">需要转换格式的对象</param>
        /// <param name="targetTypes">一个类型对应每一个需要转换格式的对象</param>
        /// <param name="outputs">转换过后的格式对象</param>
        /// <param name="makeLog">标志是否记录错误日志；true：记录日志；false：不记录日志；默认为true</param>
        public static Boolean TryConvertInOrder(Object[] values,
                                                Type[] targetTypes,
                                                out Object[] outputObjects,
                                                NullValuesMappers nullDefaultValues = null,
                                                ConvertersMapper converters = null,
                                                Enums.HandleExceptionInTry he = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            outputObjects = null;
            //空值时的处理方法
            try
            {
                outputObjects = MyConverter.ConvertInOrder(values, targetTypes, nullDefaultValues, converters);
                return true;
            }
            catch (Exception ex)
            {
                switch (he)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw ex;
                }
            }
        }

        /// <summary>
        /// 按顺序转换格式
        /// </summary>
        /// <param name="values">需要转换格式的对象</param>
        /// <param name="targetTypes">一个类型对应每一个需要转换格式的对象</param>
        /// <returns></returns>
        public static Object[] ConvertInOrder(Object[] values,
                                              Type[] targetTypes,
                                              NullValuesMappers nullDefaultValues = null,
                                              ConvertersMapper converters = null)
        {
            if (values == null || targetTypes == null) return null;
            if (values.Length != targetTypes.Length)
                throw new ArgumentException(String.Format(Properties.Resources.ExceptionParamsCountNotEqual, "values", "targetTypes"));
            for (Int32 i = 0; i < values.Length; i++)
            {
                Type type = targetTypes[i];
                //判断是否是枚举类型
                if (type.IsEnum)
                {
                    String enumValue = values[i].ToString();
                    values[i] = Enum.Parse(type, enumValue.Contains(".") ? MyString.RightOfLast(enumValue, ".") : enumValue);
                }
                else
                {
                    if (values[i] == null) values[i] = null;
                    else values[i] = MyConverter.Convert(values[i], type, nullDefaultValues, converters);
                }
            }
            return values;
        }


        /// <summary>
        /// 把datatable里面的数据转换成模型
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ConvertDatatableToModelList<T>(DataTable table)
            where T : new()
        {
            T model;
            if (table != null && table.Rows.Count > 0)
            {
                List<T> modelList = new List<T>();
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    model = new T();
                    MyObject.LoadPropertyValueFromDataRow(table.Rows[i], model);
                    modelList.Add(model);
                }
                return modelList;
            }
            return null;
        }

        #endregion
    }
}
