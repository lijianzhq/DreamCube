using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
//lijian��ӵ������ռ�
using DreamCube.Foundation.XmlHelper;
using DreamCube.Foundation.Basic.Utility;

/// <summary> 
/// ����������Դ���� 
/// </summary> 
public enum DataSourceType
{
	DS_UNKNOWN = 0,
	DS_RDB = 1,
	DS_NameValueCollectionForm = 2,
	DS_HtmlForm = 3,
	DS_XML = 4,
	DS_MEMORY = 9
	//���������ֶ�����ֵ
}

/// <summary> 
/// ObjectX ��ժҪ˵�� 
/// </summary> 
public class ObjectX
{
    //ָ��ÿ�����ݿ����Ϊ��־���ֶ���, ���ֶ�ֵҪд��Me.RdbCode�� 
    public string PrimKey = "CODE";
    //��־�����ֵ����(CTYPE_NUM ������ CTYPE_STR) 
    public Type KeyType;
    //���Rdb��¼�ı�־�ֶ�ֵ 
    public object KeyValue;
    //�����ȡ���ݵ�λ�� 
    public DataSourceType DataSource;
    //���ӵ��صڿ�����Ӷ��� 
    public RdbConnMgr oConn;
    //���ݼ�¼���� 
    public RdbRecord oRdbRec;
    //��Ҫ���ĵ��ж�ȡָ��ǰ׺����ֵ,Ĭ���������ͬ 
    public string m_Prefix;
    //���������һ����������
    public string m_sLastError;
    //ÿ����������ݴ洢��ı��� 
    public string m_Table;
    //���к�
    public string SequenceName = "";
    //�Ƿ�������� 
    private bool RdbTransDisabled;
    private bool m_bHasRemoveMsg;
    //�����ر�ָ����������Ҫ���õ�������(���ǵ���Щ���������Ч����ROW-00004����)
    public string m_TableColumns = "*";
    //�����Ƿ��ʼ���ɹ� 
    protected bool m_IsValid;
    //������ԴMyXmlNode����
    protected MyXmlNode XmlNode;
    //������ԴFORM����( NameValueCollection������HtmlForm����) 
    protected object oForm;
    //���������ֵ 
    public ArrayList Items;
    //�Ƿ��Ƴ�������վ��true���Ƴ�������վ��false��ɾ����ʱ��ֱ��ɾ���ˣ�lijian��
    public bool RemoveToRecycle = true;

    /// <summary> 
    /// ���캯�� 
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
    /// ת��Ϊ�ַ������͵�KeyValue�����KeyValue=DbNull����KeyValueStr����String.Empty
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
    /// ��¼������
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
    /// ��¼����ʱ��
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
    /// ����޸���
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
    /// ����޸�ʱ��
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
    /// ����: ��ȡ���������
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
    /// ������Զ����RemoveFromRdb()��������ɾ��ʧ�ܵ�ʱ������ʾ��Ϣ��Ҫ���ش����ԣ����ҷ���true
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
    /// ����: ��ȡ�����ȫ��
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
    /// ��ʼ������(������Դ��RDB���ݿ�) 
    /// </summary> 
    /// <param name="vCodeValue">����ֵ</param> 
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
            MyLog.MakeLog("InitRdbObject()��������:" + e.Message);
        }
    }

    /// <summary>
    /// ��ʼ������(������Դ��RDB���ݿ�) 
    /// </summary>
    /// <param name="oRdbRec">�ö�������ݿ��¼</param>
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
            MyLog.MakeLog("InitRdbObject()��������:" + e.Message);
        }
    }

    /// <summary> 
    /// ��ʼ������(������Դ��Form(����: NameValueCollection)) 
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
            MyLog.MakeLog("InitFormObject()��������:" + e.Message);
        }
    }

    /// <summary> 
    /// ��ʼ������(������Դ��Form(����: HtmlForm)) 
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
            MyLog.MakeLog("InitFormObject()��������:" + e.Message);
        }
    }

    /// <summary> 
    /// ��Form�����м��ض������� 
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
            MyLog.MakeLog("LoadDataFromForm()��������:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// ��ʼ������(������Դ��XML�ĵ�����)
    /// </summary>
    /// <param name="oXmlDoc">xml�ĵ�����</param>
    /// <param name="sXmlNodeID">�����������ڽڵ��ID</param>
    /// <remarks></remarks>
    public void InitXmlObject(ref MyXmlDoc oXmlDoc, string sXmlNodeID)
    {
        MyXmlNode node = oXmlDoc.GetNodeByID(sXmlNodeID);
        InitXmlObject(ref node);
    }

    /// <summary>
    /// ��ʼ������(������Դ��XML�ַ���)
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
            MyLog.MakeLog("InitXmlObject()��������:" + ex.Message);
        }

    }

    /// <summary>
    /// ��ʼ������(������Դ��XML�ڵ����)
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
            MyLog.MakeLog("InitXmlObject()��������:" + e.Message);
        }
    }

    /// <summary>
    /// ��XML�ĵ��м��ض�������
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
            MyLog.MakeLog("LoadDataFromXML()��������:" + ex.Message);
            return false;
        }

    }

    /// <summary>
    /// ��ʼ���ڴ����
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
            MyLog.MakeLog("InitMemoryObject()��������:" + e.Message);
        }
    }

    /// <summary> 
    /// ��ʼ������ 
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

            //ǿ��ָ��Rdb��¼������(��ֹ��Щ��һ������Ҳû��,�ҵ�һ���ֶβ���ȷ��Ψһ��)
            this.oRdbRec.PrimaryKey = this.PrimKey;

            return this.InitItems();
        }
        catch (Exception e)
        {
            MyLog.MakeLog("InitObject()��������:" + e.Message);
            MyLog.MakeLog("RdbTable=" + this.RdbTable);
            return false;
        }
    }

    /// <summary>
    /// ��ȡ���ݿ����Ӷ���ע�⣺�Ӷ��������д�÷���������ָ����������ݿ���Ϊ����������Դ
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    public virtual RdbConnMgr GetRdbConnMgrObject()
    {
        return RdbConnHelper.CreateRdbConnMgr();
    }

    /// <summary> 
    /// ��ʼ��Items 
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
            MyLog.MakeLog("InitItems()��������:" + e.Message);
            return false;
        }
    }

    /// <summary> 
    /// ��RDB����Դ��ȡ����������� 
    /// </summary> 
    /// <param name="bReadOnly"></param> 
    /// <returns></returns> 
    private RdbRecord GetRdbRecord(bool bReadOnly)
    {
        return this.oConn.GetRecord(this.GetQuerySQL(), bReadOnly, this.PrimKey);
    }

    /// <summary> 
    /// ��ȡ�õ�����¼���ݵ�SQL��ѯ��� 
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
        //lijian�����߼�����û�д�������ֵ��ʱ��Ӧ���Ǵ���һ���ڴ�������ݿ�û�ж�Ӧ�ļ�¼����ʵ���Ͼ�����Ҫ��ܹ���Ϣ����
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
    /// ��ȡ�ֶ�ֵ 
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
    /// ��ȡ�ֶε�Ĭ��ֵ 
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
    /// ����ָ���ֶ�
    /// </summary>
    /// <param name="sItemName"></param>
    /// <param name="sValue"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public bool SaveItemValue(string sItemName, object sValue)
    {
        try
        {
            //���ַ����ĳ��ȳ���2000ʱ,����Update�������и���,�����
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
            //����SetItemValue,ȷ�������е�ֵ�����µ�ֵ
            this.SetItemValue(sItemName, sValue);
        }
        return false;
    }

    /// <summary> 
    /// �����ֶ�ֵ 
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
    /// ��ʽ��SQL��ѯ���ı��� 
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
            MyLog.MakeLog("ObjectX.FormatSqlValue() ����,�Ҳ����ֶ�:" + sFieldName);
        }
        return "";
    }
    /// <summary> 
    /// �ж϶����Ƿ���ָ���ֶ� 
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
    /// �жϱ������Ƿ���������ݿ� 
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
            MyLog.MakeLog("IsExistedInRDB()��������:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// lijian���ӣ����ö����ƴ���ֶ�
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
                    //����ƴ������
                    PinyinSuggestion.SavePinyin(this.RdbTable, sColumnName, Convert.ToString(this.KeyValue), Convert.ToString(sourceColumnValue), Convert.ToString(this.PrimKey));
                }
            }
        }
        catch (Exception ex)
        {
            MyLog.MakeLog("SetPYColumnValue()��������:" + ex.Message);
        }
    }

    /// <summary> 
    /// ���������ݱ��浽RDB���ݿ� 
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
                MyLog.MakeLog("SaveToRdb()ִ��ʧ��! Me.oRdbRec������Ч");
                return "";
            }

            bool bIsNew;
            string sSQL;
            string sRid;
            Type tRidType;
            RdbRecord oRecord;

            sRid = "";
            tRidType = Type.GetType("string");
            //lijian����
            SetPYColumnValue();

            if (this.IsExistedInRDB())
            {
                bIsNew = false;

                //�����ǰ����DS_RDB����,����oRdbRec��ͨ��AddNew������,��Ҫ�滻ΪRDB���Ѵ��ڼ�¼,���򱣴��������¼�¼ 
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

                //��������޸�ʱ�� 
                if (this.HasItem("LastModifyTime"))
                {
                    this.SetItemValue("LastModifyTime", MyDatetime.NowTimeyyyyMMddHHmmss);
                }

                //��������޸��� 
                if (this.HasItem("LastModifyUser"))
                {
                    this.SetItemValue("LastModifyUser", CurrentSession.CurrentUserName);
                }

                //���ô�������, �����˲����޸� 
                this.SetReadOnlyItem("CreateTime", true);
                this.SetReadOnlyItem("CreateUser", true);

                //���ridֵΪ��,����丳ֵ, ���򱣴�����
                if (this.HasItem("rid") && string.IsNullOrEmpty(this.GetItemValue("rid").ToString()))
                {
                    this.SetItemValue("rid", MyGuid.GetNewGuid());
                }
            }
            else
            {
                bIsNew = true;

                //���ô����� 
                if (this.HasItem("CreateUser") && string.IsNullOrEmpty(Convert.ToString(this.GetItemValue("CreateUser"))))
                {
                    this.SetItemValue("CreateUser", CurrentSession.CurrentUserName);
                }

                //���ô���ʱ�� 
                if (this.HasItem("CreateTime"))
                {
                    this.SetItemValue("CreateTime", MyDatetime.NowTimeyyyyMMdd);
                }

                //��������޸��� 
                if (this.HasItem("LastModifyUser"))
                {
                    this.SetItemValue("LastModifyUser", CurrentSession.CurrentUserName);
                }

                //��������޸�ʱ�� 
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
                        //��Щ���������ֵ��û�����Զ���ŵ�, �ڱ���֮ǰ��Ԥ�����ú�������ֵ, ��:ʹ��32λ�ĵ�UNID
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
                        //���Զ����, ����ǰ��û�и�ֵ, ����ʱ��ֵ
                        if (string.IsNullOrEmpty(this.KeyValueStr))
                        {
                            //lijian�����߼��жϣ���Ϊ��Щ����û��code�ֶεģ������ȡ����codefield����Ҫ����ֵ��
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

            //�������� 
            if (!this.oRdbRec.Save())
            {
                MyLog.MakeLog("SaveToRdb()����: RdbRecord Save()ʧ��!");
                return string.Empty;
            }

            //�����¼�¼: �����¼�ı�־���Զ���ŵ�(�����Ƿ���rid�ֶ����ж�),���ȡ�¼�¼��ʶ 
            if (bIsNew && this.HasItem("rid"))
            {
                sSQL = "SELECT " + this.PrimKey + " FROM " + this.RdbTable + " WHERE rid=" + oConn.FormatSqlValue(sRid, tRidType);
                oRecord = this.oConn.GetRecord(sSQL, true);
                this.KeyValue = oRecord.GetField(this.PrimKey).Value;
                this.SetItemValue(this.PrimKey, this.KeyValue);
            }

            if (bIsNew)
            {
                //��Web�˻�дCODE�ֶ�ֵ,���Web���ٴε㱣������Ϊ�¼�¼
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
            MyLog.MakeLog("SaveToRDB()��������:" + e.Message);
            return "";
        }
    }

    /// <summary>
    /// �����ݱ��浽����
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
    /// �����������д��ASPXҳ���� 
    /// </summary> 
    /// <param name="oPage">ҳ���ж����ֶ�����ǰ׺</param> 
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
                //���ڶ������ֶ�,���ҪWrite��ҳ����,����Ϊ����һ���ַ����������ֶ�,���ɳ��ַ���֮����д��
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
    /// ��ȡ�ֶεľ�ֵ
    /// </summary>
    /// <param name="sParentDocUNID">���ĵ���DOCUNID</param>
    /// <param name="sTableName">Ŀ�����ݱ����</param>
    /// <param name="sItemName">�ֶ�����</param>
    /// <returns>����JSON����</returns>
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
                    //�����޸ļ�¼����
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
    /// �����������д��ASPXҳ���У����޸ļ�¼Ҳд�ϣ�
    /// </summary> 
    /// <param name="sParentDocUNID">���ĵ���DOCUNID</param> 
    /// <param name="oParentControl">������ʷ��¼�ؼ��ĸ��ؼ�����</param> 
    /// <param name="sPrefix">ҳ���ж����ֶ�����ǰ׺</param> 
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
                //���ڶ������ֶ�,���ҪWrite��ҳ����,����Ϊ����һ���ַ����������ֶ�,���ɳ��ַ���֮����д��
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, oField.ExtractAsString());
            }
            else if (oField.IsDateTimeField)
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, MyDatetime.GetDatePart(Convert.ToString(this.GetItemValue(oField.Name))));
            }
            else
            {
                MyPage.SetItemValue(oPage, sPrefix + oField.Name, this.GetItemValue(oField.Name));

                //д����ʷ�޸ļ�¼�Ŀؼ�
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
    /// ����ֻ���ֶ� 
    /// </summary> 
    /// <param name="sItemName"></param> 
    /// <param name="bSet"></param> 
    public void SetReadOnlyItem(string sItemName, bool bSet = true)
    {
        this.oRdbRec.SetReadOnlyField(sItemName, bSet);
    }

    /// <summary> 
    /// �ж��ǲ���ֻ���ֶ� 
    /// </summary> 
    /// <param name="sItemName"></param> 
    public bool IsReadOnlyItem(string sItemName)
    {
        return this.oRdbRec.IsReadOnlyField(sItemName);
    }

    /// <summary> 
    /// �������֮��, ������IE�εĶ����CODE�ֶ�, ��ֹ�ٴα���ʱ��������һ���¶��� 
    /// </summary> 
    private void UpdateCodeFieldInIE()
    {
        string sFieldName = this.Prefix + this.PrimKey;
        MyPage.SetWebFieldValue("parent", sFieldName, Convert.ToString(this.KeyValue));
        MyPage.SetWebFieldValue("parent", sFieldName.ToUpper(), Convert.ToString(this.KeyValue));
    }

    /// <summary> 
    /// ��RDB���ݿ�ɾ���������¼ 
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
            //lijian�����߼����ж��Ƿ�ɾ��������վ
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
                MyLog.MakeLog(string.Format("����ɾ������{0}��CODE={1}��ɾ���ߣ�{2}", this.RdbTable, this.KeyValue, CurrentSession.CurrentUserName));
                this.oRdbRec = null;
                this.DataSource = DataSourceType.DS_UNKNOWN;
                this.KeyValue = "0";
                return true;
            }
        }
        catch (Exception e)
        {
            MyLog.MakeLog("RemoveFromRdb()��������:" + e.Message);
            return false;
        }
    }

    /// <summary>
    /// ����: ���ɶ���JSON����
    /// ����: ��ѧ��
    /// </summary>
    /// <param name="sItemName">ָ��ֻ������Щ�ֶ�,�ձ�ʾ���������ֶ�,����ֶ�֮���ԷֺŸ���</param>
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
    /// ����: ���ɶ����XML
    /// ����: ��ѧ��
    /// </summary>
    /// <param name="sItemName">ָ��ֻ������Щ�ֶ�,�ձ�ʾ���������ֶ�,����ֶ�֮���ԷֺŸ���</param>
    /// <param name="sObjectTagName">�����Xml tagName</param>
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
    /// ���ƶ���
    /// </summary>
    /// <param name="bNotSaveToDBImmediately">true=copy���µĶ��󣬲���Ҫ���浽���ݿ��� false=copy���µĶ������ϱ��浽���ݿ��У�Ĭ��Ϊfalse</param>
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
            //��ʱ����Ҫcopy�Ķ������ϱ��浽���ݿ��е�
            if (!bNotSaveToDBImmediately)
            {
                if (string.IsNullOrEmpty(oNewObject.SaveToRDB()))
                {
                    throw new Exception("������ʧ��!");
                }
            }

            oConn.CommintTrans();
            return oNewObject;
        }
        catch (Exception ex)
        {
            oConn.RollbackTrans();
            MyLog.MakeLog("ObjectX.Copy()����:" + ex.Message + " ClassName=" + this.MyName + ", Code=" + this.KeyValueStr);
            return null;
        }
    }

    /// <summary>
    /// ���ƶ��� ��һ�����Ƶ�����һ����  lijian
    /// </summary>
    /// <param name="sRdbTable">��һ�����һ����¼copy������һ����</param>
    /// <param name="sNoSameRecordByField">Ŀ����� ĳ���ֶβ��ܳ����ظ�ֵ ���� ��A��copy���ݵ�B���У�B���field�ֶ���������ظ�ֵ������ɾ��ԭ���ļ�¼�����²�����ֵ</param>
    /// <param name="bNotSaveToDBImmediately">�Ƿ�����copy�������ݱ��浽���ݿ��� Ĭ�ϱ��棨false�� ���ΪTRUE �����ϱ��浽���ݿ���</param>
    /// <param name="bCopyPrimarykey">�Ƿ�copy����ֵ Ĭ���ǣ�true�� </param>
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
            //��ʱ����Ҫcopy�Ķ������ϱ��浽���ݿ��е�
            if (!bNotSaveToDBImmediately)
            {
                if (string.IsNullOrEmpty(oNewObject.SaveToRDB()))
                {
                    throw new Exception("������ʧ��!");
                }
            }

            oConn.CommintTrans();
            return oNewObject;
        }
        catch (Exception ex)
        {
            oConn.RollbackTrans();
            MyLog.MakeLog("ObjectX.CopyTo()����:" + ex.Message + " ClassName=" + this.MyName + ", Code=" + this.KeyValueStr);
            return null;
        }
    }
    #region "����"

    /// <summary> 
    /// Code�ֶε�ֵ�Ƿ���Ч 
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
    /// �������Ƿ���Ч 
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
    /// �����Ӧ��RDB������ 
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
    /// ָ��Form��ȡ����ֶ�ǰ׺��Ϊ��ʱĬ���Զ�Ӧ�Ĺ�ϵ����Ϊǰ׺ 
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

    #region "�ڲ���"

    protected class EditLogRecord
    {

        protected string _EditDate = "";
        /// <summary>
        /// �޸�����
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
        /// �Ƿ�����ʼֵ
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
        /// �޸���
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
        /// ��ֵ
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
        /// ��ֵ
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