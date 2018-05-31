using System;
using System.Collections;
using System.Collections.Generic;

//第三方的命名空间
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mini.Foundation.Json
{
    public static class MyJson
    {
        /// <summary>
        /// 把对象序列化成JSON，可以为匿名对象
        /// var o = new
        /// {
        ///    a = 1,
        ///    b = "Hello, World!",
        ///    c = new[] { 1, 2, 3 },
        ///    d = new Dictionary<string, int> { { "x", 1 }, { "y", 2 } }
        /// };
        /// </summary>
        /// <param name="target">可以传入匿名对象</param>
        /// <returns></returns>
        public static String Serialize(Object target, JsonSerializerSettings settings = null)
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.NullValueHandling = NullValueHandling.Include;
            //settings.DefaultValueHandling = DefaultValueHandling.Include;
            //if (settings == null)
            //    settings = new JsonSerializerSettings() { };
            //settings.Converters.Add(new DateTimeConverter());
            return JsonConvert.SerializeObject(target, Formatting.None, settings);
        }

        /// <summary>
        /// 根据json反序列化成指定的对象实例
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object Deserialize(String json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 根据json反序列化成对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
