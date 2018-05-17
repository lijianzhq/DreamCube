using System;

namespace Mini.Foundation.IOC
{
    /// <summary>
    /// 容器接口
    /// </summary>
    public interface IContainer : ILifetimeScope
    {
        /// <summary>
        /// 返回内部封装的container对象（应用程序通过此对象可以直接调用到内部的container对象，实现特殊需求）
        /// </summary>
        Object Container { get; }
    }
}
