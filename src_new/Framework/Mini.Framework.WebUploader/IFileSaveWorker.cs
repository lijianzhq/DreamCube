using System;
using System.Web;

namespace Mini.Framework.WebUploader
{
    interface IFileSaveWorker
    {
        RespParams SaveFile(String savePath, HttpContext context);
    }
}
