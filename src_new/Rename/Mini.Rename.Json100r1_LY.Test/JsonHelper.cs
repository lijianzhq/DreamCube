using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Mini.Rename.Json100r1.Test
{
    /// <summary>
    /// 不依赖 任何第三方JSON框架的 Json 辅助类
    /// </summary>
    public static class JsonHelper
    {
        #region  公 共 函 数

        /// <summary>
        /// 将指定对象转换成 JSON
        /// </summary>
        public static string JsonSerialize(object data)
        {
            return JsonSerialize(data, false);
        }
        /// <summary>
        /// 将指定对象转换成 JSON
        /// </summary>
        public static string JsonSerialize(object data, bool throwEeception)
        {
            if (data == null) return "null";

            try
            {
                JsonObject obj = ObjectToJsonObject(data);
                return obj == null ? "null" : obj.ToJson();
            }
            catch (Exception exp)
            {
                if (throwEeception) throw;
                else
                {
                    string logMsg = "JsonHelper.JsonSerialize(object data) 序列化错误:" + exp;
                    Tools.LogError(logMsg, "Logs/Tools/ErrorLog/");
                    return null;
                }
            }
        }



        /// <summary>
        /// 将JSON 转换成 指定类型对象 
        /// </summary>
        public static T JsonDeserialize<T>(string json)
        {
            return JsonDeserialize<T>(json, false);
        }
        /// <summary>
        /// 将JSON 转换成 指定类型对象 
        /// </summary>
        public static T JsonDeserialize<T>(string json, bool throwEeception)
        {
            if (IsNullOrWhiteSpace(json)) return default(T);

            try
            {
                JsonObject obj = StringToJsonObject(json);
                T result = JsonObjectToObject<T>(obj);
                return (T)result;
            }
            catch (Exception exp)
            {
                if (throwEeception) throw;
                else
                {
                    string logMsg = "JsonHelper.JsonDeserialize<T>(string json) 反序列化错误:" + exp;
                    Tools.LogError(logMsg, "Logs/Tools/ErrorLog/");
                    return default(T);
                }
            }
        }
        /// <summary>
        /// 将JSON 转换成 指定类型对象 
        /// </summary>
        public static object JsonDeserialize(string json, Type type)
        {
            return JsonDeserialize(json, type, false);
        }
        /// <summary>
        /// 将JSON 转换成 指定类型对象 
        /// </summary>
        public static object JsonDeserialize(string json, Type type, bool throwEeception)
        {
            if (IsNullOrWhiteSpace(json)) return null;

            try
            {
                JsonObject obj = StringToJsonObject(json);
                object result = JsonObjectToObject(obj, type);
                return result;
            }
            catch (Exception exp)
            {
                if (throwEeception) throw;
                else
                {
                    string logMsg = "JsonHelper.JsonDeserialize(string json, Type type) 反序列化错误:" + exp;
                    Tools.LogError(logMsg, "Logs/Tools/ErrorLog/");
                    return null;
                }
            }
        }


        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 JsonObject 表达一个 Json对象
        /// </summary>
        public static JsonObject JsonDeserializeObject(string json)
        {
            return JsonDeserializeObject(json, false);
        }
        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 JsonObject 表达一个 Json对象
        /// </summary>
        public static JsonObject JsonDeserializeObject(string json, bool throwEeception)
        {
            if (IsNullOrWhiteSpace(json)) return null;

            try
            {
                JsonObject result = StringToJsonObject(json);
                return result;
            }
            catch (Exception exp)
            {
                if (throwEeception) throw;
                else
                {
                    string logMsg = "JsonHelper.JsonDeserializeObject(string json) 反序列化错误:" + exp;
                    Tools.LogError(logMsg, "Logs/Tools/ErrorLog/");
                    return null;
                }
            }
        }
        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 字典、集合 表达一个 Json对象
        /// </summary>
        public static object JsonDeserializeHasnList(string json)
        {
            return JsonDeserializeHasnList<Hashtable>(json, false);
        }
        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 字典、集合 表达一个 Json对象
        /// </summary>
        public static object JsonDeserializeHasnList(string json, bool throwEeception)
        {
            return JsonDeserializeHasnList<Hashtable>(json, throwEeception);
        }
        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 字典、集合 表达一个 Json对象
        /// </summary>
        public static object JsonDeserializeHasnList<HashT>(string json) where HashT : IDictionary, new()
        {
            return JsonDeserializeHasnList<HashT>(json, false);
        }
        /// <summary>
        /// 在 未指定 Json 反序列化类型 的情况下, 用 字典、集合 表达一个 Json对象
        /// </summary>
        public static object JsonDeserializeHasnList<HashT>(string json, bool throwEeception) where HashT : IDictionary, new()
        {
            if (IsNullOrWhiteSpace(json)) return null;

            try
            {
                JsonObject obj = StringToJsonObject(json);
                object result = obj.ToHashList<HashT>();
                return result;
            }
            catch (Exception exp)
            {
                if (throwEeception) throw;
                else
                {
                    string logMsg = "JsonHelper.JsonDeserializeHasnList(string json) 反序列化错误:" + exp;
                    Tools.LogError(logMsg, "Logs/Tools/ErrorLog/");
                    return null;
                }
            }
        }


        /// <summary>
        /// 将 JsonObject 实体 分析成 指定的类型
        /// </summary>
        public static T JsonObjectToObject<T>(JsonObject obj)
        {
            if (obj == null) return default(T);
            object result = JsonObjectToObject(obj, typeof(T));
            return (T)result;
        }
        /// <summary>
        /// 将 JsonObject 实体 分析成 指定的类型
        /// </summary>
        public static object JsonObjectToObject(JsonObject obj, Type type)
        {
            if (obj == null) return null;
            return obj.ToObject(type);
        }


        #endregion


        #region  私 有 函 数

        private static DateTime m_StampRoot = new DateTime(1970, 01, 01, 00, 00, 00, 00, DateTimeKind.Utc);
        private static readonly Regex m_RegNum = new Regex(@"\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Json时间戳 转 时间
        /// </summary>
        public static DateTime StampToDateTime(string timeStamp)
        {
            Match match = m_RegNum.Match(timeStamp);
            if (!match.Success) return m_StampRoot;
            long num = long.Parse(match.Value);
            return StampToDateTime(num);
        }
        /// <summary>
        /// Json时间戳 转 时间
        /// </summary>
        public static DateTime StampToDateTime(long timeStamp)
        {
            return m_StampRoot.AddMilliseconds(timeStamp).ToLocalTime();
        }
        /// <summary>
        /// 时间 转 Json时间戳
        /// </summary>
        public static long DateTimeToStamp(DateTime time)
        {
            return (long)(time.ToUniversalTime() - m_StampRoot).TotalMilliseconds;
        }

        private static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null) return true;
            for (int i = 0; i < value.Length; i++)
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            return true;
        }

        #endregion


        #region  核 心 算 法


        private static JsonObject ObjectToJsonObject(object data)
        {
            if (data == null) return null;

            Type type = data.GetType();

            if (ReflectHelper.IsMetaType(type))
            {
                return new JsonValue(data, false);
            }
            else if (data is IDictionary)
            {
                JsonHash jsonHash = new JsonHash();
                IDictionary hash = (IDictionary)data;
                foreach (object key in hash.Keys)
                {
                    object value = hash[key];
                    JsonObject jsonV = ObjectToJsonObject(value);
                    jsonHash[key.ToString()] = jsonV;
                }
                return jsonHash;
            }
            else if (data is IList)
            {
                JsonList jsonList = new JsonList();
                foreach (object item in (IList)data)
                {
                    JsonObject jsonObj = ObjectToJsonObject(item);
                    if (jsonObj != null) jsonList.Add(jsonObj);
                }
                return jsonList;
            }
            else
            {
                Hashtable hash = new Hashtable();
                FieldInfo[] listField = type.GetFields();
                PropertyInfo[] listProp = type.GetProperties();
                foreach (FieldInfo field in listField)
                {
                    object fieldObj = ReflectHelper.GetValue(data, field);
                    hash[field.Name] = fieldObj;
                }
                foreach (PropertyInfo prop in listProp)
                {
                    object propObj = ReflectHelper.GetValue(data, prop);
                    hash[prop.Name] = propObj;
                }
                return ObjectToJsonObject(hash);
            }
        }
        private static JsonObject StringToJsonObject(string json)
        {
            List<JsonObject> queue = new List<JsonObject>();
            using (JsonReader reader = new JsonReader(json))
            {
                while (!reader.IsEnd)
                {
                    string item = reader.Read();
                    if (string.IsNullOrEmpty(item)) continue;
                    if (item.Length == 1)
                    {
                        char @char = item[0];
                        if (@char == ARRAY_BEGIN)
                        {
                            queue.Add(new JsonList());
                        }
                        else if (@char == OBJECT_BEGIN)
                        {
                            queue.Add(new JsonHash());
                        }
                        else if (@char == ITEM_SPLIT)
                        {
                            MergeLastJsonKeyValue(queue);
                        }
                        else if (@char == KV_SPLIT)
                        {
                            MergeLastJsonKeyValue(queue);
                            queue.Add(new JsonKeyValue());
                        }
                        else if (@char == ARRAY_END)
                        {
                            MergeLastJsonKeyValue(queue);

                            #region  搜索最近的一个数组开始

                            int index = queue.FindLastIndex(x => x.IsList);
                            JsonList array = (JsonList)queue[index];
                            for (int i = index + 1, count = queue.Count; i < count; i++) array.Add(queue[i]);
                            queue.RemoveRange(index + 1, queue.Count - index - 1);

                            #endregion
                        }
                        else if (@char == OBJECT_END)
                        {
                            MergeLastJsonKeyValue(queue);

                            #region  搜索最近的一个对象开始

                            int index = queue.FindLastIndex(x => x.IsHash);
                            JsonHash hash = (JsonHash)queue[index];
                            List<JsonObject> list = new List<JsonObject>();
                            for (int i = index + 1, count = queue.Count; i < count; i++) list.Add(queue[i]);
                            List<JsonObject> listKV = list.FindAll(x => (x is JsonKeyValue));

                            for (int i = 0, count = listKV.Count; i < count; i++)
                            {
                                JsonKeyValue keyValue = (JsonKeyValue)listKV[i];
                                hash.Hash[keyValue.Key.Value.ToString()] = keyValue.Value;
                            }
                            queue.RemoveRange(index + 1, queue.Count - index - 1);

                            #endregion
                        }
                    }
                    else
                    {
                        queue.Add(new JsonValue(item, true));
                    }
                }
                reader.Dispose();
            }

            int queueCount = queue.Count;
            if (queueCount == 1) return queue[0];
            if (queueCount >= 2)
            {
                JsonList jsonList = new JsonList();
                foreach (JsonObject item in queue) jsonList.Add(item);
                return jsonList;
            }

            return null;
        }
        private static void MergeLastJsonKeyValue(List<JsonObject> queue)
        {
            if (queue == null || queue.Count <= 2) return;
            int count = queue.Count;

            if (queue[count - 2] is JsonKeyValue)
            {
                //标准情况
                JsonObject key = queue[count - 3];
                if (!(key is JsonValue)) return;
                JsonObject value = queue[count - 1];
                JsonKeyValue keyValue = (JsonKeyValue)queue[count - 2];
                keyValue.Key = (JsonValue)key;
                keyValue.Value = value;
                queue.RemoveAt(count - 1);
                queue.RemoveAt(count - 3);
            }
            else if (queue[count - 1] is JsonKeyValue)
            {
                //有键无值
                JsonObject key = queue[count - 2];
                if (!(key is JsonValue)) return;
                JsonKeyValue keyValue = (JsonKeyValue)queue[count - 2];
                keyValue.Key = (JsonValue)key;
                queue.RemoveAt(count - 2);
            }


        }





        private const int ARRAY_BEGIN = '[';
        private const int ARRAY_END = ']';
        private const int OBJECT_BEGIN = '{';
        private const int OBJECT_END = '}';
        private const int ITEM_SPLIT = ',';
        private const int KV_SPLIT = ':';
        private const int STR_APOS = '\'';
        private const int STR_QUOT = '"';
        private const int STR_ESCAPE = '\\';
        private const int CHAR_SPACE = (int)' ';
        private const int CHAR_R = (int)'\r';
        private const int CHAR_N = (int)'\n';
        private const int CHAR_T = (int)'\t';
        private const int CHAR_A = (int)'\a';
        private const int CHAR_B = (int)'\b';
        private const int CHAR_F = (int)'\f';
        private const int CHAR_V = (int)'\v';
        private const int CHAR_0 = (int)'\0';

        private class JsonReader : IDisposable
        {
            public JsonReader(string json)
            {
                //Json = FormatString(json);
                Json = json;
                Length = Json == null ? 0 : Json.Length;
            }

            private string Json = string.Empty;
            private int Position = 0;
            private int Length = 0;
            internal bool IsEnd = false;

            /// <summary>
            /// 读取一个JSON的完整节点 字符串, 返回值可能有 [ ] { } , : String 
            /// </summary>
            /// <returns></returns>
            public string Read()
            {
                if (Length <= 0 || Position >= Length) return string.Empty;

                StringBuilder sb = new StringBuilder();
                bool isApos = false, isQuot = false; int len = 0;
                while (Position <= Length)
                {
                    int p = Position;
                    char @char = Json[p];
                    int @charv = (int)@char;
                    Position++; IsEnd = Position >= Length;

                    if (char.IsWhiteSpace(@char))
                    {
                        if (/*sb.Length <= 0*/len <= 0) { continue; }
                        if (@charv != CHAR_SPACE && @charv != CHAR_R && @charv != CHAR_N && @charv != CHAR_T) { continue; } //转义符 仅保留 空格 \r \n \t 
                    }

                    sb.Append(@char); len++;

                    int @pcharv = (int)((p - 1 >= 0) ? Json[p - 1] : char.MinValue);
                    if (!isApos && !isQuot) { if (@charv == STR_APOS && @pcharv != STR_ESCAPE) { isApos = true; } else if (@charv == STR_QUOT && @pcharv != STR_ESCAPE) { isQuot = true; } }
                    else if ((isApos || isQuot) && /*sb.Length > 1*/ len > 1)
                    {
                        if (isApos && @charv == STR_APOS && @pcharv != STR_ESCAPE) { isApos = false; }
                        else if (isQuot && @charv == STR_QUOT && @pcharv != STR_ESCAPE) { isQuot = false; }
                    }

                    if (!isApos && !isQuot)
                    {
                        if (IsConstChar(@charv)) break;
                        if (p + 1 < Length) { char @nchar = Json[p + 1]; if (IsConstChar((int)@nchar)) break; }
                    }
                }

                return sb.ToString().Trim();
            }

            private static bool IsConstChar(int @charv)
            {
                return (@charv == ARRAY_BEGIN || @charv == ARRAY_END || @charv == OBJECT_BEGIN || @charv == OBJECT_END || @charv == ITEM_SPLIT || @charv == KV_SPLIT);
            }
            public void Dispose()
            {
                Json = null;
                Position = Length = 0;
            }
        }

        #endregion








    }


    #region  Json 实 体

    public abstract class JsonObject
    {
        public bool IsValue { get { return this is JsonValue; } }
        public bool IsList { get { return this is JsonList; } }
        public bool IsHash { get { return this is JsonHash; } }
        public bool IsKeyValue { get { return this is JsonKeyValue; } }

        public virtual string ToJson()
        {
            return ToJson(0, false);
        }
        public virtual string ToJson(int level)
        {
            return ToJson(level, false);
        }
        public abstract string ToJson(int level, bool format);


        public abstract object ToObject(Type type);
        public object ToHashList()
        {
            return InnerToHashList<Dictionary<string, object>>();
        }
        public object ToHashList<HashT>() where HashT : IDictionary, new()
        {
            return InnerToHashList<HashT>();
        }
        protected abstract object InnerToHashList<HashT>() where HashT : IDictionary, new();

        public override string ToString()
        {
            return ToJson(0, true);
        }
    }
    public class JsonValue : JsonObject
    {
        public JsonValue() { }
        public JsonValue(object value) : this(value, false)
        {
        }
        public JsonValue(object value, bool json)
        {
            this.m_Value = FmtValue(value, json);
        }

        private object m_Value = null;

        public string Json
        {
            get
            {
                object value = m_Value;
                //理论上这两行代码 不可达, 理论上 m_Value 不可能是 Json对象
                if (value is JsonValue) return ((JsonValue)value).Json;
                if (value is JsonObject) return ((JsonObject)value).ToJson();

                if (m_Value == null) return "null";
                if (m_Value is bool) return ((bool)m_Value).ToString().ToLower();
                if (m_Value is Guid) return "\"" + ((Guid)m_Value).ToString("D") + "\"";
                if (m_Value is DateTime) return "\"" + ((DateTime)m_Value).ToString("yyyy-MM-ddTHH:mm:ss.fffffff") + "\"";
                if (m_Value is StringBuilder || m_Value is string || m_Value is char) return "\"" + StringToJsonString(m_Value.ToString()) + "\"";
                if (m_Value is byte || m_Value is short || m_Value is int || m_Value is long || m_Value is sbyte || m_Value is ushort || m_Value is uint || m_Value is ulong) return m_Value.ToString();
                if (m_Value is float || m_Value is double || m_Value is decimal) return m_Value.ToString();

                //if (m_Value is float) return ((float)m_Value).ToString();
                //if (m_Value is double) return ((double)m_Value).ToString();
                //if (m_Value is decimal) return ((decimal)m_Value).ToString();

                return "\"" + StringToJsonString(m_Value.ToString()) + "\"";
            }
        }
        public object Value
        {
            get { return m_Value; }
            set { this.m_Value = FmtValue(value, false); }
        }

        private static object FmtValue(object value, bool json)
        {
            if (value == null) { return null; }
            if (value is bool) { return (bool)value; }
            if (value is Guid) { return (Guid)value; }
            if (value is DateTime) { return (DateTime)value; }
            if (value is StringBuilder || value is string || value is char)
            {
                string temp = value.ToString();
                int len = temp.Length;
                if (json && len >= 2 && temp[0] == '"' && temp[len - 1] == '"')
                {
                    string str = JsonStringToString(temp.Substring(1, len - 2));
                    DateTime time; if (TryToDateTime(str, out time)) return time;
                    return str;
                }
                else if (json && len >= 2 && temp[0] == '\'' && temp[len - 1] == '\'')
                {
                    string str = JsonStringToString(temp.Substring(1, len - 2));
                    DateTime time; if (TryToDateTime(str, out time)) return time;
                    return str;
                }
                else
                {
                    if (json)
                    {
                        //bool float 不需要引号
                        string trim = temp.Trim();
                        string lower = trim.ToLower();
                        if (lower == "null") return null;
                        if (lower == "true") return true;
                        if (lower == "false") return false;
                        if (lower.StartsWith("new ")) return trim;
                        DateTime time; if (TryToDateTime(lower, out time)) return time;
                        double doub; if (double.TryParse(trim, out doub)) return doub;
                    }

                    return temp;
                }
            }
            return value;
        }
        private static bool TryToDateTime(string source, out DateTime time)
        {
            try
            {
                string lower = source.ToLower();
                if (lower.StartsWith("/date(") || lower.StartsWith("date(") || lower.StartsWith("\\/date("))
                {
                    //兼容微软最初的 Json时间格式   "Birthday":"\/Date(734840435887)\/" 
                    time = JsonHelper.StampToDateTime(lower);
                    return (time != DateTime.MinValue);
                }
            }
            catch (Exception) { }
            time = DateTime.MinValue;
            return false;
        }





        public override object ToObject(Type type)
        {
            if (type == null || type == typeof(object)) return Value;
            return ReflectHelper.ChangeType(Value, type);
        }
        public override string ToJson(int level, bool format)
        {
            //return new string(' ', level*2) + Value;
            return Json;
        }
        protected override object InnerToHashList<HashT>()
        {
            if (Value == null) return null;
            if (Value is JsonObject) return ((JsonObject)Value).ToHashList<HashT>();
            return Value;
        }


        private static string JsonStringToString(string json)
        {
            if (string.IsNullOrEmpty(json)) return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0, c = json.Length; i < c; i++)
            {
                char @char = json[i];
                if (@char != '\\') { sb.Append(@char); continue; }
                if (i + 1 >= c) { sb.Append(@char); continue; }
                char @charn = json[i + 1];
                if (@charn == '\\') { sb.Append('\\'); i++; continue; }
                if (@charn == 'r') { sb.Append('\r'); i++; continue; }
                if (@charn == 'n') { sb.Append('\n'); i++; continue; }
                if (@charn == 't') { sb.Append('\t'); i++; continue; }
                if (@charn == 'a') { sb.Append('\a'); i++; continue; }
                if (@charn == 'b') { sb.Append('\b'); i++; continue; }
                if (@charn == 'f') { sb.Append('\f'); i++; continue; }
                if (@charn == 'v') { sb.Append('\v'); i++; continue; }
                if (@charn == '"') { sb.Append('"'); i++; continue; }
                if (@charn == '\'') { sb.Append('\''); i++; continue; }
                if (@charn == '0') { /*sb.Append('\0');*/ i++; continue; }  // \0字符直接排除

                if (@charn == 'u')
                {
                    //\uFFFF 一是 Unicode 字符, 这类字符 一般是 4位 \uFFFF, 但是理论上 可以有5位 \uFFFFF
                    string hex = json.Substring(i + 2, 4);
                    //char h5 = json[i + 2 + 4];
                    //bool isHex5 = (h5 == '0' || h5 == '1' || h5 == '2' || h5 == '3' || h5 == '4' || h5 == '5' || h5 == '6' || h5 == '7' || h5 == '8' || h5 == '9' || h5 == 'a' || h5 == 'b' || h5 == 'c' || h5 == 'd' || h5 == 'e' || h5 == 'f' || h5 == 'A' || h5 == 'B' || h5 == 'C' || h5 == 'D' || h5 == 'E' || h5 == 'F');
                    //if (isHex5) hex = hex + h5;
                    int num;
                    if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
                    {
                        sb.Append((char)num); i = i + 5; /*if (isHex5){i++;} */continue;
                    }
                    else { sb.Append("\\u"); i++; continue; }
                }
            }
            return sb.ToString();
        }
        internal static string StringToJsonString(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0, c = str.Length; i < c; i++)
            {
                char @char = str[i];
                //if (@char == '\'') { sb.Append("\\'"); continue; } //单引号不再进行转义
                if (@char == '"') { sb.Append("\\\""); continue; }
                if (@char == '\\') { sb.Append("\\\\"); continue; }
                if (@char == '\r') { sb.Append("\\r"); continue; }
                if (@char == '\n') { sb.Append("\\n"); continue; }
                if (@char == '\t') { sb.Append("\\t"); continue; }
                if (@char == '\a') { sb.Append("\\a"); continue; }
                if (@char == '\b') { sb.Append("\\b"); continue; }
                if (@char == '\f') { sb.Append("\\f"); continue; }
                if (@char == '\v') { sb.Append("\\v"); continue; }
                if (@char == '\0') { /*sb.Append('\\\0');*/ continue; }  // \0字符直接排除
                sb.Append(@char);
            }
            return sb.ToString();
        }
    }
    public class JsonKeyValue : JsonObject
    {
        public JsonKeyValue() { }
        public JsonKeyValue(JsonValue key, JsonObject value) { this.Key = key; this.Value = value; }

        public JsonValue Key { get; set; }
        public JsonObject Value { get; set; }

        public override object ToObject(Type type)
        {
            object obj = Activator.CreateInstance(type);

            ReflectHelper.SetValue(obj, "Key", (Key == null ? string.Empty : Key.Value.ToString()));

            Type propOrFieldType = ReflectHelper.GetPropertyOrFieldType(type, "Value");
            object value = (Value == null ? null : Value.ToObject(propOrFieldType));
            ReflectHelper.SetValue(obj, "Value", value);

            return obj;
        }
        public override string ToJson(int level, bool format)
        {
            return string.Format((format ? @"{0}: {1}" : @"{0}:{1}"), (Key == null ? string.Empty : Key.ToJson(0, false)), (Value == null ? "null" : Value.ToJson(level, format)));
        }
        public new KeyValuePair<string, object> ToHashList()
        {
            return (KeyValuePair<string, object>)InnerToHashList<Dictionary<string, object>>();
        }
        public new KeyValuePair<string, object> ToHashList<HashT>() where HashT : IDictionary, new()
        {
            return (KeyValuePair<string, object>)InnerToHashList<HashT>();
        }
        protected override object InnerToHashList<HashT>()
        {
            return new KeyValuePair<string, object>((Key == null ? string.Empty : Key.Value.ToString()), (Value == null ? null : Value.ToHashList<HashT>()));
        }
    }
    public class JsonList : JsonObject, IList, IList<JsonObject>
    {
        public bool HasValues { get; set; }

        [NonSerialized]
        private List<JsonObject> m_List;
        private List<JsonObject> List
        {
            get { return m_List ?? (m_List = new List<JsonObject>()); }
        }


        public int Count
        {
            get { return this.List.Count; }
        }
        public JsonObject this[int index]
        {
            get { return this.List[index]; }
            set { this.List[index] = value; }
        }
        public void Add(JsonObject item)
        {
            this.List.Add(item);
        }
        public void Insert(int index, JsonObject item)
        {
            this.List.Insert(index, item);
        }
        public void Remove(JsonObject item)
        {
            this.List.Remove(item);
        }
        public void RemoveAt(int index)
        {
            this.List.RemoveAt(index);
        }
        public void Clear()
        {
            this.List.Clear();
        }
        public bool Contains(JsonObject item)
        {
            return this.List.Contains(item);
        }
        public int IndexOf(JsonObject item)
        {
            return this.List.IndexOf(item);
        }
        public void CopyTo(JsonObject[] array, int arrayIndex)
        {
            this.List.CopyTo(array, arrayIndex);
        }



        public override object ToObject(Type type)
        {
            if (m_List == null) return null;

            Type listType = null;
            Type itemType = null;
            int sign = 0;
            if (type.IsArray) { listType = type; itemType = listType.GetElementType(); sign = 3; }
            else if (typeof(IList<>).IsAssignableFrom(type)) { listType = type; itemType = GetListElementType(listType); sign = 2; }
            else if (typeof(IList).IsAssignableFrom(type)) { listType = type; itemType = typeof(object); sign = 1; }
            else { listType = typeof(List<>).MakeGenericType(type); itemType = type; sign = 0; }

            if (sign < 0 || listType == null || ((sign == 2 || sign == 3) && listType.IsGenericTypeDefinition))
                throw new Exception(string.Format("JsonList 无法反序列化得到类型: {0}", type.FullName));

            int count = (m_List != null) ? m_List.Count : 0;
            if (sign == 0 || sign == 1 || sign == 2)
            {
                IList list = (IList)Activator.CreateInstance(listType);
                if (count >= 1)
                    foreach (JsonObject item in m_List)
                    {
                        object obj = item.ToObject(itemType);
                        list.Add(obj);
                    }
                return list;
            }
            else if (sign == 3)
            {
                Array array = Array.CreateInstance(itemType, count);
                if (count >= 1)
                    for (int i = 0; i < count; i++)
                    {
                        JsonObject item = m_List[i];
                        object obj = item.ToObject(itemType);
                        array.SetValue(obj, i);
                    }
                return array;
            }

            return null;
        }
        public override string ToJson(int level, bool format)
        {
            string rn = !format ? string.Empty : "\r\n";
            string prev = !format ? string.Empty : new string(' ', (level) * 2);
            string prev2 = !format ? string.Empty : new string(' ', (level + 1) * 2);
            StringBuilder sb = new StringBuilder();
            sb.Append("[" + rn);
            if (m_List != null)
            {
                List<string> list = new List<string>();
                foreach (JsonObject item in m_List)
                    list.Add(prev2 + item.ToJson(level + 1, format));

                for (int i = 0, count = list.Count; i < count; i++)
                    sb.Append(list[i] + (i == count - 1 ? string.Empty : ",") + rn);
            }
            sb.Append(prev + "]");
            return sb.ToString();
        }
        public new List<object> ToHashList()
        {
            return (List<object>)InnerToHashList<Dictionary<string, object>>();
        }
        public new List<object> ToHashList<HashT>() where HashT : IDictionary, new()
        {
            return (List<object>)InnerToHashList<HashT>();
        }
        protected override object InnerToHashList<HashT>()
        {
            if (m_List == null) return null;

            List<object> list = new List<object>();
            foreach (JsonObject item in m_List)
                list.Add(item.ToHashList<HashT>());
            return list;
        }


        /// <summary>
        /// 查找指定 List类型, 对应的 集合成员类型
        /// </summary>
        public static Type GetListElementType(Type listType)
        {
            Type baseType = listType;
            Type[] itfsTypes = baseType.GetInterfaces();
            foreach (Type itfsType in itfsTypes)
            {
                if (itfsType.IsGenericType && !itfsType.IsGenericTypeDefinition && typeof(IList<>) == itfsType.GetGenericTypeDefinition())
                {
                    Type[] genericTypes = itfsType.GetGenericArguments();
                    if (genericTypes.Length == 1) return genericTypes[0];
                }
            }

            foreach (Type itfsType in itfsTypes)
            {
                if (itfsType.IsGenericType && !itfsType.IsGenericTypeDefinition && typeof(ICollection<>) == itfsType.GetGenericTypeDefinition())
                {
                    Type[] genericTypes = itfsType.GetGenericArguments();
                    if (genericTypes.Length == 1) return genericTypes[0];
                }
            }

            if (typeof(IList).IsAssignableFrom(listType)) { return typeof(object); }
            return null;
        }


        #region  中转的继承

        IEnumerator<JsonObject> IEnumerable<JsonObject>.GetEnumerator()
        {
            return ((IEnumerable<JsonObject>)this.List).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.List).GetEnumerator();
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.List).CopyTo(array, index);
        }
        bool ICollection<JsonObject>.Remove(JsonObject item)
        {
            return ((ICollection<JsonObject>)this.List).Remove(item);
        }
        int ICollection<JsonObject>.Count
        {
            get { return ((ICollection<JsonObject>)this.List).Count; }
        }
        bool ICollection<JsonObject>.IsReadOnly
        {
            get { return ((ICollection<JsonObject>)this.List).IsReadOnly; }
        }
        int ICollection.Count
        {
            get { return ((ICollection)this.List).Count; }
        }
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.List).SyncRoot; }
        }
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.List).IsSynchronized; }
        }
        int IList.Add(object value)
        {
            return ((IList)this.List).Add(value);
        }
        bool IList.Contains(object value)
        {
            return ((IList)this.List).Contains(value);
        }
        void ICollection<JsonObject>.Add(JsonObject item)
        {
            ((ICollection<JsonObject>)this.List).Add(item);
        }
        void ICollection<JsonObject>.Clear()
        {
            ((ICollection<JsonObject>)this.List).Clear();
        }
        bool ICollection<JsonObject>.Contains(JsonObject item)
        {
            return ((ICollection<JsonObject>)this.List).Contains(item);
        }
        void ICollection<JsonObject>.CopyTo(JsonObject[] array, int arrayIndex)
        {
            ((ICollection<JsonObject>)this.List).CopyTo(array, arrayIndex);
        }
        void IList.Clear()
        {
            ((IList)this.List).Clear();
        }
        int IList.IndexOf(object value)
        {
            return ((IList)this.List).IndexOf(value);
        }
        void IList.Insert(int index, object value)
        {
            ((IList)this.List).Insert(index, value);
        }
        void IList.Remove(object value)
        {
            ((IList)this.List).Remove(value);
        }
        int IList<JsonObject>.IndexOf(JsonObject item)
        {
            return ((IList<JsonObject>)this.List).IndexOf(item);
        }
        void IList<JsonObject>.Insert(int index, JsonObject item)
        {
            ((IList<JsonObject>)this.List).Insert(index, item);
        }
        void IList<JsonObject>.RemoveAt(int index)
        {
            ((IList<JsonObject>)this.List).RemoveAt(index);
        }
        JsonObject IList<JsonObject>.this[int index]
        {
            get { return ((IList<JsonObject>)this.List)[index]; }
            set { ((IList<JsonObject>)this.List)[index] = value; }
        }
        void IList.RemoveAt(int index)
        {
            ((IList)this.List).RemoveAt(index);
        }
        object IList.this[int index]
        {
            get { return ((IList)this.List)[index]; }
            set { ((IList)this.List)[index] = value; }
        }
        bool IList.IsReadOnly
        {
            get { return ((IList)this.List).IsReadOnly; }
        }
        bool IList.IsFixedSize
        {
            get { return ((IList)this.List).IsFixedSize; }
        }

        #endregion
    }
    public class JsonHash : JsonObject, IDictionary, IDictionary<string, JsonObject>
    {
        public bool HasValues { get; set; }

        [NonSerialized]
        private Dictionary<string, JsonObject> m_Hash;
        public Dictionary<string, JsonObject> Hash
        {
            get { return m_Hash ?? (m_Hash = new Dictionary<string, JsonObject>()); }
        }


        public JsonObject this[string key]
        {
            get
            {
                if (m_Hash == null || m_Hash.Count <= 0) return null;
                JsonObject value;
                if (m_Hash.TryGetValue(key, out value)) return value;
                return null;
            }
            set
            {
                if (key == null) return;
                if (!this.Hash.ContainsKey(key)) this.Hash.Add(key, value);
                else this.Hash[key] = value;
            }
        }
        public int Count
        {
            get { return this.Hash.Count; }
        }
        public void Add(string key, JsonObject value)
        {
            this.Hash.Add(key, value);
        }
        public void Remove(string key)
        {
            this.Hash.Remove(key);
        }
        public void Clear()
        {
            this.Hash.Clear();
        }
        public bool ContainsKey(string key)
        {
            return this.Hash.ContainsKey(key);
        }
        public bool TryGetValue(string key, out JsonObject value)
        {
            return this.Hash.TryGetValue(key, out value);
        }
        public ICollection<string> Keys
        {
            get { return this.Hash.Keys; }
        }
        public ICollection<JsonObject> Values
        {
            get { return this.Hash.Values; }
        }



        public override object ToObject(Type type)
        {
            if (m_Hash == null) return null;

            int sign = 0;
            if (type == null || type == typeof(object) || typeof(IDictionary).IsAssignableFrom(type)) { if (type == null || type == typeof(object)) { type = typeof(Hashtable); } sign = 1; }
            else { sign = 0; }

            if (sign < 0 || (sign == 1 && type.IsGenericTypeDefinition))
                throw new Exception(string.Format("JsonHash 无法反序列化得到类型: {0}", type.FullName));

            int count = (m_Hash != null) ? m_Hash.Count : 0;
            if (sign == 1)
            {
                IDictionary hash = (IDictionary)Activator.CreateInstance(type);
                if (count >= 1)
                    foreach (KeyValuePair<string, JsonObject> pair in m_Hash)
                    {
                        object obj = pair.Value.ToObject(typeof(object));
                        if (hash.Contains(pair.Key)) hash[pair.Key] = obj;
                        else hash.Add(pair.Key, obj);
                    }
                return hash;
            }
            else if (sign == 0)
            {
                object hash = Activator.CreateInstance(type);
                if (count >= 1)
                    foreach (KeyValuePair<string, JsonObject> pair in m_Hash)
                    {
                        string key = pair.Key;
                        Type propOrFieldType = ReflectHelper.GetPropertyOrFieldType(type, key);
                        object value = (pair.Value == null ? null : pair.Value.ToObject(propOrFieldType));
                        ReflectHelper.SetValue(hash, key, value);
                    }
                return hash;
            }

            return null;
        }

        public override string ToJson(int level, bool format)
        {
            string rn = !format ? string.Empty : "\r\n";
            string prev = !format ? string.Empty : new string(' ', (level) * 2);
            string prev2 = !format ? string.Empty : new string(' ', (level + 1) * 2);
            StringBuilder sb = new StringBuilder();
            sb.Append("{" + rn);
            if (m_Hash != null)
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, JsonObject> pair in m_Hash)
                    list.Add(prev2 + string.Format((format ? @"""{0}"": {1}" : @"""{0}"":{1}"), (pair.Key ?? string.Empty), (pair.Value == null ? "null" : pair.Value.ToJson(level + 1, format))));

                for (int i = 0, count = list.Count; i < count; i++)
                    sb.Append(list[i] + (i == count - 1 ? string.Empty : ",") + rn);
            }
            sb.Append(prev + "}");
            return sb.ToString();
        }
        public new Dictionary<string, object> ToHashList()
        {
            return (Dictionary<string, object>)InnerToHashList<Dictionary<string, object>>();
        }
        public new HashT ToHashList<HashT>() where HashT : IDictionary, new()
        {
            return (HashT)InnerToHashList<HashT>();
        }
        protected override object InnerToHashList<HashT>()
        {
            if (m_Hash == null) return null;

            HashT hash = new HashT();
            foreach (KeyValuePair<string, JsonObject> pair in m_Hash)
            {
                if (hash.Contains(pair.Key)) hash[pair.Key] = pair.Value.ToHashList<HashT>();
                else hash.Add(pair.Key, pair.Value.ToHashList<HashT>());
            }
            return hash;
        }


        #region  中转的继承

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)this.Hash).Contains(key);
        }
        void IDictionary.Add(object key, object value)
        {
            ((IDictionary)this.Hash).Add(key, value);
        }
        void ICollection<KeyValuePair<string, JsonObject>>.Add(KeyValuePair<string, JsonObject> item)
        {
            ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).Add(item);
        }
        void ICollection<KeyValuePair<string, JsonObject>>.Clear()
        {
            ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).Clear();
        }
        bool ICollection<KeyValuePair<string, JsonObject>>.Contains(KeyValuePair<string, JsonObject> item)
        {
            return ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).Contains(item);
        }
        void ICollection<KeyValuePair<string, JsonObject>>.CopyTo(KeyValuePair<string, JsonObject>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<string, JsonObject>>.Remove(KeyValuePair<string, JsonObject> item)
        {
            return ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).Remove(item);
        }
        int ICollection<KeyValuePair<string, JsonObject>>.Count
        {
            get { return ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).Count; }
        }
        bool ICollection<KeyValuePair<string, JsonObject>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, JsonObject>>)this.Hash).IsReadOnly; }
        }
        void IDictionary.Clear()
        {
            ((IDictionary)this.Hash).Clear();
        }
        IEnumerator<KeyValuePair<string, JsonObject>> IEnumerable<KeyValuePair<string, JsonObject>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, JsonObject>>)this.Hash).GetEnumerator();
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)this.Hash).GetEnumerator();
        }
        void IDictionary.Remove(object key)
        {
            ((IDictionary)this.Hash).Remove(key);
        }
        object IDictionary.this[object key]
        {
            get { return ((IDictionary)this.Hash)[key]; }
            set { ((IDictionary)this.Hash)[key] = value; }
        }
        bool IDictionary<string, JsonObject>.ContainsKey(string key)
        {
            return ((IDictionary<string, JsonObject>)this.Hash).ContainsKey(key);
        }
        void IDictionary<string, JsonObject>.Add(string key, JsonObject value)
        {
            ((IDictionary<string, JsonObject>)this.Hash).Add(key, value);
        }
        bool IDictionary<string, JsonObject>.Remove(string key)
        {
            return ((IDictionary<string, JsonObject>)this.Hash).Remove(key);
        }
        bool IDictionary<string, JsonObject>.TryGetValue(string key, out JsonObject value)
        {
            return ((IDictionary<string, JsonObject>)this.Hash).TryGetValue(key, out value);
        }
        JsonObject IDictionary<string, JsonObject>.this[string key]
        {
            get { return ((IDictionary<string, JsonObject>)this.Hash)[key]; }
            set { ((IDictionary<string, JsonObject>)this.Hash)[key] = value; }
        }
        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)this.Hash).Keys; }
        }
        ICollection<JsonObject> IDictionary<string, JsonObject>.Values
        {
            get { return ((IDictionary<string, JsonObject>)this.Hash).Values; }
        }
        ICollection<string> IDictionary<string, JsonObject>.Keys
        {
            get { return ((IDictionary<string, JsonObject>)this.Hash).Keys; }
        }
        ICollection IDictionary.Values
        {
            get { return ((IDictionary)this.Hash).Values; }
        }
        bool IDictionary.IsReadOnly
        {
            get { return ((IDictionary)this.Hash).IsReadOnly; }
        }
        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary)this.Hash).IsFixedSize; }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Hash).GetEnumerator();
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.Hash).CopyTo(array, index);
        }
        int ICollection.Count
        {
            get { return ((ICollection)this.Hash).Count; }
        }
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.Hash).SyncRoot; }
        }
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.Hash).IsSynchronized; }
        }

        #endregion
    }

    #endregion
}
