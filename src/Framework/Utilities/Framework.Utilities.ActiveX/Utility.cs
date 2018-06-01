using System;
using System.Reflection;
using System.Runtime.InteropServices;
using mshtml;

namespace DreamCube.Framework.Utilities.ActiveX
{
    [ComVisible(true)]
    public class Utility
    {
        /// <summary>
        /// 调用javascript
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="funParams">方法参数</param>
        public void DoCallJavaScript(String functionName, Object[] funParams)
        {
            Type typeIOleObject = this.GetType().GetInterface("IOleObject", true);
            Object oleClientSite = typeIOleObject.InvokeMember("GetClientSite",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public, null, this, null);

            IOleClientSite oleClientSite2 = oleClientSite as IOleClientSite;
            IOleContainer pObj;
            oleClientSite2.GetContainer(out pObj);

            //获取页面的Script集合  
            IHTMLDocument pDoc2 = (IHTMLDocument)pObj;
            Object script = pDoc2.Script;

            try
            {
                //调用JavaScript并传递参数，因为此方法可能并没有在页面中实现，所以要进行异常处理  
                script.GetType().InvokeMember(functionName,
                                                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
                                                null,
                                                script,
                                                funParams);
            }
            catch { }
        }
    }
}
