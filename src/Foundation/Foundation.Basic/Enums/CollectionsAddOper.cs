using System;

namespace DreamCube.Foundation.Basic.Enums
{
    /// <summary>
    /// 插入数据到集合中的操作选项
    /// </summary>
    public enum CollectionsAddOper
    {
        /// <summary>
        /// 不指定任何操作（默认就会直接调用集合的Add方法进行添加）
        /// </summary>
        NotSet,

        /// <summary>
        /// 如果存在了Key值，则忽略插入操作
        /// </summary>
        IgnoreIfExist,

        /// <summary>
        /// 如果存在了对应的Key值，则更新值
        /// </summary>
        ReplaceIfExist,

        /// <summary>
        /// 如果存在了对应的Key值，则抛出异常
        /// </summary>
        ExceptionIfExist
    }
}
