using System;
using System.ComponentModel.DataAnnotations;

namespace Mini.Framework.WebUploader.DBService
{
    public class ModelCommonField
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100)]
        public String CreateBy { get; set; } = "";

        /// <summary>
        /// 最后更新人
        /// </summary>
        [StringLength(100)]
        public String LastUpdateBy { get; set; } = "";

        /// <summary>
        /// 排序值（越小越靠前）
        /// </summary>
        public Int32 Order { get; set; } = 0;
    }
}
