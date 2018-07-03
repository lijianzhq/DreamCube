﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.MQCS.Plugin.DBService.PQ
{
    [Table("EASYUIDATAGRIDCOLUMN")]
    public class EasyUIDataGridColumn : ModelCommonField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Int32 ID { get; set; }

        [StringLength(2000)]
        [Column("GRIDCODE")]
        public String GridCODE { get; set; }

        [StringLength(2000)]
        [Column("CONFIG")]
        public String Config { get; set; }

        [StringLength(2000)]
        [Column("EDITDATASQL")]
        public String EditDataSQL { get; set; }
    }
}
