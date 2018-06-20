using System;

namespace Mini.Framework.WebUploader
{
    /// <summary>
    /// 结果对象
    /// </summary>
    class ResponseParams
    {
        public String Message = "";

        public Boolean Status = false;

        public Boolean Chunked = false;

        public String FileSavePath = "";
    }
}
