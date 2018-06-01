using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using DreamCube.Foundation.XmlHelper;
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Objects;
using DreamCube.Foundation.Basic.Objects.EqualityComparers;
using DreamCube.Foundation.Basic.Enums;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// ���ݿ����ӹ����ֻࣨ���ڵ��߳��е��ã�
    /// </summary> 
    public class RdbConnMgr
    {
        #region "�ֶ�"

        private RdbConnection m_ConnObj = null;
        private OleDbCommand m_Command = null;
        public ConnectParam m_ConnParam = null;
        private int m_LockKey = 0;
        private bool m_IsValid = true;
        private string m_LastestErrorMsg = "";
        
        /// <summary>
        /// ���建����
        /// </summary>
        private DictionaryCachePool<String, DataTable> dataTableCachePool =
            new DictionaryCachePool<String, DataTable>(new StringEqualityIgnoreCaseComparerGeneric());

        #endregion

        #region "����"

        /// <summary> 
        /// OleDbConnection���� 
        /// </summary> 
        public OleDbConnection OledbConnObject
        {
            get
            {
                return this.m_ConnObj.m_OledbConn;
            }
        }

        /// <summary>
        /// ����Transaction����
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public OleDbTransaction Transaction
        {
            get
            {
                return m_Command.Transaction;
            }
        }

        /// <summary>
        /// �Ƿ���
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsLocked
        {
            get
            {
                if (m_LockKey == 0)
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
        /// ��ѯ�ձ�Ϊ�˻�ȡ�����sql���ģ��
        /// Ĭ��Ϊ��select * from {0} where 1=0
        /// </summary>
        protected virtual String SelectEmptyDatableSqlTemplate
        {
            get
            {
                return "select * from {0} where 1=0";
            }
        }

        #endregion

        #region "˽��ʵ������"

        /// <summary> 
        /// ���캯��2(����ָ�������Ӳ���) 
        /// </summary> 
        /// <param name="oConnParam">���ݿ����Ӳ���������Ϊnull���������null������Զ��Ļ�ȡĬ�ϵ����Ӳ�����</param> 
        internal RdbConnMgr(ConnectParam oConnParam)
        {
            this.InitObject(oConnParam);
        }

        #endregion

        #region "����ʵ������"

        /// <summary>
        /// �������ɹ��򷵻�iKey
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Lock()
        {
            if (m_LockKey != 0)
            {
                return 0;
            }
            else
            {
                m_LockKey = Convert.ToInt32(MyRand.NextString_Number(3));
                return m_LockKey;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="iKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Unlock(int iKey)
        {
            if (m_LockKey == iKey)
            {
                m_LockKey = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <remarks></remarks>
        public void Open()
        {
            if (m_ConnObj != null)
            {
                m_ConnObj.Open();
            }
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        /// <remarks></remarks>
        public bool Close()
        {
            try
            {
                if (this.IsLocked)
                {
                    return false;
                }
                if (m_ConnObj != null)
                {
                    m_ConnObj.Close();
                }
                if (m_Command != null)
                {
                    m_Command.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog("RdbConnMgr��Close()�׳��쳣:" + ex.Message);
                return false;
            }
            finally
            {
                if (this.IsLocked == false)
                {
                    this.m_ConnObj = null;
                    this.m_Command = null;
                }
            }
        }

        /// <summary>
        /// �ж������Ƿ�ر�
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsClose
        {
            get
            {
                if (this.m_ConnObj == null || m_ConnObj.State == ConnectionState.Closed) return true;
                else return false;
            }
        }

        /// <summary> 
        /// ������Ч��־ 
        /// </summary> 
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
            set
            {
                m_IsValid = value;
            }
        }

        /// <summary>
        /// ��ȡ�����ַ���
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ConnString
        {
            get
            {
                if (m_ConnParam != null)
                {
                    return m_ConnParam.MakeConnectString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// ��ȡOleDbCommand����, ���������ؽ�
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        private OleDbCommand CommandX
        {
            get
            {
                if (this.IsClose)
                {
                    this.InitObject(this.m_ConnParam);
                }
                return m_Command;
            }
        }

        /// <summary> 
        /// ���������һ���������Ϣ 
        /// </summary> 
        public string LastestErrorMsg
        {
            get
            {
                return m_LastestErrorMsg;
            }
            set
            {
                m_LastestErrorMsg = value;
            }
        }

        /// <summary> 
        /// ���ݿ����� 
        /// </summary> 
        public int DbType
        {
            get
            {
                return this.m_ConnParam.DatabaseType;
            }
        }

        /// <summary> 
        /// ���ݿ������ǲ���Sql Server 
        /// </summary> 
        public bool IsSqlServer
        {
            get
            {
                if (this.DbType >= DatabaseVersionRange.SqlServer_Min.GetHashCode() && this.DbType <= DatabaseVersionRange.SqlServer_Max.GetHashCode())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary> 
        /// ���ݿ������ǲ���Oracle 
        /// </summary> 
        public bool IsOracle
        {
            get
            {
                if (this.DbType >= DatabaseVersionRange.Oracle_Min.GetHashCode() && this.DbType <= DatabaseVersionRange.Oracle_Max.GetHashCode())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary> 
        /// ���ݿ������ǲ���Access 
        /// </summary> 
        public bool IsAccess
        {
            get
            {
                if (this.DbType >= DatabaseVersionRange.Access_Min.GetHashCode() && this.DbType <= DatabaseVersionRange.Access_Max.GetHashCode())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary> 
        /// ִ�в�ѯ���, ��ȡ��¼������ 
        /// </summary> 
        /// <param name="sSql">Ҫִ�еĲ�ѯ���</param> 
        /// <param name="bReadOnly">�Ƿ�Ҫ�󷵻صļ�¼����ֻ����</param> 
        /// <param name="sPrimaryKey">��¼����������</param> 
        /// <returns></returns> 
        public RdbRecordSet GetRecordSet(string sSql, bool bReadOnly = true, string sPrimaryKey = "")
        {
            try
            {
                CommandX.CommandText = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;

                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);
                //������ 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //��ܹ���Ϣ 
                DataTable oSchemaTable = new DataTable();
                oAdapter.FillSchema(oSchemaTable, SchemaType.Source);

                String tableName = MyString.GetTableNameFromSQL(sSql);
                if (!dataTableCachePool.ContainsKey(tableName))
                    dataTableCachePool.TryAdd(tableName, oSchemaTable, CollectionsAddOper.IgnoreIfExist);

                return new RdbRecordSet(this, oTable, oSchemaTable, bReadOnly, sPrimaryKey, sSql);
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog(String.Format("GetRecordSet()��������{0}��{1}", e.Message, "SQL=" + sSql));
                return null;
            }
        }

        /// <summary>
        /// ����SQL��ȡ���ݱ����
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="bReadOnly"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetDataTable(string sSql, bool bReadOnly = true)
        {
            try
            {
                CommandX.CommandText = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;

                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);

                //������ 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                String tableName = MyString.GetTableNameFromSQL(sSql);
                if (!dataTableCachePool.ContainsKey(tableName))
                {
                    //��ܹ���Ϣ 
                    DataTable oSchemaTable = new DataTable();
                    oAdapter.FillSchema(oSchemaTable, SchemaType.Source);
                    dataTableCachePool.TryAdd(tableName, oSchemaTable, CollectionsAddOper.IgnoreIfExist);
                }
                return oTable;
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("GetDataTable()��������:" + e.Message);
                MyLog.MakeLog(sSql);
                return null;
            }
        }

        /// <summary>
        /// ��ȡһ������ϲ��� sWhere �ļ�¼��,sWhereΪ��ʱ,�������м�¼��
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int GetTableRecordCount(string sTableName, string sWhere)
        {
            int iCount = 0;
            string sSql = string.Format("select count(1) from {0}{1}", sTableName, string.IsNullOrEmpty(sWhere) ? string.Empty : string.Format(" where {0}", sWhere));
            DataTable dtCount = GetDataTable(sSql, true);
            if (dtCount != null)
            {
                if (dtCount.Rows.Count > 0)
                {
                    iCount = Convert.ToInt32(dtCount.Rows[0][0].ToString());
                }
            }
            return iCount;
        }

        /// <summary> 
        /// ��ȡ��¼
        /// </summary> 
        /// <param name="sSql"></param> 
        /// <param name="bReadOnly"></param> 
        /// <param name="sPrimKey"></param>
        /// <param name="bDontShowErrorLog">��ʹ����Ҳ��Ҫ��ʾ������Ϣ</param>
        /// <returns></returns> 
        public RdbRecord GetRecord(string sSql, bool bReadOnly = true, string sPrimKey = "", bool bDontShowErrorLog = false)
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;

                CommandX.CommandText =  SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);
                //������ 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //��ܹ���Ϣ 
                DataTable oSchemaTable = new DataTable();
                oAdapter.FillSchema(oSchemaTable, SchemaType.Source);

                RdbRecordSet oRecordSet = new RdbRecordSet(this, oTable, oSchemaTable, bReadOnly, sPrimKey);

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300)
                {
                    MyLog.MakeLog("�Ϻ�ʱ(" + iMillSecond + ")�Ĳ�ѯ��" + sSql);
                }

                return oRecordSet.GetRecord(0);
            }
            catch (Exception e)
            {
                if (bDontShowErrorLog == false)
                {
                    this.m_LastestErrorMsg = e.Message;
                    MyLog.MakeLog("GetRecord()��������:" + e.Message);
                    MyLog.MakeLog(sSql);
                }
                return null;
            }
        }

        /// <summary> 
        /// ��ȡһ����(��ͼ)��ǰN����¼. ��Ϊ��ȡһ�����ǰN����¼���﷨�ڸ���DBMS�е��﷨��һ��,�����ṩ�ú������������� 
        /// </summary> 
        /// <param name="sTable">Ҫ��ȡ���ݵı����ͼ��</param> 
        /// <param name="iCount">Ҫ��ȡ������</param> 
        /// <param name="sColumn">������Щ��, ��ʽ��: Location, Area, Owner. *��ʾ���е���</param> 
        /// <param name="bReadOnly"></param> 
        /// <param name="sWhere">��������, ��: Code>20</param> 
        /// <param name="sOrderBy">���򷽷�, ��: Code ASC </param> 
        /// <param name="bDistinct">�Ƿ�Ҫȥ���ظ�ֵ</param> 
        /// <returns></returns> 
        public RdbRecordSet GetTopNthRecords(string sTable, int iCount, string sColumn, bool bReadOnly = true, string sWhere = "", string sOrderBy = "", bool bDistinct = false)
        {
            try
            {
                string sSQL = "";
                if (sColumn == "") sColumn = "*";

                if (this.DbType <= DatabaseVersionRange.Oracle_Max.GetHashCode() && this.DbType >= DatabaseVersionRange.Oracle_Min.GetHashCode())
                {
                    //ȷ��ֵ���ظ�
                    if (bDistinct)
                    {
                        sSQL = "SELECT " + sColumn + " FROM (SELECT DISTINCT " + sColumn + " FROM " + sTable + ") WHERE RowNum<=" + iCount.ToString();
                    }
                    else
                    {
                        sSQL = "SELECT " + sColumn + " FROM " + sTable + " WHERE RowNum<=" + iCount.ToString();
                    }
                }
                else
                {
                    //ȷ��ֵ���ظ�
                    if (bDistinct)
                    {
                        sColumn = "DISTINCT(" + sColumn + ")";
                    }
                    sSQL = "SELECT TOP " + iCount.ToString() + " " + sColumn + " FROM " + sTable;
                }

                if (string.IsNullOrEmpty(sWhere) == false)
                {
                    sSQL += " AND(" + sWhere + ")";
                }

                if (string.IsNullOrEmpty(sOrderBy) == false)
                {
                    sSQL += " ORDER BY " + sOrderBy;
                }

                return this.GetRecordSet(sSQL, bReadOnly);
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("GetTopNthRecords()��������:" + e.Message);
                return null;
            }
        }

        /// <summary> 
        /// ��ҳȡ����: ��ȡָ����Χ������ 
        /// </summary> 
        /// <param name="sSQL"></param> 
        /// <param name="iStart">��СΪ1</param> 
        /// <param name="iLength"></param> 
        /// <returns></returns> 
        public List<RdbRecord> GetRecordInRange(string sSQL, int iStart, int iLength)
        {
            //�������ݿ����Ӷ��� 
            RdbRecordSet oRecSet = GetRecordSet(sSQL, true);
            RdbRecord oRecord;
            List<RdbRecord> aRecs = new List<RdbRecord>();
            if (oRecSet == null) return aRecs;
            for (int i = iStart - 1; i <= iStart - 1 + iLength; i++)
            {
                oRecord = oRecSet.GetRecord(i);
                if (oRecord == null)
                    continue;
                aRecs.Add(oRecord);
            }
            return aRecs;
        }

        /// <summary> 
        /// ��ҳȡ����: ��ȡ��Nҳ������ 
        /// </summary>
        /// <param name="sSQL"></param> 
        /// <param name="iPageSize"></param> 
        /// <param name="iNthPage"></param> 
        /// <returns></returns> 
        public ArrayList GetNthPageRecords(string sSQL, int iPageSize, int iNthPage)
        {
            //�������ݿ����Ӷ��� 
            RdbRecordSet oRecSet = GetRecordSet(sSQL, true);
            RdbRecord oRecord;
            ArrayList aRecs = new ArrayList();
            if (oRecSet.HasRecord)
            {
                //����ҳ���С 
                oRecSet.PageSize = iPageSize;

                //���õ�ǰҳ 
                oRecSet.CurPage = iNthPage;
            }

            for (int i = 0; i <= oRecSet.PageSize - 1; i++)
            {
                oRecord = oRecSet.GetRecordInCurPage(i);
                if (oRecord == null) continue;
                aRecs.Add(oRecord);
            }
            return aRecs;
        }

        /// <summary> 
        /// ��ȡָ������һ���¼�¼������ֵ(������Oracle���ݿ�) 
        /// </summary> 
        /// <param name="sTable"></param> 
        /// <param name="sCodeField"></param> 
        /// <returns></returns> 
        public object GetNewRecordCodeValue(string sTable, string sCodeField = "CODE", string sSequenceName = "")
        {
            if (!this.IsOracle) return "";
            try
            {
                if (string.IsNullOrEmpty(sSequenceName))
                {
                    sSequenceName = sTable + "_" + sCodeField + "_seq";
                }
                string sSQLSeq = "select " + sSequenceName + ".NextVal From dual";
                string sWhere = "";
                string sReturnCode = "";
                while (true)
                {
                    sReturnCode = Convert.ToString(this.GetValue(sSQLSeq, true));
                    if (string.IsNullOrEmpty(sReturnCode)) return "";

                    sWhere = sCodeField + "=" + this.FormatSqlValue(sTable, sCodeField, sReturnCode);

                    //��������ж�,��Ҫ��Ϊ�˱����ȡ���� sReturnCode �Ѿ����������ݿ���
                    if (this.IsRecordExist(sTable, sWhere) == false)
                    {
                        return sReturnCode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// �������еĵ�ǰֵ
        /// </summary>
        /// <param name="sSequenceName"></param>
        /// <param name="iCurValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SetSequenceValue(string sSequenceName, int iCurValue)
        {
            try
            {
                if (!this.IsOracle) return false;

                int iNextValue = Convert.ToInt32(this.GetValue("SELECT " + sSequenceName + ".NEXTVAL FROM DUAL"));
                if (iNextValue != iCurValue)
                {
                    this.ExecSQL(string.Format("Alter Sequence {0} Increment By {1}", sSequenceName, iCurValue - iNextValue - 1));
                    this.ExecSQL(string.Format("SELECT {0}.NEXTVAL FROM DUAL", sSequenceName));
                    this.ExecSQL(string.Format("Alter Sequence {0} Increment By 1", sSequenceName));
                }
                return true;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
                return false;
            }
        }

        /// <summary> 
        /// ֱ�ӷ��ز�ѯֵ 
        /// </summary> 
        /// <param name="sSql"></param> 
        /// <param name="bDontShowErrorLog">��ʹ����Ҳ��Ҫ���������־</param> 
        /// <returns></returns> 
        public object GetValue(string sSql, bool bDontShowErrorLog = false)
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;

                CommandX.CommandText = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;
                object vValue = CommandX.ExecuteScalar();

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300)
                {
                    MyLog.MakeLog("�Ϻ�ʱ(" + iMillSecond + ")�Ĳ�ѯ��" + sSql);
                }

                if (vValue != null)
                {
                    if (vValue.GetType().Equals(typeof(System.DBNull))) return "";
                    else return vValue;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                if (bDontShowErrorLog == false)
                {
                    this.m_LastestErrorMsg = e.Message;
                    MyLog.MakeLog("GetValue()��������:" + e.Message);
                    MyLog.MakeLog(sSql);
                }
                return null;
            }
        }

        /// <summary>
        /// ִ�в�ѯ,ֱ�ӷ��ز�ѯֵ,��Ϊ��ѯ���ܴ��ڶ�����¼,���Է���һ������
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="bReturnMultiColumns"></param>
        /// <param name="sValueDiv"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ArrayList GetValues(string sSql, bool bReturnMultiColumns = false, string sValueDiv = "|")
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;

                CommandX.CommandText = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;

                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);

                //������ 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300) MyLog.MakeLog("�Ϻ�ʱ(" + iMillSecond + ")�Ĳ�ѯ��" + sSql);

                //��䷵��ֵ
                if (oTable.Rows.Count > 0)
                {
                    string sValue;
                    object sTemp;
                    ArrayList aValues = new ArrayList();

                    for (int i = 0; i <= oTable.Rows.Count - 1; i++)
                    {
                        if (bReturnMultiColumns && oTable.Columns.Count > 1)
                        {
                            sValue = "";
                            for (int j = 0; j <= oTable.Columns.Count - 1; j++)
                            {
                                sTemp = oTable.Rows[i][j];
                                if (sTemp.GetType().Equals(typeof(System.DBNull)))
                                {
                                    sValue = MyString.Connect(sValue, "", sValueDiv);
                                }
                                else
                                {
                                    sValue = MyString.Connect(sValue, Convert.ToString(oTable.Rows[i][j]), sValueDiv);
                                }
                            }

                            aValues.Add(sValue);
                        }
                        else
                        {
                            sTemp = oTable.Rows[i][0];
                            if (sTemp.GetType().Equals(typeof(System.DBNull)))
                            {
                                aValues.Add("");
                            }
                            else
                            {
                                aValues.Add(oTable.Rows[i][0]);
                            }
                        }
                    }
                    return aValues;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("GetValues()��������:" + e.Message);
                MyLog.MakeLog(sSql);
                return null;
            }
        }

        /// <summary>
        /// �жϼ�¼�Ƿ����
        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsRdbRecordExisted(string sSQL)
        {
            try
            {
                DataTable oTable = this.GetDataTable(sSQL, true);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("IsRdbRecordExisted()��������:" + e.Message);
                MyLog.MakeLog("sSQL=" + sSQL);
            }
            return false;
        }

        public bool IsRecordExist(string sTable, string sWhere, string sColumnName = "*")
        {
            return this.IsRdbRecordExisted(string.Format("SELECT " + sColumnName + " FROM {0} WHERE {1}", sTable, sWhere));
        }

        /// <summary> 
        /// Ϊһ������һ���¼�¼ 
        /// </summary> 
        /// <param name="sTable"></param> 
        /// <returns></returns> 
        public RdbRecord CreateNewRecord(string sTable)
        {
            try
            {
                RdbRecordSet oRecordset = this.GetTopNthRecords(sTable, 0, "*", false);
                return oRecordset.CreateNewRecord();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��ȡ���ݿ����б�����
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ArrayList GetAllTableNames()
        {
            try
            {
                if (this.IsOracle)
                {
                    return GetValues("SELECT TABLE_NAME FROM USER_TABLES ORDER BY TABLE_NAME");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary> 
        /// ��ȡ������������� 
        /// </summary> 
        /// <param name="sTableName"></param> 
        /// <returns></returns> 
        public string GetTableColumnNames(string sTableName)
        {
            try
            {
                string sColumnNames = "";
                RdbRecord oRecord = this.CreateNewRecord(sTableName);
                for (int i = 0; i <= 100000; i++)
                {
                    RdbField oField = oRecord.GetField(i);
                    if (oField == null)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    else
                    {
                        sColumnNames = MyString.Connect(sColumnNames, oField.Name, ";");
                    }
                }
                return sColumnNames;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary> 
        /// ִ��һ��SQL��� 
        /// </summary> 
        /// <param name="sSql">��ִ�е�SQL���</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql)
        {
            int iEffectedRecordCount = 0;
            return ExecSQL(sSql, ref iEffectedRecordCount);
        }

        /// <summary> 
        /// ִ��һ��SQL��� 
        /// </summary> 
        /// <param name="sSql">��ִ�е�SQL���</param> 
        /// <param name="iEffectedRecordCount">[OUT]ִ��SQL��Ӱ��ļ�¼��</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref int iEffectedRecordCount)
        {
            string sErrorMsg = "";
            return ExecSQL(sSql, ref iEffectedRecordCount, ref sErrorMsg);
        }

        /// <summary> 
        /// ִ��һ��SQL��� 
        /// </summary> 
        /// <param name="sSql">��ִ�е�SQL���</param> 
        /// <param name="sErrorMsg">[OUT]ִ�з�������ʱ�Ĵ�����Ϣ</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref string sErrorMsg)
        {
            int iEffectedRecordCount = 0;
            return ExecSQL(sSql, ref iEffectedRecordCount, ref sErrorMsg);
        }

        /// <summary> 
        /// ִ��һ��SQL��� 
        /// </summary> 
        /// <param name="sSql">��ִ�е�SQL���</param> 
        /// <param name="iEffectedRecordCount">[OUT]ִ��SQL��Ӱ��ļ�¼��</param> 
        /// <param name="sErrorMsg">[OUT]ִ�з�������ʱ�Ĵ�����Ϣ</param> 
        /// <param name="bDontShowErrorLog">�Ƿ�Ҫ��Log�м�¼��������ʱ�Ĵ�����Ϣ</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref int iEffectedRecordCount, ref string sErrorMsg, bool bDontShowErrorLog = false)
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;
                sSql = sSql.Trim();
                //��־��Ҫ�Żػ���վ
                if (sSql.StartsWith("--")) sSql = sSql.Substring(2);
                string sFormatSQL = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;
                CommandX.CommandText = sFormatSQL;
                iEffectedRecordCount = CommandX.ExecuteNonQuery();

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300) MyLog.MakeLog("�Ϻ�ʱ(" + iMillSecond + ")�Ĳ�ѯ��" + sSql);
                sErrorMsg = "";
                return true;
            }
            catch (Exception e)
            {
                sErrorMsg = InlineAssignHelper(ref this.m_LastestErrorMsg, e.Message);
                iEffectedRecordCount = 0;
                if (bDontShowErrorLog == false)
                {
                    MyLog.MakeLog("ExecSQL()��������:" + e.Message);
                    MyLog.MakeLog(sSql);
                }
                return false;
            }
        }

        public bool ExecSQL_WithNoErrorShow(string sSql)
        {
            string sErrorMsg = "";
            int iEffectedRecordCount = 0;
            return ExecSQL(sSql, ref iEffectedRecordCount, ref sErrorMsg, true);
        }

        /// <summary> 
        /// ִ������, ������Ӱ��ļ�¼��(����ִ��ʧ�ܷ���-1) 
        /// </summary> 
        /// <param name="sCommand">Ҫִ�еĲ�������ѯ����</param> 
        /// <param name="aParameters">�������</param> 
        /// <returns></returns> 
        public int ExecCommand(String sCommand, List<OleDbParameter> aParameters)
        {
            try
            {
                CommandX.CommandText = sCommand;
                CommandX.CommandType = CommandType.Text;
                m_Command.Parameters.Clear();
                if (aParameters != null && aParameters.Count > 0)
                {
                    foreach (OleDbParameter oParam in aParameters)
                        m_Command.Parameters.Add(oParam);
                }
                int iAffectCount = CommandX.ExecuteNonQuery();
                return iAffectCount;
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("ExecCommand()��������:" + e.Message);
                MyLog.MakeLog("Command=" + sCommand);
                return -1;
            }
            finally
            {
                //�ؽ����ӹ�������OleDbCommand����(�˾�ʵ������,��Ϊִ����һ��RdbRecord��Save()֮��,
                //OleDbCommand��ò�̫������,��ִ���������ݴ�ȡ����ʱ��������)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// ִ�д洢����, �Լ�¼������ʽ���ؽ�� 
        /// </summary> 
        /// <param name="sStoreProcName">�洢��������</param> 
        /// <param name="aParams">�������</param> 
        /// <returns></returns> 
        public RdbRecordSet ExecStoreProc(string sStoreProcName, ArrayList aParams)
        {
            try
            {
                CommandX.CommandText = sStoreProcName;
                CommandX.CommandType = CommandType.StoredProcedure;
                if (aParams != null)
                {
                    foreach (OleDbParameter oParam in aParams)
                    {
                        m_Command.Parameters.Add(oParam);
                    }
                }

                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);

                //������ 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //��ܹ���Ϣ 
                DataTable oSchemaTable = null;
                return new RdbRecordSet(this, oTable, oSchemaTable, true);
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("ExecStoreProc()��������:" + e.Message);
                return null;
            }
            finally
            {
                //�ؽ����ӹ�������OleDbCommand����(�˾�ʵ������,��Ϊִ����һ��RdbRecord��Save()֮��,
                //OleDbCommand��ò�̫������,��ִ���������ݴ�ȡ����ʱ��������)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// ִ�д洢����, ��ֵ����ʽ���ؽ�� 
        /// </summary> 
        /// <param name="sStoreProcName">�洢��������</param> 
        /// <param name="aParams">�������</param> 
        /// <returns></returns> 
        public object ExecStoreProcScalar(string sStoreProcName, ArrayList aParams)
        {
            try
            {
                CommandX.CommandText = sStoreProcName;
                CommandX.CommandType = CommandType.StoredProcedure;
                if (aParams != null)
                {
                    foreach (OleDbParameter oParam in aParams)
                    {
                        m_Command.Parameters.Add(oParam);
                    }
                }

                return m_Command.ExecuteScalar();
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("ExecStoreProcScalar()��������:" + e.Message);
                return null;
            }
            finally
            {
                //�ؽ����ӹ�������OleDbCommand����(�˾�ʵ������,��Ϊִ����һ��RdbRecord��Save()֮��,
                //OleDbCommand��ò�̫������,��ִ���������ݴ�ȡ����ʱ��������)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// ִ�д洢����, ������Ӱ��ļ�¼�� 
        /// </summary> 
        /// <param name="sStoreProcName">�洢��������</param> 
        /// <param name="aParams">�������</param> 
        /// <returns></returns> 
        public int ExecStoreProcNonQuery(string sStoreProcName, ArrayList aParams)
        {
            try
            {
                CommandX.CommandText = sStoreProcName;
                CommandX.CommandType = CommandType.StoredProcedure;
                if (aParams != null)
                {
                    foreach (OleDbParameter oParam in aParams)
                    {
                        m_Command.Parameters.Add(oParam);
                    }
                }
                return CommandX.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("ExecStoreProcNonQuery()��������:" + e.Message);
                return 0;
            }
            finally
            {
                //�ؽ����ӹ�������OleDbCommand����(�˾�ʵ������,��Ϊִ����һ��RdbRecord��Save()֮��,
                //OleDbCommand��ò�̫������,��ִ���������ݴ�ȡ����ʱ��������)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// ��ȡָ�������������� 
        /// </summary> 
        /// <param name="sTable">����</param> 
        /// <returns></returns> 
        public string GetTablePrimaryKey(string sTable)
        {
            try
            {
                using (OleDbDataAdapter oAdapter = new OleDbDataAdapter("SELECT * FROM " + sTable + " Where 1=0", this.OledbConnObject))
                {
                    DataTable oTable = new DataTable();
                    oAdapter.FillSchema(oTable, SchemaType.Source);
                    if (oTable.PrimaryKey.Length > 0)
                    {
                        return oTable.PrimaryKey[0].Caption;
                    }
                    else
                    {
                        //�����ȡ��������, ���Ե�һ���ֶ�Ϊ����
                        string sKey = oTable.Columns[0].Caption;
                        if (sKey.ToLower() == "rid") sKey = oTable.Columns[1].Caption;
                        return sKey;
                    }
                }
            }
            catch (Exception ex)
            {
                return "CODE";
            }
        }

        /// <summary>
        /// ��ȡ�ֶ�����
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sFeildName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Type GetFieldType(string sTableName, string sFeildName)
        {
            string strSql = "";
            DataTable dtType = null;
            Type fType = null;
            string sType = "";
            if (this.IsSqlServer)
            {
                strSql = string.Format("SELECT B.NAME AS TYPENAME FROM SYSCOLUMNS A,SYSTYPES B ,SYSOBJECTS C WHERE A.XUSERTYPE=B.XUSERTYPE AND A.ID=C.ID AND C.NAME ='{0}' AND A.NAME='{1}'", sTableName, sFeildName);
                string sTypeName = Convert.ToString(this.GetValue(strSql));
                if (sTypeName != null)
                {
                    sTypeName = sTypeName.ToUpper();
                    if (sTypeName.Contains("CHAR"))
                    {
                        fType = Type.GetType("System.String");
                    }
                    else if (sTypeName.Contains("INT"))
                    {
                        fType = Type.GetType("System.Int64");
                    }
                    else if (sTypeName.Contains("FLOAT") || sTypeName.Contains("DOUBLE"))
                    {
                        fType = Type.GetType("System.Double");
                    }
                    else if (sTypeName.Contains("DATE"))
                    {
                        fType = Type.GetType("System.DateTime");
                    }
                }
            }
            else if (this.IsOracle)
            {
                strSql = string.Format("select data_type from user_tab_columns where table_name = upper('{0}') and column_name = upper('{1}')", sTableName, sFeildName);
                dtType = GetDataTable(strSql, true);
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    sType = dtType.Rows[0][0].ToString();
                    if (sType.ToUpper().Contains("NUMBER"))
                    {
                        fType = Type.GetType("System.Int64");
                    }
                    else if (sType.ToUpper().Contains("CHAR"))
                    {
                        fType = Type.GetType("System.String");
                    }
                    else if (sType.ToUpper().Contains("DATE"))
                    {
                        fType = Type.GetType("System.DateTime");
                    }
                }

            }
            else if (this.IsAccess)
            {
            }

            //Ĭ�����ַ�������
            if (fType == null)
            {
                fType = Type.GetType("System.String");
            }

            return fType;
        }

        /// <summary>
        /// ͨ������ �ֶ��� ����sql��ѯ���
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sFeildName"></param>
        /// <param name="vValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FormatSqlValue(string sTableName, string sFeildName, object vValue)
        {
            Type fType = GetFieldType(sTableName, sFeildName);
            if (fType == null)
            {
                return string.Empty;
            }
            else
            {
                return SqlHelper.FormatSqlValue(vValue, fType);
            }
        }

        /// <summary>
        /// ��ȡ���ڱȽ��ַ���
        /// </summary>
        /// <param name="sFieldName">Ҫ�Ƚϵ����������ֶ���</param>
        /// <param name="sCompareType">�Ƚ�����(>,>=,=,��=,��)��</param>
        /// <param name="bOnlyCompareDate">�Ƿ�ֻ�Ƚ����ڲ���</param>
        /// <param name="sDateValue">�Ƚϵ�����ֵ 2008-12-15;2008-12-15 12:12:12 ��</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetCompareDateStr(string sFieldName, string sCompareType, bool bOnlyCompareDate, string sDateValue)
        {
            string sReturn = "";
            string sDateStr = "";
            sDateStr = sDateValue;
            if (bOnlyCompareDate)
            {
                sDateStr = sDateValue.IndexOf(' ') > 0 ? MyString.Left(sDateValue, " ") : sDateValue;
            }
            if (this.IsOracle)
            {
                sReturn = string.Format("{0}{1}{2}", bOnlyCompareDate ? string.Format("to_date(to_char({0},'yyyy-mm-dd'),'yyyy-mm-dd')", sFieldName) : sFieldName, sCompareType, SqlHelper.SqlFuncToDate(sDateStr, ""));
            }
            else
            {
                return "";
            }
            return sReturn;
        }

        /// <summary>
        /// ��ȡ���ڱȽ��ַ���
        /// </summary>
        /// <param name="sFieldName">Ҫ�Ƚϵ����������ֶ���</param>
        /// <param name="sCompareType">�Ƚ�����(>,>=,=,��=,��)��</param>
        /// <param name="bOnlyCompareDate">�Ƿ�ֻ�Ƚ����ڲ���</param>
        /// <param name="oDate">�Ƚϵ����ڶ���</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetCompareDateStr(string sFieldName, string sCompareType, bool bOnlyCompareDate, DateTime oDate)
        {
            string sDateStr = "";
            sDateStr = oDate.Year.ToString() + "-" + oDate.Month.ToString() + "-" + oDate.Day.ToString();
            sDateStr = sDateStr + " ";
            sDateStr = sDateStr + oDate.Hour.ToString() + ":" + oDate.Minute.ToString() + ":" + oDate.Second.ToString();
            return GetCompareDateStr(sFieldName, sCompareType, bOnlyCompareDate, sDateStr);
        }

        /// <summary> 
        /// ��ʼ������ 
        /// </summary> 
        /// <param name="oConnParam"></param> 
        public void InitObject(ConnectParam oConnParam)
        {
            try
            {
                this.Close();
                this.m_ConnParam = oConnParam;
                this.m_ConnObj = this.GetConnection(oConnParam);
                if (this.m_ConnObj == null)
                {
                    this.IsValid = false;
                }
                else
                {
                    this.IsValid = true;
                }

                if (m_Command == null)
                {
                    RebuildOleDbCommand();
                }
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("InitObject()��������:" + e.Message);
                this.IsValid = false;
            }
            finally
            {
                if (this.m_ConnObj != null && this.m_Command != null)
                {
                    this.m_Command.Connection = this.m_ConnObj.m_OledbConn;
                }
            }
        }

        private bool g_InTransNow = false;
        private int g_TransKey = 0;
        /// <summary>
        /// ������: ��������
        /// </summary>
        /// <remarks></remarks>
        public void BeginTrans()
        {
            if (g_InTransNow == false)
            {
                if (CommandX.Transaction == null)
                {
                    CommandX.Transaction = CommandX.Connection.BeginTransaction();
                }
                else
                {
                    CommandX.Transaction.Begin();
                }
                //lijian�����߼������������ʱ���������Ӷ�������ر�
                this.Lock();
                g_InTransNow = true;
            }
        }

        /// <summary>
        /// �ύ����
        /// </summary>
        /// <remarks></remarks>
        public void CommintTrans()
        {
            CommintTrans(0);
        }

        /// <summary>
        /// �ύ����
        /// </summary>
        /// <param name="iKey"></param>
        public void CommintTrans(int iKey)
        {
            if (iKey == g_TransKey)
            {
                if (g_InTransNow == true)
                {
                    CommandX.Transaction.Commit();
                    g_InTransNow = false;
                }
                //lijian�����߼����ύ�����ʱ���ٽ�������ʹ�����Ӷ�����Թر�
                this.Unlock(this.m_LockKey);
                g_TransKey = 0;
            }
        }

        /// <summary>
        /// �ع�����
        /// </summary>
        /// <remarks></remarks>
        public void RollbackTrans()
        {
            RollbackTrans(0);
        }
        
        /// <summary>
        /// �ع�����
        /// </summary>
        /// <param name="iKey"></param>
        public void RollbackTrans(int iKey)
        {
            if (iKey == g_TransKey)
            {
                if (g_InTransNow == true)
                {
                    this.CommandX.Transaction.Rollback();
                    g_InTransNow = false;
                }
                g_TransKey = 0;
            }
        }

        /// <summary>
        /// ����: ��ס����, ������������Ƕ��(��ס֮�󷵻�Key, һ������, CommintTrans��RollbackTransֻ�������Key���ܴ�������)
        /// </summary>
        /// <returns>�������0,�����������ɹ�, �����ѱ���������</returns>
        /// <remarks></remarks>
        public int LockTrans()
        {
            if (g_TransKey != 0)
            {
                return 0;
            }
            else
            {
                while (g_TransKey == 0)
                {
                    g_TransKey = Convert.ToInt32(MyRand.NextString_Number(5));
                }
                return g_TransKey;
            }
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }

        /// <summary>
        /// ��ȡ��ҳ��¼
        /// </summary>
        /// <param name="sSqlAllFields">��ѯ�ֶΣ�����Ƕ���ѯ���뽫��Ҫ�ı�����������ϣ���:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">��ѯ�ı� ����Ҫ������ѯ������order by�Ӿ䣬Ҳ��Ҫ����"from"�ؼ��֣���:students a inner join achievement b on a.... </param>
        /// <param name="sWhere">�������˵�����,��Ҫ��ǰ��� where, �� code=1 and name='lc'</param>
        /// <param name="sIndexField">���Է�ҳ�Ĳ����ظ��������ֶ��������������������ֶΣ�����Ƕ���ѯ������ϱ������������:a.id,oracle��ҳʱ��,�������Ϊ��</param>
        /// <param name="sOrderFields">�����ֶ��Լ���ʽ��,ǰ�治Ҫ��order by,�� a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">��ǰҳ��ҳ��</param>
        /// <param name="iPageSize">ÿҳ��¼��</param>
        /// <param name="iRecordCount">������������ز�ѯ���ܼ�¼����</param>
        /// <param name="iPageCount">������������ز�ѯ����ҳ��</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RdbRecordSet ExecuteReaderPage(string sSqlAllFields, string sSqlTables, string sIndexField, string sWhere, string sOrderFields, int iPageIndex, int iPageSize, ref int iRecordCount, ref int iPageCount)
        {
            string sSql = string.Empty;
            if (this.IsOracle)
            {
                sSql = GetOraclePageSql(sSqlAllFields, sSqlTables, sWhere, sOrderFields, iPageIndex, iPageSize, ref iRecordCount, ref iPageCount);
            }
            else
            {
                sSql = GetSqlAccessPageSql(sSqlAllFields, sSqlTables, sIndexField, sWhere, sOrderFields, iPageIndex, iPageSize, ref iRecordCount, ref iPageCount);
            }
            return this.GetRecordSet(sSql, true);
        }


        /// <summary>
        /// ��ȡoracle��ҳ��ѯ���
        /// </summary>
        /// <param name="sSqlAllFields">��ѯ�ֶΣ�����Ƕ���ѯ���뽫��Ҫ�ı�����������ϣ���:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">��ѯ�ı� ����Ҫ������ѯ������order by�Ӿ䣬Ҳ��Ҫ����"from"�ؼ��֣���:students a inner join achievement b on a.... </param>
        /// <param name="sWhere">�������˵�����,��Ҫ��ǰ��� where, �� code=1 and name='lc'</param>
        /// <param name="sOrderFields">�����ֶ��Լ���ʽ��,ǰ�治Ҫ��order by��a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">��ǰҳ��ҳ��</param>
        /// <param name="iPageSize">ÿҳ��¼��</param>
        /// <param name="iRecordCount">������������ز�ѯ���ܼ�¼����</param>
        /// <param name="iPageCount">������������ز�ѯ����ҳ��</param>
        /// <returns>���ط�ҳ��sql��ѯ���</returns>
        /// <remarks></remarks>
        protected string GetOraclePageSql(string sSqlAllFields, string sSqlTables, string sWhere, string sOrderFields, int iPageIndex, int iPageSize, ref int iRecordCount, ref int iPageCount)
        {
            iRecordCount = 0;
            iPageCount = 0;
            if (iPageSize <= 0)
            {
                iPageSize = 10;
            }

            string _OrderFields = string.IsNullOrEmpty(sOrderFields) ? "" : " order by " + sOrderFields;
            string _sWhere = string.IsNullOrEmpty(sWhere) ? "" : " where " + sWhere;

            string sSqlCount = ("select count(*) from ") + sSqlTables + _sWhere;
            iRecordCount = Convert.ToInt32(Convert.ToString(GetValue(sSqlCount)));
            if (iRecordCount % iPageSize == 0)
            {
                iPageCount = iRecordCount / iPageSize;
            }
            else
            {
                iPageCount = (iRecordCount / iPageSize) + 1;
            }
            if (iPageIndex > iPageCount)
            {
                iPageIndex = iPageCount;
            }
            if (iPageIndex < 1)
            {
                iPageIndex = 1;
            }

            int iBeginIndex;
            int iEndIndex;
            iBeginIndex = (iPageIndex - 1) * iPageSize + 1;
            iEndIndex = iPageIndex * iPageSize;
            return string.Format("select *  from (select a.*, ROWNUM row_num from (select {0} from {1}{2}{3}) a) b  where b.row_num between {4} and {5}", sSqlAllFields, sSqlTables, _sWhere, _OrderFields, iBeginIndex, iEndIndex);

        }

        /// <summary>
        /// ��ȡsqlserver��ҳ��ѯ���
        /// </summary>
        /// <param name="sSqlAllFields">��ѯ�ֶΣ�����Ƕ���ѯ���뽫��Ҫ�ı�����������ϣ���:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">��ѯ�ı� ����Ҫ������ѯ������order by�Ӿ䣬Ҳ��Ҫ����"from"�ؼ��֣���:students a inner join achievement b on a.... </param>
        /// <param name="sIndexField">���Է�ҳ�Ĳ����ظ��������ֶ��������������������ֶΣ�����Ƕ���ѯ������ϱ������������:a.id</param>
        /// <param name="sWhere">�������˵�����,��Ҫ��ǰ��� where, �� code=1 and name='lc'</param>
        /// <param name="sOrderFields">�����ֶ��Լ���ʽ��,ǰ�治Ҫ��order by��a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">��ǰҳ��ҳ��</param>
        /// <param name="iPageSize">ÿҳ��¼��</param>
        /// <param name="iRecordCount">������������ز�ѯ���ܼ�¼����</param>
        /// <param name="iPageCount">������������ز�ѯ����ҳ��</param>
        /// <returns>���ط�ҳ��sql��ѯ���</returns>
        /// <remarks></remarks>
        protected string GetSqlAccessPageSql(string sSqlAllFields, string sSqlTables, string sIndexField, string sWhere, string sOrderFields, int iPageIndex, int iPageSize, ref int iRecordCount, ref int iPageCount)
        {
            iRecordCount = 0;
            iPageCount = 0;
            if (iPageSize <= 0)
            {
                iPageSize = 10;
            }

            string _sWhere = string.IsNullOrEmpty(sWhere) ? "" : " where " + sWhere;
            string sSqlCount = ("select count(*) from ") + sSqlTables + _sWhere;
            iRecordCount = Convert.ToInt32(Convert.ToString(GetValue(sSqlCount)));
            if (iRecordCount % iPageSize == 0)
            {
                iPageCount = iRecordCount / iPageSize;
            }
            else
            {
                iPageCount = iRecordCount / iPageSize + 1;
            }
            if (iPageIndex > iPageCount)
            {
                iPageIndex = iPageCount;
            }
            if (iPageIndex < 1)
            {
                iPageIndex = 1;
            }

            string sSql = null;
            string SqlTablesAndWhere = sSqlAllFields + _sWhere;
            string _OrderFields = string.IsNullOrEmpty(sOrderFields) ? "" : " order by " + sOrderFields;
            if (iPageIndex == 1)
            {
                sSql = "select top " + iPageSize + " " + sSqlAllFields + " from " + SqlTablesAndWhere + _OrderFields;
            }
            else
            {
                sSql = "select top " + iPageSize + " " + sSqlAllFields + " from ";
                if ((SqlTablesAndWhere.ToLower().IndexOf(" where ") > 0))
                {
                    string _where = Regex.Replace(SqlTablesAndWhere, "\\ where\\ ", " where (", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    sSql = sSql + _where + ") and (";
                }
                else
                {
                    sSql = sSql + SqlTablesAndWhere + " where (";
                }
                sSql = sSql + sIndexField + " not in (select top " + (iPageIndex - 1) * iPageSize + " " + sIndexField + " from " + SqlTablesAndWhere + _OrderFields;
                sSql = sSql + ")) " + _OrderFields;
            }
            return sSql;

        }

        /// <summary>
        /// �������SQL
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sNewTableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        string ExportTableSQL(string sTableName, string sNewTableName = "")
        {
            if (sNewTableName == "")
            {
                sNewTableName = sTableName;
            }
            if (this.IsOracle)
            {
                return this.MakeTableSQL_Oracle(sTableName, sNewTableName);
            }
            else if (this.IsSqlServer)
            {
                return this.MakeTableSQL_SqlServer(sTableName, sNewTableName);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ���ɱ��Oracle SQL
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sNewTableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string MakeTableSQL_Oracle(string sTableName, string sNewTableName)
        {
            try
            {
                string sSQL = string.Format("CREATE TABLE {0}(", sNewTableName);
                RdbRecord oRecord = this.CreateNewRecord(sTableName);
                RdbField oField = null;
                for (int i = 0; i <= oRecord.FieldCount - 1; i++)
                {
                    oField = oRecord.GetField(i);

                    if (oField.Name == "ROWID")
                    {
                        continue;
                    }

                    sSQL += oField.Name;
                    if (oField.IsStringField)
                    {
                        sSQL += " VARCHAR2(" + oField.MaxLength + ")";
                    }
                    else if (oField.IsNumberField)
                    {
                        sSQL += " NUMBER";
                    }
                    else if (oField.IsDateTimeField)
                    {
                        sSQL += " DATE";
                    }

                    if (oField.IsPrimaryKey)
                    {
                        sSQL += " PRIMARY KEY";
                    }

                    if (i != oRecord.FieldCount - 1)
                    {
                        sSQL += ", ";
                        //& vbCrLf
                    }
                }
                sSQL += ")";
                return sSQL;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ���ɱ��SqlServer SQL
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sNewTableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string MakeTableSQL_SqlServer(string sTableName, string sNewTableName)
        {
            return string.Empty;
        }

        /// <summary>
        /// �ж����ݿ����Ƿ����ָ�����Ƶı�
        /// </summary>
        /// <param name="sTableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsTableExisted(string sTableName)
        {
            try
            {
                RdbRecord oRecord = this.CreateNewRecord(sTableName);
                return oRecord != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// �жϱ��Ƿ����ĳ���ֶ�
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sColumnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsTableHasColumn(string sTableName, string sColumnName)
        {
            try
            {
                return this.ExecSQL_WithNoErrorShow(string.Format("SELECT {0} FROM {1}", sColumnName, sTableName));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ��ĳ�������һ����
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sColumnName"></param>
        /// <param name="sDataType">��: VARCHAR2(200)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AddColumnToTable(string sTableName, string sColumnName, string sDataType)
        {
            try
            {
                return ExecSQL(string.Format("ALTER TABLE {0} ADD {1} {2}", sTableName, sColumnName, sDataType));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ���ݱ�����ȡ���ݱ�ļܹ���Ϣ
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual DataTable GetTableSchema(String tableName)
        {
            DataTable table = null;
            if (dataTableCachePool.TryGetValue(tableName, out table, CollectionsGetOper.DefaultValueIfNotExist, null))
                return table;

            CommandX.CommandText = String.Format(this.SelectEmptyDatableSqlTemplate, tableName);
            CommandX.CommandType = CommandType.Text;

            //ָ����CommandBehavior.SchemaOnly�޷���ȡ����������Ϣ
            //using (IDataReader reader = this.ExecuteReader (comm, CommandBehavior.SchemaOnly))
            using (IDataReader reader = CommandX.ExecuteReader(CommandBehavior.KeyInfo))
            {
                table = reader.GetSchemaTable();
                dataTableCachePool.TryAdd(tableName, table, CollectionsAddOper.IgnoreIfExist);
                return table;
            }
        }

        #endregion

        #region "˽��ʵ������"

        /// <summary> 
        /// �Ӹ���ľ�̬ȫ�ֱ����л�ȡ���е�����, ���û�еĻ�,����һ�� 
        /// </summary> 
        /// <param name="oConnParam">����������ݿ����Ӳ���</param> 
        /// <returns></returns> 
        private RdbConnection GetConnection(ConnectParam oConnParam)
        {
            try
            {
                return new RdbConnection(oConnParam.MakeConnectString());
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("GetConnection()��������:" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// �ؽ��ڲ���OleDbCommand����
        /// </summary>
        /// <remarks></remarks>
        private void RebuildOleDbCommand()
        {
            //���ԭCommand���������,Rebuild֮��ҲҪ���������
            OleDbTransaction oTransation = null;
            if (this.m_Command != null)
            {
                oTransation = this.m_Command.Transaction;
                this.m_Command.Connection = null;
                this.m_Command.Dispose();
            }
            this.m_Command = new OleDbCommand();
            this.m_Command.Connection = this.m_ConnObj.m_OledbConn;
            this.m_Command.Transaction = oTransation;
        }

        #endregion

        //#region "��̬����"

        ///// <summary>
        ///// �ͻ���ִ�в�ѯ���
        ///// </summary>
        ///// <param name="sSQL"></param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public static ArrayList ClientGetValues(string sSQL)
        //{
        //    RdbConnMgr oConn = RdbConnHelper.CreateRdbConnMgr();
        //    return oConn.GetValues(sSQL, true);
        //}

        ///// <summary>
        ///// ��һ�����ݿ������׷��һ����¼(�ĳ�����objectX��������Ϊ�˼���ԭ���� �Զ����洴���ˣ��޸��˵Ĺ���)
        ///// </summary>
        ///// <param name="sTableName">������</param>
        ///// <param name="sPrimKey">��������ֶ�����</param>
        ///// <param name="sValueXml">��XML��ʽ������ֶ�ֵ</param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public static string AddRecordToTable(string sTableName, string sPrimKey, string sValueXml)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    RdbConnMgr oConn = RdbConnHelper.CreateRdbConnMgr();
        //    //    MyXmlDoc oXmlDoc = MyXmlDocMgr.LoadXmlDoc(sValueXml);

        //    //    //�����������������ֱ�ӷ���
        //    //    if (string.IsNullOrEmpty(sPrimKey))
        //    //    {
        //    //        MyLog.MakeLog("AddRecordToTable()���봫������ sPrimKeyֵ");
        //    //        return "";
        //    //    }

        //    //    ObjectX oObject = ObjectHelper.CreateNewObjectEx(sTableName, sPrimKey);

        //    //    //sValueXml�ĸ�ʽ��: <Items><Item Name="����1">��ֵ1</Item><Item Name="����2">��ֵ2</Item></Items>
        //    //    List<MyXmlNode> aNodes = oXmlDoc.GetNodes("Items/Item");
        //    //    if (aNodes != null)
        //    //    {
        //    //        foreach (MyXmlNode oNode in aNodes)
        //    //        {
        //    //            oObject.SetItemValue(oNode.GetAttributeValue("Name"), oNode.InnerText);
        //    //        }
        //    //    }

        //    //    if (string.IsNullOrEmpty(oObject.SaveToRDB()))
        //    //    {
        //    //        throw new Exception("oObject.SaveToRDB()ʧ��");
        //    //    }

        //    //    return Convert.ToString(oObject.KeyValue);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MyLog.MakeLog("AddRecordToTable������:" + ex.Message);
        //    //    return "";
        //    //}
        //}

        ///// <summary>
        ///// ���±������һ����¼
        ///// </summary>
        ///// <param name="sTableName">������</param>
        ///// <param name="sPrimKey">�����������</param>
        ///// <param name="sKeyValue">�����¼�¼������ֵ</param>
        ///// <param name="sValueXml">��XML��ʽ������ֶ�ֵ</param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public static bool UpdateRecordInTable(string sTableName, string sPrimKey, string sKeyValue, string sValueXml)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    RdbConnMgr oConn = RdbConnHelper.CreateRdbConnMgr();
        //    //    if (string.IsNullOrEmpty(sPrimKey))
        //    //    {
        //    //        sPrimKey = oConn.GetTablePrimaryKey(sTableName);
        //    //    }

        //    //    ObjectX oObject = ObjectHelper.CreateRdbObjectEx(sTableName, sKeyValue, null, sPrimKey);
        //    //    if (oObject == null)
        //    //    {
        //    //        MyLog.MakeLog("UpdateRecordInTable()����ʧ��,�Ҳ����ö���");
        //    //        return false;
        //    //    }

        //    //    MyXmlDoc oXmlDoc = MyXmlDocMgr.LoadXmlDoc(sValueXml);

        //    //    //sValueXml�ĸ�ʽ��: <Items><Item Name="����1">��ֵ1</Item><Item Name="����2">��ֵ2</Item></Items>
        //    //    List<MyXmlNode> aNodes = oXmlDoc.GetNodes("Items/Item");
        //    //    if (aNodes != null)
        //    //    {
        //    //        foreach (MyXmlNode oNode in aNodes)
        //    //        {
        //    //            oObject.SetItemValue(oNode.GetAttributeValue("Name"), oNode.InnerText);
        //    //        }
        //    //    }

        //    //    if (string.IsNullOrEmpty(oObject.SaveToRDB()))
        //    //    {
        //    //        throw new Exception("oObject.SaveToRDB()ʧ��");
        //    //    }

        //    //    return true;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MyLog.MakeLog("UpdateRecordInTable������:" + ex.Message);
        //    //    return false;
        //    //}
        //}

        ///// <summary>
        ///// �жϵ�ǰ���ݿ����Ƿ����ĳ���������ͼ
        ///// </summary>
        ///// <param name="sTableOrViewName"></param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public static bool IsTableOrViewExistsInCurrentDB(string sTableOrViewName)
        //{
        //    try
        //    {
        //        return RdbConnHelper.CreateRdbConnMgr().IsTableExisted(sTableOrViewName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //#endregion
    }
}