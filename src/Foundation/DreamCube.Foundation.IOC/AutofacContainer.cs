using System;

using Autofac;

namespace DreamCube.Foundation.IOC
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

        public AutofacContainer()
        {
            _builder = new ContainerBuilder();
        }

        public AutofacContainer(String[] configFilePaths)
        {
            _builder = new ContainerBuilder();
            //_builder.RegisterModule(new )
        }

        public T Resolve<T>()
        {
            return InnerContainer.Resolve<T>();
        }

        IContainer IContainer.RegisterType<T>()
        {
            throw new NotImplementedException();
        }

        T IContainer.Resolve<T>()
        {
            throw new NotImplementedException();
        }
    }
}
