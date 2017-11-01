using System;

namespace DreamCube.Foundation.Basic.Enums
{
    /// <summary>
    /// 从集合中获取数据的操作选项
    /// </summary>
    public enum CollectionsGetOper
    {
        /// <summary>
        /// 如果找不到指定的Key值，则返回默认值（默认时是通过default计算符号获得的）
        /// </summary>
        DefaultValueIfNotExist,

        /// <summary>
        /// 如果找不到对应的Key值，则抛出异常
        /// </summary>
        ExceptionIfNotExist
    }
}
