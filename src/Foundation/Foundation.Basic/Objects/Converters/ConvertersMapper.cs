using System;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Objects.Converters
{
    /// <summary>
    /// 转换映射表
    /// </summary>
    public class ConvertersMapper
    {
        /// <summary>
        /// 转换映射表
        /// </summary>
        private Dictionary<String, IConverter> converterCache = new Dictionary<String, IConverter>();

        /// <summary>
        /// 像映射表添加一个类型转换器
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="outputType">输出类型</param>
        /// <param name="converter">转换器</param>
        /// <param name="addOper">添加操作；默认的操作是：存在同类型的转换器时，则不添加</param>
        public void AddConverter(Type inputType, Type outputType, IConverter converter, Enums.CollectionsAddOper addOper = Enums.CollectionsAddOper.IgnoreIfExist)
        {
            ConverterKey key = new ConverterKey(inputType, outputType);
            MyDictionary.TryAdd(converterCache, key.ToString(), converter, addOper);
        }

        /// <summary>
        /// 根据输入输出类型获取一个转换器
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public IConverter GetConverter(Type inputType, Type outputType)
        {
            ConverterKey key = new ConverterKey(inputType, outputType);
            IConverter convert = null;
            MyDictionary.TryGetValue(converterCache, key.ToString(), out convert, Enums.CollectionsGetOper.DefaultValueIfNotExist, null);
            return convert;
        }
    }
}
