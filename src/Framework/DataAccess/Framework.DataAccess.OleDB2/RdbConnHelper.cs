using System;
using System.Collections;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Objects;
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Objects.EqualityComparers;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    /// <summary> 
    /// RdbMgr���󴴽��� 
    /// </summary> 
    public static class RdbConnHelper
    {
        #region "�ֶ�"

        private static string m_LastestErrorMsg = ""; //�쳣��Ϣ
        //������������������Win����,��������Web����,��ΪWeb�����е��ྲ̬���������÷�Χ������HttpContext
        private static List<RdbConnMgr> m_ExistedConnMgrs = new List<RdbConnMgr>();

        /// <summary>
        /// ���Ӷ���Ļ����Ӧ��Keyֵ����ǰ�����ı���һ�����Ӷ��󼴿ɣ�
        /// </summary>
        private static String connCacheKey = "DreamCube.Framework.DataAccess.OleDB2.RdbConnHelper";

        #endregion

        #region "����"

        /// <summary> 
        /// ���������һ���������Ϣ 
        /// </summary> 
        public static string LastestErrorMsg
        {
            get
            {
                return m_LastestErrorMsg;
            }
        }

        #endregion

        #region "������̬����"

        /// <summary> 
        /// �������ӹ������(ʹ��ָ�������Ӳ���) 
        /// </summary> 
        /// <param name="oConnParam">���Ӳ�����</param> 
        /// <param name="bCreateNewConn">�Ƿ�Ҫǿ�д���һ���µ����ݿ�����</param>
        /// <returns></returns> 
        public static RdbConnMgr CreateRdbConnMgr(ConnectParam oConnParam, bool bCreateNewConn = false)
        {
            RdbConnMgr oConnMgr = CurrentContext.GetCacheItem<RdbConnMgr>(connCacheKey);
            if (oConnMgr != null) return oConnMgr;
            oConnMgr = new RdbConnMgr(oConnParam);
            
            if (oConnMgr.IsValid)
            {
                CurrentContext.TryCacheItem(connCacheKey, oConnMgr);
                return oConnMgr;
            }
            else
            {
                m_LastestErrorMsg = oConnMgr.LastestErrorMsg;
                return null;
            }
        }

        /// <summary> 
        /// �������ӹ������(ʹ��ָ�������Ӳ���) 
        /// </summary> 
        /// <param name="iDbType"></param> 
        /// <param name="sHost">�����Access���ݿ�,������Ϊmdb�ļ�·��</param> 
        /// <param name="sPort"></param> 
        /// <param name="sDbName"></param> 
        /// <param name="sUser"></param> 
        /// <param name="sPassword"></param> 
        /// <returns></returns> 
        public static RdbConnMgr CreateRdbConnMgr(int iDbType, string sHost, string sPort, string sDbName, string sUser, string sPassword)
        {
            ConnectParam oConnParam = null;
            if (iDbType >= DatabaseVersionRange.SqlServer_Min.GetHashCode() && iDbType <= DatabaseVersionRange.SqlServer_Max.GetHashCode())
            {
                oConnParam = new SqlConnectParam(iDbType, sHost, sDbName, sUser, sPassword);
            }
            else if (iDbType >= DatabaseVersionRange.Oracle_Min.GetHashCode() && iDbType <= DatabaseVersionRange.Oracle_Max.GetHashCode())
            {
                oConnParam = new OracleConnectParam(iDbType, sHost, sPort, sDbName, sUser, sPassword);
            }
            else if (iDbType >= DatabaseVersionRange.Access_Min.GetHashCode() && iDbType <= DatabaseVersionRange.Access_Max.GetHashCode())
            {
                oConnParam = new AccessConnectParam(iDbType, sHost, sUser, sPassword);
            }
            return RdbConnHelper.CreateRdbConnMgr(oConnParam);
        }

        /// <summary> 
        /// ������е����ӻ��� 
        /// </summary> 

        public static void CloseAllConnections()
        {
            ArrayList aConnMgrs = MyWebCache.GetItem("CmisRdbConnMgrs") as ArrayList;
            ArrayList aConnStrings = MyWebCache.GetItem("CmisRdbConnStrings") as ArrayList;
            List<Int32> aIndexs = null;
            int i = 0;
            if (aConnMgrs != null)
            {
                for (i = 0; i <= aConnMgrs.Count - 1; i++)
                {
                    if (aConnMgrs[i] != null)
                    {
                        if (((RdbConnMgr)aConnMgrs[i]).Close())
                        {
                            //��¼��Ҫ������Ķ����Index
                            if (aIndexs == null)
                            {
                                aIndexs = new List<Int32>();
                            }
                            aIndexs.Add(i);
                        }
                    }
                }
            }

            //�ӻ�������������رյ����Ӷ���
            if (aIndexs != null)
            {
                for (i = 0; i <= aIndexs.Count - 1; i++)
                {
                    aConnMgrs.RemoveAt(aIndexs[i] - i);
                    aConnStrings.RemoveAt(aIndexs[i] - i);
                }
                if (aConnMgrs.Count == 0)
                {
                    MyWebCache.RemoveItem("CmisRdbConnMgrs");
                    MyWebCache.RemoveItem("CmisRdbConnStrings");
                    //MyLog.MakeLog("���汻�����!")
                }
            }
        }

        #endregion 

        #region "˽�о�̬����"

        /// <summary>
        /// �������ӹ������(������Web����)
        /// </summary>
        /// <param name="oConnParam"></param>
        /// <param name="bCreateNewConn">�Ƿ�Ҫǿ�д���һ���µ����ݿ�����</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static RdbConnMgr CreateRdbConnMgr_Web(ConnectParam oConnParam, bool bCreateNewConn = false)
        {
            string sConnString = string.Empty;
            if (oConnParam != null) sConnString = oConnParam.MakeConnectString();

            ArrayList aExistedConnMgrs = null;
            ArrayList aConnStrings = null;

            aExistedConnMgrs = (ArrayList)MyWebCache.GetItem("CmisRdbConnMgrs");
            aConnStrings = (ArrayList)MyWebCache.GetItem("CmisRdbConnStrings");

            //����Ѿ�����һ����ͬ���Ӵ������Ӷ���ֱ�ӷ��ظ����Ӷ���
            if (bCreateNewConn == false && aExistedConnMgrs != null)
            {
                for (int i = 0; i <= aExistedConnMgrs.Count - 1; i++)
                {
                    if (aConnStrings[i].ToString() == sConnString)
                    {
                        return (RdbConnMgr)aExistedConnMgrs[i];
                    }
                }
            }

            if (aExistedConnMgrs == null)
            {
                aExistedConnMgrs = new ArrayList();
                aConnStrings = new ArrayList();
            }

            RdbConnMgr oConnMgr = new RdbConnMgr(oConnParam);

            if (oConnMgr.IsValid)
            {
                aExistedConnMgrs.Add(oConnMgr);
                aConnStrings.Add(sConnString);
                MyWebCache.AddItem("CmisRdbConnMgrs", aExistedConnMgrs);
                MyWebCache.AddItem("CmisRdbConnStrings", aConnStrings);
                return oConnMgr;
            }
            else
            {
                m_LastestErrorMsg = oConnMgr.LastestErrorMsg;
                return null;
            }
        }

        /// <summary>
        /// �������ӹ������(�������������)
        /// </summary>
        /// <param name="oConnParam"></param>
        /// <param name="bCreateNewConn">�Ƿ�Ҫǿ�д���һ���µ����ݿ�����</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static RdbConnMgr CreateRdbConnMgr_Win(ConnectParam oConnParam = null, bool bCreateNewConn = false)
        {
            //string sConnString = string.Empty;
            //if (oConnParam != null) sConnString = oConnParam.MakeConnectString();

            //if (bCreateNewConn == false && m_ExistedConnMgrs.Count > 0)
            //{
            //    for (int i = 0; i <= m_ExistedConnMgrs.Count - 1; i++)
            //    {
            //        if (m_ConnStrings[i].ToString() == sConnString)
            //            return m_ExistedConnMgrs[i];
            //    }
            //}

            //RdbConnMgr oConnMgr = null;
            //if (oConnParam == null) oConnMgr = new RdbConnMgr();
            //else oConnMgr = new RdbConnMgr(oConnParam);

            //if (oConnMgr.IsValid)
            //{
            //    m_ExistedConnMgrs.Add(oConnMgr);
            //    m_ConnStrings.Add(sConnString);
            //    return oConnMgr;
            //}
            //else
            //{
            //    m_LastestErrorMsg = oConnMgr.LastestErrorMsg;
            //    return null;
            //}
            throw new NotImplementedException();
        }

        #endregion
    }
}