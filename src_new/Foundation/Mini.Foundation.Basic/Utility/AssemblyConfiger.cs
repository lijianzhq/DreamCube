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
    public class AssemblyConfiger
    {
        public ConfigFileReader ConfigFileReader { get; protected set; } = null;

        public AssemblyConfiger()
        {
            //计算程序集的配置文件路径
            var callingAssembly = Assembly.GetCallingAssembly();
            Uri uri = new Uri(Path.GetDirectoryName(callingAssembly.CodeBase));
            var filePath = Path.Combine(uri.LocalPath, callingAssembly.GetName().Name + ".config");
            ConfigFileReader = new ConfigFileReader(filePath, callingAssembly);
        }

        internal AssemblyConfiger(Assembly callingAssembly)
        {
            //计算程序集的配置文件路径
            Uri uri = new Uri(Path.GetDirectoryName(callingAssembly.CodeBase));
            var filePath = Path.Combine(uri.LocalPath, callingAssembly.GetName().Name + ".config");
            ConfigFileReader = new ConfigFileReader(filePath, callingAssembly);
        }
    }
}
#endif
