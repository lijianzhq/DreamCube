using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Utilities.Pay.Result
{
    public class AliPayBaseException : ApplicationException
    {
       public AliPayBaseException(string message, int errCode)
            : base(message)
        {
            this.errCode = errCode;
        }
        public AliPayBaseException()
        { }
        public AliPayBaseException(int errCode)
        {
            this.errCode = errCode;
        }
        private int errCode;
        public int ErrCode
        {
            get { return errCode; }
            set { this.errCode = value; }
        }
    }
}
