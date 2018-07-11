using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Mini.Framework.Datagrid.DBService
{
    public class DatagridCol : ModelCommonField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Int32 ID { get; set; }

        /// <summary>
        /// 关联的gridcode值
        /// </summary>
        [Column("GRIDCODE")]
        public String GridCODE { get; set; }

        /// <summary>
        /// 列的配置，通常就是一个json串
        /// </summary>
        [Column("CONFIG")]
        public String Config { get; set; }

        /// <summary>
        /// 如果是编辑列，这个是用于获取编辑列数据的（例如下拉框的数据）
        /// </summary>
        [Column("EDITDATASQL")]
        public String EditDataSQL { get; set; }

        /// <summary>
        /// 是否导出该列
        /// </summary>
        [Column("EXPORT")]
        public Boolean Export { get; set; }

        /// <summary>
        /// 关联的grid对象
        /// </summary>
        [JsonIgnore]
        [ForeignKey("GridCODE")]
        public Datagrid Datagrid { get; set; }
    }
}
