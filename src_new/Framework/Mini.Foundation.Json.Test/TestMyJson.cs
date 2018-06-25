using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mini.Foundation.Json;
using Mini.Foundation.Json.Newton;
using Newtonsoft.Json;

namespace Mini.Foundation.Json.Test
{
    public class TestObj1
    {
        public String Name;
    }

    public class TestObj2
    {
        [NullValueProvider]
        public String Name;
    }

    [TestFixture]
    public class TestMyJson
    {
        [Test]
        public void TestMethod()
        {
            var testObj1 = new TestObj1()
            {
                Name = null
            };
            String json = String.Empty;
            json = JsonConvert.SerializeObject(testObj1);
            Assert.AreEqual("{\"Name\":null}", json);

            String json2 = MyJson.Serialize(testObj1, new JsonSerializerSettings()
            {
                ContractResolver = new ExtendContractResolver()
            });
            //因为TestObj1没有打上NullValueProvider
            Assert.AreEqual("{\"Name\":null}", json2);

            var testObj2 = new TestObj2()
            {
                Name = null
            };
            json = String.Empty;
            json = JsonConvert.SerializeObject(testObj2);
            Assert.AreEqual("{\"Name\":null}", json);

            json2 = MyJson.Serialize(testObj2, new JsonSerializerSettings()
            {
                ContractResolver = new ExtendContractResolver()
            });
            //因为TestObj1没有打上NullValueProvider
            Assert.AreEqual("{\"Name\":\"\"}", json2);
        }
    }
}
