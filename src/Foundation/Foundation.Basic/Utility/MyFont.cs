using System;
using System.Drawing;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyFont
    {
        /// <summary>
        /// 判断字符串是否合法的字体
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static Boolean IsLegalFontFamilyName(String target)
#else
        public static Boolean IsLegalFontFamilyName(this String target)
#endif
        {
            Font tempFont = null;
            try
            {
                tempFont = new Font(target, 9f);
                return true;
            }
            catch (Exception)
            { return false; }
            finally
            { tempFont.Dispose(); }
        }
    }
}
