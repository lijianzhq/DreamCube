﻿using System;
using System.Web;
using System.IO;
using System.Linq;

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

        public static String GetFileName(String guid, String fileID, String fileName, Int32 chunks = 0, Int32 chunk = -1)
        {
            Boolean isChunkFolder = !(chunks <= 0);
            String filePath = GetConfigPath();
            Directory.CreateDirectory(filePath);
            //以客户端传过来的guid和fileid作为文件名
            String fileFullName = Path.Combine(filePath, $"{guid}_{fileID}{Path.GetExtension(fileName)}");
            if (isChunkFolder)
            {
                Directory.CreateDirectory(fileFullName);
                fileFullName = Path.Combine(fileFullName, $"{ chunk}");
            }
            return fileFullName;
        }

        /// <summary>
        /// 获取 配置的路径
        /// </summary>
        /// <returns></returns>
        static String GetConfigPath()
        {
            var path = AsmConfiger.AppSettings("File_SavePath");
            //对于~开始的就是相对网站目录路径的
            if (path.StartsWith("~"))
                return FormatWebsitePath(path);
            else if (path.StartsWith("\\"))//共享目录
                return FormatShareFolder(path);
            return String.Empty;
        }

        static String FormatShareFolder(String path)
        {
            path = path.Substring(2);
            var pathParts = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            String newPaths = "\\\\";
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) newPaths += "\\";
                newPaths += FormatDateString(pathParts[i]);
            }
            return newPaths;
        }

        static String FormatWebsitePath(String path)
        {
            var pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            String newPaths = String.Empty;
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) newPaths += "/";
                newPaths += FormatDateString(pathParts[i]);
            }
            return HttpContext.Current.Server.MapPath(newPaths);
        }

        static String FormatDateString(String value)
        {
            if (String.IsNullOrWhiteSpace(value)) return value;
            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                value = value.Substring(1, value.Length - 2);
                value = DateTime.Now.ToString(value);
            }
            return value;
        }
    }
}
