using System;

namespace DreamCube.Framework.Utilities.ValidateNO
{
    /// <summary>
    /// 验证码实体类
    /// </summary>
    public class ValidateStringInfo
    {
        /// <summary>
        /// 获取或设置保存在session中的键值
        /// </summary>
        public String Key { set; get; }

        /// <summary>
        /// 获取或设置验证码的值
        /// </summary>
        public String Code { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        public Boolean IsTimeOut { get; set; }
    }
}
