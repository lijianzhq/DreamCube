using System;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

using DreamCube.Foundation.XmlHelper;
using DreamCube.Foundation.Basic.Utility;

/// <summary> 
/// ObjectHelper 的摘要说明 
/// </summary> 
public class ObjectHelper
{
    //对象的命名空间
    public static string sDefaultAssemblyName = GetDefaultAssemblyName();

    public static string sCommonAssemblyName = "BasicObjects;CmisObjects";

    #region "GetRdbObject"

    /// <summary>
    /// 等同于 CreateRdbObject
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="vCodeValue"></param>
    /// <param name="sRdbTable"></param>
    /// <param name="oConn"></param>
    /// <param name="sPrimKey">数据库表的主键名称</param> 
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX GetRdbObjectByCode(string sClassName, object vCodeValue, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimkey = "")
    {
        return CreateRdbObject(sClassName, vCodeValue, sRdbTable, oConn, sPrimkey);
    }

    /// <summary>
    /// 只返回一个对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sSql"></param>
    /// <param name="sRdbTable"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX GetRdbObject(string sClassName, string sSql, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {
        ObjectX[] aObjects = GetRdbObjects(sClassName, sSql, sRdbTable, oConn, sPrimKey);
        if (aObjects == null)
        {
            return null;
        }
        else
        {
            return aObjects[0];
        }
    }

    public static ObjectX GetRdbObjectEx(string sRdbTable, string sSql, RdbConnMgr oConn = null, string sPrimKey = "")
    {
        return GetRdbObject("ObjectX", sSql, sRdbTable, oConn, sPrimKey);
    }

    public static ObjectX GetRdbObjectEx2(string sRdbTable, object vCodeValue, RdbConnMgr oConn = null, string sPrimkey = "")
    {
        if (oConn == null)
        {
            oConn = RdbConnHelper.CreateRdbConnMgr();
        }
        if (string.IsNullOrEmpty(sPrimkey))
        {
            sPrimkey = oConn.GetTablePrimaryKey(sRdbTable);
        }
        string sSQL = string.Format("SELECT {0} FROM {1} WHERE {0}={2}", sPrimkey, sRdbTable, oConn.FormatSqlValue(sRdbTable, sPrimkey, vCodeValue));
        return GetRdbObject("ObjectX", sSQL, sRdbTable, oConn, sPrimkey);
    }

    public static ObjectX GetRdbObjectEx3(object vCodeValue, string sRdbTable = "", string sClassName = "ObjectX", RdbConnMgr oConn = null, string sPrimkey = "")
    {
        return CreateRdbObject(sClassName, vCodeValue, sRdbTable, oConn, sPrimkey);
    }

    #endregion

    #region "GetRdbObjects"

    /// <summary>
    /// 批量获取对象
    /// </summary>
    /// <param name="sClassName">类名称</param>
    /// <param name="aCodeValues">对象主键值(数组)</param>
    /// <param name="sRdbTable"></param>
    /// <returns>返回ObjectX数组</returns>
    /// <remarks></remarks>
    public static ObjectX[] GetRdbObjects(string sClassName, string[] aCodeValues, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {
        if (aCodeValues == null || aCodeValues.Length == 0)
        {
            return null;
        }
        ObjectX[] aObjectx = null;
        // ERROR: Not supported in C#: ReDimStatement

        for (int i = 0; i <= aCodeValues.Length - 1; i++)
        {
            aObjectx[i] = CreateRdbObject(sClassName, aCodeValues[i], sRdbTable, oConn, sPrimKey);
        }
        return aObjectx;
    }

    public static ObjectX[] GetRdbObjectsEx1(string sRdbTable, string[] aCodeValues, RdbConnMgr oConn = null, string sPrimKey = "")
    {
        return GetRdbObjects("ObjectX", aCodeValues, sRdbTable, oConn, sPrimKey);
    }

    /// <summary>
    /// 批量获取对象（本函数效率较低，建议采用GetRdbObjectsEx3）
    /// </summary>
    /// <param name="sClassName">类名称</param>
    /// <param name="sPrimKeySelectSql">主键查询SQL语句</param>
    /// <param name="sRdbTable"></param>
    /// <returns>返回ObjectX数组</returns>
    /// <remarks></remarks>
    public static ObjectX[] GetRdbObjects(string sClassName, string sPrimKeySelectSql, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {

        //****************************************************************************************************
        //如果查询的是sRdbTable，则调用GetRdbObjectsEx3，以提高效率
        //****************************************************************************************************
        string sTempSQL = sPrimKeySelectSql.ToUpper();
        if (string.IsNullOrEmpty(sRdbTable))
        {
            sRdbTable = Convert.ToString(GetClassTableName(sClassName));
        }

        if (string.IsNullOrEmpty(sPrimKey))
        {
            sPrimKey = GetClassPrimKey(sClassName);
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            sRdbTable = sRdbTable.ToUpper();
            sPrimKey = sPrimKey.ToUpper();
            if (sTempSQL.StartsWith("SELECT * FROM " + sRdbTable))
            {
                //MyLog.MakeLog("转成GetRdbObjectsEx3：" & sPrimKeySelectSql)
                return GetRdbObjectsEx3(sClassName, sPrimKeySelectSql, sRdbTable, oConn, sPrimKey);
            }
            else if (sTempSQL.StartsWith("SELECT " + sPrimKey + " FROM " + sRdbTable))
            {
                //MyLog.MakeLog("转成GetRdbObjectsEx3：" & sPrimKeySelectSql)
                sTempSQL = "SELECT *" + MyString.Right(sPrimKeySelectSql, sPrimKeySelectSql.Length - ("SELECT " + sPrimKey).Length);
                //MyLog.MakeLog("转换后：" & sTempSQL)
                return GetRdbObjectsEx3(sClassName, sTempSQL, sRdbTable, oConn, sPrimKey);
            }
        }
        //****************************************************************************************************


        if (oConn == null)
        {
            oConn = ObjectHelper.GetRdbConnMgrOfClass(sClassName);
        }
        RdbRecordSet oRecSet = oConn.GetRecordSet(sPrimKeySelectSql, true);
        if (oRecSet == null || oRecSet.RecordCount < 1)
        {
            return null;
        }
        ObjectX[] aObjectx = null;
        ObjectX oObject = null;
        RdbRecord oRecord = null;
        for (int i = 0; i <= oRecSet.RecordCount - 1; i++)
        {
            oRecord = oRecSet.GetRecord(i);
            oObject = ObjectHelper.CreateRdbObject(sClassName, oRecord.GetField(0).Value, sRdbTable, oConn, sPrimKey);
            if (oObject != null)
            {
                if (aObjectx == null)
                {
                    // ERROR: Not supported in C#: ReDimStatement

                }
                else
                {
                    Array.Resize(ref aObjectx, aObjectx.Length + 1);
                }
                aObjectx[aObjectx.Length - 1] = oObject;
            }
        }
        return aObjectx;
    }

    public static ObjectX[] GetRdbObjectsEx2(string sRdbTable, string sSql, RdbConnMgr oConn = null, string sPrimKey = "")
    {
        return GetRdbObjects("ObjectX", sSql, sRdbTable, oConn, sPrimKey);
    }

    /// <summary>
    /// 批量获取对象（说明：本函数的效率比GetRdbObjects要高许多）
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sRecordSelectSql">通过该SQL，查询返回的是整个对象的数据，而不仅仅是CODE值</param>
    /// <param name="sRdbTable"></param>
    /// <param name="oConn"></param>
    /// <param name="sPrimKey"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX[] GetRdbObjectsEx3(string sClassName, string sRecordSelectSql, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {
        if (oConn == null)
        {
            oConn = ObjectHelper.GetRdbConnMgrOfClass(sClassName);
        }

        RdbRecordSet oRecSet = oConn.GetRecordSet(sRecordSelectSql, false);
        if (oRecSet == null || oRecSet.RecordCount < 1)
        {
            return null;
        }

        ObjectX[] aObjectx = null;
        ObjectX oObject = null;
        RdbRecord oRecord = null;
        for (int i = 0; i <= oRecSet.RecordCount - 1; i++)
        {
            oRecord = oRecSet.GetRecord(i);
            oObject = ObjectHelper.CreateRdbObject(sClassName, oRecord, sRdbTable, oConn, sPrimKey);
            if (oObject != null)
            {
                if (aObjectx == null)
                {
                    // ERROR: Not supported in C#: ReDimStatement

                }
                else
                {
                    Array.Resize(ref aObjectx, aObjectx.Length + 1);
                }
                aObjectx[aObjectx.Length - 1] = oObject;
            }
        }
        return aObjectx;
    }

    #endregion

    #region "GetRdbObjectsByItem"

    /// <summary>
    /// 功能: 根据对象的某个属性的值来获取对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sItemName"></param>
    /// <param name="sItemValue"></param>
    /// <param name="sSqlOrderBy"></param>
    /// <param name="sTableName"></param>
    /// <param name="oConn"></param>
    /// <param name="sPrimKey"></param>
    /// <param name="bIgnoreCase">匹配时是否忽略大小写</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX[] GetRdbObjectsByItem(string sClassName, string sItemName, string sItemValue, string sSqlOrderBy = "", string sTableName = "", RdbConnMgr oConn = null, string sPrimKey = "", bool bIgnoreCase = true)
    {
        if (oConn == null)
        {
            oConn = ObjectHelper.GetRdbConnMgrOfClass(sClassName);
        }
        if (string.IsNullOrEmpty(sTableName))
        {
            sTableName = Convert.ToString(ObjectHelper.GetClassTableName(sClassName));
        }

        string sSQL = "";

        //If bIgnoreCase Then
        //    sSQL = String.Format("SELECT {3} FROM {0} WHERE {1}={2}", sTableName, oConn.SqlFuncLower(sItemName), oConn.SqlFuncLower(oConn.FormatSqlValue(sTableName, sItemName, sItemValue)), GetClassPrimKey(sClassName))
        //Else
        //    sSQL = String.Format("SELECT {3} FROM {0} WHERE {1}={2}", sTableName, sItemName, oConn.FormatSqlValue(sTableName, sItemName, sItemValue), GetClassPrimKey(sClassName))
        //End If

        //刘学亮 2014-10-01 （修改原因：速度优化）
        if (bIgnoreCase)
        {
            sSQL = string.Format("SELECT * FROM {0} WHERE {1}={2}", sTableName, oConn.SqlFuncLower(sItemName), oConn.SqlFuncLower(oConn.FormatSqlValue(sTableName, sItemName, sItemValue)));
        }
        else
        {
            sSQL = string.Format("SELECT * FROM {0} WHERE {1}={2}", sTableName, sItemName, oConn.FormatSqlValue(sTableName, sItemName, sItemValue));
        }

        if (string.IsNullOrEmpty(sSqlOrderBy) == false)
        {
            sSqlOrderBy = sSqlOrderBy.Trim();
            if (sSqlOrderBy.StartsWith("order by", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                sSqlOrderBy = "ORDER BY " + sSqlOrderBy;
            }
            sSQL += " " + sSqlOrderBy.Trim();
        }

        RdbRecordSet oRecSet = oConn.GetRecordSet(sSQL, true);
        if (oRecSet != null && oRecSet.RecordCount > 0)
        {
            ObjectX[] aObjects = null;
            ObjectX oObject;
            for (int i = 0; i <= oRecSet.RecordCount - 1; i++)
            {
                //oObject = ObjectHelper.CreateRdbObject(sClassName, oRecSet.GetRecord(i).GetField(0).Value, sTableName, oConn, sPrimKey)

                //刘学亮 2014-10-01 （修改原因：速度优化）
                oObject = ObjectHelper.CreateRdbObject(sClassName, oRecSet.GetRecord(i), sTableName, oConn, sPrimKey);

                if (oObject != null)
                {
                    if (aObjects == null)
                    {
                        // ERROR: Not supported in C#: ReDimStatement

                    }
                    else
                    {
                        Array.Resize(ref aObjects, aObjects.Length + 1);
                    }
                    aObjects[aObjects.Length - 1] = oObject;
                }
            }
            return aObjects;
        }
        else
        {
            return null;
        }
    }

    public static ObjectX[] GetRdbObjectsByItemEx(string sRdbTable, string sClassName, string sItemName, string sItemValue, string sSqlOrderBy = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {
        return GetRdbObjectsByItem("ObjectX", sItemName, sItemValue, sSqlOrderBy, sRdbTable, oConn, sPrimKey);
    }

    /// <summary>
    /// 只获取一个对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sItemName"></param>
    /// <param name="sItemValue"></param>
    /// <param name="sRdbTable"></param>
    /// <param name="oConn"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX GetRdbObjectByItem(string sClassName, string sItemName, string sItemValue, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimKey = "")
    {
        if (oConn == null)
        {
            oConn = ObjectHelper.GetRdbConnMgrOfClass(sClassName);
        }
        if (string.IsNullOrEmpty(sRdbTable))
        {
            sRdbTable = Convert.ToString(ObjectHelper.GetClassTableName(sClassName));
        }

        //Dim sSQL As String = String.Format("SELECT {3} FROM {0} WHERE {1}={2}", sRdbTable, sItemName, oConn.FormatSqlValue(sRdbTable, sItemName, sItemValue), GetClassPrimKey(sClassName))
        //Dim sObjectCode As Object = oConn.GetValue(sSQL)
        //If sObjectCode IsNot Nothing Then
        //    Return ObjectHelper.CreateRdbObject(sClassName, sObjectCode, sRdbTable, oConn, sPrimKey)
        //Else
        //    Return Nothing
        //End If

        //刘学亮 2014-10-01 （修改原因：速度优化）
        string sSQL = string.Format("SELECT * FROM {0} WHERE {1}={2}", sRdbTable, sItemName, oConn.FormatSqlValue(sRdbTable, sItemName, sItemValue));
        RdbRecord oRecord = oConn.GetRecord(sSQL, false);
        if (oRecord != null)
        {
            return ObjectHelper.CreateRdbObject(sClassName, oRecord, sRdbTable, oConn, sPrimKey);
        }
        else
        {
            return null;
        }

    }

    public static ObjectX GetRdbObjectByItemEx(string sRdbTable, string sItemName, string sItemValue, RdbConnMgr oConn = null, string sPrimKey = "")
    {
        return GetRdbObjectByItem("ObjectX", sItemName, sItemValue, sRdbTable, oConn, sPrimKey);
    }
    #endregion

    #region "CreateFormObject"

    /// <summary> 
    /// 创建对象，数据来源是Request.Form
    /// </summary> 
    /// <param name="sClassName"></param> 
    /// <param name="oForm"></param> 
    /// <param name="sPrefix"></param> 
    /// <param name="sRdbTable"></param>
    /// <returns></returns> 
    public static ObjectX CreateFormObject(string sClassName, ref NameValueCollection oForm, string sPrefix = "", string sRdbTable = "", string sPrimKey = "")
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }
        if (sRdbTable != "")
        {
            oObject.RdbTable = sRdbTable;
        }

        if (sPrimKey != "")
        {
            oObject.PrimKey = sPrimKey;
        }

        oObject.InitFormObject(oForm, sPrefix);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }

    public static ObjectX CreateFormObjectEx1(string sRdbTable, ref NameValueCollection oForm, string sPrefix = "", string sPrimKey = "")
    {
        return CreateFormObject("ObjectX", ref oForm, sPrefix, sRdbTable, sPrimKey);
    }


    /// <summary>
    /// 创建对象，数据来源是Page.Form
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="oForm"></param>
    /// <param name="sPrefix"></param>
    /// <param name="sRdbTable"></param>
    /// <param name="sPrimKey"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateFormObject(string sClassName, HtmlForm oForm, string sPrefix = "", string sRdbTable = "", string sPrimKey = "")
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimKey) == false)
        {
            oObject.PrimKey = sPrimKey;
        }

        oObject.InitFormObject(oForm, sPrefix);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }

    public static ObjectX CreateFormObjectEx2(string sRdbTable, ref HtmlForm oForm, string sPrefix = "", string sPrimKey = "")
    {
        return CreateFormObject("ObjectX", oForm, sPrefix, sRdbTable, sPrimKey);
    }
    #endregion

    #region "CreateRdbObject"

    /// <summary> 
    /// 创建RDB类型对象 
    /// </summary> 
    /// <param name="sClassName">类名称,可以是短名也可以是全名</param> 
    /// <param name="vCodeValue">主键值</param> 
    /// <param name="sRdbTable">对象的数据来源表</param> 
    /// <param name="sPrimKey">数据库表的主键名称</param> 
    /// <returns></returns> 
    public static ObjectX CreateRdbObject(string sClassName, object vCodeValue, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimkey = "")
    {

        //MyDebug.LockDebug("CreateRdbObject")

        //MyDebug.SetStartDebugTime()

        //MyDebug.PrintElapsedTime(1, "CreateRdbObject")

        if (string.IsNullOrEmpty(sClassName))
        {
            sClassName = "ObjectX";
        }
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }
        //MyDebug.PrintElapsedTime(2, "CreateRdbObject")

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }
        //MyDebug.PrintElapsedTime(3, "CreateRdbObject")

        if (string.IsNullOrEmpty(sPrimkey) == false)
        {
            oObject.PrimKey = sPrimkey;
        }
        //MyDebug.PrintElapsedTime(4, "CreateRdbObject")

        oObject.InitRdbObject(vCodeValue, oConn);

        //MyDebug.PrintElapsedTime(5, "CreateRdbObject")

        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            if (vCodeValue != null && vCodeValue.ToString() != string.Empty)
            {
                //MyLog.MakeLog("ObjectHelper.CreateRdbObject() 创建不成功!sClassName=" & sClassName & ", CodeValue=" & vCodeValue)
            }
            return null;
        }
    }

    public static ObjectX CreateRdbObjectEx(string sRdbTable, object vCodeValue, RdbConnMgr oConn = null, string sPrimkey = "")
    {
        return CreateRdbObject("ObjectX", vCodeValue, sRdbTable, oConn, sPrimkey);
    }

    /// <summary>
    /// 创建RDB类型对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="oRdbRecord"></param>
    /// <param name="sRdbTable"></param>
    /// <param name="oConn"></param>
    /// <param name="sPrimkey"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateRdbObject(string sClassName, RdbRecord oRdbRecord, string sRdbTable = "", RdbConnMgr oConn = null, string sPrimkey = "")
    {

        if (string.IsNullOrEmpty(sClassName))
        {
            sClassName = "ObjectX";
        }
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimkey) == false)
        {
            oObject.PrimKey = sPrimkey;
        }

        oObject.InitRdbObject(oRdbRecord, oConn);

        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region "CreateXmlObject"

    /// <summary>
    /// 创建XML对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="oXmlNode"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateXmlObject(string sClassName, ref MyXmlNode oXmlNode, string sRdbTable = "", string sPrimKey = "")
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimKey) == false)
        {
            oObject.PrimKey = sPrimKey;
        }

        oObject.InitXmlObject(ref oXmlNode);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 创建XML对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sXmlString"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateXmlObject(string sClassName, string sXmlString, string sRdbTable = "", string sPrimKey = "")
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimKey) == false)
        {
            oObject.PrimKey = sPrimKey;
        }

        oObject.InitXmlObject(sXmlString);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 创建XML对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="oXmlDoc"></param>
    /// <param name="sNodeID"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateXmlObject(string sClassName, ref MyXmlDoc oXmlDoc, string sNodeID, string sRdbTable = "", string sPrimKey = "")
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimKey) == false)
        {
            oObject.PrimKey = sPrimKey;
        }

        oObject.InitXmlObject(ref oXmlDoc, sNodeID);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region "CreateMemoryObject"

    /// <summary>
    /// 创建内存对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sRdbTable"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateMemoryObject(string sClassName, string sRdbTable = "", string sPrimKey = "", RdbConnMgr oConnX = null, string sSequenceName = "")
    {

        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(sRdbTable) == false)
        {
            oObject.RdbTable = sRdbTable;
        }

        if (string.IsNullOrEmpty(sPrimKey) == false)
        {
            oObject.PrimKey = sPrimKey;
        }

        if (string.IsNullOrEmpty(sSequenceName) == false)
        {
            oObject.SequenceName = sSequenceName;
        }

        oObject.InitMemoryObject(oConnX);
        if (oObject.IsValid)
        {
            return oObject;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region "CreateNewObject"

    /// <summary>
    /// 创建对象新实例
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="sRdbTable"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static ObjectX CreateNewObject(string sClassName, string sRdbTable = "", string sPrimKey = "", RdbConnMgr oConnX = null)
    {
        try
        {
            return CreateMemoryObject(sClassName, sRdbTable, sPrimKey, oConnX);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static ObjectX CreateNewObjectEx(string sRdbTable, string sPrimKey = "", RdbConnMgr oConnX = null)
    {
        return CreateNewObject("ObjectX", sRdbTable, sPrimKey, oConnX);
    }

    public static ObjectX CreateNewObjectEx2(string sRdbTable, string sPrimKey = "", string sSequenceName = "", RdbConnMgr oConnX = null)
    {
        try
        {
            return CreateMemoryObject("ObjectX", sRdbTable, sPrimKey, oConnX, sSequenceName);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    #endregion

    #region "其它函数"

    /// <summary>
    /// 获取默认的命名空间
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    private static string GetDefaultAssemblyName()
    {
        if (CurrentContext.InWebContext())
        {
            return System.Configuration.ConfigurationManager.AppSettings["DefaultAssemblyName"];
        }
        else
        {
            return WinResource.GetString("DefaultAssemblyName");
        }
    }

    /// <summary> 
    /// 创建空对象 
    /// </summary> 
    /// <param name="sClassName"></param> 
    /// <returns></returns> 
    public static ObjectX CreateEmptyObject(string sClassName)
    {
        Type tClass = GetClassType(sClassName);
        if (tClass != null)
        {
            return (ObjectX)Activator.CreateInstance(tClass);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 获取对象表名
    /// </summary>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static object GetClassTableName(string sClassName)
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject != null)
        {
            return oObject.RdbTable;
        }
        else
        {
            MyLog.MakeLog("ObjectHelper.GetObjectTableName()出错!创建 " + sClassName + " 对象失败");
        }
        return "";
    }

    /// <summary>
    /// 获取对象主键名
    /// </summary>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string GetClassPrimKey(string sClassName)
    {
        ObjectX oObject = ObjectHelper.CreateEmptyObject(sClassName);
        if (oObject != null)
        {
            return oObject.PrimKey;
        }
        else
        {
            MyLog.MakeLog("ObjectHelper.GetObjectPrimKey()出错!创建 " + sClassName + " 对象失败");
        }
        return "";
    }

    /// <summary>
    /// 获取指定类的数据库连接对象
    /// </summary>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static RdbConnMgr GetRdbConnMgrOfClass(string sClassName)
    {
        try
        {
            ObjectX oObject = CreateEmptyObject(sClassName);
            return oObject.GetRdbConnMgrObject();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// 判断是否是一个有效的类名
    /// </summary>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static bool IsValidClassName(string sClassName)
    {
        if (string.IsNullOrEmpty(sClassName) || ObjectHelper.GetClassType(sClassName) == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    /// <summary>
    /// 获取Class类型
    /// </summary>
    /// <param name="sClassName"></param>
    /// <param name="bThrowOnError"></param>
    /// <param name="bIgnoreCase"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    private static Type GetTypeByClassName(string sClassName, bool bThrowOnError = false, bool bIgnoreCase = true)
    {
        try
        {
            return Type.GetType(sClassName, bThrowOnError, bIgnoreCase);
        }
        catch (Exception ex)
        {
            return null;
        }
        return null;
    }

    /// <summary>
    /// 根据类型的字符串，创建一个类型BO程序集中的类型实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sType"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static T CreateInstansce<T>(string sType)
    {
        try
        {
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i <= assemblys.Length - 1; i++)
            {
                Type oType = GetClassType(sType);
                if (oType == null)
                {
                    continue;
                }
                object oObj = assemblys[i].CreateInstance(oType.FullName);
                if (oObj != null)
                {
                    return (T)oObj;
                }
            }
        }
        catch (Exception ex)
        {
            MyLog.MakeLog(ex);
        }
        return default(T);
    }

    /// <summary>
    /// 获取sClassName 的类型,先用默认程序集创建,如果不成功,再用 sCommonAssemblyName 来创建
    /// </summary>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static Type GetClassType(string sClassName)
    {
        string sFullClassName = sClassName;
        string sAssemblyName = sCommonAssemblyName;
        Type t = null;

        if (string.IsNullOrEmpty(sDefaultAssemblyName) == false)
        {
            sAssemblyName = MyString.Connect(sAssemblyName, sDefaultAssemblyName.Replace(",", ";"), ";");
        }

        Assembly oAssembly = Assembly.GetEntryAssembly();
        if (oAssembly != null)
        {
            sAssemblyName = MyString.Connect(sAssemblyName, oAssembly.GetName().Name, ";");
        }

        string[] aAssembleNames = sAssemblyName.Split(';');
        foreach (string asm in aAssembleNames)
        {
            if (sClassName.IndexOf(".") < 0)
            {
                sFullClassName = asm + "." + sClassName + "," + asm;
            }
            else if (sClassName.StartsWith(asm))
            {
                sFullClassName = sClassName + "," + asm;
            }
            t = GetTypeByClassName(sFullClassName, true, true);
            if (t != null)
            {
                break; // TODO: might not be correct. Was : Exit For
            }
        }

        if (t == null) t = GetTypeByClassName(sClassName, true, true);

        if (t == null && sClassName.IndexOf(".") != -1)
        {
            sAssemblyName = MyString.Left(sClassName, ".");
            sClassName = MyString.Right(sClassName, ".");
            sFullClassName = sAssemblyName + "." + sClassName + "," + sAssemblyName;
            t = GetTypeByClassName(sFullClassName, true, true);
        }
        return t;
    }

    /// <summary>
    /// 调用oObject函数的RemoveFromRdb函数,删除对象
    /// </summary>
    /// <param name="oObject"></param>
    /// <returns>删除成功返回True; 失败返回False</returns>
    /// <remarks></remarks>
    public static bool RemoveRdbObject(ref ObjectX oObject, string sClassName)
    {
        Type tType = GetClassType(sClassName);
        MethodInfo[] methods = tType.GetMethods();
        foreach (MethodInfo oFunc in methods)
        {
            if (MyString.StrEqual(oFunc.Name, "RemoveFromRdb", true))
            {
                return Convert.ToBoolean(oFunc.Invoke(oObject, null));
            }
        }
        return false;
    }

    /// <summary>
    /// 导出一个对象的JSON字符串
    /// </summary>
    /// <param name="sRdbTable"></param>
    /// <param name="vCodeValue"></param>
    /// <param name="sItemName"></param>
    /// <param name="sPrimkey"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string ExportObjectAsJSON(string sRdbTable, object vCodeValue, string sItemName = "", string sPrimkey = "CODE")
    {
        ObjectX oObject = GetRdbObjectEx2(sRdbTable, vCodeValue, null, sPrimkey);
        if (oObject != null)
        {
            return oObject.MakeJsonCode(sItemName);
        }
        else
        {
            return "";
        }
    }

    #endregion
}