using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Utilities.Pay.Enums
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum TradeStatusType
    {
        //等待买家付款
        WAIT_BUYER_PAY,
        //买家已付款，等待卖家发货
        WAIT_SELLER_SEND_GOODS,
        //卖家已发货，等待买家确认
        WAIT_BUYER_CONFIRM_GOODS,
        //交易成功结束
        TRADE_FINISHED,
        //交易中途关闭（已结束，未成功完成）
        TRADE_CLOSED 
    }
}
