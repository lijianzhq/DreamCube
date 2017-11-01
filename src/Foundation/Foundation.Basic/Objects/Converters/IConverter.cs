using System;

namespace DreamCube.Foundation.Basic.Objects.Converters
{
    public interface IConverter
    {
        /// <summary>
        /// 输出类型
        /// </summary>
        Type ResultType
        {
            get;
        }

        /// <summary>
        /// 输入类型
        /// </summary>
        Type InputType
        {
            get;
        }

        /// <summary>
        /// 数据转换接口
        /// </summary>
        /// <param name="inputValue">输入需要转换的值</param>
        /// <param name="resultType">转换的结果类型</param>
        /// <returns></returns>
        Object Convert(Object inputValue);
    }
}
