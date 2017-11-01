using System;
using System.Collections.Generic;
using System.Data;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyDataRow
    {
        /// <summary>
        /// 把datarow对象转换成dictionary
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> ConvertToDic(DataRow row)
        {
            if (row == null || row.Table == null) return null;
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            for (Int32 i = 0, j = row.Table.Columns.Count; i < j; i++)
                dic.Add(row.Table.Columns[i].ColumnName, row[i]);
            return dic;
        }
    }
}
