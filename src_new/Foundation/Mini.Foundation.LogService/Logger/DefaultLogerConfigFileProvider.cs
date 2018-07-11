using System;
using System.IO;
#if (NET45 || NET40 || NET35 || NET20)
using System.Configuration;
#endif

using Mini.Foundation.TraceService;
using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.LogService.Logger
{
    public class DefaultLogerConfigFileProvider : ILogerConfigFileProvider
    {
        public String GetLogConfigFile()
        {
#if (NET45 ||NET40||NET35||NET20)
            try
            {

                //String cfgFilePath = ConfigurationManager.AppSettings["LogConfigFilePath"];
                String cfgFilePath = AsmConfigerHelper.GetConfiger().ConfigFileReader.AppSettings("LogConfigFilePath");
                cfgFilePath = String.IsNullOrEmpty(cfgFilePath) ? @"config\logconfig.xml" : cfgFilePath;
                String cfgFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFilePath);
                return cfgFileFullPath;
            }
            catch (Exception ex)
            {
                Tracer.Instance.TraceError("GetLogConfigFile() error:{0}", ex.Message);
            }
            return null;
#else
            throw new NotImplementedException();
#endif
        }
    }
}
