using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
//lijian添加的命名空间
using DreamCube.Foundation.XmlHelper;
using DreamCube.Foundation.Basic.Utility;

/// <summary> 
/// 对象数据来源类型 
/// </summary> 
public enum DataSourceType
{
	DS_UNKNOWN = 0,
	DS_RDB = 1,
	DS_NameValueCollectionForm = 2,
	DS_HtmlForm = 3,
	DS_XML = 4,
	DS_MEMORY = 9
	//数据来自手动设置值
}

/// <summary> 
/// ObjectX 的摘要说明 
/// </summary> 
public class ObjectX
{
    //指明每个数据库表作为标志的字段名, 该字段值要写到Me.RdbCode中 
    public string PrimKey = "CODE";
    //标志域的数值类型(CTYPE_NUM 或者是 CTYPE_STR) 
    public Type KeyType;
    //存放Rdb记录的标志字段值 
    public object KeyValue;
    //对象获取数据的位置 
    public DataSourceType DataSource;
    //联接到关第库的联接对象 
    public RdbConnMgr oConn;
    //数据记录对象 
    public RdbRecord oRdbRec;
    //需要从文档中读取指定前缀的域值,默认与表名相同 
    public string m_Prefix;
    //最近发生的一个错误描述
    public string m_sLastError;
    //每个对象的数据存储表的表名 
    public string m_Table;
    //序列号
    public string SequenceName = "";
    //是否禁用事务 
    private bool RdbTransDisabled;
    private bool m_bHasRemoveMsg;
    //对象特别指定本对象需要引用到的列名(考虑到有些表的类型无效导致ROW-00004错误)
    public string m_TableColumns = "*";
    //对象是否初始化成功 
    protected bool m_IsValid;
    //数据来源MyXmlNode对象
    protected MyXmlNode XmlNode;
    //数据来源FORM对象( NameValueCollection或者是HtmlForm对象) 
    protected object oForm;
    //对象的属性值 
    public ArrayList Items;
    //是否移除到回收站，true：移除到回收站；false：删除的时候直接删除了（lijian）
    public bool RemoveToRecycle = true;

    /// <summary> 
    /// 构造函数 
    /// </summary> 
    public ObjectX()
    {
        this.DataSource = DataSourceType.DS_UNKNOWN;
        this.KeyType = Type.GetType("System.Int64");
        //Me.KeyValue = "0"
        this.KeyValue = "";
        this.m_IsValid = false;
        this.RdbTransDisabled = false;
        this.m_bHasRemoveMsg = false;
    }

    /// <summary>
    /// 转化为字符串类型的KeyValue，如果KeyValue=DbNull，则KeyValueStr返回String.Empty
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string KeyValueStr
    {
        get
        {
            string sValue = string.Empty;
            if (this.KeyValue.GetType().Equals(typeof(System.DBNull)) == false)
            {
                sValue = this.KeyValue.ToString();
            }
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = Convert.ToString(this.GetItemValue(this.PrimKey));
            }
            return sValue;
        }
    }

    /// <summary>
    /// 记录创建者
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string Creator
    {
        get
        {
            return Convert.ToString(this.GetItemValue("CREATEUSER"));
        }
    }

    /// <summary>
    /// 记录创建时间
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string CreateTime
    {
        get
        {
            return Convert.ToString(this.GetItemValue("CREATETIME"));
        }
    }

    /// <summary>
    /// 最近修改者
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string LastModifyUser
    {
        get
        {
            return Convert.ToString(this.GetItemValue("LASTMODIFYUSER"));
        }
    }

    /// <summary>
    /// 最近修改时间
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string LastModifyTime
    {
        get
        {
            return Convert.ToString(this.GetItemValue("LASTMODIFYTIME"));
        }
    }

    /// <summary>
    /// 功能: 获取本类的名称
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string MyName
    {
        get
        {
            return this.GetType().Name;
        }
    }
    /// <summary>
    /// 如果在自定义的RemoveFromRdb()函数中在删除失败的时候有提示信息，要重载此属性，并且返回true
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public Boolean HasRemoveMsg
    {
        get
        {
            return m_bHasRemoveMsg;
        }
        set
        {
            m_bHasRemoveMsg = value;
        }
    }



    /// <summary>
    /// 功能: 获取本类的全名
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public string MyFullName
    {
        get
        {
            return this.GetType().FullName;
        }
    }

    /// <summary> 
    /// 初始化对象(数据来源是RDB数据库) 
    /// </summary> 
    /// <param name="vCodeValue">主键值</param> 
    public void InitRdbObject(object vCodeValue, RdbConnMgr oConnX = null)
    {
        try
        {
            this.KeyValue = vCodeValue;
            this.DataSource = DataSourceType.DS_RDB;
            this.m_IsValid = this.InitObject(oConnX);
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitRdbObject()发生错误:" + e.Message);
        }
    }

    /// <summary>
    /// 初始化对象(数据来源是RDB数据库) 
    /// </summary>
    /// <param name="oRdbRec">该对象的数据库记录</param>
    /// <param name="oConnX"></param>
    /// <remarks></remarks>
    public void InitRdbObject(RdbRecord oRdbRec, RdbConnMgr oConnX = null)
    {
        try
        {
            this.KeyValue = oRdbRec.GetField(this.PrimKey).Value;
            this.oRdbRec = oRdbRec;
            this.DataSource = DataSourceType.DS_RDB;
            this.m_IsValid = this.InitObject(oConnX);
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitRdbObject()发生错误:" + e.Message);
        }
    }

    /// <summary> 
    /// 初始化对象(数据来源是Form(类型: NameValueCollection)) 
    /// </summary> 
    /// <param name="oFormX"></param> 
    public void InitFormObject(NameValueCollection oFormX, string sPrefix)
    {
        try
        {
            this.m_IsValid = false;
            this.DataSource = DataSourceType.DS_NameValueCollectionForm;
            this.oForm = oFormX;
            this.Prefix = sPrefix;
            if (!MyPage.HasItem(oFormX, this.Prefix + this.PrimKey))
            {
                return;
            }
            this.KeyValue = MyPage.GetItemValue(oFormX, this.Prefix + this.PrimKey);
            if (this.InitObject())
            {
                this.m_IsValid = this.LoadDataFromForm();
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitFormObject()发生错误:" + e.Message);
        }
    }

    /// <summary> 
    /// 初始化对象(数据来源是Form(类型: HtmlForm)) 
    /// </summary> 
    /// <param name="oFormX"></param> 
    /// <param name="sPrefix"></param> 
    public void InitFormObject(HtmlForm oFormX, string sPrefix)
    {
        try
        {
            this.m_IsValid = false;
            this.DataSource = DataSourceType.DS_HtmlForm;
            this.oForm = oFormX;
            this.Prefix = sPrefix;
            if (!MyPage.HasItem(oFormX, this.Prefix + this.PrimKey))
            {
                return;
            }
            this.KeyValue = MyPage.GetItemValue(oFormX, this.Prefix + this.PrimKey);
            if (this.InitObject())
            {
                this.m_IsValid = this.LoadDataFromForm();
                if (this.m_IsValid)
                {
                    this.KeyValue = this.GetItemValue(this.PrimKey);
                }
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitFormObject()发生错误:" + e.Message);
        }
    }

    /// <summary> 
    /// 从Form对象中加载对象数据 
    /// </summary> 
    /// <returns></returns> 
    private bool LoadDataFromForm()
    {
        try
        {
            RdbField oField;
            if (this.DataSource == DataSourceType.DS_NameValueCollectionForm)
            {
                NameValueCollection oForm1 = (NameValueCollection)this.oForm;
                for (int i = 0; i <= this.Items.Count - 1; i++)
                {
                    if (this.Items[i] == null)
                    {
                        continue;
                    }
                    oField = (RdbField)(this.Items[i]);
                    if (MyPage.HasItem(oForm1, this.Prefix + oField.Name))
                    {
                        ((RdbField)this.Items[i]).Value = MyPage.GetItemValue(oForm1, this.Prefix + oField.Name);
                    }
                    else if (MyPage.HasItem(oForm1, oField.Name))
                    {
                        ((RdbField)this.Items[i]).Value = MyPage.GetItemValue(oForm1, oField.Name);
                    }
                }
            }
            else
            {
                HtmlForm oForm2 = (HtmlForm)this.oForm;
                for (int i = 0; i <= this.Items.Count - 1; i++)
                {
                    if (this.Items[i] == null)
                    {
                        continue;
                    }
                    oField = (RdbField)(this.Items[i]);
                    if (MyPage.HasItem(oForm2, this.Prefix + oField.Name))
                    {
                        ((RdbField)this.Items[i]).Value = MyPage.GetItemValue(oForm2, this.Prefix + oField.Name);
                        //If oField.IsBinaryField Then
                        //    oField.Value = System.Text.Encoding.UTF8.GetBytes(MyPage.GetItemValue(oForm2, Me.Prefix + oField.Name))
                        //    Dim ss As Object = oField.Value
                        //    Dim s8s As System.Type = oField.DataType
                        //End If
                    }
                    else if (MyPage.HasItem(oForm2, oField.Name))
                    {
                        ((RdbField)this.Items[i]).Value = MyPage.GetItemValue(oForm2, oField.Name);
                    }
                }
            }

            return true;
        }
        catch (Exception e)
        {
            MyLog.MakeLog("LoadDataFromForm()发生错误:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// 初始化对象(数据来源是XML文档对象)
    /// </summary>
    /// <param name="oXmlDoc">xml文档对象</param>
    /// <param name="sXmlNodeID">对象数据所在节点的ID</param>
    /// <remarks></remarks>
    public void InitXmlObject(ref MyXmlDoc oXmlDoc, string sXmlNodeID)
    {
        MyXmlNode node = oXmlDoc.GetNodeByID(sXmlNodeID);
        InitXmlObject(ref node);
    }

    /// <summary>
    /// 初始化对象(数据来源是XML字符串)
    /// </summary>
    /// <param name="sXmlString"></param>
    /// <remarks></remarks>

    public void InitXmlObject(string sXmlString)
    {
        try
        {
            MyXmlDoc oXmlDoc = MyXmlDocMgr.LoadXmlDoc(sXmlString, false);
            if (oXmlDoc != null)
            {
                MyXmlNode node = oXmlDoc.GetFirstNode();
                InitXmlObject(ref node);
            }
        }
        catch (Exception ex)
        {
            MyLog.MakeLog("InitXmlObject()发生错误:" + ex.Message);
        }

    }

    /// <summary>
    /// 初始化对象(数据来源是XML节点对象)
    /// </summary>
    /// <param name="oXmlNode"></param>
    /// <remarks></remarks>
    public void InitXmlObject(ref MyXmlNode oXmlNode)
    {
        try
        {
            this.m_IsValid = false;
            this.DataSource = DataSourceType.DS_XML;
            this.XmlNode = oXmlNode;

            MyXmlNode oKeyNode = this.XmlNode.GetSubNodeByID(this.PrimKey);
            if (oKeyNode != null)
            {
                this.KeyValue = oKeyNode.InnerText;
            }

            if (this.InitObject())
            {
                this.m_IsValid = this.LoadDataFromXML();
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitXmlObject()发生错误:" + e.Message);
        }
    }

    /// <summary>
    /// 从XML文档中加载对象数据
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    private bool LoadDataFromXML()
    {
        try
        {
            RdbField oField;
            MyXmlNode oXmlNode;
            for (int i = 0; i <= this.Items.Count - 1; i++)
            {
                if (this.Items[i] == null)
                {
                    continue;
                }
                oField = (RdbField)(this.Items[i]);
                oXmlNode = this.XmlNode.GetSubNodeByID(oField.Name);
                if (oXmlNode != null)
                {
                    ((RdbField)this.Items[i]).Value = oXmlNode.InnerText;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MyLog.MakeLog("LoadDataFromXML()发生错误:" + ex.Message);
            return false;
        }

    }

    /// <summary>
    /// 初始化内存对象
    /// </summary>
    /// <remarks></remarks>
    public void InitMemoryObject(RdbConnMgr oConnX = null)
    {
        try
        {
            this.m_IsValid = false;
            this.DataSource = DataSourceType.DS_MEMORY;
            this.m_IsValid = this.InitObject(oConnX);
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitMemoryObject()发生错误:" + e.Message);
        }
    }

    /// <summary> 
    /// 初始化对象 
    /// </summary> 
    private bool InitObject(RdbConnMgr oConnX = null)
    {
        try
        {
            if (this.oConn == null)
            {
                if (oConnX == null)
                {
                    this.oConn = this.GetRdbConnMgrObject();
                }
                else
                {
                    this.oConn = oConnX;
                }
            }

            if (this.oRdbRec == null)
            {
                this.oRdbRec = this.GetRdbRecord(false);
                if (this.oRdbRec == null)
                {
                    this.oRdbRec = this.oConn.CreateNewRecord(this.RdbTable);
                }
            }

            //强行指定Rdb记录的主键(防止有些表一个主键也没有,且第一个字段不能确保唯一性)
            this.oRdbRec.PrimaryKey = this.PrimKey;

            return this.InitItems();
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitObject()发生错误:" + e.Message);
            MyLog.MakeLog("RdbTable=" + this.RdbTable);
            return false;
        }
    }

    /// <summary>
    /// 获取数据库连接对象。注意：子对象可以重写该方法来重新指定另外的数据库作为对象数据来源
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    public virtual RdbConnMgr GetRdbConnMgrObject()
    {
        return RdbConnHelper.CreateRdbConnMgr();
    }

    /// <summary> 
    /// 初始化Items 
    /// </summary> 
    private bool InitItems()
    {
        try
        {
            if (this.oRdbRec == null)
            {
                return false;
            }

            if (this.Items == null)
            {
                this.Items = new ArrayList(this.oRdbRec.FieldCount);
            }
            else
            {
                this.Items.Clear();
            }

            RdbField oField;
            for (int i = 0; i <= this.oRdbRec.FieldCount - 1; i++)
            {
                oField = this.oRdbRec.GetField(i);
                this.Items.Add(oField);
                if (MyString.StrEqual(oField.Name, this.PrimKey))
                {
                    this.KeyValue = oField.Value;
                    this.KeyType = oField.DataType;
                }
            }
            return true;
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitItems()发生错误:" + e.Message);
            return false;
        }
    }

    /// <summary> 
    /// 从RDB数据源获取本对象的数据 
    /// </summary> 
    /// <param name="bReadOnly"></param> 
    /// <returns></returns> 
    private RdbRecord GetRdbRecord(bool bReadOnly)
    {
        return this.oConn.GetRecord(this.GetQuerySQL(), bReadOnly, this.PrimKey);
    }

    /// <summary> 
    /// 获取得到本记录数据的SQL查询语句 
    /// </summary> 
    /// <returns></returns> 
    public string GetQuerySQL()
    {
        string sCodeValue;
        string sSQL;
        if (this.IsCodeValid)
        {
            sCodeValue = this.KeyValue.ToString();
        }
        else
        {
            sCodeValue = "";
        }
        //lijian更改逻辑，当没有传入主键值的时候，应该是创建一个内存对象（数据库没有对应的记录），实际上就是需要表架构信息而已
        if (string.IsNullOrEmpty(sCodeValue))
        {
            sSQL = "SELECT " + m_TableColumns + " FROM " + this.RdbTable + " WHERE 1=0";
        }
        else
        {
            sSQL = "SELECT " + m_TableColumns + " FROM " + this.RdbTable + " WHERE " + this.PrimKey + "=" + this.oConn.FormatSqlValue(sCodeValue, this.oConn.GetFieldType(this.RdbTable, this.PrimKey));
        }
        return sSQL;
    }

    /// <summary> 
    /// 获取字段值 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <returns></returns> 
    public object GetItemValue(string sItemName)
    {
        if (this.IsValid)
        {
            RdbField oField = this.oRdbRec.GetField(sItemName);
            if (oField == null)
            {
                return "";
            }
            else
            {
                if (oField.Value == System.DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return oField.Value;
                }
            }
        }
        else
        {
            return "";
        }
    }

    /// <summary> 
    /// 获取字段的默认值 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <returns></returns> 
    public object GetItemDefaultValue(string sItemName)
    {
        if (this.IsValid)
        {
            RdbField oField = this.oRdbRec.GetField(sItemName);
            if (oField == null)
            {
                return "";
            }
            else
            {
                if (oField.DefaultValue == System.DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return oField.DefaultValue;
                }
            }
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 保存指定字段
    /// </summary>
    /// <param name="sItemName"></param>
    /// <param name="sValue"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public bool SaveItemValue(string sItemName, object sValue)
    {
        try
        {
            //当字符串的长度超过2000时,调用Update方法进行更新,会出错
            if (sValue.GetType().ToString() == "System.String" && sValue.ToString().Length > 2000)
            {
                this.SetItemValue(sItemName, sValue);
                this.SaveToRDB();
            }
            else
            {
                string sWhere = string.Format("{0}={1}", this.PrimKey, this.FormatSqlValue(this.PrimKey, this.GetItemValue(this.PrimKey)));
                if (this.oConn.IsRecordExist(this.RdbTable, sWhere, ""))
                {
                    string sSql = string.Format("update {0} set {1} ={2}", this.RdbTable, sItemName, this.FormatSqlValue(sItemName, sValue));
                    if (sWhere != "")
                    {
                        sSql += " WHERE " + sWhere;
                    }
                    return this.oConn.ExecSQL(sSql);
                }
                else
                {
                    return false;
                }
            }
        }
        finally
        {
            //调用SetItemValue,确保缓存中的值是最新的值
            this.SetItemValue(sItemName, sValue);
        }
        return false;
    }

    /// <summary> 
    /// 设置字段值 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <param name="vItemValue"></param> 
    public void SetItemValue(string sItemName, object vItemValue)
    {
        if (this.IsValid)
        {
            RdbField oField = this.oRdbRec.GetField(sItemName);

            if (oField != null)
            {
                if (vItemValue == null)
                {
                    vItemValue = DBNull.Value;
                }

                oField.Value = vItemValue;
            }
        }
    }

    /// <summary>
    /// 格式化SQL查询语句的变量 
    /// </summary>
    /// <param name="sFieldName"></param>
    /// <param name="vFieldValue"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected string FormatSqlValue(string sFieldName, object vFieldValue)
    {
        RdbField oField = this.oRdbRec.GetField(sFieldName);
        if (oField != null)
        {
            return this.oConn.FormatSqlValue(vFieldValue, oField.DataType);
        }
        else
        {
            MyLog.MakeLog("ObjectX.FormatSqlValue() 出错,找不到字段:" + sFieldName);
        }
        return "";
    }
    /// <summary> 
    /// 判断对象是否有指定字段 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <returns></returns> 
    public bool HasItem(string sItemName)
    {
        RdbField oField = this.oRdbRec.GetField(sItemName);
        if (oField == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary> 
    /// 判断本对象是否存在于数据库 
    /// </summary> 
    /// <returns></returns> 
    public bool IsExistedInRDB()
    {
        try
        {
            if (!this.IsValid || !this.IsCodeValid)
            {
                return false;
            }

            RdbRecord oRecord = this.oConn.GetRecord(this.GetQuerySQL(), true);
            if (oRecord != null)
            {
                this.SetItemValue(this.PrimKey, this.KeyValue);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("IsExistedInRDB()发生错误:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// lijian增加，设置对象的拼音字段
    /// </summary>
    /// <remarks></remarks>
    private void SetPYColumnValue()
    {
        try
        {
            RdbField[] pyRdbFiels = this.oRdbRec.GetFieldsStartWith("AUTOPY_", StringComparison.CurrentCultureIgnoreCase);
            if (pyRdbFiels != null)
            {
                for (int index = 0; index <= pyRdbFiels.Length - 1; index++)
                {
                    string sColumnName = MyString.Right(pyRdbFiels[index].Name, "AUTOPY_", true, "");
                    object sourceColumnValue = this.GetItemValue(sColumnName);
                    if (string.IsNullOrEmpty(Convert.ToString(sourceColumnValue)))
                    {
                        continue;
                    }
                    //保存拼音数据
                    PinyinSuggestion.SavePinyin(this.RdbTable, sColumnName, Convert.ToString(this.KeyValue), Convert.ToString(sourceColumnValue), Convert.ToString(this.PrimKey));
                }
            }
        }
        catch (Exception ex)
        {
            MyLog.MakeLog("SetPYColumnValue()发生错误:" + ex.Message);
        }
    }

    /// <summary> 
    /// 将对象数据保存到RDB数据库 
    /// </summary> 
    /// <returns></returns> 
    public virtual string SaveToRDB()
    {

        RdbField oField;
        try
        {
            if (!this.IsValid)
            {
                return "";
            }
            if (this.oRdbRec == null)
            {
                MyLog.MakeLog("SaveToRdb()执行失败! Me.oRdbRec对象无效");
                return "";
            }

            bool bIsNew;
            string sSQL;
            string sRid;
            Type tRidType;
            RdbRecord oRecord;

            sRid = "";
            tRidType = Type.GetType("string");
            //lijian增加
            SetPYColumnValue();

            if (this.IsExistedInRDB())
            {
                bIsNew = false;

                //如果当前不是DS_RDB对象,则其oRdbRec是通过AddNew得来的,需要替换为RDB的已存在记录,否则保存后会生成新记录 
                if (this.DataSource != DataSourceType.DS_RDB)
                {
                    oRecord = this.oRdbRec;
                    this.oRdbRec = this.GetRdbRecord(false);
                    for (int i = 0; i <= oRecord.FieldCount - 1; i++)
                    {
                        oField = oRecord.GetField(i);
                        if ((oField.Name.ToLower() != "rid"))
                        {
                            this.SetItemValue(oField.Name, oField.Value);
                        }
                    }
                }

                //设置最近修改时间 
                if (this.HasItem("LastModifyTime"))
                {
                    this.SetItemValue("LastModifyTime", MyDatetime.NowTimeyyyyMMddHHmmss);
                }

                //设置最近修改人 
                if (this.HasItem("LastModifyUser"))
                {
                    this.SetItemValue("LastModifyUser", CurrentSession.CurrentUserName);
                }

                //设置创建日期, 创建人不可修改 
                this.SetReadOnlyItem("CreateTime", true);
                this.SetReadOnlyItem("CreateUser", true);

                //如果rid值为空,则给其赋值, 否则保存会出错
                if (this.HasItem("rid") && string.IsNullOrEmpty(this.GetItemValue("rid").ToString()))
                {
                    this.SetItemValue("rid", MyGuid.GetNewGuid());
                }
            }
            else
            {
                bIsNew = true;

                //设置创建人 
                if (this.HasItem("CreateUser") && string.IsNullOrEmpty(Convert.ToString(this.GetItemValue("CreateUser"))))
                {
                    this.SetItemValue("CreateUser", CurrentSession.CurrentUserName);
                }

                //设置创建时间 
                if (this.HasItem("CreateTime"))
                {
                    this.SetItemValue("CreateTime", MyDatetime.NowTimeyyyyMMdd);
                }

                //设置最近修改人 
                if (this.HasItem("LastModifyUser"))
                {
                    this.SetItemValue("LastModifyUser", CurrentSession.CurrentUserName);
                }

                //设置最近修改时间 
                if (this.HasItem("LastModifyTime"))
                {
                    this.SetItemValue("LastModifyTime", MyDatetime.NowTimeyyyyMMdd);
                }

                if (this.HasItem("rid"))
                {
                    sRid = MyGuid.GetNewGuid(MyGuid.GuidStringFormatType.Normal);
                    this.SetItemValue("rid", sRid);
                    oField = this.oRdbRec.GetField("rid");
                    tRidType = oField.DataType;
                }
                else
                {
                    if (this.KeyValue == DBNull.Value)
                    {
                        this.KeyValue = "";
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(this.KeyValue)))
                    {
                        //有些对象的主键值是没有用自动编号的, 在保存之前就预先设置好了主键值, 如:使用32位的的UNID
                        this.KeyValue = this.GetItemValue(this.PrimKey);
                    }

                    string sTempCode = Convert.ToString(this.KeyValue);
                    if (string.IsNullOrEmpty(sTempCode))
                    {
                        sTempCode = this.oConn.GetNewRecordCodeValue(this.RdbTable, this.PrimKey, this.SequenceName).ToString();
                    }
                    if (sTempCode != "")
                    {
                        this.KeyValue = sTempCode;
                        this.SetItemValue(this.PrimKey, sTempCode);
                    }
                    else
                    {
                        //非自动编号, 保存前又没有赋值, 则临时赋值
                        if (string.IsNullOrEmpty(this.KeyValueStr))
                        {
                            //lijian增加逻辑判断，因为有些表是没有code字段的，如果获取不到codefield，则不要设置值了
                            RdbField oTempField = this.oRdbRec.GetField(this.PrimKey);

                            if (oTempField != null)
                            {
                                int iLength = oTempField.MaxLength;
                                if (iLength >= 32 && oTempField.IsStringField)
                                {
                                    sTempCode = MyGuid.GetNewGuid(MyGuid.GuidStringFormatType.ToUpper);
                                }
                                else
                                {
                                    sTempCode = MyRand.NextString_Number(iLength);
                                }
                                this.KeyValue = sTempCode;
                                this.SetItemValue(this.PrimKey, sTempCode);
                            }
                        }
                    }
                }

            }

            //保存数据 
            if (!this.oRdbRec.Save())
            {
                MyLog.MakeLog("SaveToRdb()错误: RdbRecord Save()失败!");
                return string.Empty;
            }

            //对于新记录: 如果记录的标志是自动编号的(根据是否有rid字段来判断),则获取新记录标识 
            if (bIsNew && this.HasItem("rid"))
            {
                sSQL = "SELECT " + this.PrimKey + " FROM " + this.RdbTable + " WHERE rid=" + oConn.FormatSqlValue(sRid, tRidType);
                oRecord = this.oConn.GetRecord(sSQL, true);
                this.KeyValue = oRecord.GetField(this.PrimKey).Value;
                this.SetItemValue(this.PrimKey, this.KeyValue);
            }

            if (bIsNew)
            {
                //往Web端回写CODE字段值,免得Web端再次点保存会另存为新记录
                if (this.DataSource == DataSourceType.DS_HtmlForm || this.DataSource == DataSourceType.DS_NameValueCollectionForm)
                {
                    MyPage.SetWebFieldValue("parent", this.Prefix + this.PrimKey, this.KeyValue.ToString());
                }
            }

            if (this.DataSource == DataSourceType.DS_MEMORY)
            {
                this.DataSource = DataSourceType.DS_RDB;
            }

            return this.KeyValue.ToString();
        }
        catch (Exception e)
        {
            MyLog.MakeLog("SaveToRDB()发生错误:" + e.Message);
            return "";
        }
    }

    /// <summary>
    /// 将数据保存到对象
    /// </summary>
    /// <param name="oObject"></param>
    /// <remarks></remarks>

    public void SaveToObject(ref ObjectX oObject)
    {
        if (this.Items == null || oObject == null)
        {
            return;
        }

        RdbField oField;
        for (int i = 0; i <= this.Items.Count - 1; i++)
        {
            if (this.Items[i] == null)
            {
                continue;
            }
            oField = (RdbField)this.Items[i];
            oObject.SetItemValue(oField.Name, oField.Value);
        }

    }

    /// <summary> 
    /// 将对象的数据写到ASPX页面中 
    /// </summary> 
    /// <param name="oPage">页面中对象字段名的前缀</param> 
    public virtual void WriteToPage(string sPrefix = "", System.Web.UI.Page oPage = null)
    {
        if (this.Items == null)
        {
            return;
        }
        if (oPage == null)
        {
            oPage = (System.Web.UI.Page)HttpContext.Current.Handler;
        }

        if (string.IsNullOrEmpty(sPrefix))
        {
            sPrefix = this.RdbTable;
        }

        if (sPrefix.EndsWith("_") == false)
        {
            sPrefix += "_";
        }

        RdbField oField;
        for (int i = 0; i <= this.Items.Count - 1; i++)
        {
            if (this.Items[i] == null)
            {
                continue;
            }
            oField = (RdbField)this.Items[i];
            if (oField.IsBinaryField)
            {
                //对于二进制字段,如果要Write到页面上,则认为这是一个字符串二进制字段,换成成字符串之后再写入
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, oField.ExtractAsString());
            }
            else if (oField.IsDateTimeField)
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, MyDatetime.GetDatePart(Convert.ToString(this.GetItemValue(oField.Name))));
            }
            else
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, this.GetItemValue(oField.Name));
            }
        }
    }

    /// <summary>
    /// 获取字段的旧值
    /// </summary>
    /// <param name="sParentDocUNID">父文档的DOCUNID</param>
    /// <param name="sTableName">目标数据表表名</param>
    /// <param name="sItemName">字段列名</param>
    /// <returns>返回JSON数据</returns>
    /// <remarks></remarks>
    public static string GetEditLogValue(string sParentDocUNID, string sTableName, string sItemName)
    {
        try
        {
            string sSQL = "SELECT * FROM OBJECTEDITLOG WHERE PARENTDOCUNID='{2}' AND TABLENAME='{0}' AND FIELDNAME='{1}' ORDER BY CREATETIME ASC";
            RdbConnMgr oConn = RdbConnHelper.CreateRdbConnMgr();
            RdbRecordSet oRecordSet = oConn.GetRecordSet(string.Format(sSQL, sTableName.ToUpper(), sItemName.ToUpper(), sParentDocUNID));
            if (oRecordSet != null && oRecordSet.HasRecord)
            {
                List<EditLogRecord> aEditLogList = new List<EditLogRecord>();
                EditLogRecord oTempLogRecord = null;
                for (int i = 0; i <= oRecordSet.RecordCount - 1; i++)
                {
                    RdbRecord oRecord = oRecordSet.GetRecord(i);
                    //创建修改记录对象
                    oTempLogRecord = new EditLogRecord();
                    oTempLogRecord.IsFirstValue = false;
                    oTempLogRecord.EditUser = Convert.ToString(oRecord.GetField("editUser").Value);
                    oTempLogRecord.OldValue = Convert.ToString(oRecord.GetField("oldValue").Value);
                    oTempLogRecord.NewValue = Convert.ToString(oRecord.GetField("newValue").Value);
                    oTempLogRecord.EditDate = Convert.ToString(oRecord.GetField("editDate").Value);
                    aEditLogList.Add(oTempLogRecord);
                }
                return DreamCube.Foundation.Serialization.MyJson.Serialize(aEditLogList);
            }
        }
        catch (Exception ex)
        {
            MyLog.MakeLog(ex);
        }
        return string.Empty;
    }

    /// <summary> 
    /// 将对象的数据写到ASPX页面中（把修改记录也写上）
    /// </summary> 
    /// <param name="sParentDocUNID">父文档的DOCUNID</param> 
    /// <param name="oParentControl">承载历史记录控件的父控件对象</param> 
    /// <param name="sPrefix">页面中对象字段名的前缀</param> 
    public virtual void WriteEditLogToPage(string sParentDocUNID, ref PlaceHolder oParentControl, string sPrefix = "")
    {
        if (this.Items == null)
        {
            return;
        }
        Page oPage = (System.Web.UI.Page)HttpContext.Current.Handler;

        if (string.IsNullOrEmpty(sPrefix))
        {
            sPrefix = this.RdbTable;
        }

        if (sPrefix.EndsWith("_") == false)
        {
            sPrefix += "_";
        }

        RdbField oField;
        for (int i = 0; i <= this.Items.Count - 1; i++)
        {
            if (this.Items[i] == null)
            {
                continue;
            }
            oField = (RdbField)this.Items[i];
            if (oField.IsBinaryField)
            {
                //对于二进制字段,如果要Write到页面上,则认为这是一个字符串二进制字段,换成成字符串之后再写入
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, oField.ExtractAsString());
            }
            else if (oField.IsDateTimeField)
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, MyDatetime.GetDatePart(Convert.ToString(this.GetItemValue(oField.Name))));
            }
            else
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, this.GetItemValue(oField.Name));

                //写入历史修改记录的控件
                string sOldValue = GetEditLogValue(sParentDocUNID, this.RdbTable, oField.Name);
                if (!string.IsNullOrEmpty(sOldValue))
                {
                    TextBox oTextBox = new TextBox();
                    oTextBox.Attributes.Add("id", sPrefix + oField.Name + "_EditLogData");
                    oTextBox.Attributes.Add("sysEditOldValueField", "FieldID:" + sPrefix + oField.Name);
                    oTextBox.Text = sOldValue;
                    oParentControl.Controls.Add(oTextBox);
                }
            }
        }
    }

    /// <summary> 
    /// 设置只读字段 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <param name="bSet"></param> 
    public void SetReadOnlyItem(string sItemName, bool bSet = true)
    {
        this.oRdbRec.SetReadOnlyField(sItemName, bSet);
    }

    /// <summary> 
    /// 判断是不是只读字段 
    /// </summary> 
    /// <param name="sItemName"></param> 
    public bool IsReadOnlyItem(string sItemName)
    {
        return this.oRdbRec.IsReadOnlyField(sItemName);
    }

    /// <summary> 
    /// 保存对象之后, 更新在IE段的对象的CODE字段, 防止再次保存时创建另外一个新对象 
    /// </summary> 
    private void UpdateCodeFieldInIE()
    {
        string sFieldName = this.Prefix + this.PrimKey;
        MyPage.SetWebFieldValue("parent", sFieldName, Convert.ToString(this.KeyValue));
        MyPage.SetWebFieldValue("parent", sFieldName.ToUpper(), Convert.ToString(this.KeyValue));
    }

    /// <summary> 
    /// 从RDB数据库删除本对象记录 
    /// </summary> 
    /// <returns></returns> 
    public virtual bool RemoveFromRdb()
    {
        try
        {
            if (!this.IsValid || !this.IsCodeValid)
            {
                return false;
            }

            string sSQL = "DELETE " + MyString.Right(this.GetQuerySQL(), m_TableColumns);
            //lijian增加逻辑，判断是否删除到回收站
            if (!this.RemoveToRecycle)
            {
                sSQL = "--" + sSQL;
            }
            int iAffect = 0;
            this.oConn.ExecSQL(sSQL, ref iAffect);
            if (iAffect == 0)
            {
                return false;
            }
            else
            {
                MyLog.MakeLog(string.Format("对象被删除！表：{0}；CODE={1}；删除者：{2}", this.RdbTable, this.KeyValue, CurrentSession.CurrentUserName));
                this.oRdbRec = null;
                this.DataSource = DataSourceType.DS_UNKNOWN;
                this.KeyValue = "0";
                return true;
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("RemoveFromRdb()发生错误:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// 功能: 生成对象JSON代码
    /// 作者: 刘学亮
    /// </summary>
    /// <param name="sItemName">指定只导出哪些字段,空表示导入所有字段,多个字段之间以分号隔开</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string MakeJsonCode(string sItemName = "")
    {
        if (this.oRdbRec == null)
        {
            return "";
        }
        else
        {
            return this.oRdbRec.MakeJsonCode(sItemName);
        }
    }


    /// <summary>
    /// 功能: 生成对象的XML
    /// 作者: 刘学亮
    /// </summary>
    /// <param name="sItemName">指定只导出哪些字段,空表示导入所有字段,多个字段之间以分号隔开</param>
    /// <param name="sObjectTagName">对象的Xml tagName</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string MakeXML(string sObjectTagName = "", string sItemName = "")
    {
        if (this.oRdbRec == null)
        {
            return "";
        }
        else
        {
            if (string.IsNullOrEmpty(sObjectTagName))
            {
                sObjectTagName = "ObjectX";
            }
            string sXml = string.Format("<{0} RdbTable=\"{2}\" id=\"{1}\" Prefix='{3}'>", sObjectTagName, this.KeyValue, this.RdbTable, this.Prefix);
            sXml += Microsoft.VisualBasic.Constants.vbCrLf + this.oRdbRec.MakeXML("Item", sItemName);
            sXml += Microsoft.VisualBasic.Constants.vbCrLf + "</" + sObjectTagName + ">";
            return sXml;
        }
    }

    /// <summary>
    /// 复制对象
    /// </summary>
    /// <param name="bNotSaveToDBImmediately">true=copy到新的对象，不需要保存到数据库中 false=copy到新的对象，马上保存到数据库中；默认为false</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public virtual ObjectX Copy(bool bNotSaveToDBImmediately = false)
    {
        oConn.BeginTrans();
        try
        {
            ObjectX oNewObject = ObjectHelper.CreateNewObject(this.MyName, this.RdbTable);
            foreach (RdbField oField in this.Items)
            {
                if (oField.IsPrimaryKey == false)
                {
                    oNewObject.SetItemValue(oField.Name, oField.Value);
                }
            }
            //lijian
            //有时候不需要copy的对象马上保存到数据库中的
            if (!bNotSaveToDBImmediately)
            {
                if (string.IsNullOrEmpty(oNewObject.SaveToRDB()))
                {
                    throw new Exception("对象复制失败!");
                }
            }

            oConn.CommintTrans();
            return oNewObject;
        }
        catch (Exception ex)
        {
            oConn.RollbackTrans();
            MyLog.MakeLog("ObjectX.Copy()出错:" + ex.Message + " ClassName=" + this.MyName + ", Code=" + this.KeyValueStr);
            return null;
        }
    }

    /// <summary>
    /// 复制对象 从一个表复制到另外一个表  lijian
    /// </summary>
    /// <param name="sRdbTable">把一个表的一条记录copy到另外一个表</param>
    /// <param name="sNoSameRecordByField">目标表中 某个字段不能出现重复值 例如 从A表copy内容到B表中，B表的field字段如果存在重复值，则先删除原来的记录，重新插入新值</param>
    /// <param name="bNotSaveToDBImmediately">是否马上copy到的数据保存到数据库中 默认保存（false） 如果为TRUE 则不马上保存到数据库中</param>
    /// <param name="bCopyPrimarykey">是否copy主键值 默认是（true） </param>
    /// <returns></returns>
    /// <remarks></remarks>
    public virtual ObjectX CopyTo(string sRdbTable, string sNoSameRecordByField = "", bool bNotSaveToDBImmediately = false, bool bCopyPrimarykey = true)
    {
        oConn.BeginTrans();
        try
        {
            ObjectX oNewObject = ObjectHelper.CreateNewObject(this.MyName, sRdbTable);
            foreach (RdbField oField in this.Items)
            {
                if (oField.IsPrimaryKey)
                {
                    if (!bCopyPrimarykey)
                    {
                        continue;
                    }
                }
                oNewObject.SetItemValue(oField.Name, oField.Value);
            }

            if (!string.IsNullOrEmpty(sNoSameRecordByField))
            {
                string sFieldValue = Convert.ToString(oNewObject.GetItemValue(sNoSameRecordByField));
                ObjectX oObject = ObjectHelper.GetRdbObjectEx2(sRdbTable, sFieldValue, null, sNoSameRecordByField);
                if (oObject != null)
                {
                    oObject.RemoveFromRdb();
                }
            }

            //lijian
            //有时候不需要copy的对象马上保存到数据库中的
            if (!bNotSaveToDBImmediately)
            {
                if (string.IsNullOrEmpty(oNewObject.SaveToRDB()))
                {
                    throw new Exception("对象复制失败!");
                }
            }

            oConn.CommintTrans();
            return oNewObject;
        }
        catch (Exception ex)
        {
            oConn.RollbackTrans();
            MyLog.MakeLog("ObjectX.CopyTo()出错:" + ex.Message + " ClassName=" + this.MyName + ", Code=" + this.KeyValueStr);
            return null;
        }
    }
    #region "属性"

    /// <summary> 
    /// Code字段的值是否有效 
    /// </summary> 
    private bool IsCodeValid
    {
        get
        {
            //If (Me.KeyValue Is Nothing) OrElse (Me.KeyValue.ToString() = "") OrElse (Me.KeyValue.ToString() = "0") Then
            if ((this.KeyValue == null) || (this.KeyValue.ToString() == ""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary> 
    /// 本对象是否有效 
    /// </summary> 
    public bool IsValid
    {
        get
        {
            if (this.DataSource == DataSourceType.DS_RDB)
            {
                return this.m_IsValid & this.IsCodeValid;
            }
            else
            {
                return this.m_IsValid;
            }
        }
    }


    /// <summary> 
    /// 对象对应的RDB表名称 
    /// </summary> 
    public string RdbTable
    {
        get
        {
            return this.m_Table;
        }
        set
        {
            this.m_Table = value;
        }
    }

    /// <summary> 
    /// 指从Form读取域的字段前缀，为空时默认以对应的关系表名为前缀 
    /// </summary> 
    public string Prefix
    {
        get
        {
            if (this.m_Prefix == null)
            {
                return this.RdbTable + "_";
            }
            else
            {
                return this.m_Prefix;
            }
        }

        set
        {
            if ((value == null) || (value == ""))
            {
                this.m_Prefix = this.RdbTable + "_";
            }
            else if (!value.EndsWith("_"))
            {
                this.m_Prefix = value + "_";
            }
            else
            {
                this.m_Prefix = value;
            }
        }
    }
    #endregion

    #region "内部类"

    protected class EditLogRecord
    {

        protected string _EditDate = "";
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string EditDate
        {
            get
            {
                return _EditDate;
            }
            set
            {
                _EditDate = value;
            }
        }

        private bool _IsFirstValue = false;
        /// <summary>
        /// 是否是起始值
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsFirstValue
        {
            get
            {
                return _IsFirstValue;
            }
            set
            {
                _IsFirstValue = value;
            }
        }

        private string _EditUser = "";
        /// <summary>
        /// 修改人
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string EditUser
        {
            get
            {
                return _EditUser;
            }
            set
            {
                _EditUser = value;
            }
        }

        private string _OldValue = "";
        /// <summary>
        /// 旧值
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string OldValue
        {
            get
            {
                return _OldValue;
            }
            set
            {
                _OldValue = value;
            }
        }

        private string _NewValue = "";
        /// <summary>
        /// 新值
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string NewValue
        {
            get
            {
                return _NewValue;
            }
            set
            {
                _NewValue = value;
            }
        }

    }

    #endregion
}