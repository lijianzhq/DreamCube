using System;

namespace DreamCube.Framework.Utilities.ValidateNO
{
    /// <summary>
    /// 添加到背景图片的噪音程度
    /// </summary>
    public enum BackgroundNoiseLevel
    {
        /// <summary>
        /// 不添加背景噪音
        /// </summary>
        Node,

        /// <summary>
        /// 少量噪音
        /// </summary>
        Low,

        /// <summary>
        /// 中等程度
        /// </summary>
        Medium,

        /// <summary>
        /// 高级程度
        /// </summary>
        High,

        /// <summary>
        /// 最多
        /// </summary>
        Extreme
    }
}
