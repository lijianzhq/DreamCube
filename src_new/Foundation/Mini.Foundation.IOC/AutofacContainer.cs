using System;

using Autofac;

namespace Mini.Foundation.IOC
{
    public class AutofacContainer : IContainer
    {
        private ContainerBuilder _builder = null;
        private Autofac.IContainer _innerContainer = null;

        protected Autofac.IContainer InnerContainer
        {
            get
            {
                if (_innerContainer == null)
                    _innerContainer = _builder.Build(Autofac.Builder.ContainerBuildOptions.None);
                return _innerContainer;
            }
        }

        public AutofacContainer() : this(null)
        {
        }

        public AutofacContainer(String[] configFilePaths)
        {
            if (configFilePaths != null)
                _builder = AutofacContainerBuilderHelper.CreateContainerBuilder(configFilePaths);
            else
                _builder = new ContainerBuilder();
        }

        public T Resolve<T>()
        {

            using (var scope = _innerContainer.BeginLifetimeScope())
            {
                // Resolve services from a scope that is a child
                // of the root container.
                var service = scope.Resolve<IService>();

                // You can also create nested scopes...
                using (var unitOfWorkScope = scope.BeginLifetimeScope())
                {
                    var anotherService = unitOfWorkScope.Resolve<IOther>();
                }
            }

            return InnerContainer.Resolve<T>();
        }
    }
}
