using System;
using System.Web;
using System.IO;
using System.Linq;

using Mini.Foundation.LogService;
using Mini.Foundation.Json;
using Mini.Foundation.Basic.Utility;

namespace Mini.Framework.WebUploader
{
    class InWebFileWorker : IFileWorker
    {
        public virtual RespParams ProcessRequest(String savePath, HttpContext context)
        {
            //在此处写入您的处理程序实现。
            FileSaveRespParams result = null;
            String optype = context.Request["optype"];//操作类型
            if (optype == "save") //保存文件数据
            {
                result = DoSaveFile(savePath, context) as FileSaveRespParams;
            }
            else if (optype == "merge") //合并文件
            {
                result = MergeFile(savePath, context) as FileSaveRespParams;
            }
            else if (optype == "download")
            {
                WriteFileDataToClient(context);
            }
            //保存数据库记录
            if (result != null && result.Status && (!result.Chunked || optype == "merge"))
                result.FileCode = SaveDBRecord(context, result.FileSavePath);
            return result;
        }

        protected virtual void WriteFileDataToClient(HttpContext context)
        {
            var rqParams = new RequestParams(context);
            if (String.IsNullOrWhiteSpace(rqParams.FileCode)) return;

            String fileSaveFullPath = String.Empty;
            DBService.UploadFile fileObj = null;
            using (var db = new DBService.DB())
            {
                fileObj = db.UploadFiles.Where(it => it.CODE == rqParams.FileCode).SingleOrDefault();
                if (fileObj != null) fileSaveFullPath = fileObj.SavePath;
            }
            var worker = Helper.GetFileWorker(fileSaveFullPath) as InWebFileWorker;
            worker.DoWriteFileDataToClient(context, fileObj);
        }

        protected virtual void DoWriteFileDataToClient(HttpContext context, DBService.UploadFile fileObj)
        {
            using (var fs = GetFileStreamBySavePath(context, fileObj.SavePath))
            {
                const long ChunkSize = 1024 * 1024;//100K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
                byte[] buffer = new byte[ChunkSize];

                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileObj.FileName));
                Int32 read = 0;
                do
                {
                    read = fs.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    if (read > 0 && context.Response.IsClientConnected)
                    {
                        context.Response.OutputStream.Write(buffer, 0, read);
                        context.Response.Flush();
                    }
                } while (read > 0 && context.Response.IsClientConnected);
                context.Response.End();
            }
        }

        protected virtual Stream GetFileStreamBySavePath(HttpContext context, String fileSavePath)
        {
            return File.OpenRead(context.Server.MapPath(fileSavePath));
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual RespParams MergeFile(String savePath, HttpContext context)
        {
            //获得参数
            var rqParam = new RequestParams(context);
            var result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            String folderName = MyString.LastLeftOf(rqParam.fileSavePath.Replace("\\", "/"), "/");
            if (!String.IsNullOrEmpty(folderName) && Directory.Exists(folderName))
            {
                String fileFullName = $"{folderName}{Path.GetExtension(rqParam.fileName)}";
                //using (var file = File.Create(fileFullName))
                var files = Directory.GetFiles(folderName).OrderBy(it => Convert.ToInt32(Path.GetFileNameWithoutExtension(it)));
                //校验分片数是否一致
                if (files.Count() != rqParam.chunks) return result;
                using (var file = GetMergeFileStream(savePath, ref fileFullName, rqParam))
                {
                    foreach (String tempFile in files)
                    {
                        var bytes = File.ReadAllBytes(tempFile);
                        file.Write(bytes, 0, bytes.Length);
                        //System.Threading.Thread.Sleep(3000);
                    }
                    //file.Flush();
                }
                result.FileSavePath = fileFullName;
                Directory.Delete(folderName, true);
                result.Status = true;
            }
            else
            {
                String msg = $"merge file params:[{MyJson.Serialize(rqParam)}] faild, directory can not find!";
                Log.Root.LogInfo(msg);
                result.Message = msg;
            }
            return result;
        }

        /// <summary>
        /// 获取合并文件的流（新的文件流，以待写入文件数据）
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="fileFullName"></param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected virtual Stream GetMergeFileStream(String savePath, ref String fileFullName, RequestParams rqParam)
        {
            return File.Create(fileFullName);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual RespParams DoSaveFile(String savePath, HttpContext context)
        {
            var result = new FileSaveRespParams()
            {
                Status = false,
                Message = "",
                FileSavePath = "",
            };
            //获得参数
            var rqParam = new RequestParams(context);
            String fileFullName = GetFileName(savePath, rqParam);
            result.Chunked = rqParam.chunks > 0;
            //返回文件存放路径
            result.FileSavePath = fileFullName;
            context.Request.Files[0].SaveAs(fileFullName);
            result.Status = true;
            return result;
        }

        /// <summary>
        /// 根据参数，生成保存的文件名
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected virtual String GetFileName(String savePath, RequestParams rqParam)
        {
            Boolean isChunkFolder = !(rqParam.chunks <= 0);
            String filePath = FormatPath(savePath, rqParam);
            //以客户端传过来的guid和fileid作为文件名
            String fileFullName = Path.Combine(filePath, $"{rqParam.guid}_{rqParam.fileid}{Path.GetExtension(rqParam.fileName)}");
            if (isChunkFolder)
            {
                Directory.CreateDirectory(fileFullName);
                fileFullName = Path.Combine(fileFullName, $"{ rqParam.chunk}");
            }
            return fileFullName;
        }

        /// <summary>
        /// 负责解析保存路径模板的
        /// </summary>
        /// <param name="savePath">配置的保存路径模板</param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected virtual String FormatPath(String savePath, RequestParams rqParam)
        {
            var pathParts = savePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            String newPaths = String.Empty;
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) newPaths += "/";
                newPaths += FormatPathPartString(pathParts[i], rqParam);
            }
            String filePath = HttpContext.Current.Server.MapPath(newPaths);
            Directory.CreateDirectory(filePath);
            return filePath;
        }

        /// <summary>
        /// 格式化保存目录的每一部分字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rqParam"></param>
        /// <returns></returns>
        protected virtual String FormatPathPartString(String value, RequestParams rqParam)
        {
            if (String.IsNullOrWhiteSpace(value)) return value;
            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                value = value.Substring(1, value.Length - 2);
                value = DateTime.Now.ToString(value);
            }
            else if (value.StartsWith("[") && value.StartsWith("]"))
            {
                String key = value.Substring(1, value.Length - 2);
                if (rqParam.SavePathParams != null && rqParam.SavePathParams.ContainsKey(key))
                    return rqParam.SavePathParams[key];
            }
            return value;
        }

        /// <summary>
        /// 不同类型的存放方式，数据库存放的路径也不相同（派生类可以重写）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        protected virtual String FormatDBSaveFileFullPath(HttpContext context, String fileFullName)
        {
            return "~\\" + MyString.RightOf(fileFullName.Replace("/", "\\"), context.Server.MapPath("~").Replace("/", "\\"));
        }

        /// <summary>
        /// 保存数据库记录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        protected virtual String SaveDBRecord(HttpContext context, String fileFullName)
        {
            var rqParam = new RequestParams(context);
            var file = new DBService.UploadFile()
            {
                RefTableCode = rqParam.RefTableCode,
                RefTableName = rqParam.RefTableName,
                BarCode = rqParam.BarCode,
                SavePath = FormatDBSaveFileFullPath(context, fileFullName),
                CODE = DBService.DB.GetGuid(),
                FileName = rqParam.fileName,
                CreateBy = rqParam.userid,
                LastUpdateBy = rqParam.userid
            };
            DBService.DB.SaveUploadFileRecord(file);
            return file.CODE;
        }

    }
}
