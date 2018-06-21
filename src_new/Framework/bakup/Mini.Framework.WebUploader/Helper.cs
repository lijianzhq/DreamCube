using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    class Helper
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
            String filePath = HttpContext.Current.Server.MapPath(AsmConfiger.AppSettings("File_SavePath"));
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

        //public static String GetFileName(HttpContext context)
        //{
        //    String filePath = context.Server.MapPath(AsmConfiger.AppSettings("File_SavePath"));
        //    //以客户端传过来的guid和fileid作为文件名
        //    filePath = Path.Combine(filePath, $"{context.Request["guid"]}_{context.Request["id"]}.{Path.GetExtension(context.Request.Files[0].FileName)}");
        //    String fileFullPath = String.Empty;
        //    if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
        //    {
        //        //当前分片在上传分片中的顺序（从0开始）
        //        Int32 chunk = Convert.ToInt32(context.Request.Form["chunk"]);
        //        //总分片数
        //        Int32 chunks = Convert.ToInt32(context.Request.Form["chunks"]);
        //        fileFullPath = Path.Combine(filePath + "_chunk", $"{ chunk}");
        //    }
        //    Directory.CreateDirectory(fileFullPath);
        //    return fileFullPath;
        //}
    }
}
