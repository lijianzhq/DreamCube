using NUnit.Framework;
using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class PropertyReaderTest
    {
        [Test]
        public void TestMethod()
        {
            string Text = @"{
	field: 'EFFECT_CONF_SCHE',
	title: '效果确认方案',
	width: 180,
	formatter: function(value, row) {
		return row.EFFECT_CONF_SCHE_NAME; //return返回的必须是textField字段的值
	},
	editor: {
		type: 'combobox',
		options: {
			editable: false,
			valueField: 'EFFECT_CONF_SCHE', //必须与列的field字段一致
			textField: 'EFFECT_CONF_SCHE_NAME',
			onBeforeLoad: function(data) {
				var editor = $(this);
				var postData = {
					OpType: 'loadColumnEditData',
					GridCODE: me.config.GridCODE,
					FieldCODE: 'EFFECT_CONF_SCHE'//这个值需要告诉服务器端，是获取的哪个列的值（每个列值可以单独配置sql的）
				};
				data = $.extend(data, postData);
			},
			loadFilter: function(data) {
				if (data.OpResult) {
                    console.log(data.OpData);
					return data.OpData;
				}
				return [];
			},
			url: me.config.server,
			required: true
		}
	}
}";
            var p = new JsonPropertyReader(Text);
            var dic = p.Read();
            Assert.AreEqual(dic.Count, 5);
            Assert.AreEqual(dic["field"], "EFFECT_CONF_SCHE");
            Assert.AreEqual(dic["title"], "效果确认方案");
        }
    }
}
