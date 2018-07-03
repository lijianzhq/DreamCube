using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace LY.MQCS.Plugin.DBService.PQ
{
    public class EasyUIDataGrid
    {
        /// <summary>
        /// 配置字符串
        /// </summary>
        public String Config;

        /// <summary>
        /// SQL语句
        /// </summary>
        [JsonIgnore]
        public String SQL;

        /// <summary>
        /// 初始化完毕之后是否自动加载数据
        /// </summary>
        public Boolean LoadDataAfterInital { get; set; }

        public List<EasyUIDataGridColumn> Columns { get; set; }
    }
}
