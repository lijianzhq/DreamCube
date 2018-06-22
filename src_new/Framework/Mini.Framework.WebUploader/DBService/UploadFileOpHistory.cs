using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini.Framework.WebUploader.DBService
{
    /// <summary>
    /// 文件操作历史表（主要记录查看，下载）
    /// </summary>
    [Table("UPLOADFILEOPHISTORY")]
    public class UploadFileOpHistory : ModelCommonField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Int32 ID { get; set; }

        /// <summary>
        /// 文件编码
        /// </summary>
        [StringLength(50)]
        [Column("UPLOADFILECODE")]
        public String UploadFileCODE { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Column("OPTYPE")]
        public FileOpType OpType { get; set; }
    }
}
