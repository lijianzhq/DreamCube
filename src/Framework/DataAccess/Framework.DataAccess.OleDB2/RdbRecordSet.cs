using System;
using System.Data;
using System.Text;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// 记录集类 
    /// </summary> 
    public class RdbRecordSet
    {
        #region "字段"

        private DataTable m_DataTable = null;
        private DataTable m_SchemaTable = null;
        private RdbConnMgr m_ConnMgr = null;
        private int m_PageSize = -1;
        private int m_CurPage = 1;
        private bool m_ReadOnly = false;
        private string m_PrimaryKey = "";
        private string m_SQL = "";

        #endregion

        #region "属性"

        /// <summary> 
        /// 记录个数 
        /// </summary> 
        public int RecordCount
        {
            get
            {
                return m_DataTable.Rows.Count;
            }
        }

        /// <summary> 
        /// 列数　lijian
        /// </summary> 
        public int ColumnCount
        {
            get
            {
                return m_DataTable.Columns.Count;
            }
        }

        /// <summary> 
        /// 页大小(如果PageSize发生了改变,则将CurPage定为1), 
        /// 如果没分页的话, PageSize等于RecordCount 
        /// </summary> 
        public int PageSize
        {
            get
            {
                if (m_PageSize == -1)
                {
                    return this.RecordCount;
                }
                else
                {
                    return this.m_PageSize;
                }
            }
            set
            {
                int iOldSize = this.PageSize;

                if (value <= 0)
                {
                    m_PageSize = -1;
                }
                else if (value >= this.RecordCount)
                {
                    m_PageSize = this.RecordCount;
                }
                else
                {
                    m_PageSize = value;
                }

                //如果PageSize发生了改变,则将CurPage定为1 
                if (iOldSize != this.PageSize)
                {
                    this.CurPage = 1;
                }
            }
        }

        /// <summary> 
        /// 当前页序号(第一页是1) 
        /// </summary> 
        public int CurPage
        {
            get
            {
                return m_CurPage;
            }
            set
            {
                if (value <= 0)
                {
                    m_CurPage = 1;
                }
                else if (value >= this.PageCount)
                {
                    m_CurPage = this.PageCount;
                }
                else
                {
                    m_CurPage = value;
                }
            }
        }

        /// <summary> 
        /// 总页数 
        /// </summary> 
        public int PageCount
        {
            get
            {
                if (this.PageSize <= 0)
                {
                    return 1;
                }
                else
                {
                    return (int)(Math.Ceiling((double)this.RecordCount / (double)this.PageSize));
                }
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
        /// 是否有记录 
        /// </summary> 
        public bool HasRecord
        {
            get
            {
                if (this.RecordCount > 0)
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
        /// 各列的标题 
        /// </summary> 
        public string[] ColumnCaptions
        {
            get
            {
                if (m_DataTable.Columns.Count > 0)
                {
                    string[] aCulumnNames = new string[m_DataTable.Columns.Count - 1];
                    for (int i = 0; i <= aCulumnNames.Length - 1; i++)
                    {
                        aCulumnNames[i] = m_DataTable.Columns[i].Caption;
                    }
                    return aCulumnNames;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary> 
        /// 表构架信息表 
        /// </summary> 
        public DataTable SchemaTable
        {
            get
            {
                return m_SchemaTable;
            }
        }

        /// <summary> 
        /// ConnMgr对象 
        /// </summary> 
        public RdbConnMgr ConnMgr
        {
            get
            {
                return this.m_ConnMgr;
            }
        }

        #endregion

        #region "公共实例方法"

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="oTable"></param> 
        public RdbRecordSet(RdbConnMgr oConnMgr, DataTable oTable, DataTable oSchemaTable, bool bReadOnly, string sPrimaryKey = "", string sSQL = "")
        {
            this.m_DataTable = oTable;
            this.m_SchemaTable = oSchemaTable;
            this.m_ConnMgr = oConnMgr;
            this.m_ReadOnly = bReadOnly;
            this.m_PrimaryKey = sPrimaryKey;
            this.m_SQL = sSQL;
        }

        /// <summary> 
        /// 根据序号获取记录(不考虑分页情况, 第一条记录的序号是0) 
        /// </summary> 
        /// <param name="iIndex">记录序号, 从0开始</param> 
        /// <returns></returns> 
        public RdbRecord GetRecord(int iIndex)
        {
            if (iIndex >= 0 && iIndex < this.RecordCount)
            {
                return new RdbRecord(m_DataTable, m_ConnMgr.GetTableSchema(m_DataTable.TableName), m_DataTable.Rows[iIndex], this.ReadOnly, m_PrimaryKey);
            }
            else
            {
                return null;
            }
        }

        /// <summary> 
        /// 获取当前页的第N条记录(0表示第一条记录) 
        /// </summary> 
        /// <param name="iIndex"></param> 
        /// <returns></returns> 
        public RdbRecord GetRecordInCurPage(int iIndex)
        {
            //如果没分页的话, 用GetRecord函数 
            if (this.PageCount <= 1)
            {
                return this.GetRecord(iIndex);
            }

            if (iIndex >= 0 && iIndex < this.PageSize)
            {
                iIndex = this.PageSize * (this.CurPage - 1) + iIndex;
                if (iIndex >= 0 && iIndex < this.RecordCount)
                {
                    return new RdbRecord(m_DataTable, m_ConnMgr.GetTableSchema(m_DataTable.TableName), m_DataTable.Rows[iIndex], this.ReadOnly, m_PrimaryKey);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary> 
        /// 创建新记录 
        /// </summary> 
        /// <returns></returns> 
        public RdbRecord CreateNewRecord()
        {
            try
            {
                if (this.ReadOnly == false)
                {
                    DataRow oNewRow = m_DataTable.NewRow();
                    return new RdbRecord(m_DataTable, m_ConnMgr.GetTableSchema(m_DataTable.TableName), oNewRow, false, m_PrimaryKey);
                }
                else
                {
                    throw new Exception("RdbRecordSet.CreateNewRecord()出错!记录集是只读的");
                }
            }
            catch (Exception e)
            {
                MyLog.MakeLog("CreateNewRecord()发生错误:" + e.Message);
                return null;
            }
        }

        /// <summary> 
        /// 保存修改 
        /// </summary> 
        /// <returns></returns> 
        public bool SaveAll()
        {
            try
            {
                if (this.HasRecord == false)
                {
                    return false;
                }
                int i;
                for (i = 0; i <= this.RecordCount - 1; i++)
                {
                    this.GetRecord(i).Save();
                }

                return true;
            }
            catch (Exception e)
            {
                ConnMgr.LastestErrorMsg = e.Message;
                MyLog.MakeLog("SaveAll()发生错误:" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 生成JSON代码
        /// </summary>
        /// <param name="sItemName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string MakeJsonCode(string sItemName = "")
        {
            RdbRecord oRecord;
            oRecord = this.GetRecord(0);
            if (oRecord == null)
            {
                return "";
            }

            StringBuilder oTotal = new StringBuilder();
            oTotal.Append("[");
            int iRecordCount = this.RecordCount;
            for (int i = 0; i <= iRecordCount - 1; i++)
            {
                oRecord = this.GetRecord(i);
                oTotal.Append(oRecord.MakeJsonCode(sItemName));
                if (i != iRecordCount - 1)
                {
                    oTotal.Append(",");
                }
            }
            oTotal.Append("]");
            return oTotal.ToString();
        }

        #endregion
    }
}