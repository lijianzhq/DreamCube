using System;
using System.Web;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyEnvironment
    {
        /// <summary>
        /// web中的textarea的换行符号
        /// </summary>
        /// <returns></returns>
        public static String NewLine_WebTextBox
        {
            get
            {
                return "\n";
            }
        }

        /// <summary>
        /// 判断当前执行环境是否在web环境
        /// </summary>
        /// <returns></returns>
        public static Boolean IsInWeb()
        {
            try
            {
                return HttpContext.Current != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 当前环境的新行
        /// </summary>
        public static String CurrentEnvironmentNewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }
    }
}
