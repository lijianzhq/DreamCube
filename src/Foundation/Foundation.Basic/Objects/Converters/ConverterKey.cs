using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Objects.Converters
{
    class ConverterKey
    {
        /// <summary>
        /// 如果当获取到的数据格式与指定的数据对象的数据类型不一致，需要进行转换；
        /// 此类作为转换方法字段表的Key值，根据传入的参数类型以及所需要的目标类型，对应转换方法表中的一个方法
        /// </summary>
        #region "属性"

        /// <summary>
        /// 输入的行数据类型
        /// </summary>
        public Type InputType
        {
            get;
            set;
        }

        /// <summary>
        /// 格式化成目标的类型
        /// </summary>
        public Type ResultType
        {
            get;
            set;
        }

        #endregion

        #region "公共方法"

        public ConverterKey(Type inputType, Type resultType)
        {
            this.InputType = inputType;
            this.ResultType = resultType;
        }

        /// <summary>
        /// 重写了Object.GetHashCode()的方法
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// 重写了Object.ToString()的方法
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("{0}_{1}", this.InputType, this.ResultType);
        }

        /// <summary>
        /// 对象的输入输出类型一样时，认为两个key值是一样的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals(Object obj)
        {
            ConverterKey targetObj = obj as ConverterKey;
            if (targetObj == null) return false;
            return InputType == targetObj.InputType && ResultType == targetObj.ResultType;
        }

        #endregion
    }
}
