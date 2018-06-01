using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// 记录类 
    /// </summary> 
    public class RdbRecord
    {
        #region "字段"

        private DataTable m_DataTable = null;
        private DataTable m_schemaTable = null; //表架构信息
        private DataRow m_Row = null;
        private bool m_ReadOnly = false;
        private String m_PrimaryKey = "";
        private RdbConnMgr connMgr = null; //链接对象
        private Boolean? isExistInDB = null;//是否存在数据库中
        private List<String> m_ReadOnlyFieldNames = null;
        private List<Boolean> m_ReadOnlyFieldSets = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 包装的DataRow对象
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataRow DataRow
        {
            get
            {
                return m_Row;
            }
        }

        /// <summary> 
        /// 字段个数 
        /// </summary> 
        public int FieldCount
        {
            get
            {
                return m_DataTable.Columns.Count;
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ReadOnly
        {
            get
            {
                return m_ReadOnly;
            }
        }

        /// <summary> 
        /// 记录主键名称(注意: 认为一个表单只有一个主键) 
        /// </summary> 
        public string PrimaryKey
        {
            get
            {
                if (m_PrimaryKey != "")
                {
                    return m_PrimaryKey;
                }
                if (SchemaTable.PrimaryKey.Length > 0)
                {
                    m_PrimaryKey = SchemaTable.PrimaryKey[0].Caption;
                }
                else
                {
                    using (OleDbDataAdapter oAdapter = new OleDbDataAdapter("SELECT * FROM " + this.TableName + " Where 1=0", ConnMgr.OledbConnObject))
                    {
                        DataTable oTable = new DataTable();
                        oAdapter.SelectCommand.Transaction = ConnMgr.Transaction;
                        oAdapter.FillSchema(oTable, SchemaType.Source);
                        if (oTable.PrimaryKey.Length > 0)
                        {
                            m_PrimaryKey = oTable.PrimaryKey[0].Caption;
                        }
                        else
                        {
                            //如果表单确实没有设置Primary key,则取第一个字段作为主键
                            m_PrimaryKey = this.GetField(0).Name;
                        }
                    }
                }
                return m_PrimaryKey;
            }
            set
            {
                m_PrimaryKey = value;
            }
        }

        /// <summary>
        /// 记录所属的表名
        /// </summary>
        public string TableName
        {
            get
            {
                return this.m_DataTable.TableName;
            }
        }

        /// <summary>
        /// 链接对象
        /// </summary>
        public RdbConnMgr ConnMgr
        {
            get
            {
                return this.connMgr;
            }
        }

        /// <summary>
        /// 表架构信息
        /// </summary>
        public DataTable SchemaTable
        {
            get
            {
                return this.m_schemaTable;
            }
        }

        #endregion

        #region "公共实例方法"

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oTable">记录所属的表</param>
        /// <param name="schemaTable">表架构信息表</param>
        /// <param name="oRow">包装的记录对象</param>
        /// <param name="bReadOnly"></param>
        /// <param name="sPrimaryKey"></param>
        /// <remarks></remarks>
        public RdbRecord(DataTable oTable, DataTable schemaTable, DataRow oRow, bool bReadOnly, string sPrimaryKey = "")
        {
            this.m_schemaTable = schemaTable;
            this.m_DataTable = oTable;
            this.m_Row = oRow;
            this.m_ReadOnly = bReadOnly;
            this.m_PrimaryKey = sPrimaryKey;
        }

        /// <summary> 
        /// 获取第iIndex个字段, 注意: 第一个字段的序号是0 
        /// </summary> 
        /// <returns></returns> 
        public RdbField GetField(int iIndex)
        {
            if (iIndex < 0 || iIndex >= this.FieldCount) return null;
            else return new RdbField(this, m_Row, m_DataTable.Columns[iIndex]);
        }

        /// <summary>
        /// 纪录是否已存在于数据库中
        /// </summary>
        /// <param name="noCache">是否每次调用此方法都访问数据库，false：每次都访问数据库获取最新的值</param>
        /// <returns></returns>
        public bool IsExistedInDatabase(Boolean noCache = false)
        {
            if (isExistInDB == null || noCache)
            {
                string sPrimaryKey = this.PrimaryKey;
                RdbField oField = this.GetField(sPrimaryKey);
                if (oField == null)
                {
                    //如果返回的结果中不包含主键, 则保存的时候无法确定是否是新记录还是老记录 
                    isExistInDB = false;
                }
                else
                {
                    if (String.IsNullOrEmpty(Convert.ToString(oField.Value))) isExistInDB = false;
                    else
                    {
                        isExistInDB = this.ConnMgr.IsRecordExist(this.TableName, sPrimaryKey + "=" + SqlHelper.FormatSqlValue(oField.Value, oField.DataType), sPrimaryKey);
                    }
                }
            }
            return isExistInDB.Value;
        }

        /// <summary>
        /// 获取列名以某个标记开始的所有列；例如一个表有列：A_1,A_2,C_1,C_2；获取以A_开始的所有列，有两个：A_1和A_2
        /// </summary>
        /// <param name="sFieldNameStartPart"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RdbField[] GetFieldsStartWith(string sFieldNameStartPart, StringComparison stringComparison)
        {
            if (m_DataTable != null)
            {
                List<RdbField> columns = new List<RdbField>();
                Int32 i = 0;
                Int32 j = m_DataTable.Columns.Count;
                while (i < j)
                {
                    if (m_DataTable.Columns[i].ColumnName.StartsWith(sFieldNameStartPart, stringComparison))
                    {
                        columns.Add(new RdbField(this, m_Row, m_DataTable.Columns[i]));
                    }
                    i += 1;
                }
                return columns.ToArray();
            }
            return null;
        }

        /// <summary> 
        /// 通过名字获取字段对象 
        /// </summary> 
        /// <param name="sFieldName">字段名称</param> 
        /// <returns></returns> 
        public RdbField GetField(string sFieldName)
        {
            if (m_DataTable.Columns[sFieldName] == null) return null;
            else return new RdbField(this, m_Row, m_DataTable.Columns[sFieldName]);
        }

        /// <summary> 
        /// 从数据库删除本记录 
        /// </summary> 
        /// <returns></returns> 
        public bool Remove()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Remove()发生异常:记录是只读的!");
                RdbField oField = this.GetField(this.PrimaryKey);
                string sWhere = this.PrimaryKey + "=" + SqlHelper.FormatSqlValue(oField.Value, oField.DataType);
                string sSql = "DELETE FROM " + this.TableName + " WHERE " + sWhere;
                return ConnMgr.ExecSQL(sSql);
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Remove()发生错误:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// 讲记录保存到数据库中 
        /// </summary> 
        /// <returns></returns> 
        public bool SaveUpdate()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()发生异常：记录是只读的！");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveUpdateCommand(), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()发生错误:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// 讲记录保存到数据库中 
        /// </summary> 
        /// <returns></returns> 
        public bool SaveInsert()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()发生异常：记录是只读的！");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveInsertCommand(), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()发生错误:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// 讲记录保存到数据库中 
        /// </summary> 
        /// <param name="bSaveAsNewRecordAnyWay">无论如何都要保存为新纪录</param> 
        /// <returns></returns> 
        public bool Save(bool bSaveAsNewRecordAnyWay = false)
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()发生异常：记录是只读的！");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveCommand(bSaveAsNewRecordAnyWay), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()发生错误:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 生成对象JSON代码
        /// </summary>
        /// <param name="sItemName">指定只导出哪些字段,空表示导入所有字段,多个字段之间以分号隔开</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string MakeJsonCode(string sItemName = "")
        {
            StringBuilder oSb = new StringBuilder();
            RdbField oField;
            int iBound = this.FieldCount - 1;

            if (iBound == -1) return "";
            if (sItemName != "") sItemName = sItemName.Replace(',', ';');

            oSb.Append("{");
            for (int i = 0; i <= iBound; i++)
            {
                oField = this.GetField(i);
                if (oField == null) continue;
                if (string.IsNullOrEmpty(sItemName) || MyString.ContainsEx(sItemName, oField.Name, ";", true))
                {
                    oSb.Append("\"" + oField.Name + "\"" + ":" + "\"" + MyString.TurnToJs(oField.Value) + "\"");
                    oSb.Append(",");
                }
            }
            if (oSb.Length > 1) oSb.Remove(oSb.Length - 1, 1);
            oSb.Append("}");
            return oSb.ToString();
        }

        /// <summary>
        /// 功能: 生成记录的XML
        /// </summary>
        /// <param name="sItemTagName">字段的Xml TagName</param>
        /// <param name="sExportedItemNames">指定只导出哪些字段,空表示导入所有字段,多个字段之间以分号隔开</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string MakeXML(string sItemTagName = "", string sExportedItemNames = "")
        {
            int iBound = this.FieldCount - 1;
            if (iBound == -1) return "";

            if (string.IsNullOrEmpty(sItemTagName)) sItemTagName = "Field";
            if (string.IsNullOrEmpty(sExportedItemNames) == false) sExportedItemNames = sExportedItemNames.Replace(',', ';');

            StringBuilder oXml = new StringBuilder();
            RdbField oField;

            for (int i = 0; i <= iBound; i++)
            {
                oField = this.GetField(i);
                if (oField == null) continue;
                if (string.IsNullOrEmpty(sExportedItemNames) || MyString.ContainsEx(sExportedItemNames, oField.Name, ";", true))
                {
                    if (i > 0) oXml.Append(Environment.NewLine);
                    oXml.Append("<" + sItemTagName + " id=\"" + oField.Name + "\"><![CDATA[" + oField.Value + "]]></" + sItemTagName + ">");
                }
            }
            return oXml.ToString();
        }

        /// <summary>
        /// 把记录写到页面控件上
        /// </summary>
        /// <param name="sPrefix"></param>
        /// <param name="oPage"></param>
        public void WriteToPage(string sPrefix, Page oPage = null)
        {
            if (this.m_DataTable == null) return;
            if (oPage == null) oPage = (System.Web.UI.Page)HttpContext.Current.Handler;

            if (sPrefix.EndsWith("_") == false) sPrefix += "_";

            RdbField oField;
            for (int i = 0; i <= this.FieldCount - 1; i++)
            {
                if (this.GetField(i) == null) continue;
                oField = (RdbField)this.GetField(i);
                if (oField.IsBinaryField)
                {
                    //对于二进制字段,如果要Write到页面上,则认为这是一个字符串二进制字段,换成成字符串之后再写入
                    MyPage.SetItemValue(oPage, sPrefix + oField.Name, oField.ExtractAsString());
                }
                else if (oField.IsDateTimeField)
                {
                    MyPage.SetItemValue(oPage, sPrefix + oField.Name, MyDatetime.GetDatePart(this.GetField(oField.Name).Value.ToString()));
                }
                else
                {
                    MyPage.SetItemValue(oPage, sPrefix + oField.Name, this.GetField(oField.Name).Value.ToString());
                }
            }
        }

        /// <summary>
        /// 设置只读字段
        /// </summary>
        /// <remarks></remarks>
        public void SetReadOnlyField(string sFieldName, bool bSet = true)
        {
            m_ReadOnlyFieldNames = m_ReadOnlyFieldNames == null ? new List<String>() : m_ReadOnlyFieldNames;
            m_ReadOnlyFieldSets = m_ReadOnlyFieldSets == null ? new List<Boolean>() : m_ReadOnlyFieldSets;
            Object sTempFieldName = sFieldName.ToUpper();
            Object bTempSet = bSet;
            if (m_ReadOnlyFieldNames == null || m_ReadOnlyFieldNames.IndexOf(sFieldName) == -1)
            {
                m_ReadOnlyFieldNames.Add(sFieldName.ToUpper());
                m_ReadOnlyFieldSets.Add(bSet);
            }
        }

        /// <summary> 
        /// 判断是不是只读字段 
        /// </summary> 
        /// <param name="sFieldName"></param> 
        public bool IsReadOnlyField(string sFieldName)
        {
            if (m_ReadOnlyFieldNames == null)
            {
                return false;
            }
            else
            {
                int iIndex = m_ReadOnlyFieldNames.IndexOf(sFieldName.ToUpper());
                if (iIndex != -1) return Convert.ToBoolean(m_ReadOnlyFieldSets[iIndex]);
                else return false;
            }
        }

        #endregion

        #region "私有实例方法"

        /// <summary>
        /// 功能: 生成记录保存命令
        /// </summary>
        /// <param name="bSaveAsNewRecordAnyWay">无论如何也要保存为新纪录</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string MakeSaveCommand(bool bSaveAsNewRecordAnyWay = false)
        {
            string sCommand = "";
            int i = 0;
            int iCount = 0;
            string[] aFieldNames = new string[this.FieldCount - 1];
            string[] aParamNames = new string[this.FieldCount - 1];
            for (i = 0; i <= this.FieldCount - 1; i++)
            {
                if (this.IsReadOnlyField(this.GetField(i).Name) == false && (!this.GetField(i).ReadOnly) && this.GetField(i).IsBinaryField == false)
                {
                    aFieldNames[iCount] = this.GetField(i).Name;
                    aParamNames[iCount] = "?";
                    iCount += 1;
                }
            }

            if (bSaveAsNewRecordAnyWay || !this.IsExistedInDatabase())
            {
                sCommand = "INSERT INTO " + this.TableName + " (" + String.Join(",", aFieldNames, 0, iCount) + ") VALUES (" + String.Join(",", aParamNames, 0, iCount) + ")";
            }
            else
            {
                string[] aTemps = new string[iCount - 1];
                for (i = 0; i <= iCount - 1; i++)
                {
                    aTemps[i] = aFieldNames[i] + "=" + aParamNames[i];
                }
                RdbField oField = this.GetField(this.PrimaryKey);
                string sWhere = this.PrimaryKey + "=" + SqlHelper.FormatSqlValue(oField.Value, oField.DataType);
                sCommand = "UPDATE " + this.TableName + " SET " + String.Join(",", aTemps) + " WHERE " + sWhere;
            }

            return sCommand;
        }

        /// <summary>
        /// 功能: 生成记录保存命令
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string MakeSaveUpdateCommand()
        {
            int i = 0;
            int iCount = 0;
            string[] aFieldNames = new string[this.FieldCount - 1];
            string[] aParamNames = new string[this.FieldCount - 1];
            for (i = 0; i <= this.FieldCount - 1; i++)
            {
                if (this.IsReadOnlyField(this.GetField(i).Name) == false && (!this.GetField(i).ReadOnly) && this.GetField(i).IsBinaryField == false)
                {
                    aFieldNames[iCount] = this.GetField(i).Name;
                    aParamNames[iCount] = "?";
                    iCount += 1;
                }
            }

            string[] aTemps = new string[iCount - 1];
            for (i = 0; i <= iCount - 1; i++)
            {
                aTemps[i] = aFieldNames[i] + "=" + aParamNames[i];
            }
            RdbField oField = this.GetField(this.PrimaryKey);
            string sWhere = this.PrimaryKey + "=" + SqlHelper.FormatSqlValue(oField.Value, oField.DataType);
            string sCommand = "UPDATE " + this.TableName + " SET " + String.Join(",", aTemps) + " WHERE " + sWhere;
            return sCommand;
        }

        /// <summary>
        /// 功能: 生成记录保存命令
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string MakeSaveInsertCommand()
        {
            int i = 0;
            int iCount = 0;
            string[] aFieldNames = new string[this.FieldCount - 1];
            string[] aParamNames = new string[this.FieldCount - 1];
            for (i = 0; i <= this.FieldCount - 1; i++)
            {
                if (this.IsReadOnlyField(this.GetField(i).Name) == false && (!this.GetField(i).ReadOnly) && this.GetField(i).IsBinaryField == false)
                {
                    aFieldNames[iCount] = this.GetField(i).Name;
                    aParamNames[iCount] = "?";
                    iCount += 1;
                }
            }
            string sCommand = "INSERT INTO " + this.TableName + " (" + String.Join(",", aFieldNames, 0, iCount) + ") VALUES (" + String.Join(",", aParamNames, 0, iCount) + ")";
            return sCommand;
        }

        /// <summary>
        /// 构造SQL保存变量对象
        /// </summary>
        /// <returns></returns>
        private List<OleDbParameter> MakeSaveParameters()
        {
            List<OleDbParameter> aParameters = new List<OleDbParameter>();
            RdbField oField;
            OleDbParameter oParam;
            int i;
            Int32 length = this.FieldCount;
            for (i = 0; i <= length - 1; i++)
            {
                oField = this.GetField(i);
                if (!this.IsReadOnlyField(oField.Name) && !oField.ReadOnly)
                {
                    oParam = new OleDbParameter();
                    oParam.Value = oField.Value;
                    if (oField.IsBinaryField) oParam.DbType = DbType.Binary;
                    aParameters.Add(oParam);
                }
            }
            if (i == 0) return null;
            else return aParameters;
        }

        #endregion
    }
}