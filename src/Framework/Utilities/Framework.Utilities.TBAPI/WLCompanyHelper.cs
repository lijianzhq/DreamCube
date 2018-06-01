using System;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api.Request;

using DreamCube.Foundation.Serialization;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 物流公司的帮助类
    /// </summary>
    public static class WLCompanyHelper
    {
        /// <summary>
        /// 根据物流公司名称获取物流公司信息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appsecret"></param>
        public static LogisticsCompaniesGetResponse GetAllWLCompanyDetails(String appkey, String appsecret)
        {
            ITopClient client = TBUtility.GetTopClient(appkey, appsecret);
            LogisticsCompaniesGetRequest req = new LogisticsCompaniesGetRequest();
            //id：物流公司ID 
            //code：物流公司code 
            //name：物流公司名称 
            //reg_mail_no：物流公司对应的运单规则
            req.Fields = "id,code,name,reg_mail_no";
            req.IsRecommended = true;
            req.OrderMode = "offline";
            LogisticsCompaniesGetResponse response = client.Execute(req);
            return response;
        }

        /// <summary>
        /// 根据物流公司名称获取物流公司信息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appsecret"></param>
        public static String GetAllWLCompanyDetailsStr(String appkey, String appsecret)
        {
            return MyJson.Serialize(GetAllWLCompanyDetails(appkey, appsecret).LogisticsCompanies);
        }
    }
}
