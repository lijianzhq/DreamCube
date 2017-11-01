using System;
using System.ComponentModel;

namespace DreamCube.Foundation.Basic.Enums
{
    /// <summary>
    /// 在循环中处理异常的方式
    /// </summary>
    public enum HandleExceptionInLoop
    {
        /// <summary>
        /// 忽略异常，直到循环结束，循环结束后，统一抛出异常（不推荐使用，暂时还没有很好的方式实现此模式）
        /// </summary>
        [Description("Properties.Resources.Des_HEIL_ContinueAndThrowUntilLoopEnd")]
        ContinueAndThrowUntilLoopEnd,

        /// <summary>
        /// 继续返回，并且记录日志
        /// </summary>
        [Description("Properties.Resources.Des_HEIL_ContinueReturnAndMylog")]
        ContinueReturnAndMylog,

        /// <summary>
        /// 忽略所有异常（不记录日志），方法照常返回结果
        /// </summary>
        [Description("Properties.Resources.Des_HEIL_ContinueReturnAndIgnoreAllException")]
        ContinueReturnAndIgnoreAllException,

        /// <summary>
        /// 停止循环，马上抛出异常
        /// </summary>
        [Description("Properties.Resources.Des_HEIL_ThrowExceptionRightNow")]
        ThrowExceptionRightNow
    }
}
