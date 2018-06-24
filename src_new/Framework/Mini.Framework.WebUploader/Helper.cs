﻿using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.LogService;

namespace Mini.Framework.WebUploader
{
    static class Helper
    {
        private static AssemblyConfiger _asmConfiger = null;
        public static AssemblyConfiger AsmConfiger
        {
            get
            {
                if (_asmConfiger == null)
                {
                    Mini.Foundation.Basic.DllExceptionEvent.ExceptionEvent += DllExceptionEvent_ExceptionEvent; ;
                    _asmConfiger = new AssemblyConfiger();
                }
                return _asmConfiger;
            }
        }

        private static void DllExceptionEvent_ExceptionEvent(Type arg1, Exception arg2)
        {
            Log.Root.LogError($"type[{arg1.FullName}]ex:", arg2);
        }

        private static Dictionary<String, IFileWorker> _workers =
            new Dictionary<String, IFileWorker>() {
                { "~",new InWebFileWorker()},
                { "\\\\",new ShareFolderFileWorker()},
                { "ftp",new FtpFileWorker()},
            };

        /// <summary>
        /// 根据存放路径不同，返回不同的worker对象
        /// </summary>
        /// <param name="savePath">存放目录</param>
        /// <returns></returns>
        public static IFileWorker GetFileWorker(String savePath)
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
