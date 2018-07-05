using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Mini.Framework.Datagrid.DBService
{
    public class Datagrid : ModelCommonField
    {
        [Key]
        [Column("CODE")]
        public String CODE { get; set; }

        /// <summary>
        /// 配置字符串
        /// </summary>
        [Column("CONFIG")]
        public String Config { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        [JsonIgnore]
        [Column("SQL")]
        public String SQL { get; set; }

        /// <summary>
        /// 初始化完毕之后是否自动加载数据
        /// </summary>
        [Column("LOADDATAAFTERINITAL")]
        public Boolean LoadDataAfterInital { get; set; }

        public List<DatagridCol> Columns { get; set; }
    }
}
