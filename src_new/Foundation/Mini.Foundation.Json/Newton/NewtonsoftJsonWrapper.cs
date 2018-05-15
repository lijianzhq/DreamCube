using System;

using Newtonsoft.Json;

namespace Mini.Foundation.Json.Newton
{
    public class NewtonsoftJsonWrapper : IJson
    {
        public String Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
