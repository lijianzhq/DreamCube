using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini.Framework.WebUploader.DBService
{
    /// <summary>
    /// 文件操作历史表（主要记录查看，下载）
    /// </summary>
    public class UploadFileOpHistory : ModelCommonField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 ID { get; set; }

        /// <summary>
        /// 文件编码
        /// </summary>
        [StringLength(50)]
        public String UploadFileCODE { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public FileOpType OpType { get; set; }
    }
}
