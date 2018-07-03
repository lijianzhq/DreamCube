using System;
using System.Text;

namespace Mini.Foundation.Basic.Utility
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// 格式化异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static String FormatException(Exception ex)
        {
            var sb = new StringBuilder();
            Int32 i = 0;
            while (ex != null && i <= 100)
            {
                sb.Append(ex.Message);
                sb.Append(ex.StackTrace);
                sb.AppendLine();
                ex = ex.InnerException;
                i++;
            }
            return sb.ToString();
        }
    }
}
