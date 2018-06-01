using System;

namespace DreamCube.Framework.Utilities.ValidateNO
{
    /// <summary>
    /// 线条噪音程度
    /// </summary>
    public enum LineNoiseLevel
    {
        /// <summary>
        /// 不添加线条噪音
        /// </summary>
        None,

        /// <summary>
        /// 少量线条噪音
        /// </summary>
        Low,

        /// <summary>
        /// 中等
        /// </summary>
        Medium,

        /// <summary>
        /// 高度噪音
        /// </summary>
        High,

        /// <summary>
        /// 极限程度噪音
        /// </summary>
        Extreme
    }
}
