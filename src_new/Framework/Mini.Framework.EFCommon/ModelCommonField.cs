using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini.Framework.EFCommon_LY
{
    public class ModelCommonField
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("ISENABLE")]
        public Boolean IsEnable { get; set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEON")]
        public DateTime CreateOn { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Column("LASTUPDATEON")]
        public DateTime LastUpdateOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100)]
        [Column("CREATEBY")]
        public String CreateBy { get; set; } = "";

        /// <summary>
        /// 最后更新人
        /// </summary>
        [StringLength(100)]
        [Column("LASTUPDATEBY")]
        public String LastUpdateBy { get; set; } = "";

        /// <summary>
        /// 排序值（越小越靠前）
        /// </summary>
        [Column("ORDERNO")]
        public Int32 OrderNO { get; set; } = 0;
    }
}
