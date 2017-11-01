using System;
using System.Data;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyDataTable
    {
        /// <summary>
        /// 把数据表转换成对象列表(自动根据数据表的列名，填充到对应的对象属性名中)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ConvertToModelList<T>(DataTable table) where T : new()
        {
            List<T> modelList = new List<T>();
            if (table == null || table.Rows.Count == 0) return null;
            for (Int32 i = 0, j = table.Rows.Count; i < j; i++)
            {
                T model = new T();
                MyObject.LoadPropertyValueFromDataRow(table.Rows[i], model);
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// 根据列名获取指定的列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static DataColumn GetColumnByName(DataTable table, String columnName, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (String.IsNullOrEmpty(columnName)) return null;
            for (Int32 i = 0, j = table.Columns.Count; i < j; i++)
            {
                if (String.Compare(table.Columns[i].ColumnName, columnName, stringComparison) == 0)
                    return table.Columns[i];
            }
            return null;
        }

        /// <summary>
        /// 根据列名获取指定的列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="column">名字的列</param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static Boolean TryGetColumnByName(DataTable table, String columnName, out DataColumn column, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            column = GetColumnByName(table, columnName, stringComparison);
            return column != null;
        }

        /// <summary>
        /// 获取列名以某个标记开始的所有列；例如一个表有列：A_1,A_2,C_1,C_2；获取以A_开始的所有列，有两个：A_1和A_2
        /// </summary>
        /// <param name="table">要查询的目标数据表</param>
        /// <param name="columnNameStartPart">要查询列名的开始字符串</param>
        /// <param name="stringComparison">匹配列名的方式</param>
        /// <returns></returns>
#if NET20
        public static DataColumn[] GetColumnsStartWithName(DataTable table, String columnNameStartPart, StringComparison stringComparison = StringComparison.CurrentCulture)
#else
        public static DataColumn[] GetColumnsStartWithName(this DataTable table, String columnNameStartPart,StringComparison stringComparison = StringComparison.CurrentCulture)
#endif
        {
            if(table == null && table.Columns!=null)return null;
            List<DataColumn> columns = new List<DataColumn>();
            for (Int32 i = 0, j = table.Columns.Count; i < j; i++)
            {
                if (table.Columns[i].ColumnName.StartsWith(columnNameStartPart, stringComparison))
                    columns.Add(table.Columns[i]);
            }
            return columns.ToArray();
        }

        /// <summary>
        /// 获取datatable对象指定某部分的数据行
        /// </summary>
        /// <param name="data">原始的datatable对象</param>
        /// <param name="startRowIndex">起始行序号</param>
        /// <param name="length">获取数据的长度</param>
        /// <returns></returns>
#if NET20
        public static DataTable GetPart(DataTable data, Int32 startRowIndex, Int32 length)
#else
        public static DataTable GetPart(this DataTable data, Int32 startRowIndex, Int32 length)
#endif
        {
            if (data == null) return null;
            Int32 datalength = data.Rows.Count;
            if (datalength <= startRowIndex)
            {
                data.Rows.Clear();
                return data;
            }
            //移除前面的记录
            for (Int32 i = 0; i < startRowIndex; i++)
                data.Rows.RemoveAt(i);
            //更新数据长度值
            datalength = data.Rows.Count;
            //移除后面的记录
            for (Int32 i = datalength - 1, j = startRowIndex + length; i >= j; i--)
                data.Rows.RemoveAt(i);
            return data;
        }
    }
}
