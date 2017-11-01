using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 操作网页page对象
    /// </summary>
    public static class MyPage
    {
        #region "属性"

        /// <summary>
        /// 获取当前网页所在的目录物理路径，返回的字符串最后不包含斜杠符号：“\\”
        /// </summary>
        public static String CurFolderPhysicalPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(".");
            }
        }

        /// <summary>
        /// 获取当前网页所在的目录的上层目录的物理路径，返回的字符串最后不包含斜杠：“\\”
        /// </summary>
        public static String ParentFolderPhysicalPath
        {
            get
            {
                String curpath = CurFolderPhysicalPath;
                return MyString.LeftOfLast(curpath, "\\");
            }
        }

        /// <summary>
        /// 获取当前网页所在的相对目录（相对web根目录），返回的字符串最后不包含斜杠“/”
        /// </summary>
        public static String CurFolderPath
        {
            get
            {
                String url = HttpContext.Current.Request.ServerVariables["URL"];
                return MyString.LeftOfLast(url, "/");
            }
        }

        /// <summary>
        /// 获取当前网页所在的目录的父目录，返回的字符串最后不包含斜杠“/”
        /// </summary>
        public static String ParentFolderPath
        {
            get
            {
                String url = HttpContext.Current.Request.ServerVariables["URL"];
                String parentFolder = MyString.LeftOfLast(url, "/", true, "");
                if (parentFolder.Length == 0) return "/";
                return MyString.LeftOfLast(parentFolder, "/");
            }
        }

        /// <summary>
        /// 当前网页所在目录的url路径，返回的字符串最后包含斜杠“/”
        /// </summary>
        public static String CurFolderUrl
        {
            get
            {
                String pathInfo = MyString.LeftOfLast(HttpContext.Current.Request.Path, "/") + "/";
                return MyString.LeftOfLast(MyWebsite.GetServerBasicAddr(), "/") + pathInfo;
            }
        }

        /// <summary>
        /// 当前网页的父目录url路径，返回的字符串最后包含斜杠“/”
        /// </summary>
        public static String ParentFolderUrl
        {
            get
            {
                String curUrl = CurFolderUrl;
                return MyString.LeftOfLast(MyString.LeftOfLast(curUrl, "/"), "/") + "/";
            }
        }

        /// <summary>
        /// 获取上一级再上一级文件夹的Url地址
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GrandParentFolderUrl
        {
            get
            {
                String sUrl = MyString.LeftOfLast(MyPage.ParentFolderUrl, "/", true, "");
                if (sUrl.IndexOf("://") == -1) return "";
                else return sUrl;
            }
        }

        #endregion

        #region "公共方法"

        /// <summary> 
        /// 设置IE端的字段的值 
        /// </summary> 
        /// <param name="sFrame">字段所在帧结构名称</param> 
        /// <param name="sFieldName">字段名称</param> 
        /// <param name="vFieldValue">字段值</param> 
        public static void SetWebFieldValue(string sFrame, string sFieldName, String sFieldValue)
        {
            string sScript = "var oField = " + sFrame + ".document.all['" + sFieldName + "']; ";
            sScript += "var sValue = '" + sFieldValue + "'; ";
            sScript += "if(oField){if(oField.length>1 && oField[0].type=='radio'){parentWindow.SetRadioFieldValue(oField[0].name, sValue);}else{oField.value = sValue;}; ";
            sScript += "if(oField.onchange && typeof(oField.readyOnly)=='undefined') oField.onchange();}; ";
            MyPage.ExecJs(sScript);
        }

        /// <summary>
        /// 获取Form的所有Control
        /// </summary>
        /// <param name="oForm"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Control> GetAllControls(HtmlForm oForm = null)
        {
            if (oForm == null)
            {
                Page oPage = (Page)HttpContext.Current.Handler;
                oForm = oPage.Form;
            }
            return GetAllChildControls(oForm);
        }

        /// <summary>
        /// 获取所有的子控件
        /// </summary>
        /// <param name="tagName">控件的标签名，例如：TextBox</param>
        public static List<Control> GetAllChildControls(String tagName = "")
        {
            Page oPage = (Page)HttpContext.Current.Handler;
            return GetAllChildControls(oPage, tagName);
        }

        /// <summary>
        /// 获取所有的子控件
        /// </summary>
        /// <param name="c">父控件对象</param>
        /// <param name="tagName">控件的标签名，例如：TextBox</param>
        public static List<Control> GetAllChildControls(Control c, String tagName)
        {
            List<Control> controls = new List<Control>();
            if (!String.IsNullOrEmpty(tagName)) tagName = tagName.ToUpper();
            if (c != null)
            {
                Queue<Control> controlQueue = new Queue<Control>();
                //if (c.HasControls())
                //{
                //    //首先进队列
                //    foreach (Control tempC in c.Controls)
                //        controlQueue.Enqueue(tempC);
                //}
                controlQueue.Enqueue(c);
                while (controlQueue.Count > 0)
                {
                    Control deQueueControl = controlQueue.Dequeue();
                    //加入列表中
                    if (String.IsNullOrEmpty(tagName) || String.Compare(tagName, deQueueControl.GetType().Name.ToString(), false) == 0)
                        controls.Add(deQueueControl);
                    //循环所有的子控件，加入列表中，并判断该控件是否还有子控件，有的话继续加入列表中
                    foreach (Control tempC in deQueueControl.Controls)
                    {
                        if (String.IsNullOrEmpty(tagName) || String.Compare(tagName, deQueueControl.GetType().Name.ToString(), false) == 0)
                            controls.Add(tempC);
                        if (tempC.HasControls()) controlQueue.Enqueue(tempC);
                    }
                }
            }
            return controls;
        }

        /// <summary>
        /// 获取所有的子控件
        /// </summary>
        /// <param name="c">父控件对象</param>
        /// <param name="controlTypeList">获取的子控件类型（指定或者某一类的子控件）</param>
        public static List<Control> GetAllChildControls(Control c, List<Type> controlTypeList = null)
        {
            List<Control> controls = new List<Control>();
            if (c != null)
            {
                Queue<Control> controlQueue = new Queue<Control>();
                //if (c.HasControls())
                //{
                //    //首先进队列
                //    foreach (Control tempC in c.Controls)
                //        controlQueue.Enqueue(tempC);
                //}
                controlQueue.Enqueue(c);
                while (controlQueue.Count > 0)
                {
                    Control deQueueControl = controlQueue.Dequeue();
                    //加入列表中
                    if (controlTypeList == null || MyEnumerable.Contains<Type>(controlTypeList, deQueueControl.GetType()))
                        controls.Add(deQueueControl);
                    //循环所有的子控件，加入列表中，并判断该控件是否还有子控件，有的话继续加入列表中
                    foreach (Control tempC in deQueueControl.Controls)
                    {
                        if (controlTypeList == null || MyEnumerable.Contains<Type>(controlTypeList, tempC.GetType()))
                            controls.Add(tempC);
                        if (tempC.HasControls()) controlQueue.Enqueue(tempC);
                    }
                }
            }
            return controls;
        }

        /// <summary> 
        /// 获取HtmlForm对象中某个域的值,如果字段不存在,返回空字符串 
        /// </summary> 
        /// <param name="oForm"></param> 
        /// <param name="sItemName"></param> 
        /// <returns></returns> 
        public static String GetItemValue(HtmlForm oForm, string sItemName)
        {
            Control oControl = oForm.FindControl(sItemName);
            if (oControl == null)
            {
                return "";
            }

            if (oControl is TextBox)
            {
                return ((TextBox)oControl).Text;
            }
            else if (oControl is HiddenField)
            {
                return ((HiddenField)oControl).Value;
            }
            else if (oControl is ListBox)
            {
                return ((ListBox)oControl).Text;
            }
            else if (oControl is CheckBox)
            {
                //对于一个CheckBox,请用CheckedValue来指定选中后代表的值, 用NotCheckedValue来存放没选中时代表的值
                CheckBox oCheckBox = (CheckBox)oControl;
                if (oCheckBox.Checked)
                {
                    if (oCheckBox.Attributes["CheckedValue"] != null)
                    {
                        return oCheckBox.Attributes["CheckedValue"];
                    }
                    else
                    {
                        if (oCheckBox.Attributes["Value"] != null)
                        {
                            return oCheckBox.Attributes["Value"];
                        }
                        else
                        {
                            return "1";
                        }
                    }
                }
                else
                {
                    if (oCheckBox.Attributes["NotCheckedValue"] != null)
                    {
                        return oCheckBox.Attributes["NotCheckedValue"];
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            else if (oControl is CheckBoxList)
            {
                string sValue = string.Empty;
                CheckBoxList oCheckBoxList = (CheckBoxList)oControl;
                foreach (ListItem oItem in oCheckBoxList.Items)
                {
                    if (oItem.Selected)
                    {
                        sValue = MyString.Connect(sValue, oItem.Value, ",");
                    }
                }
                return sValue;
            }
            else if (oControl is Label)
            {
                return ((Label)oControl).Text;
            }
            else if (oControl is Literal)
            {
                return ((Literal)oControl).Text;
            }
            else if (oControl is DropDownList)
            {
                return ((DropDownList)oControl).Text;
            }
            else if (oControl is RadioButtonList)
            {
                return ((RadioButtonList)oControl).SelectedValue;
            }
            else if (oControl is HtmlInputControl)
            {
                return ((HtmlInputControl)oControl).Value;
            }
            else if (oControl is HtmlInputHidden)
            {
                return ((HtmlInputHidden)oControl).Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取HtmlForm对象中某个域的值,如果字段不存在,返回空字符串
        /// </summary>
        /// <param name="sItemName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetItemValue(string sItemName)
        {
            Page oPage = (Page)System.Web.HttpContext.Current.Handler;
            HtmlForm form = oPage.Form;
            return MyPage.GetItemValue(form, sItemName);
        }

        /// <summary> 
        /// 在浏览器端弹出消息框 
        /// </summary> 
        /// <param name="sMsg"></param> 
        public static void Alert(String sMsg)
        {
            if (!String.IsNullOrEmpty(sMsg))
                HttpContext.Current.Response.Write("<Script>alert(\"" + sMsg.Replace("\"", "\\\"") + "\");</Script>");
        }

        /// <summary>
        /// 在客户端显示弹出对话框
        /// </summary>
        /// <param name="webPage"></param>
        /// <param name="msg">要显示的信息</param>
#if NET20
        public static void Alert(Page webPage, String msg)
#else
        public static void Alert(this Page webPage, String msg)
#endif
        {
            if (webPage == null) webPage = HttpContext.Current.Handler as Page;
            if (webPage != null) webPage.ClientScript.RegisterStartupScript(webPage.GetType(), "alert", "alert('" + msg + "');", true);
        }

        /// <summary>
        /// 在客户端执行一段脚本
        /// </summary>
        /// <param name="webPage"></param>
        /// <param name="js">要执行的命令</param>
#if NET20
        public static void ExecJs(Page webPage, String js)
#else
        public static void ExecJs(this Page webPage, String js)
#endif
        {
            if (webPage == null) webPage = HttpContext.Current.Handler as Page;
            if (webPage != null) webPage.ClientScript.RegisterStartupScript(webPage.GetType(), "ExecJs", "<script language=\"javascript\">" + js + ";</script>");
        }

        /// <summary>
        /// 在客户端执行一段脚本
        /// </summary>
        /// <param name="js">要执行的命令</param>
        public static void ExecJs(String js)
        {
            Page webPage = HttpContext.Current.Handler as Page;
            if (webPage != null) webPage.ClientScript.RegisterStartupScript(webPage.GetType(), "ExecJs", "<script language=\"javascript\">" + js + ";</script>");
        }

        /// <summary>
        /// 在客户端执行一段脚本
        /// </summary>
        /// <param name="js">要执行的命令</param>
        public static void ExecJs2(String js)
        {
            HttpContext.Current.Response.Write("<script type='text/javascript'>try{" + MyString.TurnToJs(js) + "}catch(e){};</script>");
        }

        /// <summary>
        /// 在网页上输出兼容模式代码
        /// </summary>
        /// <remarks></remarks>
        public static void OpenCompatibleMode(Page webPage = null)
        {
            HttpResponse response = webPage == null ? webPage.Response : HttpContext.Current.Response;
            response.Write("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=EmulateIE7\" />");
        }

        /// <summary> 
        /// 设置Page中WebControl控件的值 
        /// </summary> 
        /// <param name="oPage"></param> 
        /// <param name="sItemName"></param> 
        /// <param name="vItemValue"></param> 
        public static void SetItemValue(System.Web.UI.Page oPage, string sItemName, object vItemValue)
        {
            Control oItem = oPage.FindControl(sItemName);
            if (oItem == null)
            {
                foreach (Control cp in oPage.Controls)
                {
                    foreach (Control ct in cp.Controls)
                    {
                        if (ct is HtmlForm)
                        {
                            foreach (Control con in ct.Controls)
                            {
                                foreach (Control c in con.Controls)
                                {
                                    if (c.ID == sItemName)
                                    {
                                        oItem = c;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (oItem == null)
            {
                foreach (Control cp in oPage.Controls)
                {
                    foreach (Control ct in cp.Controls)
                    {
                        if (ct is ContentPlaceHolder)
                        {
                            foreach (Control con in ct.Controls)
                            {
                                foreach (Control c in con.Controls)
                                {
                                    if (c.ID == sItemName)
                                    {
                                        oItem = c;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (oItem == null)
            {
                return;
            }

            if (vItemValue == null)
            {
                vItemValue = "";
            }

            if (oItem is TextBox)
            {
                TextBox oTextBox = (TextBox)oItem;
                if (oTextBox.TextMode == TextBoxMode.Password)
                {
                    oTextBox.Attributes.Add("Value", vItemValue.ToString());
                }
                else
                {
                    oTextBox.Text = vItemValue.ToString();
                }
            }
            else if (oItem is HiddenField)
            {
                ((HiddenField)oItem).Value = vItemValue.ToString();
            }
            else if (oItem is ListBox)
            {
                ((ListBox)oItem).Text = vItemValue.ToString();
            }
            else if (oItem is CheckBox)
            {
                CheckBox oCheckBox = (CheckBox)oItem;
                if (oCheckBox.Attributes["CheckedValue"] != null)
                {
                    //如果CheckBox有CheckedValue属性, 则当vItemValue等于CheckedValue时, 状态为选中
                    oCheckBox.Attributes["Value"] = vItemValue.ToString();
                    if (vItemValue.ToString() == oCheckBox.Attributes["CheckedValue"])
                    {
                        oCheckBox.Checked = true;
                    }
                    else
                    {
                        oCheckBox.Checked = false;
                    }
                }
                else
                {
                    //如果CheckBox没有CheckedValue属性,则当值等于0的时候,状态为不选中,其他值会使得Checkbox选中
                    if (Convert.ToInt32(vItemValue) == 0)
                    {
                        oCheckBox.Checked = false;
                    }
                    else
                    {
                        oCheckBox.Checked = true;
                    }
                }
            }
            else if (oItem is CheckBoxList)
            {
                CheckBoxList oCheckBoxList = (CheckBoxList)oItem;
                string sItemValue = vItemValue.ToString();
                foreach (ListItem oListItem in oCheckBoxList.Items)
                {
                    if (MyString.ContainsEx(sItemValue, oListItem.Value, ",", true))
                    {
                        oListItem.Selected = true;
                    }
                    else
                    {
                        oListItem.Selected = false;
                    }
                }
            }
            else if (oItem is Label)
            {
                ((Label)oItem).Text = vItemValue.ToString();
            }
            else if (oItem is Literal)
            {
                ((Literal)oItem).Text = vItemValue.ToString();
            }
            else if (oItem is DropDownList)
            {
                ((DropDownList)oItem).SelectedIndex = ((DropDownList)oItem).Items.IndexOf(((DropDownList)oItem).Items.FindByValue(vItemValue.ToString()));
            }
            else if (oItem is RadioButtonList)
            {
                ((RadioButtonList)oItem).SelectedValue = Convert.ToString(vItemValue);
            }
            else if (oItem is HtmlInputControl)
            {
                ((HtmlInputControl)oItem).Value = vItemValue.ToString();
            }
            else if (oItem is HtmlInputHidden)
            {
                ((HtmlInputHidden)oItem).Value = vItemValue.ToString();
            }
        }

        /// <summary>
        /// 通过ID获取下级控件(递归)
        /// </summary>
        /// <param name="sControlID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Control GetSubControlByID(Control oControl, string sControlID)
        {
            Control oReturn = oControl.FindControl(sControlID);
            if (oReturn != null) return oReturn;

            foreach (Control cl in oControl.Controls)
            {
                if (cl.HasControls())
                {
                    oReturn = GetSubControlByID(cl, sControlID);
                    if (oReturn != null)
                        return oReturn;
                }
                else
                {
                    if (cl.ID != null)
                    {
                        if (MyString.StrEqual(cl.ID, sControlID, true))
                            return cl;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 通过ID获取内部控件(递归)
        /// </summary>
        /// <param name="oForm"></param>
        /// <param name="sControlID"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Control GetSubControlByID(ref HtmlForm oForm, string sControlID)
        {
            return GetSubControlByID(oForm, sControlID);
        }

        /// <summary> 
        /// 判断Form(类型:HtmlForm)是否含有指定字段 
        /// </summary> 
        /// <param name="oForm"></param> 
        /// <param name="sItemName"></param> 
        /// <returns></returns> 
        public static bool HasItem(HtmlForm oForm, string sItemName)
        {
            Control oControl = GetSubControlByID(ref oForm, sItemName);
            return oControl != null;
        }

        /// <summary> 
        /// 判断Form(类型:NameValueCollection)是否含有指定字段 
        /// </summary> 
        /// <param name="oForm"></param> 
        /// <param name="sItemName"></param> 
        /// <returns></returns> 
        public static bool HasItem(NameValueCollection oForm, string sItemName)
        {
            foreach (object key in oForm.Keys)
            {
                if (key == null)
                    continue;
                if (MyString.StrEqual(key.ToString(), sItemName, true))
                    return true;
            }
            return false;
        }

        /// <summary> 
        /// 从Form中获取指定字段的名称, 如果字段不存在,返回空字符串 
        /// </summary> 
        /// <param name="oForm"></param> 
        /// <param name="sItemName"></param> 
        /// <returns></returns> 
        public static string GetItemValue(NameValueCollection oForm, string sItemName)
        {
            string sValue = oForm[sItemName];
            if (sValue == null) return "";
            else return sValue;
        }

        #endregion
    }
}
