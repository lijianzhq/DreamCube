using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini.Framework.WebUploader.DBService
{
    /// <summary>
    /// 文件记录
    /// </summary>
    [Table("UPLOADFILE")]
    public class UploadFile : ModelCommonField
    {
        /// <summary>
        /// 文件编码
        /// </summary>
        [Key]
        [StringLength(50)]
        [Column("CODE")]
        public String CODE { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [StringLength(500)]
        [Column("FILENAME")]
        public String FileName { get; set; } = String.Empty;

        /// <summary>
        /// 关联表名
        /// </summary>
        [StringLength(50)]
        [Column("REFTABLENAME")]
        public String RefTableName { get; set; } = String.Empty;

        /// <summary>
        /// 关联表的CODE值
        /// </summary>
        [StringLength(50)]
        [Column("REFTABLECODE")]
        public String RefTableCode { get; set; } = String.Empty;

        /// <summary>
        /// 对应的bar标示符
        /// </summary>
        [StringLength(50)]
        [Column("BARCODE")]
        public String BarCode { get; set; } = String.Empty;

        /// <summary>
        /// 存放路径
        /// </summary>
        [StringLength(500)]
        [Column("SAVEPATH")]
        public String SavePath { get; set; } = String.Empty;

        /// <summary>
        /// 所有日志
        /// </summary>
        public virtual List<UploadFileOpHistory> OpHistory { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [Column("STATUS")]
        public FileStatus Status { get; set; }

        /// <summary>
        /// 附加属性
        /// </summary>
        [StringLength(2000)]
        [Column("ATTRIBUTE1")]
        public String Attribute1 { get; set; } = "";

        /// <summary>
        /// 附加属性
        /// </summary>
        [StringLength(2000)]
        [Column("ATTRIBUTE2")]
        public String Attribute2 { get; set; } = "";

        /// <summary>
        /// 附加属性
        /// </summary>
        [StringLength(2000)]
        [Column("ATTRIBUTE3")]
        public String Attribute3 { get; set; } = "";

        /// <summary>
        /// 附加属性
        /// </summary>
        [StringLength(2000)]
        [Column("ATTRIBUTE4")]
        public String Attribute4 { get; set; } = "";

        /// <summary>
        /// 附加属性
        /// </summary>
        [StringLength(2000)]
        [Column("ATTRIBUTE5")]
        public String Attribute5 { get; set; } = "";
    }
}
