using System;
using System.Collections.Generic;

namespace Mini.Framework.WebUploader
{
    /// <summary>
    /// 结果对象
    /// </summary>
    class RespParams
    {
        /// <summary>
        /// 返回给客户端的信息
        /// </summary>
        public String Message = "";

        /// <summary>
        /// true结果成功；false结果失败
        /// </summary>
        public Boolean Status = false;
    }
}
