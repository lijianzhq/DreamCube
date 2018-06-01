using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Utilities.Pay.Enums
{
    /// <summary>
    /// 退货状态
    /// </summary>
    public enum RefundStatusType
    {
        //退款协议等待卖家确认中
        WAIT_SELLER_AGREE,
        //卖家不同意协议，等待买家修改
        SELLER_REFUSE_BUYER,
        //退款协议达成，等待买家退货
        WAIT_BUYER_RETURN_GOODS,
        //等待卖家收货
        WAIT_SELLER_CONFIRM_GOODS,
        //退款成功
        REFUND_SUCCESS,
        //退款关闭
        REFUND_CLOSED
    }
}
