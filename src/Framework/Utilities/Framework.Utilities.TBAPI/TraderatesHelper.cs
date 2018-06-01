using System;
using System.Collections.Generic;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api.Request;

using DCUtility = DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 评价的帮助类
    /// </summary>
    public static class TraderatesHelper
    {
        /// <summary>
        /// 获取店铺订单
        /// </summary>
        /// <param name="client">client对象</param>
        /// <param name="sessionKey">获取到的session值</param>
        /// <param name="orderID">订单编号</param>
        /// <param name="result">评价结果（中差评）</param>
        /// <param name="oid">子订单编号</param>
        /// <param name="content">评价内容</param>
        /// <param name="role">评价角色，一般都是seller了</param>
        public static Boolean AddTraderate(ITopClient client, String sessionKey, String orderID, String result, String oid = "", String content = "", String role = "seller")
        {
            TraderateAddRequest req = new TraderateAddRequest();
            req.Tid = long.Parse(orderID);
            if (!String.IsNullOrEmpty(oid)) req.Oid = long.Parse(oid); //子订单编号
            req.Result = result;
            req.Role = role;
            req.Content = content;
            req.Anony = false;
            TraderateAddResponse response = client.Execute(req, sessionKey);
            if (response.IsError) TBUtility.MakeLog(response);
            return !response.IsError && response.TradeRate != null;
        }

        /// <summary>
        /// 获取店铺订单
        /// </summary>
        /// <param name="client">client对象</param>
        /// <param name="sessionKey">获取到的session值</param>
        /// <param name="startDate">搜索订单的开始日期</param>
        /// <param name="endDate">搜索订单的结束日期</param>
        /// <param name="status">订单的状态</param>
        /// <param name="field">订单的字段</param>
        /// <param name="type">交易类型</param>
        public static TraderatesGetResponse GetTraderates(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String field = "", Int32 pageIndex = 1)
        {
            TraderatesGetRequest req = new TraderatesGetRequest();
            req.Fields = String.IsNullOrEmpty(field) ? "tid,oid,role,nick,result,created,rated_nick,item_title,item_price,content,reply,num_iid" : field;
            req.RateType = "get";
            req.Role = "buyer";
            req.PageNo = 1L;
            req.PageSize = 100L;
            req.StartDate = startDate;
            req.EndDate = endDate;
            req.UseHasNext = true;
            TraderatesGetResponse response = client.Execute(req, sessionKey);
            return response;
        }

        /// <summary>
        /// 获取店铺订单
        /// </summary>
        /// <param name="client">client对象</param>
        /// <param name="sessionKey">获取到的session值</param>
        /// <param name="startDate">搜索订单的开始日期</param>
        /// <param name="endDate">搜索订单的结束日期</param>
        /// <param name="status">订单的状态</param>
        /// <param name="field">订单的字段</param>
        /// <param name="type">交易类型</param>
        public static List<TradeRate> GetAllTraderates(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String field = "")
        {
            TraderatesGetResponse response = GetTraderates(client, sessionKey, startDate, endDate, field);
            List<TradeRate> tradeList = new List<TradeRate>();
            if (!response.IsError)
            {
                tradeList.AddRange(response.TradeRates);
                Int32 index = 2;
                while (!response.IsError && response.HasNext)
                {
                    response = GetTraderates(client, sessionKey, startDate, endDate, field, index);
                    if (response.IsError) TBUtility.MakeLog(response);
                    else tradeList.AddRange(response.TradeRates);
                    index++;
                }
            }
            else
            {
                TBUtility.MakeLog(response);
                return null;
            }
            return tradeList;
        }
    }
}
