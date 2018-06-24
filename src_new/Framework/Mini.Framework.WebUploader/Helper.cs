using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    static class Helper
    {
        private static AssemblyConfiger _asmConfiger = null;
        public static AssemblyConfiger AsmConfiger
        {
            get
            {
                if (_asmConfiger == null) _asmConfiger = new AssemblyConfiger();
                return _asmConfiger;
            }
        }

        private static Dictionary<String, IFileSaveWorker> _workers =
            new Dictionary<String, IFileSaveWorker>() {
                { "~",new InWebFileSaveWorker()},
                { "\\\\",new ShareFolderFileSaveWorker()},
                { "ftp",new FtpFileSaveWorker()},
            };

        /// <summary>
        /// 根据存放路径不同，返回不同的worker对象
        /// </summary>
        /// <param name="savePath">存放目录</param>
        /// <returns></returns>
        public static IFileSaveWorker GetFileSaveWorker(String savePath)
        {
            String protocol = String.Empty;//协议
            if (savePath.StartsWith("ftp", StringComparison.InvariantCultureIgnoreCase))
            {
                protocol = "ftp";
            }
            else if (savePath.StartsWith("~"))
            {
                protocol = "~";
            }
            else if (savePath.StartsWith("\\\\"))
            {
                protocol = "\\\\";
            }
            if (String.IsNullOrEmpty(protocol)) return null;
            if (_workers.ContainsKey(protocol)) return _workers[protocol];
            return null;
        }
    }
}
