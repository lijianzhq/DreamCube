using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.Foundation.IOC
{
    public interface ILifetimeScope : IDisposable
    {
        ILifetimeScope BeginLifetimeScope();

        T Resolve<T>();
    }
}
