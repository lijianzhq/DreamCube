using System;

#if !(NETSTANDARD1_0 || NETSTANDARD1_3)
using System.Data;
#endif

//第三方的命名空间
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Mini.Foundation.Json.Newton
{

#if !(NETSTANDARD1_0 || NETSTANDARD1_3)

    public class DBNullToDefaultValueDataTableConverter : DataTableConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataTable table = (DataTable)value;
            DefaultContractResolver resolver = serializer.ContractResolver as DefaultContractResolver;

            writer.WriteStartArray();

            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (serializer.NullValueHandling == NullValueHandling.Ignore && (row[column] == null || row[column] == DBNull.Value))
                        continue;

                    writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
                    Object colVal = row[column];
                    if (colVal == null || Convert.IsDBNull(colVal))
                    {
                        colVal = this.GetColDefaultVal(column);
                    }
                    serializer.Serialize(writer, colVal);
                }
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        /// <summary>
        /// 根据框架的设定需求，序列化datatable的时候，字符串列值为dbnull的时候，返回""，而不要返回null。
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private Object GetColDefaultVal(DataColumn col)
        {
            if (col.DataType == typeof(String))
            {
                return "";
            }
            return null;
        }
    }

#endif

}
