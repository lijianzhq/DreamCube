using System;
using System.Collections.Generic;

namespace Framework.Utilities.Pay
{
    /*
     * 标准双接口，商品类（包含虚拟商品）
     */
    public class StandardGoods
    {
        #region 不可空参数
        //接口名称
        protected string _service;
        //合作身份者ID
        protected string _parntner;
        //参数编码字符集
        protected string _inputCharset;
        //签名方式
        protected string _signType;
        //密钥
        protected string _sign;
        //商户网站唯一订单号
        protected string _outTradeNo;
        //商品名称
        protected string _subject;
        //收款类型
        protected string _paymentType;
        //物流类型
        protected string _logisticsType;
        //物流费用
        protected decimal _logisticsFee;
        //物流支付类型
        protected string _logisticsPayment;
        //卖家支付宝账号
        protected string _sellerEmail;
        //商品单价
        protected decimal _price;
        //商品数量
        protected int _quantity;

        #endregion

        #region  可空参数

        //服务器异步通知页面路径
        protected string _notifyUrl;
        //商品描述
        protected string _body;
        //折扣
        protected decimal _discount;
        //交易金额
        protected decimal _totalFee;
        //商品展示URL
        protected string _showUrl;
        //卖家支付宝账号对应的支付宝唯一用户号
        protected string _sellerId;
        //买家支付宝账号
        protected string _buyerEmail;
        //买家支付宝账号对应的支付宝唯一用户号
        protected string _buyerId;
        //卖家别名支付宝账号
        protected string _sellerAccountName;
        //买家别名支付宝账号
        protected string _buyerAccountName;
        //收货人姓名
        protected string _receiveName;
        //收货人地址
        protected string _receiveAddress;
        //收货人邮编
        protected string _receiveZip;
        //收货人电话
        protected string _receivePhone;
        //收货人手机
        protected string _receiveMobile;
        //物流类型1
        protected string _logisticsType1;
        //物流运费1
        protected decimal _logisticsFee1;
        //物流支付类型1
        protected string _logisticsPayment1;
        //物流类型2
        protected string _logisticsType12;
        //物流运费2
        protected decimal _logisticsFee2;
        //物流支付类型2
        protected string _logisticsPayment2;
        //买家逾期不付款，自动关闭交易
        protected string _itBPay;
        //卖家逾期不发货，允许买家退款
        protected string _tSSend1;
        //卖家逾期不发货，建议买家退款
        protected string _tSSend2;
        //买家逾期不确认收货，自动完成交易（平邮）
        protected string _tBRecPost;
        //防钓鱼时间戳
        protected string _antiPhishingKey;
        //快捷登录授权令牌
        protected string _token;

        #endregion

        public StandardGoods() { }

        #region 构造函数-不可空参数
        /// <summary>
        /// 构造函数-不可空参数
        /// </summary>
        /// <param name="sevice"></param>
        /// <param name="parntner"></param>
        /// <param name="inputCharset"></param>
        /// <param name="signType"></param>
        /// <param name="sign"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="subject"></param>
        /// <param name="paymentType"></param>
        /// <param name="logisticsType"></param>
        /// <param name="logisticsFee"></param>
        /// <param name="logisticsPayment"></param>
        /// <param name="sellerEmail"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        public StandardGoods(string sevice, string parntner, string inputCharset, string signType, string sign, string outTradeNo, string subject, string paymentType, string logisticsType, decimal logisticsFee, string logisticsPayment, string sellerEmail, decimal price, int quantity)
        {
            this._service = sevice;
            this._parntner = parntner;
            this._inputCharset = inputCharset;
            this._signType = signType;
            this._sign = sign;
            this._outTradeNo = outTradeNo;
            this._subject = subject;
            this._paymentType = paymentType;
            this._logisticsType = logisticsType;
            this._logisticsFee = logisticsFee;
            this._logisticsPayment = logisticsPayment;
            this._sellerEmail = sellerEmail;
            this._price = price;
            this._quantity = quantity;
        }

        #endregion

        /// <summary>
        /// create_digital_goods_trade_p
        /// </summary>
        public string Service
        {
            get
            {
                if (_service == null)
                {
                    throw new ArgumentNullException(_service);
                }
                return _service;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Service", value, value.ToString());
                _service = value;
            }
        }

        /// <summary>
        /// 合作身份者ID:签约的支付宝账号对应的支付宝唯一用户号。以2088开头的16位纯数字组成
        /// </summary>
        public string Parntner
        {
            get
            {
                if (_parntner == null)
                {
                    throw new ArgumentNullException(_parntner);
                }
                return _parntner;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("Invalid value for Service", value, value.ToString());
                _parntner = value;
            }
        }

        /// <summary>
        /// 参数编码字符集:商户网站使用的编码格式，如utf-8、gbk、gb2312等。默认utf-8
        /// </summary>
        public string Input_Charset
        {
            get
            {
                if (_inputCharset == null)
                    return "utf-8";
                return _inputCharset;
            }
            set
            {
                if (value != null && value.Length > 10)
                    throw new ArgumentOutOfRangeException("Invalid value for Input_Charset", value, value.ToString());
                _inputCharset = value;
            }
        }

        /// <summary>
        /// sign_type 不可空
        /// </summary>
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
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Sign_Type", value, value.ToString());
                }
                _signType = value;
            }
        }

        /// <summary>
        /// sign密钥，不可空
        /// </summary>
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
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Sign", value, value.ToString());
                }
                _sign = value;
            }
        }

        /// <summary>
        /// notify_url 可空
        /// </summary>
        public string Notify_Url
        {
            get
            {
                if (_notifyUrl == null)
                {
                    _notifyUrl ="";
                }
                return _sign;
            }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Notify_Url", value, value.ToString());
                }
                _sign = value;
            }
        }

        /// <summary>
        /// out_trade_no支付宝合作商户网站唯一订单号（确保在合作伙伴系统中唯一）。 不可空
        /// </summary>
        public string Out_Trade_No
        {
            get
            {
                if (_outTradeNo == null)
                {
                    throw new ArgumentNullException(_outTradeNo);
                }
                return _sign;
            }
            set
            {
                if (value != null && value.Length > 64)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Out_Trade_No", value, value.ToString());
                }
                _sign = value;
            }
        }

        /// <summary>
        /// subject商品的标题/交易标题/订单标题/订单关键字等。该参数最长为128个汉字。 不可空
        /// </summary>
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
                if (value != null && value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
                }
                _subject = value;
            }
        }

        /// <summary>
        /// payment_type收款类型，只支持1：商品购买。 不可空
        /// </summary>
        public string Payment_type
        {
            get
            {
                return "1";
            }
            set
            {
                if (value != null && value.ToString() == "1")
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Payment_type", value, value.ToString());
                }
                _paymentType = value;
            }
        }

        /// <summary>
        /// logistics_type第一组物流类型。取值范围请参见附录“11.3 物流类型”。 不可空
        /// </summary>
        public string Logistics_Type
        {
            get
            {
                if (_logisticsType == null)
                {
                    return "POST";
                }
                return _logisticsType;
            }
            set
            {
                if (value != null && value.Length > 10)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Logistics_Type", value, value.ToString());
                }
                _logisticsType = value;
            }
        }

        /// <summary>
        /// logistics_fee第一组物流运费。单位为：RMB Yuan。精确到小数点后两位。缺省值为0元。 不可空
        /// </summary>
        public decimal Logistics_fee
        {
            get
            {
                if (_logisticsFee == null)
                {
                    return 0;
                }
                return decimal.Round(_logisticsFee, 2); 
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Logistics_fee", "必须为0.01在100000000.00之间");
                }
                _logisticsFee = value;
            }
        }

        /// <summary>
        /// logistics_payment第一组物流支付类型。取值范围请参见附录“11.4 物流支付类型”。 不可空,默认为“SELLER_PAY”
        /// </summary>
        public string Logistics_payment
        {
            get
            {
                if (_logisticsPayment == null)
                {
                    return "SELLER_PAY";
                }
                return _logisticsPayment;
            }
            set
            {
                if (value !=null && value.Length>50)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Logistics_payment", value, value.ToString());
                }
                _logisticsPayment = value;
            }
        }

        /// <summary>
        /// seller_email登录时，seller_email和seller_id两者必填一个。 不可空，
        /// </summary>
        public string Seller_Email
        {
            get
            {
                if (_sellerEmail == null)
                {
                    return "908619101@QQ.COM";//暂定
                }
                return _sellerEmail;
            }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Seller_Email", value, value.ToString());
                }
                _sellerEmail = value;
            }
        }

        /// <summary>
        /// price商品单价，单位为：RMB Yuan。取值范围为[0.01,1000000.00]，精确到小数点后两位。 不可空，
        /// </summary>
        public decimal Price
        {
            get
            {
                if (_price == null || _price==0)
                {
                    return 0.01m;//暂定
                }
                return decimal.Round(_price, 2); 
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Price", "必须为0.01在100000000.00之间");
                }
                _price = value;
            }
        }

        /// <summary>
        /// quantity商品的数量。 不可空，
        /// </summary>
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0 || value > 1000000)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "必须为小于1000000的正整数");
                } 
                _quantity = value;
            }
        }


        //可空参数
        /// <summary>
        /// body对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。，
        /// </summary>
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                if (value !=null && value.Length>400)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
                }
                _body = value;
            }
        }

        /// <summary>
        /// discount折扣：单位为：RMB Yuan。取值范围为[-100000000.00,100000000.00]，精确到小数点后两位。
        /// </summary>
        public decimal Discount
        {
            get
            {
                if (_discount == null)
                    return 0.00m;
                return _discount;
            }
            set
            {
                if (value < -100000000.00m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_discount.ToString(), "必须为[-100000000.00,100000000.00]");
                }
                _discount = value;
            }
        }

        /// <summary>
        /// total_fee交易金额大于0元。担保交易单笔交易金额不能超过100万，精确到小数点后两位。即时到账无金额上限。
        /// </summary>
        public decimal Total_Fee
        {
            get
            {
                if (_totalFee == null)
                    return 0.01m;
                return _totalFee;
            }
            set
            {
                if (value < 0|| value > 100000000)
                {
                    throw new ArgumentNullException(_totalFee.ToString(), "必须为[0,100000000.00]");
                }
                _totalFee = value;
            }
        }

        /// <summary>
        /// total_fee交易金额大于0元。担保交易单笔交易金额不能超过100万，精确到小数点后两位。即时到账无金额上限。
        /// </summary>
        public string Show_Url
        {
            get
            {
                if (_showUrl == null)
                    _showUrl = "";
                return _showUrl;
            }
            set
            {
                if (value != null && value.Length > 400)
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Show_Url", value, value.ToString());
                }
                _showUrl = value;
            }
        }
    }
}
