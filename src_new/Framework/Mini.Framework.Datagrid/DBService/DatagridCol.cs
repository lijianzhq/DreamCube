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

        [Column("GRIDCODE")]
        public String GridCODE { get; set; }

        [Column("CONFIG")]
        public String Config { get; set; }

        [Column("EDITDATASQL")]
        public String EditDataSQL { get; set; }

        [JsonIgnore]
        [ForeignKey("GridCODE")]
        public Datagrid Datagrid { get; set; }
    }
}
