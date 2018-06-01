using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DreamCube.Framework.Utilities.CSV
{
    public class CSVFile
    {
        #region "字段"

        /// <summary>
        /// 此CSV文件对应的所有列名（按顺序）
        /// </summary>
        private List<String> columnNames;

        /// <summary>
        /// 对应的文件路径
        /// </summary>
        private String filePath;

        #endregion

        #region "构造方法"

        internal CSVFile(String filePath, List<String> columnNames)
        {
            this.filePath = filePath;
            this.columnNames = columnNames;
        }

        /// <summary>
        /// 根据csv文件创建一个csv对象（要确保传入的文件路径存在，否则创建对象失败）
        /// </summary>
        /// <param name="filePath"></param>
        internal CSVFile(String filePath)
        {
            this.filePath = filePath;
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 把datatable的数据添加到文件中（datatable的列名顺序必须一一和此对象的列名顺序一致）;采用UTF8格式
        /// </summary>
        /// <param name="data"></param>
        public void AppendData(DataTable data)
        {
            AppendData(data, Encoding.UTF8);
        }

        /// <summary>
        /// 把datatable的数据添加到文件中（datatable的列名顺序必须一一和此对象的列名顺序一致）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding">编码格式</param>
        public void AppendData(DataTable data, Encoding encoding)
        {
            if (!IsDataValidate(data))
                throw new ArgumentException("传入的datatable列名与csv列名不一致，无法把数据添加到csv中");
            if (encoding == null) encoding = Encoding.UTF8;
            using (StreamWriter sw = new StreamWriter(
                        new FileStream(this.filePath, FileMode.Append, FileAccess.Write), encoding))
            {
                for (Int32 i = 0, j = data.Rows.Count; i < j; i++)
                {
                    StringBuilder builder = new StringBuilder();
                    for (Int32 k = 0, l = data.Columns.Count; k < l; k++)
                    {
                        if (i == 0) builder.Append(data.Rows[i][k]);
                        else builder.Append("," + data.Rows[i][k]);
                    }
                    sw.WriteLine(builder.ToString());
                }
            }
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 验证数据是否合法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Boolean IsDataValidate(DataTable data)
        {
            if (data == null || data.Columns.Count != this.columnNames.Count) return false;
            for (Int32 i = 0, j = data.Columns.Count; i < j; i++)
            {
                if (data.Columns[i].ColumnName != this.columnNames[i])
                    return false;
            }
            return true;
        }

        #endregion
    }
}
