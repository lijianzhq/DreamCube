using System;
using System.Web;

namespace Mini.Framework.WebUploader
{
    interface IFileWorker
    {
        RespParams ProcessRequest(String savePath, HttpContext context);
    }
}
