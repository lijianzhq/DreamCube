using System;
using System.ComponentModel;

namespace DreamCube.Foundation.Basic.Enums
{
    /// <summary>
    /// 在Try的方法体中处理异常的方式
    /// </summary>
    public enum HandleExceptionInTry
    {
        /// <summary>
        /// 继续返回值，并且记录日志
        /// </summary>
        [Description("Properties.Resources.Des_HEIT_ReturnAndMakeLog")]
        ReturnAndMakeLog,

        /// <summary>
        /// 继续返回值，并且不记录日志
        /// </summary>
        [Description("Properties.Resources.Des_HEIT_IgnoreAllException")]
        ReturnAndIgnoreLog,

        /// <summary>
        /// 向上抛出异常
        /// </summary>
        [Description("Properties.Resources.Des_HEIT_ThrowException")]
        ThrowException
    }
}
