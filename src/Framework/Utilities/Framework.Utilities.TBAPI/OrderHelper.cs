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
    /// <summary>
    /// 订单相关的类
    /// </summary>
    public static class OrderHelper
    {
        #region "私有字段"

        /// <summary>
        /// 交易的字段
        /// </summary>
        private static String tradField = "seller_nick, buyer_nick, title, type, created, tid, seller_rate,seller_can_rate, buyer_rate,can_rate, status, payment, discount_fee, adjust_fee, post_fee, total_fee, pay_time, end_time, modified, consign_time, buyer_obtain_point_fee, point_fee, real_point_fee, received_payment, pic_path, num_iid, num, price, cod_fee, cod_status, shipping_type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, receiver_zip, receiver_mobile, receiver_phone,seller_flag,alipay_id,alipay_no,is_lgtype,is_force_wlb,is_brand_sale,buyer_area,has_buyer_message, credit_card_fee, lg_aging_type, lg_aging, step_trade_status,step_paid_fee,mark_desc,has_yfx,yfx_fee,yfx_id,yfx_type,trade_source,send_time,is_daixiao,is_wt,is_part_consign,zero_purchase,trade_from";

        /// <summary>
        /// 订单的字段
        /// </summary>
        private static String orderField = "orders";

        #endregion

        #region "公共方法"

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<Shipping> GetOrderWLXX(String appKey, String appSecret, String sessionKey, String orderNumber)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetOrderWLXX(client, sessionKey, orderNumber);
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<Shipping> GetOrderWLXX(ITopClient client, String sessionKey, String orderNumber)
        {
            LogisticsOrdersGetRequest req = new LogisticsOrdersGetRequest();
            req.Fields = "out_sid,company_name";
            req.Tid = long.Parse(orderNumber);
            LogisticsOrdersGetResponse response = client.Execute(req, sessionKey);
            return response.Shippings;
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static String GetOrderStatus(String appKey, String appSecret, String sessionKey, String orderNumber)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetOrderStatus(client, sessionKey, orderNumber);
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static String GetOrderStatus(ITopClient client, String sessionKey, String orderNumber)
        {
            TradeGetRequest req = new TradeGetRequest();
            req.Fields = "status";
            req.Tid = long.Parse(orderNumber);
            TradeGetResponse response = client.Execute(req, sessionKey);
            if (!response.IsError)
            {
                return response.Trade.Status;
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取订单的物流标签
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<LogisticsTag> GetOrderServiceTags(ITopClient client, String sessionKey, String orderNumber)
        {
            TradeGetRequest req = new TradeGetRequest();
            req.Fields = "service_tags";
            req.Tid = long.Parse(orderNumber);
            TradeGetResponse response = client.Execute(req, sessionKey);
            if (!response.IsError)
            {
                return response.Trade.ServiceTags;
            }
            return null;
        }

        /// <summary>
        /// 获取订单的当前状态
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static Trade GetTradeInfo(ITopClient client, String sessionKey, String orderNumber, String fields = "")
        {
            TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            if (!String.IsNullOrEmpty(fields))
                req.Fields = fields;
            else
                req.Fields = tradField;
            req.Tid = long.Parse(orderNumber);
            TradeFullinfoGetResponse response = client.Execute(req, sessionKey);
            if (!response.IsError)
            {
                return response.Trade;
            }
            else
            {
                TBUtility.MakeLog(response);
            }
            return null;
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
        public static TradesSoldGetResponse GetShopOrders(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String field = "", OrderStatus status = OrderStatus.ALL_ORDERS, TradeType tradeType = TradeType.SYS_DEFAULT, Int32 pageIndex = 1)
        {
            TradesSoldGetRequest req = new TradesSoldGetRequest();
            //返回订单的字段
            req.Fields = String.IsNullOrEmpty(field) ? (tradField + "," + orderField) : field;
            //查询三个月内交易创建时间开始。格式:yyyy-MM-dd HH:mm:ss
            req.StartCreated = startDate;
            //查询交易创建时间结束。格式:yyyy-MM-dd HH:mm:ss
            req.EndCreated = endDate;
            //订单的状态
            if (status != OrderStatus.ALL_ORDERS) req.Status = Enum.GetName(typeof(OrderStatus), status);
            //交易类型列表，同时查询多种交易类型可用逗号分隔。
            //默认同时查询guarantee_trade, auto_delivery, ec, cod,step的5种交易类型的数据；
            //查询所有交易类型的数据，需要设置下面全部可选值。 
            //可选值： 
            //fixed(一口价) 
            //auction(拍卖) 
            //guarantee_trade(一口价、拍卖) 
            //step(分阶段付款，万人团，阶梯团订单） 
            //independent_simple_trade(旺店入门版交易) 
            //independent_shop_trade(旺店标准版交易) 
            //auto_delivery(自动发货)
            //ec(直冲) 
            //cod(货到付款) 
            //game_equipment(游戏装备) 
            //shopex_trade(ShopEX交易) 
            //netcn_trade(万网交易) 
            //external_trade(统一外部交易) 
            //instant_trade (即时到账) 
            //b2c_cod(大商家货到付款) 
            //hotel_trade(酒店类型交易) 
            //super_market_trade(商超交易) 
            //super_market_cod_trade(商超货到付款交易) 
            //taohua(淘花网交易类型） 
            //waimai(外卖交易类型） 
            //nopaid（即时到帐/趣味猜交易类型） 
            //step (万人团) 
            //eticket(电子凭证) 
            //tmall_i18n（天猫国际）;
            //nopaid （无付款交易）insurance_plus（保险）finance（基金） 
            //注：guarantee_trade是一个组合查询条件，并不是一种交易类型，获取批量或单个订单中不会返回此种类型的订单。 pre_auth_type(预授权0元购)
            if (tradeType != TradeType.SYS_DEFAULT) req.Type = Enum.GetName(typeof(TradeType), tradeType);
            //可选值有ershou(二手市场的订单）,service（商城服务子订单）mark（双十一大促活动异常订单）作为扩展类型筛选只能做单个ext_type查询，不能全部查询所有的ext_type类型
            //req.ExtType = "service";
            //评价状态，默认查询所有评价状态的数据，除了默认值外每次只能查询一种状态。
            //可选值： RATE_UNBUYER(买家未评) 
            // RATE_UNSELLER(卖家未评) 
            // RATE_BUYER_UNSELLER(买家已评，卖家未评) 
            // RATE_UNBUYER_SELLER(买家未评，卖家已评) 
            // RATE_BUYER_SELLER(买家已评,卖家已评)
            // req.RateStatus = "RATE_UNBUYER";
            //卖家对交易的自定义分组标签，目前可选值为：time_card（点卡软件代充），fee_card（话费软件代充）
            //req.Tag = "time_card";
            //页码。取值范围:大于零的整数; 默认值:1
            req.PageNo = pageIndex;
            //每页条数。取值范围:大于零的整数; 默认值:40;最大值:100
            req.PageSize = 100;
            //是否启用has_next的分页方式，如果指定true,则返回的结果中不包含总记录数，但是会新增一个是否存在下一页的的字段，通过此种方式获取增量交易，接口调用成功率在原有的基础上有所提升。
            req.UseHasNext = true;
            TradesSoldGetResponse response = client.Execute(req, sessionKey);
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
        public static List<Trade> GetShopAllOrders(ITopClient client, String sessionKey, DateTime? startDate, DateTime? endDate, String field = "", OrderStatus status = OrderStatus.ALL_ORDERS, TradeType tradeType = TradeType.SYS_DEFAULT)
        {
            TradesSoldGetResponse response = GetShopOrders(client, sessionKey, startDate, endDate, field, status, tradeType);
            List<Trade> tradeList = new List<Trade>();
            if (!response.IsError)
            {
                tradeList.AddRange(response.Trades);
                Int32 index = 2;
                while (!response.IsError && response.HasNext)
                {
                    response = GetShopOrders(client, sessionKey, startDate, endDate, field, status, tradeType, index);
                    if (response.IsError) TBUtility.MakeLog(response);
                    else tradeList.AddRange(response.Trades);
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

        /// <summary>
        /// 根据起始日期获取订单数据
        /// </summary>
        /// <param name="appKey">应用的AppKey</param>
        /// <param name="appSecret">应用的AppSecret</param>
        /// <param name="sessionKey">获取的sessionKey值</param>
        /// <param name="startDate">搜索订单的开始日期</param>
        /// <param name="endDate">搜索订单的结束日期</param>
        /// <param name="status">订单的状态</param>
        /// <param name="field">订单的字段</param>
        /// <param name="type">交易类型</param>
        public static TradesSoldGetResponse GetShopOrders(String appKey, String appSecret, String sessionKey, DateTime startDate, DateTime endDate, OrderStatus status, String field = "", TradeType tradeType = TradeType.SYS_DEFAULT)
        {
            ITopClient client = TBUtility.GetTopClient(appKey, appSecret);
            return GetShopOrders(client, sessionKey, startDate, endDate, field, status, tradeType);
        }

        #endregion

        #region "枚举值"

        /// <summary>
        /// 交易类型
        /// </summary>
        public enum TradeType
        {
            /// <summary>
            /// 采用系统默认值
            /// </summary>
            SYS_DEFAULT,

            /// <summary>
            /// (一口价) 
            /// </summary>
            FIXED,

            /// <summary>
            /// (拍卖) 
            /// </summary>
            AUCTION,

            /// <summary>
            /// (一口价、拍卖) 
            /// </summary>
            GUARANTEE_TRADE,

            /// <summary>
            /// (分阶段付款，万人团，阶梯团订单） 
            /// </summary>
            STEP,

            /// <summary>
            /// (旺店入门版交易) 
            /// </summary>
            INDEPENDENT_SIMPLE_TRADE,

            /// <summary>
            /// (旺店标准版交易)
            /// </summary>
            INDEPENDENT_SHOP_TRADE,

            /// <summary>
            /// (自动发货)
            /// </summary>
            AUTO_DELIVERY,

            /// <summary>
            /// (直冲) 
            /// </summary>
            EC,

            /// <summary>
            /// (货到付款) 
            /// </summary>
            COD,

            /// <summary>
            /// (游戏装备) 
            /// </summary>
            GAME_EQUIPMENT,

            /// <summary>
            /// (ShopEX交易) 
            /// </summary>
            SHOPEX_TRADE,

            /// <summary>
            /// (万网交易) 
            /// </summary>
            NETCN_TRADE,

            /// <summary>
            /// (统一外部交易) 
            /// </summary>
            EXTERNAL_TRADE,

            /// <summary>
            /// (即时到账) 
            /// </summary>
            INSTANT_TRADE,

            /// <summary>
            /// (大商家货到付款) 
            /// </summary>
            B2C_COD,

            /// <summary>
            /// (酒店类型交易) 
            /// </summary>
            HOTEL_TRADE,

            /// <summary>
            /// (商超交易) 
            /// </summary>
            SUPER_MARKET_TRADE,

            /// <summary>
            /// (商超货到付款交易) 
            /// </summary>
            SUPER_MARKET_COD_TRADE,

            /// <summary>
            /// (淘花网交易类型） 
            /// </summary>
            TAOHUA,

            /// <summary>
            /// (外卖交易类型） 
            /// </summary>
            WAIMAI,

            /// <summary>
            /// （即时到帐/趣味猜交易类型） （无付款交易）
            /// </summary>
            NOPAID,

            /// <summary>
            /// (电子凭证) 
            /// </summary>
            ETICKET,

            /// <summary>
            /// （天猫国际）
            /// </summary>
            TMALL_I18N,

            /// <summary>
            /// （保险）
            /// </summary>
            INSURANCE_PLUS,

            /// <summary>
            /// （基金）
            /// </summary>
            FINANCE
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// 所有状态的订单
            /// </summary>
            [Description("所有状态的订单")]
            ALL_ORDERS,

            /// <summary>
            /// (没有创建支付宝交易)
            /// </summary>
            [Description("没有创建支付宝交易")]
            TRADE_NO_CREATE_PAY,

            /// <summary>
            /// 等待买家付款
            /// </summary>
            [Description("等待买家付款")]
            WAIT_BUYER_PAY,

            /// <summary>
            /// 等待卖家发货,即:买家已付款
            /// </summary>
            [Description("买家已付款")]
            WAIT_SELLER_SEND_GOODS,

            /// <summary>
            /// 卖家部分发货
            /// </summary>
            [Description("卖家部分发货")]
            SELLER_CONSIGNED_PART,

            /// <summary>
            /// (等待买家确认收货,即:卖家已发货)
            /// </summary>
            [Description("卖家已发货")]
            WAIT_BUYER_CONFIRM_GOODS,

            /// <summary>
            /// (买家已签收,货到付款专用) 
            /// </summary>
            [Description("买家已签收(货到付款专用)")]
            TRADE_BUYER_SIGNED,

            /// <summary>
            /// (交易成功)
            /// </summary>
            [Description("交易成功")]
            TRADE_FINISHED,

            /// <summary>
            /// (交易关闭)
            /// </summary>
            [Description("交易关闭")]
            TRADE_CLOSED,

            /// <summary>
            /// (交易被淘宝关闭)
            /// </summary>
            [Description("交易被淘宝关闭")]
            TRADE_CLOSED_BY_TAOBAO,

            /// <summary>
            /// (包含：WAIT_BUYER_PAY、TRADE_NO_CREATE_PAY) 
            /// </summary>
            ALL_WAIT_PAY,

            /// <summary>
            /// (包含：TRADE_CLOSED、TRADE_CLOSED_BY_TAOBAO) 
            /// </summary>
            ALL_CLOSED,

            /// <summary>
            /// (余额宝0元购合约中)
            /// </summary>
            WAIT_PRE_AUTH_CONFIRM

        }

        #endregion
    }
}
