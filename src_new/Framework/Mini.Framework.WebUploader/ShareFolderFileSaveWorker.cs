using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    class ShareFolderFileSaveWorker : InWebFileSaveWorker
    {
        /// <summary>
        /// 获取 配置的路径
        /// </summary>
        /// <param name="savePath">当前分片</param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected override string FormatPath(string savePath, RequestParams rqParam)
        {
            String path = savePath.Substring(2);
            var pathParts = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            String newPaths = "\\\\";
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) newPaths += "\\";
                newPaths += FormatPathPartString(pathParts[i], rqParam);
            }
            Directory.CreateDirectory(newPaths);
            return newPaths;
        }
    }
}
