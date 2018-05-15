using System;

namespace DreamCube.Foundation.IOC
{
    /// <summary>
    /// 容器接口
    /// </summary>
    public interface IContainer
    {
        T Resolve<T>();

        IContainer RegisterType<T>();
    }
}
