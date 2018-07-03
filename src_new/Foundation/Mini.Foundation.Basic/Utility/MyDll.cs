#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;
using System.IO;
using System.Reflection;
using System.Configuration;

namespace Mini.Foundation.Basic.Utility
{
    public static class MyDll
    {
        /// <summary>
        /// 读取调用此方法的dll所在的路径
        /// 返回的路径最后不带斜杠符号的
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentDllPath()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Uri uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            return uri.LocalPath;
        }

        /// <summary>
        /// 获取当前程序集（调用此方法的dll）的配置文件路径；
        /// 配置文件的文件名格式为：程序集名.dll.config；
        /// 此方法效率过低
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentDllConfigFilePath()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Uri uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            return Path.Combine(uri.LocalPath, assembly.GetName().Name + ".config");
        }

        ///<summary> 
        ///读取dll的配置信息的方法；
        ///参数dllConfigFilePath可以直接通过此类的GetCurrentDllConfigFilePath()方法获取;
        ///此方法内部采用了缓存技术
        ///</summary> 
        ///<returns></returns> 
        public static Configuration GetDllConfiguration(String dllConfigFilePath)
        {
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = dllConfigFilePath;
            return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
        }
    }
}
#endif
