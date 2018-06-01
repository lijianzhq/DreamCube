using System;
using System.Text;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.CSV
{
    /// <summary>
    /// CSVFile文件的帮助类
    /// </summary>
    public static class CSVFileHelper
    {
        /// <summary>
        /// 根据文件路径和列名，创建一个新的CSV文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static CSVFile CreateNewCSVFile(String filePath, List<String> columns)
        {
            if (String.IsNullOrEmpty(filePath) || columns == null || columns.Count == 0)
                throw new ArgumentNullException();
            filePath = filePath.Replace("/", "\\");
            String folderPath = MyString.LeftOfLast(filePath, "\\");
            //确保目录存在
            MyIO.EnsurePath(folderPath);
            StringBuilder builder = new StringBuilder();
            for (Int32 i = 0, j = columns.Count; i < j; i++)
            {
                if (i == 0) builder.Append(columns[i]);
                else builder.Append("," + columns[i]);
            }
            MyIO.Write(filePath, builder.ToString(), true);
            return new CSVFile(filePath, columns);
        }

        /// <summary>
        /// 根据CSV文件加载CSV文件对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static CSVFile LoadCSVFile(String filePath)
        {
            throw new NotImplementedException();
        }
    }
}
