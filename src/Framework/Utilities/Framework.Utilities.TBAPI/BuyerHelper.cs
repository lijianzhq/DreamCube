using System;
using System.Collections.Generic;
using System.Text;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 买家信息查询帮助类
    /// </summary>
    public static class BuyerHelper
    {
        /// <summary>
        /// 获取用户的等级信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <param name="userNick"></param>
        /// <returns></returns>
        public static UserGetResponse GetUserInfo(String appKey, String appSecret, String sessionKey, String userNick)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            UserGetRequest req = new UserGetRequest();
            req.Fields = "buyer_credit,sex";
            req.Nick = userNick;
            UserGetResponse response = client.Execute(req, sessionKey);
            return response;
        }
    }
}
