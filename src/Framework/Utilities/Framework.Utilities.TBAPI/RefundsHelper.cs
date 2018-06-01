using System;
using System.Collections.Generic;
using System.ComponentModel;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    public static class RefundsHelper
    {
        #region "私有字段"

        /// <summary>
        /// 退货的字段信息
        /// </summary>
        private static String fields = "refund_id, tid, title, buyer_nick, seller_nick, total_fee, status, created, refund_fee, oid, good_status, company_name, sid, payment, reason, desc, has_good_return, modified, order_status,refund_phase";

        #endregion

        #region "公共方法"

        /// <summary>
        /// 获取退款的详细信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static Refund GetTKDetails(ITopClient client, String sessionKey, long refundID)
        {
            RefundGetRequest req = new RefundGetRequest();
            req.Fields = "refund_remind_timeout";
            req.RefundId = refundID;
            RefundGetResponse response = client.Execute(req, sessionKey);
            return response.Refund;
        }

        /// <summary>
        /// 获取退款的详细信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static Refund GetTKDetails(String appKey, String appSecret, String sessionKey, long refundID)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetTKDetails(client, sessionKey, refundID);
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static RefundsReceiveGetResponse GetOrderTKData(String appKey, String appSecret, String sessionKey, DateTime? startDate, DateTime? endDate, String fields = "", long pageIndex = 1L)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetOrderTKData(client, sessionKey, startDate, endDate, fields, pageIndex);
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static RefundsReceiveGetResponse GetOrderTKData(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String fields = "", long pageIndex = 1L)
        {
            RefundsReceiveGetRequest req = new RefundsReceiveGetRequest();
            if (!String.IsNullOrEmpty(fields))
                req.Fields = fields;
            else
                req.Fields = RefundsHelper.fields;
            req.StartModified = startDate;
            req.EndModified = endDate;
            req.PageNo = pageIndex;
            req.PageSize = 40L;
            req.UseHasNext = true;
            RefundsReceiveGetResponse response = client.Execute(req, sessionKey);
            return response;
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<Refund> GetAllOrderTKData(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String fields = "")
        {
            List<Refund> refundList = new List<Refund>();
            RefundsReceiveGetResponse response = GetOrderTKData(client, sessionKey, startDate, endDate, fields, 1);
            if (!response.IsError)
            {
                refundList.AddRange(response.Refunds);
                long pageNO = 2L;
                while (response.HasNext)
                {
                    response = GetOrderTKData(client, sessionKey, startDate, endDate, fields, pageNO);
                    if (!response.IsError) refundList.AddRange(response.Refunds);
                    else TBUtility.MakeLog(response);
                    pageNO++;
                }
            }
            else
            {
                TBUtility.MakeLog(response);
                return null;
            }
            return refundList;
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<Refund> GetAllOrderTKData(String appKey, String appSecret, String sessionKey, DateTime? startDate, DateTime? endDate, String fields = "")
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetAllOrderTKData(client, sessionKey, startDate, endDate, fields);
        }

        #endregion
    }
}
