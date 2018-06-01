using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using Framework.Utilities.Pay.Enums;
using Framework.Utilities.Pay.Result;
using System.Web.UI;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Framework.Utilities.Pay
{
    /// <summary>
    /// 支付宝接口
    /// 服务名称：trade_create_by_buyer标准双接口
    /// 版本号：1.6
    /// 功能描述：标准双接口提供的功能为：用户可以选择担保交易或者即时到账交易两种付款方式之一来完成付款
    /// 交易类型：担保交易、即时到账交易
    /// 交易流程：
    /// 1、合作商户系统：构造请求数据
    /// 2、合作商户系统：发送请求数据
    /// 3、支付宝系统：请求的交易
    /// 4、支付宝系统：返回响应数据
    /// 5、合作商户系统：对响应的数据进行处理
    /// 
    /// 如何判断该笔交易是通过即时到帐方式付款还是通过担保交易方式付款?
    ///  担保交易的交易状态变化顺序是：等待买家付款→买家已付款，等待卖家发货→卖家已发货，等待买家收货→买家已收货，交易完成
    ///  即时到帐的交易状态变化顺序是：等待买家付款→交易完成
    ///  每当收到支付宝发来通知中，就可以获取到这笔交易的交易状态，并且商户需要利用商户订单号查询商户网站的订单数据，
    ///  得到这笔订单在商户网站中的状态是什么，把商户网站中的订单状态与从支付宝通知中获取到的状态来做对比。
    ///  如果商户网站中目前的状态是等待买家付款，而从支付宝通知获取来的状态是买家已付款，等待卖家发货，那么这笔交易买家是用担保交易方式付款的
    ///  如果商户网站中目前的状态是等待买家付款，而从支付宝通知获取来的状态是交易完成，那么这笔交易买家是用即时到帐方式付款的
    /// </summary>
    public class AliPay
    {
        public delegate void ProcessNotifyEventHandler(object sender, NotifyEventArgs e);

        #region 一般交易处理

        /// <summary>
        /// 等待买家付款
        /// </summary>
        public event ProcessNotifyEventHandler WaitBuyerPay;

        /// <summary>
        /// 买家已付款，等待卖家发货
        /// </summary>
        public event ProcessNotifyEventHandler WaitSellerSendGoods;

        /// <summary>
        /// 卖家已发货，等待买家确认
        /// </summary>
        public event ProcessNotifyEventHandler WaitBuyerConfirmGoods;

        /// <summary>
        /// 交易成功结束
        /// </summary>
        public event ProcessNotifyEventHandler TradeFinished;

        /// <summary>
        /// 交易中途关闭（未完成）
        /// </summary>
        public event ProcessNotifyEventHandler TradeClosed;

        #endregion

        #region 退货处理

        /// <summary>
        /// 退款协议等待卖家确认中
        /// </summary>
        public event ProcessNotifyEventHandler WaitSellerAgree;

        /// <summary>
        /// 卖家不同意协议，等待买家修改
        /// </summary>
        public event ProcessNotifyEventHandler SellerRefuseBuyer;

        /// <summary>
        /// 退款协议达成，等待买家退货
        /// </summary>
        public event ProcessNotifyEventHandler WaitBuyerReturnGoods;

        /// <summary>
        /// 等待卖家收货
        /// </summary>
        public event ProcessNotifyEventHandler WaitSellerConfirmGoods;

        /// <summary>
        /// 退款成功
        /// </summary>
        public event ProcessNotifyEventHandler RefundSuccess;

        /// <summary>
        /// 退款关闭
        /// </summary>
        public event ProcessNotifyEventHandler RefundClosed;


        #endregion

        #region 一般交易处理
        public virtual void OnWaitBuyerPay(NotifyEventArgs e)
        {
            if (WaitBuyerPay != null)
                WaitBuyerPay(this, e);
        }

        public virtual void OnWaitSellerSendGoods(NotifyEventArgs e)
        {
            if (WaitSellerSendGoods != null)
                WaitSellerSendGoods(this, e);
        }

        public virtual void OnWaitBuyerConfirmGoods(NotifyEventArgs e)
        {
            if (WaitBuyerConfirmGoods != null)
                WaitBuyerConfirmGoods(this, e);
        }

        public virtual void OnTradeFinished(NotifyEventArgs e)
        {
            if (TradeFinished != null)
                TradeFinished(this, e);
        }

        public virtual void OnTradeClosed(NotifyEventArgs e)
        {
            if (TradeClosed != null)
                TradeClosed(this, e);
        }

        #endregion

        #region 退货处理

        public virtual void OnWaitSellerAgree(NotifyEventArgs e)
        {
            if (WaitSellerAgree != null)
                WaitSellerAgree(this, e);
        }

        public virtual void OnSellerRefuseBuyer(NotifyEventArgs e)
        {
            if (SellerRefuseBuyer != null)
                SellerRefuseBuyer(this, e);
        }

        public virtual void OnWaitBuyerReturnGoods(NotifyEventArgs e)
        {
            if (WaitBuyerReturnGoods != null)
                WaitBuyerReturnGoods(this, e);
        }

        public virtual void OnWaitSellerConfirmGoods(NotifyEventArgs e)
        {
            if (WaitSellerConfirmGoods != null)
                WaitSellerConfirmGoods(this, e);
        }

        public virtual void OnRefundSuccess(NotifyEventArgs e)
        {
            if (RefundSuccess != null)
                RefundSuccess(this, e);
        }

        public virtual void OnRefundClosed(NotifyEventArgs e)
        {
            if (RefundClosed != null)
                RefundClosed(this, e);
        }

        #endregion

        /// <summary>
        /// 处理交易类型
        /// </summary>
        /// <param name="tradeStatusType"></param>
        /// <returns></returns>
        public static TradeStatusType FormatTradeStatusType(string tradeStatusType)
        {
            foreach (TradeStatusType tsType in Enum.GetValues(typeof(TradeStatusType)))
            {
                if (tsType.ToString().ToUpper() == tradeStatusType.ToUpper() || tsType.ToString("d") == tradeStatusType)
                    return tsType;
                
            }
            throw new Exception("发生异常！尚未为此类型定义枚举：" + tradeStatusType);
        }

        /// <summary>
        /// 处理退货类型
        /// </summary>
        /// <param name="refundStatusType"></param>
        /// <returns></returns>
        public static RefundStatusType FormatRefundStatusType(string refundStatusType)
        {
            foreach (RefundStatusType rfType in Enum.GetValues(typeof(RefundStatusType)))
            {
                if (rfType.ToString().ToUpper() == refundStatusType.ToUpper() || rfType.ToString("d") == refundStatusType)
                    return rfType;

            }
            throw new Exception("发生异常！尚未为此类型定义枚举：" + refundStatusType);
        }

        /// <summary>
        /// 创建虚拟交易
        /// </summary>
        /// <param name="gatewayUrl"></param>
        /// <param name="standardGoods"></param>
        /// <param name="page"></param>
        public void CreateDigitalTrade(string gatewayUrl, StandardGoods digitalGoods, Page page)
        {
        }

        /// <summary>
        /// 创建标准交易
        /// </summary>
        /// <param name="gatewayUrl">提交支付宝的地址</param>
        /// <param name="standardGoods">交易参数</param>
        /// <param name="page">Page对象</param>
        public void CreateStandardTrade(string gatewayUrl, StandardGoods standardGoods, Page page)
        {
            HttpResponse Respose = page.Response;
            Respose.Redirect(gatewayUrl + "?" + Md5SignParam(standardGoods));
        }

        /// <summary>
        /// 处理返回的Notify
        /// </summary>
        /// <param name="page">传如Page对象</param>
        /// <param name="verifyUrl">验证的地址，如：https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">账户的交易安全校验码（key）</param>
        /// <param name="verify">verify对象</param>
        /// <param name="encode">编码</param>
        /// <exception cref="自定义异常">CommonAliPayBaseException</exception>
        public void ProcessNotify(Page page, string verifyUrl, string key, Verify verify, string encode)
        {
            if (VerifyNotify(verifyUrl, verify))
            {
                NotifyEventArgs nEA = new NotifyEventArgs();
                nEA = ParseNotify(page.Request.Form, nEA);
                SortedList<string, string> sortedList = GetParam(nEA);
                string param = GetUrlParam(sortedList, false);
                string sign = GetMd5Sign(encode, param + key);
                if (sign == nEA.Sign)
                {
                    try
                    {
                        ///交易处理
                        switch (nEA.Trade_Status.ToUpper())
                        {
                            case "WAIT_BUYER_PAY": OnWaitBuyerPay(nEA); break;
                            case "WAIT_SELLER_SEND_GOODS": OnWaitSellerSendGoods(nEA); break;
                            case "WAIT_BUYER_CONFIRM_GOODS": OnWaitBuyerConfirmGoods(nEA); break;
                            case "TradeFinished": OnWaitBuyerPay(nEA); break;
                            default: break;
                        }

                        ///退货处理
                        if (nEA.Refund_Status != null && nEA.Refund_Status.Length > 0)
                        {
                            switch (nEA.Refund_Status.ToUpper())
                            {
                                case "WAIT_SELLER_AGREE": OnWaitSellerAgree(nEA); break;
                                case "SELLER_REFUSE_BUYER": OnSellerRefuseBuyer(nEA); break;
                                case "WAIT_BUYER_RETURN_GOODS": OnWaitBuyerReturnGoods(nEA); break;
                                case "WAIT_SELLER_CONFIRM_GOODS": OnWaitSellerConfirmGoods(nEA); break;
                                case "REFUND_SUCCESS": OnRefundSuccess(nEA); break;
                                case "REFUND_CLOSED": OnRefundClosed(nEA); break;
                                default:
                                    break;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        page.Response.Write("false");
                        throw e;
                    }
                }
                else
                {
                    page.Response.Write("fail");
                    throw new AliPayBaseException("支付宝通知签名验证失败", 101);
                }
            }
            else
            {
                page.Response.Write("fail");
                throw new AliPayBaseException("支付宝通知验证失败", 101);
            }
        }

        /// <summary>
        /// 通知验证接口
        /// </summary>
        /// <param name="verifyUrl"></param>
        /// <param name="verify">验证参数</param>
        /// <returns>true，通过验证</returns>
        private bool VerifyNotify(string verifyUrl, Verify verify)
        {
            string param;
            SortedList<string, string> sl = GetParam(verify);
            param = GetUrlParam(sl, true);
            string result = PostData(verifyUrl, param, "utf-8");
            return bool.Parse(result);
        }

        /// <summary>
        ///  解析Form集合到DigitalNotifyEventArgs,值类型会被初始化为null
        /// </summary>
        /// <param name="nv"></param>
        /// <param name="obj"></param>
         /// <remarks>为防止值类型的默认值污染参数集合,用了nullable范型</remarks>
        private NotifyEventArgs ParseNotify(NameValueCollection nv, object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo  pi in propertyInfos)
            {
                string v = nv.Get(pi.Name.ToLower());
                if (v != null)
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(obj, v, null);
                    }
                    else if (pi.PropertyType == typeof(int?)) {
                        pi.SetValue(obj, int.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(decimal?))
                    {
                        pi.SetValue(obj, decimal.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        pi.SetValue(obj, DateTime.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(bool))
                    {
                        pi.SetValue(obj, bool.Parse(v), null);
                    }
                    else {
                        pi.SetValue(obj, v, null);
                    }
                }
            }

            return (NotifyEventArgs)obj;
        }

        /// <summary>
        /// 获取Md5 sign后的参数
        /// </summary>
        /// <param name="standardGoods"></param>
        /// <returns></returns>
        private string Md5SignParam(StandardGoods standardGoods)
        {
            if (standardGoods.Sign_Type.ToLower() != "md5")
            {
                throw new AliPayBaseException("Sign_Type 不支持MD5", 100);
            }

            SortedList<string, string> goods = GetParam(standardGoods);
            string param = GetUrlParam(goods,false)+standardGoods.Sign;
            string encodeParam = GetUrlParam(goods, true) + "&";
            string sign = GetMd5Sign(standardGoods.Input_Charset, param);
            return encodeParam + string.Format("sign={0}&sign_type{1}", HttpUtility.HtmlEncode(sign), HttpUtility.HtmlEncode(standardGoods.Sign_Type));
        }

        /// <summary>
        /// 获取字符串的MD5签名
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetMd5Sign(string encode, string param)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.GetEncoding(encode).GetBytes(param));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

       /// <summary>
       /// 获取排序后的参数
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        private SortedList<string, string> GetParam(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            SortedList<string, string> sortedList = new SortedList<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.GetValue(obj, null) != null)
                {
                    if (pi.Name == "Sign" || pi.Name == "Sign_Type")
                    {
                        continue;
                    }
                    sortedList.Add(pi.Name.ToLower(),pi.GetValue(obj,null).ToString());
                }
            }
            return sortedList;
        }

        /// <summary>
        /// 获取Url的参数
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="isEncode">参数是否经过编码，被签名的参数不用编码，post的参数要编码</param>
        /// <returns></returns>
        private string GetUrlParam(SortedList<string, string> sortedList, bool isEncode)
        {
            StringBuilder param = new StringBuilder();
            StringBuilder encodeParam = new StringBuilder();
            if (isEncode == false)
            {
                foreach (KeyValuePair<string,string> kvp in sortedList)
                {
                    string t = string.Format("{0}={1}", kvp.Key, kvp.Value);
                    param.Append(t + "&");
                }
                return param.ToString().TrimEnd('&');
            }
            else
            {
                foreach (KeyValuePair<string,string> kvp in sortedList)
                {
                    string et = string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value));
                    encodeParam.Append(et + "&");
                }
                return encodeParam.ToString().TrimEnd('&');
            }
        }
        /// <summary>
        /// 提交post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private string PostData(string url, string data, string encode)
        {
            CookieContainer cc = new CookieContainer();
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = cc;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream requestStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();
            HttpWebResponse respose = request.GetResponse() as HttpWebResponse;
            Uri responseUri = respose.ResponseUri;
            Stream receiveStream = respose.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding(encode));
            string result = readStream.ReadToEnd();
            return result;
        }
    }
}
