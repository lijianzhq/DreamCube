using System;
using System.IO;
using System.Configuration;

using DreamCube.Foundation.TraceService;

namespace DreamCube.Foundation.LogService.Logger
{
    public class DefaultLogerConfigFileProvider : ILogerConfigFileProvider
    {
        public String GetLogConfigFile()
        {
            try
            {
                String cfgFilePath = ConfigurationManager.AppSettings["LogConfigFilePath"];
                cfgFilePath = String.IsNullOrEmpty(cfgFilePath) ? @"config\logconfig.xml" : cfgFilePath;
                String cfgFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFilePath);
                return cfgFileFullPath;
            }
            catch (Exception ex)
            {
                Tracer.Instance.TraceError("GetLogConfigFile() error:{0}", ex.Message);
            }
            return null;
        }
    }
}
