using System;

namespace Mini.Foundation.Basic.CommonObj
{
    public class ExecResult
    {   
        /// <summary>
        /// 结果，true为操作成功；false为操作失败
        /// </summary>
        public Boolean OpResult { get; set; }

        /// <summary>
        /// 返回给客户端的消息（通常可以是异常消息）
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public Object OpData { get; set; }
    }
}
