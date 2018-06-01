using System;
using System.Collections.Generic;

using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    /// <summary>
    /// 商品相关的类
    /// </summary>
    public class GoodItemHelper
    {
        #region "私有字段"

        //private static String goodFields = "cid,seller_cids,props,input_pids,input_str,pic_url,num,list_time,delist_time,stuff_status,location,price,post_fee,express_fee,ems_fee,has_discount,freight_payer,has_invoice,has_warranty,has_showcase,modified,increment,approve_status,postage_id,product_id,item_imgs,prop_imgs,is_virtual,is_taobao,is_ex,is_timing,videos,is_3D,score,one_station,second_kill,violation,is_prepay,ww_status,wap_desc,wap_detail_url,cod_postage_id,sell_promise,detail_url,num_iid,title,nick,type,desc,skus,props_name,created,promoted_service,is_lightning_consignment,is_fenxiao,auction_point,property_alias,template_id,after_sale_id,is_xinpin,sub_stock,inner_shop_auction_template_id,outer_shop_auction_template_id,food_security,features,locality_life,desc_module_info,item_weight,item_size,with_hold_quantity,change_prop,delivery_time,paimai_info,sell_point,valid_thru,outer_id,auto_fill,desc_modules,custom_made_type_id,wireless_desc,is_offline,barcode,is_cspu,newprepay,sub_title,video_id,mpic_video,sold_quantity,second_result,qualification,shop_type,open_iid,global_stock_type,global_stock_country";
        private static String goodFields = "cid,seller_cids,props,pic_url,num,list_time,delist_time,stuff_status,location,price,post_fee,express_fee,ems_fee,has_discount,freight_payer,has_invoice,has_warranty,has_showcase,modified,increment,approve_status,postage_id,product_id,is_virtual,is_taobao,is_ex,is_timing,videos,is_3D,score,one_station,second_kill,violation,is_prepay,ww_status,wap_desc,cod_postage_id,sell_promise,detail_url,num_iid,title,nick,type,desc,skus,props_name,created,promoted_service,is_lightning_consignment,is_fenxiao,auction_point,property_alias,template_id,after_sale_id,is_xinpin,sub_stock,inner_shop_auction_template_id,outer_shop_auction_template_id,food_security,features,locality_life,item_weight,item_size,with_hold_quantity,change_prop,delivery_time,paimai_info,sell_point,valid_thru,outer_id,auto_fill,desc_modules,custom_made_type_id,wireless_desc,is_offline,barcode,is_cspu,newprepay,sub_title,video_id,mpic_video,sold_quantity,second_result,qualification,shop_type,open_iid,global_stock_type,global_stock_country";

        #endregion

        #region "公共方法"

        /// <summary>
        /// 根据商品的id，获取单个商品的信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <param name="num_iid"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ItemGetResponse GetItem(String appKey, String appSecret, String sessionKey, long num_iid, String fields = "")
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            ItemGetRequest req = new ItemGetRequest();
            req.Fields = String.IsNullOrEmpty(fields) ? goodFields : fields;
            req.NumIid = num_iid;
            ItemGetResponse response = client.Execute(req, sessionKey);
            return response;
        }

        /// <summary>
        /// 获取当前会话用户出售中的商品列表
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <param name="fields">获取商品的字段信息</param>
        /// <returns></returns>
        public static List<Item> GetCurrentShopAllGoods(String appKey, String appSecret, String sessionKey,String fields = "")
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            req.Fields = String.IsNullOrEmpty(fields) ? goodFields : fields;
            //搜索字段。搜索商品的title。
            //req.Q = "N97";
            //商品类目ID。ItemCat中的cid字段。可以通过taobao.itemcats.get取到
            //支持最小值为：0
            //req.Cid = 1512L;
            //卖家店铺内自定义类目ID。多个之间用“,”分隔。可以根据taobao.sellercats.list.get获得.(注：目前最多支持32个ID号传入)
            //req.SellerCids = "11";
            //页码。取值范围:大于零的整数。默认值为1,即默认返回第一页数据。
            //用此接口获取数据时，当翻页获取的条数（page_no*page_size）超过10万,为了保护后台搜索引擎，接口将报错。所以请大家尽可能的细化自己的搜索条件，例如根据修改时间分段获取商品
            req.PageNo = 1L;
            //是否参与会员折扣。可选值：true，false。默认不过滤该条件
            //req.HasDiscount = true;
            //是否橱窗推荐。 可选值：true，false。默认不过滤该条件
            //req.HasShowcase = true;
            //排序方式。格式为column:asc/desc ，column可选值:list_time(上架时间),delist_time(下架时间),num(商品数量)，modified(最近修改时间)，sold_quantity（商品销量）,;默认上架时间降序(即最新上架排在前面)。如按照上架时间降序排序方式为list_time:desc
            //req.OrderBy = "list_time:desc";
            //商品是否在淘宝显示
            //req.IsTaobao = true;
            //商品是否在外部网店显示
            //req.IsEx = true;
            //每页条数。取值范围:大于零的整数;最大值：200；默认值：40。用此接口获取数据时，当翻页获取的条数（page_no*page_size）超过2万,为了保护后台搜索引擎，接口将报错。所以请大家尽可能的细化自己的搜索条件，例如根据修改时间分段获取商品
            req.PageSize = 100L;
            //起始的修改时间
            //DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
            //req.StartModified = dateTime;
            //DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
            //req.EndModified = dateTime;
            //是否挂接了达尔文标准产品体系。
            //req.IsCspu = true;
            List<Item> allItems = new List<Item>();
            ItemsOnsaleGetResponse response = client.Execute(req, sessionKey);
            long pageNO = 1;
            if (!response.IsError)
            {
                allItems.AddRange(response.Items);
                //循环获取所有的数据
                while (response.TotalResults > allItems.Count)
                {
                    pageNO++;
                    req = new ItemsOnsaleGetRequest();
                    req.Fields = String.IsNullOrEmpty(fields) ? goodFields : fields;
                    req.PageNo = pageNO;
                    req.PageSize = 100L;
                    response = client.Execute(req, sessionKey);
                    allItems.AddRange(response.Items);
                }
            }
            return allItems;
        }

        #endregion
    }
}
