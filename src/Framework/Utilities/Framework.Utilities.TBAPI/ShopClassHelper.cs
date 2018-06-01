using System;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 店铺自定义类目的帮助类
    /// </summary>
    public static class ShopClassHelper
    {
        /// <summary>
        /// 根据类目的ID获取类目的名称
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="shopName">店铺名称</param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static SellercatsListGetResponse GetShopClass(String appKey, String appSecret, String shopName)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            SellercatsListGetRequest req = new SellercatsListGetRequest();
            req.Nick = shopName;
            SellercatsListGetResponse response = client.Execute(req);
            return response;
        }

        /// <summary>
        /// 根据类目的ID获取类目的名称
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="shopName">店铺名称</param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static String GetShopClassName(String appKey, String appSecret, String shopName, String cid)
        {
            SellercatsListGetResponse response = GetShopClass(appKey, appSecret, shopName);
            if (!response.IsError && response.SellerCats.Count>0)
            {
                String[] cids = cid.Split(',');
                String shopClassName = String.Empty;
                for (var k = 0; k < cids.Length; k++)
                {
                    if (!String.IsNullOrEmpty(cids[k]))
                    {
                        for (var i = 0; i < response.SellerCats.Count; i++)
                        {
                            if (response.SellerCats[i].Cid.ToString() == cids[k])
                            {
                                if (!String.IsNullOrEmpty(shopClassName)) shopClassName += ",";
                                shopClassName += response.SellerCats[i].Name;
                            }
                        }
                    }
                }
                return shopClassName;
            }
            return String.Empty;
        }
    }
}
