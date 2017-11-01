using System;
using Microsoft.VisualBasic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyNumber
    {
        /// <summary>
        /// 将所给的整数iNumber转换成字符串, 并且保证字符串长度为iBitCount
        /// </summary>
        /// <param name="iNumber"></param>
        /// <param name="iBitCount"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String IntToString(Int32 iNumber, int iBitCount)
        {
            string sNumber = iNumber.ToString();
            if (sNumber.Length < iBitCount)
            {
                for (int i = 1; i <= iBitCount - sNumber.Length; i++)
                {
                    sNumber = "0" + sNumber;
                }
                return sNumber;
            }
            return sNumber.Substring(0, iBitCount);
        }

        /// <summary>
        /// 四舍五入格式化数字，与Round的区别：本函数返回字符串，并且不论iNumber是否是整数还是小数，均保持N为小数点
        /// 如果小数点位数为 0 则返回四舍五入的整数
        /// </summary>
        /// <param name="iNumber"></param>
        /// <param name="iDecimalsCount"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Round(double iNumber, int iDecimalsCount)
        {
            iNumber = Math.Round(iNumber, iDecimalsCount);
            String sNumber = iNumber.ToString();

            //如果小数点位数为 0 则返回四舍五入的整数
            if (iDecimalsCount <= 0)
            {
                return sNumber;
            }

            if (sNumber.IndexOf(".") == -1)
            {
                sNumber += ".";
                for (int i = 1; i <= iDecimalsCount; i++)
                {
                    sNumber += "0";
                }
            }
            else
            {
                string sXiaoShu = MyString.Right(sNumber, ".");
                if (sXiaoShu.Length < iDecimalsCount)
                {
                    sNumber = MyString.Left(sNumber, ".") + ".";
                    int iX = iDecimalsCount - sXiaoShu.Length;
                    for (int i = 1; i <= iX; i++)
                    {
                        sXiaoShu += "0";
                    }
                    sNumber += sXiaoShu;
                }
            }
            return sNumber;
        }

        /// <summary>
        /// 四舍五入格式化数字，与Round的区别：本函数返回字符串，并且不论iNumber是否是整数还是小数，均保持N为小数点
        /// 如果小数点位数为 0 则返回四舍五入的整数
        /// </summary>
        /// <param name="iNumber"></param>
        /// <param name="iDecimalsCount"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Double Round2(double iNumber, int iDecimalsCount)
        {
            iNumber = Math.Round(iNumber, iDecimalsCount);
            String sNumber = iNumber.ToString();

            //如果小数点位数为 0 则返回四舍五入的整数
            if (iDecimalsCount <= 0) return Convert.ToDouble(sNumber);

            if (sNumber.IndexOf(".") == -1)
            {
                sNumber += ".";
                for (int i = 1; i <= iDecimalsCount; i++)
                {
                    sNumber += "0";
                }
            }
            else
            {
                string sXiaoShu = MyString.Right(sNumber, ".");
                if (sXiaoShu.Length < iDecimalsCount)
                {
                    sNumber = MyString.Left(sNumber, ".") + ".";
                    int iX = iDecimalsCount - sXiaoShu.Length;
                    for (int i = 1; i <= iX; i++)
                    {
                        sXiaoShu += "0";
                    }
                    sNumber += sXiaoShu;
                }
            }
            return Double.Parse(sNumber);
        }

        /// <summary>
        /// Math.Truncate()这个方法是获取浮点数据的整数部分，而此方法是获取小数部分的
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Double TruncateEx(Double d)
        {
            String input = d.ToString();
            return Double.Parse(String.Format("0.{0}", MyString.Right(input, ".")));
        }

        /// <summary>
        /// double数字相加减之后，去掉精度差算法
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static Double AddDouble(Double num1, Double num2)
        {
            Double Sum = 0;
            Int32 a1 = 0;
            Int32 a2 = 0;
            Double m = 0;

            String temp1 = num1.ToString();
            if (temp1.IndexOf(".") >= 0)
            {
                a1 = temp1.Split(new char[] { '.' })[1].Length;
            }
            else
            {
                a1 = 0;
            }
            String temp2 = num2.ToString();
            if (temp2.IndexOf(".") >= 0)
            {
                a2 = temp2.Split(new char[] { '.' })[1].Length;
            }
            else
            {
                a2 = 0;
            }
            m = Math.Pow(10, Math.Max(a1, a2));
            Sum = (num1 * m + num2 * m) / m;
            return Sum;
        }

        /// <summary>
        /// 四舍五入格式化数字，与Round的区别：
        /// 此方法返回字符串类型（必须是返回字符串类型，如果的double类型的话，则会自动的去掉小数为0的部分的）
        /// 不论指定的数字是否是整数还是小数或者小数部分是0，均保持N为小数点
        /// </summary>
        /// <param name="d">数字</param>
        /// <param name="decimals">小数位数，0表示不保留小数</param>
        /// <param name="integer">整数部分
        /// （控制整数部分的长度，这个非常有意思，默认是-1，整数的长度就是原来的长度，
        /// 当传入大于0的值时，如果整数部分小于这个值时，则不做任何处理，当整数部分的长度
        /// 不足这个值时，则需要在整数前面加0
        /// </param>
        /// <returns></returns>
        public static String Format(Double d, Int32 decimals = 0, Int32 integer = -1)
        {
            Double newD = Math.Round(d, decimals);
            String newDStr = newD.ToString();
           
            //处理小数部分
            if (decimals == 0)
            {
                //去掉小数部分
                newDStr = MyString.LeftOfLast(newDStr, ".", false, newDStr);
            }
            else
            {
                //获取到小数的部分
                String xs = MyString.RightOfLast(newDStr, ".");
                if (xs.Length != decimals)
                {
                    //在小数后面补上0
                    if (xs.Length == 0) newDStr += ".";
                    for (Int32 i = 0; i < decimals - xs.Length; i++)
                        newDStr += "0";
                }
            }

            //处理整数部分
            if (integer > 0)
            {
                //获取到整数的部分
                String zs = MyString.LeftOfLast(newDStr, ".", false, newDStr);
                if (zs.Length < integer)
                {
                    for (Int32 i = 0; i < integer - zs.Length; i++)
                        newDStr = "0" + newDStr;
                }
            }
            return newDStr;
        }

        #region "计算一个数字表达式的值, 如根据100-90计算出10"

        /// <summary>
        /// 计算一个数字表达式的值, 如根据"100-90"计算出10
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CaculateNumberExpressionValue(string text)
        {
            //非有效的表达式计算结果为空
            string strtemp1;
            string strtemp2;
            string strtemp3;
            int siz_;
            int number = 0;
            int i;
            text = Strings.StrConv(text, Constants.vbNarrow);
            text = Strings.Trim(text);
            //CheckNumberExpression是否有 {3+2)-5}+6)-7这种格式
            if (CheckNumberExpression(text) == false)
            {
                return "";
            }
            text = Strings.Replace(text, "[", "(");
            text = Strings.Replace(text, "{", "(");
            text = Strings.Replace(text, "]", ")");
            text = Strings.Replace(text, "}", ")");
            text = Strings.Replace(text, "÷", "/");
            text = Strings.Replace(text, "×", "*");
            text = Strings.Replace(text, " ", "");
            //初次CheckNumberExpression是否是合格的表达式
            if (text.StartsWith(".") == true | text.StartsWith("+") == true | text.StartsWith("-") == true)
            {
                return "";
            }
            else if (text.StartsWith("*") == true | text.StartsWith("/") == true | text.StartsWith("%") == true | text.StartsWith("^") == true)
            {
                return "";
            }
            else if (text.EndsWith("*") == true | text.EndsWith("/") == true | text.EndsWith("%") == true | text.EndsWith("^") == true)
            {
                return "";
            }
            else if (text.EndsWith(".") == true | text.EndsWith("+") == true | text.EndsWith("-") == true)
            {
                return "";
            }
            if (text.IndexOf(".(") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".)") != -1)
            {
                return "";
            }
            else if (text.IndexOf("..") != -1)
            {
                return "";
            }
            else if (text.IndexOf(").") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".(") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".+") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".-") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".*") != -1)
            {
                return "";
            }
            else if (text.IndexOf("./") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".%") != -1)
            {
                return "";
            }
            else if (text.IndexOf("+.") != -1)
            {
                return "";
            }
            else if (text.IndexOf("-.") != -1)
            {
                return "";
            }
            else if (text.IndexOf("*.") != -1)
            {
                return "";
            }
            else if (text.IndexOf("/.") != -1)
            {
                return "";
            }
            else if (text.IndexOf("%.") != -1)
            {
                return "";
            }
            else if (text.IndexOf("^.") != -1)
            {
                return "";
            }
            else if (text.IndexOf(".^") != -1)
            {
                return "";
            }
            //CheckNumberExpression是否存在 3.33.3这种数据
            strtemp1 = text;
            i = strtemp1.IndexOf(".");
            if (i > 0)
            {
                strtemp1 = Strings.Mid(strtemp1, i + 2, strtemp1.Length - i - 1);
                i = strtemp1.IndexOf(".");
                while (i > 0)
                {
                    strtemp2 = Strings.Mid(strtemp1, 1, i);
                    strtemp1 = Strings.Mid(strtemp1, i + 2, strtemp1.Length - i - 1);
                    i = strtemp2.Length;
                    while (i > 0)
                    {
                        strtemp3 = Strings.Mid(strtemp1, i, 1);
                        if (Convert.ToInt32(strtemp3) < 0 | Convert.ToInt32(strtemp3) > 9)
                        {
                            break; // TODO: might not be correct. Was : Exit While
                        }
                        i = i - 1;
                    }
                    if (i == 0)
                    {
                        return "";
                    }
                    i = strtemp1.IndexOf(".");
                }
            }
            strtemp1 = text;
            strtemp1 = Strings.Replace(strtemp1, "(", "");
            strtemp1 = Strings.Replace(strtemp1, ")", "");
            strtemp1 = Strings.Replace(strtemp1, "+", "");
            strtemp1 = Strings.Replace(strtemp1, "-", "");
            strtemp1 = Strings.Replace(strtemp1, "*", "");
            strtemp1 = Strings.Replace(strtemp1, "/", "");
            strtemp1 = Strings.Replace(strtemp1, "%", "");
            strtemp1 = Strings.Replace(strtemp1, ".", "");
            strtemp1 = Strings.Replace(strtemp1, "^", "");
            if (strtemp1 == null)
            {
                return "";
            }
            siz_ = strtemp1.Length;
            while (siz_ > 0)
            {
                strtemp2 = Strings.Mid(strtemp1, siz_, 1);
                if (Convert.ToInt32(strtemp2) < 0 || Convert.ToInt32(strtemp2) > 9)
                {
                    return "";
                }
                siz_ = siz_ - 1;
            }
            //第二次CheckNumberExpression表达式是否正确
            strtemp1 = text;
            i = strtemp1.LastIndexOf("(");
            while (i > -1)
            {
                strtemp1 = Strings.Mid(strtemp1, 1, i);
                i = strtemp1.LastIndexOf("(");
                number = number + 1;
            }
            strtemp1 = text;
            i = strtemp1.LastIndexOf(")");
            while (i > -1)
            {
                strtemp1 = Strings.Mid(strtemp1, 1, i);
                i = strtemp1.LastIndexOf(")");
                number = number - 1;
            }
            if (number != 0)
            {
                return "";
            }
            strtemp1 = text;
            strtemp1 = Strings.Replace(strtemp1, "+)", "!");
            strtemp1 = Strings.Replace(strtemp1, "-)", "!");
            strtemp1 = Strings.Replace(strtemp1, "*)", "!");
            strtemp1 = Strings.Replace(strtemp1, "/)", "!");
            strtemp1 = Strings.Replace(strtemp1, "%)", "!");
            strtemp1 = Strings.Replace(strtemp1, "^)", "!");
            strtemp1 = Strings.Replace(strtemp1, "()", "!");
            strtemp1 = Strings.Replace(strtemp1, ")(", "!");
            strtemp1 = Strings.Replace(strtemp1, "(+", "!");
            strtemp1 = Strings.Replace(strtemp1, "(*", "!");
            strtemp1 = Strings.Replace(strtemp1, "(/", "!");
            strtemp1 = Strings.Replace(strtemp1, "(%", "!");
            strtemp1 = Strings.Replace(strtemp1, "(^", "!");
            if (strtemp1.LastIndexOf("!") > -1)
            {
                return "";
            }

            //正式计算
            i = text.LastIndexOf("(");
            while (i > -1)
            {
                strtemp1 = Strings.Mid(text, 1, i);
                i = i + 2;
                strtemp2 = Strings.Mid(text, i, text.Length - i + 1);
                siz_ = strtemp2.IndexOf(")");
                text = Strings.Mid(strtemp2, siz_ + 2, strtemp2.Length - siz_ - 1);
                strtemp2 = Strings.Mid(strtemp2, 1, siz_);
                strtemp2 = CaculateNumberExpressionValueCall(strtemp2);
                if (strtemp2 == "")
                {
                    return "";
                }
                text = strtemp1 + strtemp2 + text;
                i = text.LastIndexOf("(");
            }
            return CaculateNumberExpressionValueCall(text);
        }

        private static bool CheckNumberExpression(string text)
        {
            //CheckNumberExpression是否有 {3+2)-5}+6)-7这种格式
            string temp1;
            string temp2;
            int i;
            i = text.IndexOf("{");
            if (i > -1)
            {
                temp1 = Strings.Mid(text, 1, i);
                text = Strings.Mid(text, i + 2, text.Length - i - 1);
                i = text.IndexOf("}");
                if (i == -1)
                {
                    return false;
                }
                temp2 = Strings.Mid(text, 1, i);
                if (CheckNumberExpression(temp2) == false)
                {
                    return false;
                }
                text = Strings.Mid(text, i + 2, text.Length - i - 1);
                text = temp1 + temp2 + text;
                i = text.IndexOf("{");
            }
            i = text.IndexOf("[");
            if (i > -1)
            {
                temp1 = Strings.Mid(text, 1, i);
                text = Strings.Mid(text, i + 2, text.Length - i - 1);
                i = text.IndexOf("]");
                if (i == -1)
                {
                    return false;
                }
                temp2 = Strings.Mid(text, 1, i);
                if (CheckNumberExpression(temp2) == false)
                {
                    return false;
                }
                text = Strings.Mid(text, i + 2, text.Length - i - 1);
                text = temp1 + temp2 + text;
                i = text.IndexOf("[");
            }
            return true;
        }

        private static string CaculateNumberExpressionValueCall(string text)
        {
            int i;
            int k;
            int Type_;
            //代表1+,2-,3*,4/,5%
            string temp1;
            string temp2;
            string temp3;
            i = text.IndexOf("^");
            while ((i > 0))
            {
                temp1 = Microsoft.VisualBasic.Strings.Mid(text, 1, i);
                temp2 = Microsoft.VisualBasic.Strings.Mid(text, i + 2, text.Length - i - 1);
                i = temp1.Length;
                while (i > 0)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp1, i, 1);
                    if (Convert.ToInt32(temp3) > 9 | Convert.ToInt32(temp3) < 0)
                    {
                        if (temp3 != ".")
                        {
                            break; // TODO: might not be correct. Was : Exit While
                        }
                    }
                    i = i - 1;
                }
                temp1 = Microsoft.VisualBasic.Strings.Mid(temp1, i + 1, temp1.Length - i);
                k = temp2.Length + 1;
                i = 1;
                while (k > i)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp2, i, 1);
                    if (Convert.ToInt32(temp3) <= 9 && Convert.ToInt32(temp3) >= 0)
                    {
                        i = i + 1;
                    }
                    else if (temp3 == ".")
                    {
                        i = i + 1;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                text = Microsoft.VisualBasic.Strings.Mid(temp2, i, k - i);
                temp2 = Microsoft.VisualBasic.Strings.Mid(temp2, 1, i - 1);
                temp1 = System.Math.Pow(Convert.ToDouble(temp1), Convert.ToDouble(temp2)).ToString();
                if (temp1 == "非数字")
                {
                    return "";
                }
                text = temp1 + text;
                i = text.IndexOf("^");
            }
            //**************************************************************
            i = text.IndexOf("*");
            k = text.IndexOf("/");
            while (i > 0 | k > 0)
            {
                if (i > 0)
                {
                    if (k > 0)
                    {
                        if (k < i)
                        {
                            Type_ = 4;
                            i = k;
                        }
                        else
                        {
                            Type_ = 3;
                        }
                    }
                    else
                    {
                        Type_ = 3;
                    }
                }
                else
                {
                    i = k;
                    Type_ = 4;
                }
                temp1 = Microsoft.VisualBasic.Strings.Mid(text, 1, i);
                temp2 = Microsoft.VisualBasic.Strings.Mid(text, i + 2, text.Length - i - 1);
                k = 1;
                while (k < temp2.Length + 1)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp2, k, 1);
                    if (Convert.ToInt32(temp3) >= 0 & Convert.ToInt32(temp3) <= 9)
                    {
                        k = k + 1;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                text = Microsoft.VisualBasic.Strings.Mid(temp2, k, temp2.Length + 1 - k);
                temp2 = Microsoft.VisualBasic.Strings.Mid(temp2, 1, k - 1);
                k = temp1.Length;
                while (k > 0)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp1, k, 1);
                    if (Convert.ToInt32(temp3) >= 0 & Convert.ToInt32(temp3) <= 9)
                    {
                        k = k - 1;
                    }
                    else if (temp3 == ".")
                    {
                        k = k - 1;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                temp3 = Microsoft.VisualBasic.Strings.Mid(temp1, 1, k);
                temp1 = Microsoft.VisualBasic.Strings.Mid(temp1, k + 1, temp1.Length - k);
                if (Type_ == 4)
                {
                    if (Convert.ToDouble(temp2) == 0.0)
                    {
                        return "";
                    }
                    temp1 = (Convert.ToDouble(temp1) / Convert.ToDouble(temp2)).ToString();
                }
                else
                {
                    temp1 = (Convert.ToDouble(temp1) * Convert.ToDouble(temp2)).ToString();
                }
                text = temp3 + temp1 + text;
                i = text.IndexOf("*");
                k = text.IndexOf("/");
            }
            //******************************************************************
            i = text.IndexOf("+");
            k = text.IndexOf("-");
            while (i > 0 | k > 0)
            {
                if (i > 0)
                {
                    if (k > 0)
                    {
                        if (k < i)
                        {
                            Type_ = 2;
                            i = k;
                        }
                        else
                        {
                            Type_ = 1;
                        }
                    }
                    else
                    {
                        Type_ = 1;
                    }
                }
                else
                {
                    i = k;
                    Type_ = 2;
                }
                temp1 = Microsoft.VisualBasic.Strings.Mid(text, 1, i);
                temp2 = Microsoft.VisualBasic.Strings.Mid(text, i + 2, text.Length - i - 1);
                k = 1;
                while (k < temp2.Length + 1)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp2, k, 1);
                    if (Convert.ToInt32(temp3) >= 0 && Convert.ToInt32(temp3) <= 9)
                    {
                        k = k + 1;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                text = Microsoft.VisualBasic.Strings.Mid(temp2, k, temp2.Length + 1 - k);
                temp2 = Microsoft.VisualBasic.Strings.Mid(temp2, 1, k - 1);
                k = temp1.Length;
                while (k > 0)
                {
                    temp3 = Microsoft.VisualBasic.Strings.Mid(temp1, k, 1);
                    if (Convert.ToInt32(temp3) >= 0 & Convert.ToInt32(temp3) <= 9)
                    {
                        k = k - 1;
                    }
                    else if (temp3 == ".")
                    {
                        k = k - 1;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                temp3 = Microsoft.VisualBasic.Strings.Mid(temp1, 1, k);
                temp1 = Microsoft.VisualBasic.Strings.Mid(temp1, k + 1, temp1.Length - k);
                if (Type_ == 1)
                {
                    temp1 = (Convert.ToDouble(temp1) + Convert.ToDouble(temp2)).ToString();
                }
                else
                {
                    temp1 = (Convert.ToDouble(temp1) - Convert.ToDouble(temp2)).ToString();
                }
                text = temp3 + temp1 + text;
                i = text.IndexOf("+");
                k = text.IndexOf("-");
            }
            //**************************************%
            i = text.IndexOf("%");
            while (i > 0)
            {
                temp1 = Microsoft.VisualBasic.Strings.Mid(text, 1, i);
                temp2 = Microsoft.VisualBasic.Strings.Mid(text, i + 2, text.Length - i - 1);
                k = temp2.IndexOf("%");
                if (k > 0)
                {
                    text = Microsoft.VisualBasic.Strings.Mid(temp2, k + 1, temp2.Length - k);
                    temp2 = Microsoft.VisualBasic.Strings.Mid(temp2, 1, k - 1);
                }
                else
                {
                    text = "";
                }
                if (Convert.ToSingle(temp2) == 0)
                {
                    return "";
                }
                text = (Convert.ToSingle(temp1) % Convert.ToSingle(temp2)).ToString() + text;
                i = text.IndexOf("%");
            }
            return text;
        }

        /// <summary>
        /// 格式化金额数字，插入千隔符
        /// </summary>
        /// <param name="vMoneyValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatMoneyNumber(object vMoneyValue)
        {
            string sMoneyValue = vMoneyValue.ToString();
            string sReturn = "";
            string sXiaoShu = "";

            if (sMoneyValue.IndexOf(".") != -1)
            {
                sXiaoShu = MyString.Right(sMoneyValue, ".");
                sMoneyValue = MyString.Left(sMoneyValue, ".");
            }

            int iIndex;
            char sNum;
            for (int i = 1; i <= sMoneyValue.Length; i++)
            {
                iIndex = sMoneyValue.Length - i;
                sNum = sMoneyValue[iIndex];
                sReturn = sNum + sReturn;
                if (i % 3 == 0 && i != sMoneyValue.Length)
                {
                    sReturn = "," + sReturn;
                }
            }

            if (sXiaoShu != "")
            {
                sReturn += "." + sXiaoShu;
            }

            return sReturn;
        }

        #endregion
    }
}
