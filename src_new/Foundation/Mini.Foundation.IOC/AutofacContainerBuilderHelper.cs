using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Xml;

using Autofac;
using Autofac.Configuration;

namespace Mini.Foundation.IOC
{
    public static class AutofacContainerBuilderHelper
    {
        public static ContainerBuilder CreateContainerBuilder(IConfiguration configuration)
        {
            ArgumentsHelper.ArgumentNotNull(configuration, nameof(configuration));
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationModule(configuration));
            return builder;
        }

        /// <summary>
        /// 根据配置文件路径创建containerbuilder对象，支持.json和.xml两种后缀名的配置文件
        /// </summary>
        /// <param name="configFilePaths">路径可以是相对程序运行的路径；也可以是绝对路径</param>
        /// <returns></returns>
        public static ContainerBuilder CreateContainerBuilder(string[] configFilePaths)
        {
            return CreateContainerBuilder(LoadConfiguration(configFilePaths));
        }

        public static ContainerBuilder ConfigureContainerWithJsonFile(string configFile)
        {
            return CreateContainerBuilder(LoadConfiguration(new String[] { configFile }));
        }

        public static ContainerBuilder ConfigureContainerWithXmlFile(string configFile)
        {
            return CreateContainerBuilder(LoadConfiguration(new String[] { configFile }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static IConfiguration LoadConfiguration(String[] configFile)
        {
            ArgumentsHelper.ArgumentNotNull(configFile, nameof(configFile));
            var providerList = new List<IConfigurationProvider>();
            for (var i = 0; i < configFile.Length; i++)
            {
                var provider = GetProvider(configFile[i]);
                if (provider != null)
                    providerList.Add(provider);
            }
            if (providerList.Count > 0)
                return new ConfigurationRoot(providerList);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static IConfigurationProvider GetProvider(String filePath)
        {
            ArgumentsHelper.ArgumentNotNullOrEmpty(filePath, nameof(filePath));
            //if (String.IsNullOrEmpty(filePath)) return null;
            Int32 index = filePath.LastIndexOf(".");
            if (index == -1) return null;
            using (var stream = File.OpenRead(filePath))
            {
                string fileExtension = filePath.Substring(index).ToLower();
                switch (fileExtension)
                {
                    case ".xml":
                        return new ConfigurationProvider<XmlConfigurationSource>(stream);
                    case ".json":
                        return new ConfigurationProvider<JsonConfigurationSource>(stream);
                }
            }
            return null;
        }
    }
}
