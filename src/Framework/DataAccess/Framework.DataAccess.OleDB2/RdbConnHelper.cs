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
    /// RdbMgr对象创建类 
    /// </summary> 
    public static class RdbConnHelper
    {
        #region "字段"

        private static string m_LastestErrorMsg = ""; //异常信息
        //以下两个参数适用于Win环境,不能用于Web环境,因为Web环境中的类静态变量其作用范围是所有HttpContext
        private static List<RdbConnMgr> m_ExistedConnMgrs = new List<RdbConnMgr>();

        /// <summary>
        /// 连接对象的缓存对应的Key值（当前上下文保持一个连接对象即可）
        /// </summary>
        private static String connCacheKey = "DreamCube.Framework.DataAccess.OleDB2.RdbConnHelper";

        #endregion

        #region "属性"

        /// <summary> 
        /// 最近发生的一个错误的信息 
        /// </summary> 
        public static string LastestErrorMsg
        {
            get
            {
                return m_LastestErrorMsg;
            }
        }

        #endregion

        #region "公共静态方法"

        /// <summary> 
        /// 创建连接管理对象(使用指定的连接参数) 
        /// </summary> 
        /// <param name="oConnParam">连接参数类</param> 
        /// <param name="bCreateNewConn">是否要强行创建一个新的数据库链接</param>
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
        /// 创建连接管理对象(使用指定的连接参数) 
        /// </summary> 
        /// <param name="iDbType"></param> 
        /// <param name="sHost">如果是Access数据库,本参数为mdb文件路径</param> 
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
        /// 清空所有的联接缓存 
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
                            //记录下要被清除的对象的Index
                            if (aIndexs == null)
                            {
                                aIndexs = new List<Int32>();
                            }
                            aIndexs.Add(i);
                        }
                    }
                }
            }

            //从缓存中清除调被关闭的连接对象
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
                    //MyLog.MakeLog("缓存被清空了!")
                }
            }
        }

        #endregion 

        #region "私有静态方法"

        /// <summary>
        /// 创建连接管理对象(适用于Web程序)
        /// </summary>
        /// <param name="oConnParam"></param>
        /// <param name="bCreateNewConn">是否要强行创建一个新的数据库链接</param>
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

            //如果已经存在一个相同连接串的链接对象，直接返回该链接对象
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
        /// 创建连接管理对象(适用于桌面程序)
        /// </summary>
        /// <param name="oConnParam"></param>
        /// <param name="bCreateNewConn">是否要强行创建一个新的数据库链接</param>
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