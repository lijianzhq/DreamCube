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
    /// ��¼�� 
    /// </summary> 
    public class RdbRecord
    {
        #region "�ֶ�"

        private DataTable m_DataTable = null;
        private DataTable m_schemaTable = null; //��ܹ���Ϣ
        private DataRow m_Row = null;
        private bool m_ReadOnly = false;
        private String m_PrimaryKey = "";
        private RdbConnMgr connMgr = null; //���Ӷ���
        private Boolean? isExistInDB = null;//�Ƿ�������ݿ���
        private List<String> m_ReadOnlyFieldNames = null;
        private List<Boolean> m_ReadOnlyFieldSets = null;

        #endregion

        #region "����"

        /// <summary>
        /// ��װ��DataRow����
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
        /// �ֶθ��� 
        /// </summary> 
        public int FieldCount
        {
            get
            {
                return m_DataTable.Columns.Count;
            }
        }

        /// <summary>
        /// �Ƿ�ֻ��
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
        /// ��¼��������(ע��: ��Ϊһ����ֻ��һ������) 
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
                            //�����ȷʵû������Primary key,��ȡ��һ���ֶ���Ϊ����
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
        /// ��¼�����ı���
        /// </summary>
        public string TableName
        {
            get
            {
                return this.m_DataTable.TableName;
            }
        }

        /// <summary>
        /// ���Ӷ���
        /// </summary>
        public RdbConnMgr ConnMgr
        {
            get
            {
                return this.connMgr;
            }
        }

        /// <summary>
        /// ��ܹ���Ϣ
        /// </summary>
        public DataTable SchemaTable
        {
            get
            {
                return this.m_schemaTable;
            }
        }

        #endregion

        #region "����ʵ������"

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="oTable">��¼�����ı�</param>
        /// <param name="schemaTable">��ܹ���Ϣ��</param>
        /// <param name="oRow">��װ�ļ�¼����</param>
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
        /// ��ȡ��iIndex���ֶ�, ע��: ��һ���ֶε������0 
        /// </summary> 
        /// <returns></returns> 
        public RdbField GetField(int iIndex)
        {
            if (iIndex < 0 || iIndex >= this.FieldCount) return null;
            else return new RdbField(this, m_Row, m_DataTable.Columns[iIndex]);
        }

        /// <summary>
        /// ��¼�Ƿ��Ѵ��������ݿ���
        /// </summary>
        /// <param name="noCache">�Ƿ�ÿ�ε��ô˷������������ݿ⣬false��ÿ�ζ��������ݿ��ȡ���µ�ֵ</param>
        /// <returns></returns>
        public bool IsExistedInDatabase(Boolean noCache = false)
        {
            if (isExistInDB == null || noCache)
            {
                string sPrimaryKey = this.PrimaryKey;
                RdbField oField = this.GetField(sPrimaryKey);
                if (oField == null)
                {
                    //������صĽ���в���������, �򱣴��ʱ���޷�ȷ���Ƿ����¼�¼�����ϼ�¼ 
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
        /// ��ȡ������ĳ����ǿ�ʼ�������У�����һ�������У�A_1,A_2,C_1,C_2����ȡ��A_��ʼ�������У���������A_1��A_2
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
        /// ͨ�����ֻ�ȡ�ֶζ��� 
        /// </summary> 
        /// <param name="sFieldName">�ֶ�����</param> 
        /// <returns></returns> 
        public RdbField GetField(string sFieldName)
        {
            if (m_DataTable.Columns[sFieldName] == null) return null;
            else return new RdbField(this, m_Row, m_DataTable.Columns[sFieldName]);
        }

        /// <summary> 
        /// �����ݿ�ɾ������¼ 
        /// </summary> 
        /// <returns></returns> 
        public bool Remove()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Remove()�����쳣:��¼��ֻ����!");
                RdbField oField = this.GetField(this.PrimaryKey);
                string sWhere = this.PrimaryKey + "=" + SqlHelper.FormatSqlValue(oField.Value, oField.DataType);
                string sSql = "DELETE FROM " + this.TableName + " WHERE " + sWhere;
                return ConnMgr.ExecSQL(sSql);
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Remove()��������:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// ����¼���浽���ݿ��� 
        /// </summary> 
        /// <returns></returns> 
        public bool SaveUpdate()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()�����쳣����¼��ֻ���ģ�");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveUpdateCommand(), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()��������:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// ����¼���浽���ݿ��� 
        /// </summary> 
        /// <returns></returns> 
        public bool SaveInsert()
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()�����쳣����¼��ֻ���ģ�");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveInsertCommand(), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()��������:" + e.Message);
                return false;
            }
        }

        /// <summary> 
        /// ����¼���浽���ݿ��� 
        /// </summary> 
        /// <param name="bSaveAsNewRecordAnyWay">������ζ�Ҫ����Ϊ�¼�¼</param> 
        /// <returns></returns> 
        public bool Save(bool bSaveAsNewRecordAnyWay = false)
        {
            try
            {
                if (this.ReadOnly) throw new Exception("RdbRecord->Save()�����쳣����¼��ֻ���ģ�");
                int iReturn = ConnMgr.ExecCommand(this.MakeSaveCommand(bSaveAsNewRecordAnyWay), this.MakeSaveParameters());
                if (iReturn > 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("RdbRecord->Save()��������:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// ���ɶ���JSON����
        /// </summary>
        /// <param name="sItemName">ָ��ֻ������Щ�ֶ�,�ձ�ʾ���������ֶ�,����ֶ�֮���ԷֺŸ���</param>
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
        /// ����: ���ɼ�¼��XML
        /// </summary>
        /// <param name="sItemTagName">�ֶε�Xml TagName</param>
        /// <param name="sExportedItemNames">ָ��ֻ������Щ�ֶ�,�ձ�ʾ���������ֶ�,����ֶ�֮���ԷֺŸ���</param>
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
        /// �Ѽ�¼д��ҳ��ؼ���
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
                    //���ڶ������ֶ�,���ҪWrite��ҳ����,����Ϊ����һ���ַ����������ֶ�,���ɳ��ַ���֮����д��
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
        /// ����ֻ���ֶ�
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
        /// �ж��ǲ���ֻ���ֶ� 
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

        #region "˽��ʵ������"

        /// <summary>
        /// ����: ���ɼ�¼��������
        /// </summary>
        /// <param name="bSaveAsNewRecordAnyWay">�������ҲҪ����Ϊ�¼�¼</param>
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
        /// ����: ���ɼ�¼��������
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
        /// ����: ���ɼ�¼��������
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
        /// ����SQL�����������
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