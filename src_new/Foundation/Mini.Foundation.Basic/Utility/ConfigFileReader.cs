#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 程序集配置对象
    /// （调用方一定要缓存这个对象实例，对象实例内部会监听配置文件的变化，配置文件发生变化时，会重新 读取配置文件的内容）
    /// </summary>
    public class ConfigFileReader
    {
        private String _filePath = String.Empty;
        private static FileSystemWatcher _watcher = null;
        private Assembly _callingAssembly;

        public ConfigFileReader(String filePath, Assembly callingAssembly = null)
        {
            if (callingAssembly == null)
            {
                //计算程序集的配置文件路径
                callingAssembly = Assembly.GetCallingAssembly();
            }
            _callingAssembly = callingAssembly;
            _filePath = filePath;
            //监听配置文件 
            WatchConfigFile();
            //执行一次初始化动作 
            LoadConfig();
        }

        private Configuration _configer = null;
        public virtual Configuration Config => _configer;

        /// <summary>
        /// 读取appsettings节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tryReadFromAppConfig">
        /// 是否先从app.config里面尝试读取（假如读取不到的话，再从程序集配置文件中读取）
        /// 注意：
        /// 从app.config里面读取的key格式为：assemblyname.key，例如：Mini.Foundation.Basic.xxxx
        /// </param>
        /// <returns></returns>
        public String AppSettings(String key, Boolean tryReadFromAppConfigFirst = true)
        {
            String value = String.Empty;
            if (tryReadFromAppConfigFirst)
            {
                var newKey = $"{_callingAssembly.GetName().Name}.{key}";
                value = ConfigurationManager.AppSettings[newKey];
                if (!String.IsNullOrEmpty(value)) return value;
            }
            Configuration config = this.Config;
            KeyValueConfigurationElement configEl = config.AppSettings.Settings[key];
            return configEl == null ? "" : configEl.Value;
        }

        public ConnectionStringSettings ConnectionString(String key)
        {
            Configuration config = this.Config;
            return config.ConnectionStrings?.ConnectionStrings[key];
        }

        #region "protected method"

        protected virtual void WatchConfigFile()
        {
            try
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = MyString.LastLeftOf(_filePath.Replace("\\", "/"), "/");
                _watcher.Changed += ConfigFile_Changed;
                _watcher.IncludeSubdirectories = true;
                _watcher.EnableRaisingEvents = true;
                _watcher.Filter = MyString.LastRightOf(_filePath.Replace("\\", "/"), "/");
            }
            catch (Exception ex)
            {
                if (!DllExceptionEvent.TryFireExceptionEvent(_callingAssembly, typeof(AssemblyConfiger), ex))
                    throw ex;
            }
        }

        protected virtual void ConfigFile_Changed(object sender, FileSystemEventArgs e) => this.LoadConfig();

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void LoadConfig()
        {
            try
            {
                _configer = MyDll.GetDllConfiguration(_filePath);
            }
            catch (Exception ex)
            {
                if (!DllExceptionEvent.TryFireExceptionEvent(_callingAssembly, typeof(AssemblyConfiger), ex))
                    throw ex;
            }
        }

        #endregion
    }
}
#endif
