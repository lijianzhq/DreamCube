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
    /// 发货帮助类
    /// </summary>
    public static class OrderSendHelper
    {
        private static Dictionary<String, String> wlNameAndCode = null;

        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="appKey">appkey</param>
        /// <param name="appsecret">appsecret</param>
        private static void InitialWLNameAndCodeMapper(String appKey, String appsecret)
        {
            if (wlNameAndCode == null)
            {
                wlNameAndCode = new Dictionary<String, String>();
                LogisticsCompaniesGetResponse response = WLCompanyHelper.GetAllWLCompanyDetails(appKey, appsecret);
                if (!response.IsError)
                {
                    for (var i = 0; i < response.LogisticsCompanies.Count; i++)
                    {
                        wlNameAndCode.Add(response.LogisticsCompanies[i].Name, response.LogisticsCompanies[i].Code); 
                    }
                }
            }
        }

        /// <summary>
        /// 操作发货
        /// </summary>
        /// <param name="appKey">appkey</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="sessionKey">sessionKey</param>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="wldh">物流单号</param>
        /// <param name="wlgsdm">物流公司代码</param>
        public static Boolean SendOrder(String appKey, String appsecret, String sessionKey, String orderNumber, String wldh, String wlgsmc)
        {
            InitialWLNameAndCodeMapper(appKey, appsecret);
            ITopClient client = TBUtility.GetTopClient(appKey, appsecret);
            LogisticsOfflineSendRequest req = new LogisticsOfflineSendRequest();
            req.Tid = new Nullable<long>(long.Parse(orderNumber));  //淘宝交易ID
            req.OutSid = wldh; //运单号
            req.CompanyCode = wlNameAndCode[wlgsmc];//物流公司代码
            LogisticsOfflineSendResponse response = client.Execute(req, sessionKey);
            if (response.IsError)
            {
                TBUtility.MakeLog(response);
            }
            return response.Shipping.IsSuccess;
        }
    }
}
