using System;

namespace DreamCube.Foundation.Basic.Objects.Converters
{
    public abstract class BasicConverter<TInputType, TResultType> : IConverter
    {
        #region "属性"

        /// <summary>
        /// 输出类型
        /// </summary>
        public Type ResultType
        {
            get { return typeof(TResultType); }
        }

        /// <summary>
        /// 输入类型
        /// </summary>
        public Type InputType
        {
            get { return typeof(TInputType); }
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 数据转换接口
        /// </summary>
        /// <param name="inputValue">输入需要转换的值</param>
        /// <param name="resultType">转换的结果类型</param>
        /// <returns></returns>
        public abstract TResultType Convert(Object inputValue);

        #endregion

        #region IConverter 成员

        /// <summary>
        /// 显式实现接口，以解决重命名的情况
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        Object IConverter.Convert(Object inputValue)
        {
            return this.Convert(inputValue);
        }

        #endregion
    }
}
