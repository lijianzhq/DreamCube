using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mini.Foundation.LogService.Logger
{
    public interface ILogerConfigFileProvider
    {
        String GetLogConfigFile();
    }
}
