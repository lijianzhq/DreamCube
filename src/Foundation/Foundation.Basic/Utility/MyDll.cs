using System;
using System.IO;
using System.Configuration;
using System.Reflection;

//自定义命名空间
using DreamCube.Foundation.Basic.Cache;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyDll
    {
        /// <summary>
        /// 缓存区
        /// </summary>
        private static DictionaryCachePool<String, Configuration> cacheBlock = new DictionaryCachePool<String, Configuration>();

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
            Configuration tempConfig = null;
            if (cacheBlock.TryGetValue(dllConfigFilePath, out tempConfig))
                return tempConfig;
            else
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = dllConfigFilePath;
                tempConfig = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
                cacheBlock.TryAdd(dllConfigFilePath, tempConfig);
                return tempConfig;
            }
        }
    }
}
