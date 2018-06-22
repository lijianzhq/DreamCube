using System;
using System.Collections.Generic;

namespace Mini.Framework.WebUploader
{
    /// <summary>
    /// 结果对象
    /// </summary>
    class FileSaveRespParams: RespParams
    {
        public Boolean Chunked = false;
        public String FileSavePath = "";
        public String FileCode = "";
    }
}
