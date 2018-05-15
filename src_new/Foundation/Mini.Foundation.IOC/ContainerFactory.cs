using System;

namespace Mini.Foundation.IOC
{
    /// <summary>
    /// 容器工厂
    /// </summary>
    public static class ContainerFactory
    {
        private static IContainer _container = null;
        private static String[] _configFilePath = null;
        private static Object _locker = new Object();

        /// <summary>
        /// 注册配置文件路径（此方法必须在GetContainer()方法调用之前调用）
        /// </summary>
        /// <param name="filePaths">配置文件路径</param>
        public static void RegisterConfigFile(String[] filePaths)
        {
            _configFilePath = filePaths;
        }

        /// <summary>
        /// 获取容器
        /// </summary>
        /// <returns></returns>
        public static IContainer GetContainer()
        {
            if (_container == null)
            {
                lock (_locker)
                {
                    if (_container == null)
                    {
                        _container = new AutofacContainer(_configFilePath);
                    }
                }
            }
            return _container;
        }
    }
}
