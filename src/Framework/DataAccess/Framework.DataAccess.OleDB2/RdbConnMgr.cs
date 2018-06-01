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
    /// 数据库连接管理类（只能在单线程中调用）
    /// </summary> 
    public class RdbConnMgr
    {
        #region "字段"

        private RdbConnection m_ConnObj = null;
        private OleDbCommand m_Command = null;
        public ConnectParam m_ConnParam = null;
        private int m_LockKey = 0;
        private bool m_IsValid = true;
        private string m_LastestErrorMsg = "";
        
        /// <summary>
        /// 表定义缓冲区
        /// </summary>
        private DictionaryCachePool<String, DataTable> dataTableCachePool =
            new DictionaryCachePool<String, DataTable>(new StringEqualityIgnoreCaseComparerGeneric());

        #endregion

        #region "属性"

        /// <summary> 
        /// OleDbConnection对象 
        /// </summary> 
        public OleDbConnection OledbConnObject
        {
            get
            {
                return this.m_ConnObj.m_OledbConn;
            }
        }

        /// <summary>
        /// 返回Transaction对象
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
        /// 是否被锁
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
        /// 查询空表、为了获取表定义的sql语句模板
        /// 默认为：select * from {0} where 1=0
        /// </summary>
        protected virtual String SelectEmptyDatableSqlTemplate
        {
            get
            {
                return "select * from {0} where 1=0";
            }
        }

        #endregion

        #region "私有实例方法"

        /// <summary> 
        /// 构造函数2(接受指定的连接参数) 
        /// </summary> 
        /// <param name="oConnParam">数据库连接参数（可以为null，如果传入null，则会自动的获取默认的连接参数）</param> 
        internal RdbConnMgr(ConnectParam oConnParam)
        {
            this.InitObject(oConnParam);
        }

        #endregion

        #region "公共实例方法"

        /// <summary>
        /// 上锁，成功则返回iKey
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
        /// 解锁
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
        /// 打开连接
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
        /// 关闭链接
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
                MyLog.MakeLog("RdbConnMgr的Close()抛出异常:" + ex.Message);
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
        /// 判断连接是否关闭
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
        /// 对象有效标志 
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
        /// 获取连接字符串
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
        /// 获取OleDbCommand对象, 不存在则重建
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
        /// 最近发生的一个错误的信息 
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
        /// 数据库类型 
        /// </summary> 
        public int DbType
        {
            get
            {
                return this.m_ConnParam.DatabaseType;
            }
        }

        /// <summary> 
        /// 数据库类型是不是Sql Server 
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
        /// 数据库类型是不是Oracle 
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
        /// 数据库类型是不是Access 
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
        /// 执行查询语句, 获取记录集对象 
        /// </summary> 
        /// <param name="sSql">要执行的查询语句</param> 
        /// <param name="bReadOnly">是否要求返回的记录集是只读的</param> 
        /// <param name="sPrimaryKey">记录集的主键列</param> 
        /// <returns></returns> 
        public RdbRecordSet GetRecordSet(string sSql, bool bReadOnly = true, string sPrimaryKey = "")
        {
            try
            {
                CommandX.CommandText = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;

                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);
                //表数据 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //表架构信息 
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
                MyLog.MakeLog(String.Format("GetRecordSet()发生错误：{0}，{1}", e.Message, "SQL=" + sSql));
                return null;
            }
        }

        /// <summary>
        /// 根据SQL获取数据表对象
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

                //表数据 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                String tableName = MyString.GetTableNameFromSQL(sSql);
                if (!dataTableCachePool.ContainsKey(tableName))
                {
                    //表架构信息 
                    DataTable oSchemaTable = new DataTable();
                    oAdapter.FillSchema(oSchemaTable, SchemaType.Source);
                    dataTableCachePool.TryAdd(tableName, oSchemaTable, CollectionsAddOper.IgnoreIfExist);
                }
                return oTable;
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("GetDataTable()发生错误:" + e.Message);
                MyLog.MakeLog(sSql);
                return null;
            }
        }

        /// <summary>
        /// 获取一个表符合参数 sWhere 的记录数,sWhere为空时,返回所有记录数
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
        /// 获取记录
        /// </summary> 
        /// <param name="sSql"></param> 
        /// <param name="bReadOnly"></param> 
        /// <param name="sPrimKey"></param>
        /// <param name="bDontShowErrorLog">即使出错，也不要显示错误信息</param>
        /// <returns></returns> 
        public RdbRecord GetRecord(string sSql, bool bReadOnly = true, string sPrimKey = "", bool bDontShowErrorLog = false)
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;

                CommandX.CommandText =  SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(CommandX);
                //表数据 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //表架构信息 
                DataTable oSchemaTable = new DataTable();
                oAdapter.FillSchema(oSchemaTable, SchemaType.Source);

                RdbRecordSet oRecordSet = new RdbRecordSet(this, oTable, oSchemaTable, bReadOnly, sPrimKey);

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300)
                {
                    MyLog.MakeLog("较耗时(" + iMillSecond + ")的查询：" + sSql);
                }

                return oRecordSet.GetRecord(0);
            }
            catch (Exception e)
            {
                if (bDontShowErrorLog == false)
                {
                    this.m_LastestErrorMsg = e.Message;
                    MyLog.MakeLog("GetRecord()发生错误:" + e.Message);
                    MyLog.MakeLog(sSql);
                }
                return null;
            }
        }

        /// <summary> 
        /// 获取一个表(视图)的前N条记录. 因为获取一个表的前N条记录的语法在各个DBMS中的语法不一致,所以提供该函数解决这个问题 
        /// </summary> 
        /// <param name="sTable">要获取数据的表或视图名</param> 
        /// <param name="iCount">要获取多少条</param> 
        /// <param name="sColumn">返回哪些列, 格式如: Location, Area, Owner. *表示所有的列</param> 
        /// <param name="bReadOnly"></param> 
        /// <param name="sWhere">过滤条件, 如: Code>20</param> 
        /// <param name="sOrderBy">排序方法, 如: Code ASC </param> 
        /// <param name="bDistinct">是否要去掉重复值</param> 
        /// <returns></returns> 
        public RdbRecordSet GetTopNthRecords(string sTable, int iCount, string sColumn, bool bReadOnly = true, string sWhere = "", string sOrderBy = "", bool bDistinct = false)
        {
            try
            {
                string sSQL = "";
                if (sColumn == "") sColumn = "*";

                if (this.DbType <= DatabaseVersionRange.Oracle_Max.GetHashCode() && this.DbType >= DatabaseVersionRange.Oracle_Min.GetHashCode())
                {
                    //确保值不重复
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
                    //确保值不重复
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
                MyLog.MakeLog("GetTopNthRecords()发生错误:" + e.Message);
                return null;
            }
        }

        /// <summary> 
        /// 分页取数据: 获取指定范围的数据 
        /// </summary> 
        /// <param name="sSQL"></param> 
        /// <param name="iStart">最小为1</param> 
        /// <param name="iLength"></param> 
        /// <returns></returns> 
        public List<RdbRecord> GetRecordInRange(string sSQL, int iStart, int iLength)
        {
            //创建数据库联接对象 
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
        /// 分页取数据: 获取第N页的数据 
        /// </summary>
        /// <param name="sSQL"></param> 
        /// <param name="iPageSize"></param> 
        /// <param name="iNthPage"></param> 
        /// <returns></returns> 
        public ArrayList GetNthPageRecords(string sSQL, int iPageSize, int iNthPage)
        {
            //创建数据库联接对象 
            RdbRecordSet oRecSet = GetRecordSet(sSQL, true);
            RdbRecord oRecord;
            ArrayList aRecs = new ArrayList();
            if (oRecSet.HasRecord)
            {
                //设置页面大小 
                oRecSet.PageSize = iPageSize;

                //设置当前页 
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
        /// 获取指定表单下一条新记录的主键值(适用于Oracle数据库) 
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

                    //加上这个判断,主要是为了避免获取到的 sReturnCode 已经存在于数据库中
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
        /// 设置序列的当前值
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
        /// 直接返回查询值 
        /// </summary> 
        /// <param name="sSql"></param> 
        /// <param name="bDontShowErrorLog">即使出错也不要输出错误日志</param> 
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
                    MyLog.MakeLog("较耗时(" + iMillSecond + ")的查询：" + sSql);
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
                    MyLog.MakeLog("GetValue()发生错误:" + e.Message);
                    MyLog.MakeLog(sSql);
                }
                return null;
            }
        }

        /// <summary>
        /// 执行查询,直接返回查询值,因为查询可能存在多条记录,所以返回一个数组
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

                //表数据 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300) MyLog.MakeLog("较耗时(" + iMillSecond + ")的查询：" + sSql);

                //填充返回值
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
                MyLog.MakeLog("GetValues()发生错误:" + e.Message);
                MyLog.MakeLog(sSql);
                return null;
            }
        }

        /// <summary>
        /// 判断记录是否存在
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
                MyLog.MakeLog("IsRdbRecordExisted()发生错误:" + e.Message);
                MyLog.MakeLog("sSQL=" + sSQL);
            }
            return false;
        }

        public bool IsRecordExist(string sTable, string sWhere, string sColumnName = "*")
        {
            return this.IsRdbRecordExisted(string.Format("SELECT " + sColumnName + " FROM {0} WHERE {1}", sTable, sWhere));
        }

        /// <summary> 
        /// 为一个表创建一条新记录 
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
        /// 获取数据库所有表名称
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
        /// 获取表的所有列名称 
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
        /// 执行一条SQL语句 
        /// </summary> 
        /// <param name="sSql">被执行的SQL语句</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql)
        {
            int iEffectedRecordCount = 0;
            return ExecSQL(sSql, ref iEffectedRecordCount);
        }

        /// <summary> 
        /// 执行一条SQL语句 
        /// </summary> 
        /// <param name="sSql">被执行的SQL语句</param> 
        /// <param name="iEffectedRecordCount">[OUT]执行SQL受影响的记录数</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref int iEffectedRecordCount)
        {
            string sErrorMsg = "";
            return ExecSQL(sSql, ref iEffectedRecordCount, ref sErrorMsg);
        }

        /// <summary> 
        /// 执行一条SQL语句 
        /// </summary> 
        /// <param name="sSql">被执行的SQL语句</param> 
        /// <param name="sErrorMsg">[OUT]执行发生错误时的错误信息</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref string sErrorMsg)
        {
            int iEffectedRecordCount = 0;
            return ExecSQL(sSql, ref iEffectedRecordCount, ref sErrorMsg);
        }

        /// <summary> 
        /// 执行一条SQL语句 
        /// </summary> 
        /// <param name="sSql">被执行的SQL语句</param> 
        /// <param name="iEffectedRecordCount">[OUT]执行SQL受影响的记录数</param> 
        /// <param name="sErrorMsg">[OUT]执行发生错误时的错误信息</param> 
        /// <param name="bDontShowErrorLog">是否要在Log中记录发生错误时的错误信息</param> 
        /// <returns></returns> 
        public bool ExecSQL(string sSql, ref int iEffectedRecordCount, ref string sErrorMsg, bool bDontShowErrorLog = false)
        {
            try
            {
                long iMillSecond1 = MyDatetime.NowMillisecond;
                sSql = sSql.Trim();
                //标志不要放回回收站
                if (sSql.StartsWith("--")) sSql = sSql.Substring(2);
                string sFormatSQL = SqlHelper.SqlFuncNested(sSql);
                CommandX.CommandType = CommandType.Text;
                CommandX.CommandText = sFormatSQL;
                iEffectedRecordCount = CommandX.ExecuteNonQuery();

                long iMillSecond2 = MyDatetime.NowMillisecond;
                long iMillSecond = iMillSecond2 - iMillSecond1;
                if (iMillSecond > 300) MyLog.MakeLog("较耗时(" + iMillSecond + ")的查询：" + sSql);
                sErrorMsg = "";
                return true;
            }
            catch (Exception e)
            {
                sErrorMsg = InlineAssignHelper(ref this.m_LastestErrorMsg, e.Message);
                iEffectedRecordCount = 0;
                if (bDontShowErrorLog == false)
                {
                    MyLog.MakeLog("ExecSQL()发生错误:" + e.Message);
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
        /// 执行命令, 返回受影响的记录数(命令执行失败返回-1) 
        /// </summary> 
        /// <param name="sCommand">要执行的参数化查询命令</param> 
        /// <param name="aParameters">命令参数</param> 
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
                MyLog.MakeLog("ExecCommand()发生错误:" + e.Message);
                MyLog.MakeLog("Command=" + sCommand);
                return -1;
            }
            finally
            {
                //重建连接管理对象的OleDbCommand对象(此举实属无奈,因为执行了一次RdbRecord的Save()之后,
                //OleDbCommand变得不太好用了,在执行其他数据存取操作时经常出错)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// 执行存储过程, 以记录集的形式返回结果 
        /// </summary> 
        /// <param name="sStoreProcName">存储过程名称</param> 
        /// <param name="aParams">输入参数</param> 
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

                //表数据 
                DataTable oTable = new DataTable();
                oAdapter.Fill(oTable);

                //表架构信息 
                DataTable oSchemaTable = null;
                return new RdbRecordSet(this, oTable, oSchemaTable, true);
            }
            catch (Exception e)
            {
                this.m_LastestErrorMsg = e.Message;
                MyLog.MakeLog("ExecStoreProc()发生错误:" + e.Message);
                return null;
            }
            finally
            {
                //重建连接管理对象的OleDbCommand对象(此举实属无奈,因为执行了一次RdbRecord的Save()之后,
                //OleDbCommand变得不太好用了,在执行其他数据存取操作时经常出错)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// 执行存储过程, 以值的形式返回结果 
        /// </summary> 
        /// <param name="sStoreProcName">存储过程名称</param> 
        /// <param name="aParams">输入参数</param> 
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
                MyLog.MakeLog("ExecStoreProcScalar()发生错误:" + e.Message);
                return null;
            }
            finally
            {
                //重建连接管理对象的OleDbCommand对象(此举实属无奈,因为执行了一次RdbRecord的Save()之后,
                //OleDbCommand变得不太好用了,在执行其他数据存取操作时经常出错)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// 执行存储过程, 返回受影响的记录数 
        /// </summary> 
        /// <param name="sStoreProcName">存储过程名称</param> 
        /// <param name="aParams">输入参数</param> 
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
                MyLog.MakeLog("ExecStoreProcNonQuery()发生错误:" + e.Message);
                return 0;
            }
            finally
            {
                //重建连接管理对象的OleDbCommand对象(此举实属无奈,因为执行了一次RdbRecord的Save()之后,
                //OleDbCommand变得不太好用了,在执行其他数据存取操作时经常出错)
                this.RebuildOleDbCommand();
            }
        }

        /// <summary> 
        /// 获取指定表单的主键名称 
        /// </summary> 
        /// <param name="sTable">表单名</param> 
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
                        //如果获取不到主键, 则以第一个字段为主键
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
        /// 获取字段类型
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

            //默认是字符串类型
            if (fType == null)
            {
                fType = Type.GetType("System.String");
            }

            return fType;
        }

        /// <summary>
        /// 通过表名 字段名 构造sql查询语句
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
        /// 获取日期比较字符串
        /// </summary>
        /// <param name="sFieldName">要比较的日期类型字段名</param>
        /// <param name="sCompareType">比较类型(>,>=,=,《=,《)等</param>
        /// <param name="bOnlyCompareDate">是否只比较日期部分</param>
        /// <param name="sDateValue">比较的日期值 2008-12-15;2008-12-15 12:12:12 等</param>
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
        /// 获取日期比较字符串
        /// </summary>
        /// <param name="sFieldName">要比较的日期类型字段名</param>
        /// <param name="sCompareType">比较类型(>,>=,=,《=,《)等</param>
        /// <param name="bOnlyCompareDate">是否只比较日期部分</param>
        /// <param name="oDate">比较的日期对象</param>
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
        /// 初始化连接 
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
                MyLog.MakeLog("InitObject()发生错误:" + e.Message);
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
        /// 事务处理: 启动事务
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
                //lijian增加逻辑，启动事务的时候，锁上连接对象不允许关闭
                this.Lock();
                g_InTransNow = true;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <remarks></remarks>
        public void CommintTrans()
        {
            CommintTrans(0);
        }

        /// <summary>
        /// 提交事务
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
                //lijian增加逻辑，提交事务的时候，再解锁对象，使得连接对象可以关闭
                this.Unlock(this.m_LockKey);
                g_TransKey = 0;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <remarks></remarks>
        public void RollbackTrans()
        {
            RollbackTrans(0);
        }
        
        /// <summary>
        /// 回滚事务
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
        /// 功能: 锁住事务, 用来处理事务嵌套(锁住之后返回Key, 一个数字, CommintTrans及RollbackTrans只能用这个Key才能处理事务)
        /// </summary>
        /// <returns>如果返回0,表明锁定不成功, 事务已被被人锁定</returns>
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
        /// 获取分页记录
        /// </summary>
        /// <param name="sSqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">查询的表 但不要包含查询条件和order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... </param>
        /// <param name="sWhere">用来过滤的条件,不要在前面加 where, 如 code=1 and name='lc'</param>
        /// <param name="sIndexField">用以分页的不能重复的索引字段名，最好是主表的主键字段，如果是多表查询，请带上表名或别名，如:a.id,oracle分页时候,此项可以为空</param>
        /// <param name="sOrderFields">排序字段以及方式如,前面不要加order by,如 a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">当前页的页码</param>
        /// <param name="iPageSize">每页记录数</param>
        /// <param name="iRecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="iPageCount">输出参数，返回查询的总页数</param>
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
        /// 获取oracle分页查询语句
        /// </summary>
        /// <param name="sSqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">查询的表 但不要包含查询条件和order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... </param>
        /// <param name="sWhere">用来过滤的条件,不要在前面加 where, 如 code=1 and name='lc'</param>
        /// <param name="sOrderFields">排序字段以及方式如,前面不要加order by：a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">当前页的页码</param>
        /// <param name="iPageSize">每页记录数</param>
        /// <param name="iRecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="iPageCount">输出参数，返回查询的总页数</param>
        /// <returns>返回分页的sql查询语句</returns>
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
        /// 获取sqlserver分页查询语句
        /// </summary>
        /// <param name="sSqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，如:a.id,a.name,b.score</param>
        /// <param name="sSqlTables">查询的表 但不要包含查询条件和order by子句，也不要包含"from"关键字，如:students a inner join achievement b on a.... </param>
        /// <param name="sIndexField">用以分页的不能重复的索引字段名，最好是主表的主键字段，如果是多表查询，请带上表名或别名，如:a.id</param>
        /// <param name="sWhere">用来过滤的条件,不要在前面加 where, 如 code=1 and name='lc'</param>
        /// <param name="sOrderFields">排序字段以及方式如,前面不要加order by：a.OrderID desc,CnName desc</param>
        /// <param name="iPageIndex">当前页的页码</param>
        /// <param name="iPageSize">每页记录数</param>
        /// <param name="iRecordCount">输出参数，返回查询的总记录条数</param>
        /// <param name="iPageCount">输出参数，返回查询的总页数</param>
        /// <returns>返回分页的sql查询语句</returns>
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
        /// 导出表的SQL
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
        /// 生成表的Oracle SQL
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
        /// 生成表的SqlServer SQL
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
        /// 判断数据库中是否存在指定名称的表
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
        /// 判断表是否具有某个字段
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
        /// 往某个表添加一个列
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sColumnName"></param>
        /// <param name="sDataType">如: VARCHAR2(200)</param>
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
        /// 根据表名获取数据表的架构信息
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

            //指定：CommandBehavior.SchemaOnly无法读取到主键的信息
            //using (IDataReader reader = this.ExecuteReader (comm, CommandBehavior.SchemaOnly))
            using (IDataReader reader = CommandX.ExecuteReader(CommandBehavior.KeyInfo))
            {
                table = reader.GetSchemaTable();
                dataTableCachePool.TryAdd(tableName, table, CollectionsAddOper.IgnoreIfExist);
                return table;
            }
        }

        #endregion

        #region "私有实例方法"

        /// <summary> 
        /// 从该类的静态全局变量中获取已有的连接, 如果没有的话,创建一个 
        /// </summary> 
        /// <param name="oConnParam">本对象的数据库连接参数</param> 
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
                MyLog.MakeLog("GetConnection()发生错误:" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 重建内部的OleDbCommand对象
        /// </summary>
        /// <remarks></remarks>
        private void RebuildOleDbCommand()
        {
            //如果原Command有事务对象,Rebuild之后也要有这个对象
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

        //#region "静态方法"

        ///// <summary>
        ///// 客户端执行查询语句
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
        ///// 往一个数据库表里面追加一条记录(改成了用objectX来保存是为了兼容原来的 自动保存创建人，修改人的功能)
        ///// </summary>
        ///// <param name="sTableName">表名称</param>
        ///// <param name="sPrimKey">表的主键字段名称</param>
        ///// <param name="sValueXml">以XML方式打包的字段值</param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public static string AddRecordToTable(string sTableName, string sPrimKey, string sValueXml)
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{
        //    //    RdbConnMgr oConn = RdbConnHelper.CreateRdbConnMgr();
        //    //    MyXmlDoc oXmlDoc = MyXmlDocMgr.LoadXmlDoc(sValueXml);

        //    //    //如果不传入主键，则直接返回
        //    //    if (string.IsNullOrEmpty(sPrimKey))
        //    //    {
        //    //        MyLog.MakeLog("AddRecordToTable()必须传入主键 sPrimKey值");
        //    //        return "";
        //    //    }

        //    //    ObjectX oObject = ObjectHelper.CreateNewObjectEx(sTableName, sPrimKey);

        //    //    //sValueXml的格式如: <Items><Item Name="列名1">列值1</Item><Item Name="列名2">列值2</Item></Items>
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
        //    //        throw new Exception("oObject.SaveToRDB()失败");
        //    //    }

        //    //    return Convert.ToString(oObject.KeyValue);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MyLog.MakeLog("AddRecordToTable出错了:" + ex.Message);
        //    //    return "";
        //    //}
        //}

        ///// <summary>
        ///// 更新表里面的一个记录
        ///// </summary>
        ///// <param name="sTableName">表名称</param>
        ///// <param name="sPrimKey">表的主键名称</param>
        ///// <param name="sKeyValue">被更新记录的主键值</param>
        ///// <param name="sValueXml">以XML方式打包的字段值</param>
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
        //    //        MyLog.MakeLog("UpdateRecordInTable()更新失败,找不到该对象");
        //    //        return false;
        //    //    }

        //    //    MyXmlDoc oXmlDoc = MyXmlDocMgr.LoadXmlDoc(sValueXml);

        //    //    //sValueXml的格式如: <Items><Item Name="列名1">列值1</Item><Item Name="列名2">列值2</Item></Items>
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
        //    //        throw new Exception("oObject.SaveToRDB()失败");
        //    //    }

        //    //    return true;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MyLog.MakeLog("UpdateRecordInTable出错了:" + ex.Message);
        //    //    return false;
        //    //}
        //}

        ///// <summary>
        ///// 判断当前数据库中是否存在某个表或者视图
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