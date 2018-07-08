#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0)
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JScript;

using Mini.Foundation.Basic.Utility;

namespace Mini.Foundation.Basic.Test
{
    [TestFixture]
    public class JScriptHelperTest
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
            var result = JScriptHelper.Eval("1+2+3+4+5+6+7+8+9");//45
            Assert.AreEqual(result.ToString(), "45");

        }

        [Test]
        public void TestMethod_JSON()
        {
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
            JSObject result = JScriptHelper.Eval("({\"name\":\"lijian\"})") as JSObject;
            Assert.AreEqual(result["name"], "lijian");
        }

        [Test]
        public void TestMethod_JSON2()
        {
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
            var str = @"{
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
					//GridCODE: me.config.GridCODE,
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
            JSObject result = JScriptHelper.Eval($"({str})") as JSObject;
            Assert.AreEqual(result["field"], "EFFECT_CONF_SCHE");
        }
    }
}
#endif