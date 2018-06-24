#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0)
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
    public class AssemblyConfiger
    {
        private String _filePath = String.Empty;
        private static FileSystemWatcher _watcher = null;

        public AssemblyConfiger()
        {
            //计算程序集的配置文件路径
            Assembly assembly = Assembly.GetCallingAssembly();
            Uri uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            _filePath = Path.Combine(uri.LocalPath, assembly.GetName().Name + ".config");
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
        /// <returns></returns>
        public String AppSettings(String key)
        {
            Configuration config = this.Config;
            KeyValueConfigurationElement configEl = config.AppSettings.Settings[key];
            return configEl == null ? "" : configEl.Value;
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
                if (!DllExceptionEvent.TryFireExceptionEvent(typeof(AssemblyConfiger), ex))
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
                if (!DllExceptionEvent.TryFireExceptionEvent(typeof(AssemblyConfiger), ex))
                    throw ex;
            }
        }

        #endregion
    }
}
#endif
