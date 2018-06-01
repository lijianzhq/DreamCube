using System;
using System.Collections.Generic;

namespace Framework.Utilities.Pay
{
    /// <summary>
    /// 交易通知信息
    /// </summary>
    public class NotifyEventArgs
    {
        /// <summary>
        /// 不可空参数
        /// </summary>
        protected string _isSuccess;
        protected string _partnerId;
        protected string _signType;
        protected string _sign;
        protected string _charset;
        protected string _notifyId;
        protected string _notifyType;
        protected DateTime? _notifyTime;
        protected string _tradeNo;
        protected string _subject;
        protected decimal? _price;
        protected int? _quantity;
        protected decimal? _discount;
        protected decimal? _totalFee;
        protected string _sellerEmail;
        protected string _sellerId;
        protected string _buyerEmail;
        protected string _buyerId;
        protected string _tradeStatus;
        protected string _isTotalFeeAdjust;
        protected string _useCoupon;

        /// <summary>
        /// 可空参数
        /// </summary>
        protected string _outTradeNo;
        protected string _body;
        protected string _paymentType;
        protected string _logisticsType;
        protected decimal? _logisticsFee;
        protected string _logisticsPayment;
        protected string _receiveName;
        protected string _receiveAddress;
        protected string _receiveZip;
        protected string _receivePhone;
        protected string _receiveMobile;
        protected string _refundStatus;
        protected string _showUrl;
        protected string _buyerActions;
        protected string _sellerActions;
        protected DateTime? _gmtCreate;
        protected DateTime? _gmtPayment;
        protected DateTime? _gmtLogisticsModify;
        protected DateTime? _gmtRefund;

        public NotifyEventArgs() { }

        public NotifyEventArgs(string signType,string sign,string notifyId,string notifyType,DateTime? notifyTime,string tradeNo,string subject,decimal? price,int? quantity,decimal? discount,decimal? totalFee,string sellerEmail,string sellerId,string buyerEmail,string buyerId,string tradeStatus,string isTotalFeeAdjust,string useCoupon)
        {
            _signType = signType;
            _sign = sign;
            _notifyId = notifyId;
            _notifyType = notifyType;
            _notifyTime = notifyTime;
            _tradeNo = tradeNo;
            _subject = subject;
            _price = price;
            _quantity = quantity;
            _discount = discount;
            _totalFee = totalFee;
            _sellerEmail = sellerEmail;
            _sellerId = sellerId;
            _buyerEmail = buyerEmail;
            _buyerId = buyerId;
            _tradeStatus = tradeStatus;
            _isTotalFeeAdjust = isTotalFeeAdjust;
            _useCoupon = useCoupon;
        }

        public string Is_Success
        {
            get
            {
                if (_isSuccess == null)
                {
                    throw new ArgumentNullException(_isSuccess);
                }
                return _isSuccess;
            }
            set
            {
                if (value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException("Invalid value for Is_Success", value, value.ToString());
                _isSuccess = value;
            }
        }

        public string PartnerId
        {
            get
            {
                if (_partnerId == null)
                {
                    throw new ArgumentNullException(_partnerId);
                }
                return _partnerId;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("Invalid value for PartnerId", value, value.ToString());
                _partnerId = value;
            }
        }

        public string Sign_Type
        {
            get
            {
                if (_signType == null)
                {
                    throw new ArgumentNullException(_signType);
                }
                return _signType;
            }
            set
            {
                _signType = value;
            }
        }

        public string Sign
        {
            get
            {
                if (_sign == null)
                {
                    throw new ArgumentNullException(_sign);
                }
                return _sign;
            }
            set
            {
                _sign = value;
            }
        }

        public string Charset
        {
            get
            {
                if (_charset == null)
                {
                    throw new ArgumentNullException(_charset);
                }
                return _charset;
            }
            set
            {
                _charset = value;
            }
        }

        public string Notify_Id
        {
            get
            {
                if (_notifyId == null)
                {
                    throw new ArgumentNullException(_notifyId);
                }
                return _notifyId;
            }
            set
            {
                _notifyId = value;
            }
        }

        public string Notify_Type
        {
            get
            {
                if (_notifyType == null)
                {
                    throw new ArgumentNullException(_notifyType);
                }
                return _notifyType;
            }
            set
            {
                _notifyType = value;
            }
        }

        public DateTime? Notify_Time
        {
            get
            {
                if (_notifyTime == null)
                {
                    throw new ArgumentNullException(_notifyTime.ToString());
                }
                return _notifyTime;
            }
            set
            {
                _notifyTime = value;
            }
        }

        public string Trade_No
        {
            get
            {
                if (_tradeNo == null)
                {
                    throw new ArgumentNullException(_tradeNo);
                }
                return _tradeNo;
            }
            set
            {
                if(value!=null && value.Length>64)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Trade_No", value, value.ToString());
                }
                _tradeNo = value;
            }
        }

        public string Subject
        {
            get
            {
                if (_subject == null)
                {
                    throw new ArgumentNullException(_subject);
                }
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public decimal? Price
        {
            get
            {
                if (_price == null)
                {
                    throw new ArgumentNullException(_price.ToString());
                }
                return _price;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_price.ToString(), "必须为0.01在100000000.00之间");
                }
                _price = value;
            }
        }

        public int? Quantity
        {
            get
            {
                if (_quantity == null)
                {
                    throw new ArgumentNullException(_quantity.ToString());
                }
                return _quantity;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "不可小于0！");
                }
                _quantity = value;
            }
        }

        public decimal? Discount
        {
            get
            {
                if (_discount == null)
                {
                    throw new ArgumentNullException(_discount.ToString());
                }
                return _discount;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_discount.ToString(), "必须为0.01在100000000.00之间");
                }
                _discount = value;
            }
        }

        public decimal? Total_Fee
        {
            get
            {
                if (_totalFee == null)
                {
                    throw new ArgumentNullException(_totalFee.ToString());
                }
                return _totalFee;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_totalFee.ToString(), "必须为0.01在100000000.00之间");
                }
                _totalFee = value;
            }
        }

        public string Seller_Email
        {
            get
            {
                if (_sellerEmail == null)
                {
                    throw new ArgumentNullException(_sellerEmail);
                }
                return _sellerEmail;
            }
            set
            {
                _sellerEmail = value;
            }
        }

        public string Seller_Id
        {
            get
            {
                if (_sellerId == null)
                {
                    throw new ArgumentNullException(_sellerId);
                }
                return _sellerId;
            }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Seller_Id", value, value.ToString());
                }
                _sellerId = value;
            }
        }

        public string Buyer_Email
        {
            get
            {
                if (_buyerEmail == null)
                {
                    throw new ArgumentNullException(_buyerEmail);
                }
                return _buyerEmail;
            }
            set
            {
                _buyerEmail = value;
            }
        }

        public string Buyer_Id
        {
            get
            {
                if (_buyerId == null)
                {
                    throw new ArgumentNullException(_buyerId);
                }
                return _buyerId;
            }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Buyer_Id", value, value.ToString());
                }
                _buyerId = value;
            }
        }

        public string Trade_Status
        {
            get
            {
                if (_tradeStatus == null)
                {
                    throw new ArgumentNullException(_tradeStatus);
                }
                return _tradeStatus;
            }
            set
            {
                _tradeStatus = value;
            }
        }

        public string Is_Total_Fee_Adjust
        {
            get
            {
                if (_isTotalFeeAdjust == null)
                {
                    throw new ArgumentNullException(_isTotalFeeAdjust);
                }
                return _isTotalFeeAdjust;
            }
            set
            {
                _isTotalFeeAdjust = value;
            }
        }

        public string Use_Coupon
        {
            get
            {
                if (_useCoupon == null)
                {
                    throw new ArgumentNullException(_useCoupon);
                }
                return _useCoupon;
            }
            set
            {
                _useCoupon = value;
            }
        }

        //可空
        public string Out_Trade_No
        {
            get
            {
                return _outTradeNo;
            }
            set
            {
                _outTradeNo = value;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        public string Payment_Type
        {
            get
            {
                return _paymentType;
            }
            set
            {
                _paymentType = value;
            }
        }

        public string Logistics_Type
        {
            get
            {
                return _logisticsType;
            }
            set
            {
                _logisticsType = value;
            }
        }

        public decimal? Logistics_Fee
        {
            get
            {
                return _logisticsFee;
            }
            set
            {
                _logisticsFee = value;
            }
        }

        public string Logistics_Payment
        {
            get
            {
                return _logisticsPayment;
            }
            set
            {
                _logisticsPayment = value;
            }
        }

        public string Receive_Name
        {
            get
            {
                return _receiveName;
            }
            set
            {
                _receiveName = value;
            }
        }

        public string Receive_Address
        {
            get
            {
                return _receiveAddress;
            }
            set
            {
                _receiveAddress = value;
            }
        }

        public string Receive_Zip
        {
            get
            {
                return _receiveZip;
            }
            set
            {
                _receiveZip = value;
            }
        }

        public string Receive_Phone
        {
            get
            {
                return _receivePhone;
            }
            set
            {
                _receivePhone = value;
            }
        }

        public string Receive_Mobile
        {
            get
            {
                return _receiveMobile;
            }
            set
            {
                _receiveMobile = value;
            }
        }

        public string Refund_Status
        {
            get
            {
                return _refundStatus;
            }
            set
            {
                _refundStatus = value;
            }
        }

        public string Show_Url
        {
            get
            {
                return _showUrl;
            }
            set
            {
                _showUrl = value;
            }
        }

        public string Buyer_Actions
        {
            get
            {
                return _buyerActions;
            }
            set
            {
                _buyerActions = value;
            }
        }

        public string Seller_Actions
        {
            get
            {
                return _sellerActions;
            }
            set
            {
                _sellerActions = value;
            }
        }

        //public string Seller_Actions
        //{
        //    get
        //    {
        //        return _sellerActions;
        //    }
        //    set
        //    {
        //        _sellerActions = value;
        //    }
        //}

        public DateTime? Gmt_Create
        {
            get
            {
                return _gmtCreate;
            }
            set
            {
                _gmtCreate = value;
            }
        }

        public DateTime? Gmt_Payment
        {
            get
            {
                return _gmtPayment;
            }
            set
            {
                _gmtPayment = value;
            }
        }

        public DateTime? Gmt_Logistics_Modify
        {
            get
            {
                return _gmtLogisticsModify;
            }
            set
            {
                _gmtLogisticsModify = value;
            }
        }

        public DateTime? Gmt_Refund
        {
            get
            {
                return _gmtRefund;
            }
            set
            {
                _gmtRefund = value;
            }
        }

    }
}
