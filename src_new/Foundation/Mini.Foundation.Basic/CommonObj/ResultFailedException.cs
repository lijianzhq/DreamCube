using System;

namespace Mini.Foundation.Basic.CommonObj
{
    public class ResultFailedException : Exception
    {
        public ResultFailedException() : base()
        {
        }

        public ResultFailedException(string message) : base(message)
        {
        }
    }
}
