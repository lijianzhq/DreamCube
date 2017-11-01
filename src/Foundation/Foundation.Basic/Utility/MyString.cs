using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyString
    {
        public enum TrimType
        {
            /// <summary>
            /// 左边去掉空格
            /// </summary>
            trimLeft,
            /// <summary>
            /// 右边去掉空格
            /// </summary>
            trimRight,
            /// <summary>
            /// 两边去掉空格
            /// </summary>
            trimBoth,
            /// <summary>
            /// 不需要去掉空格
            /// </summary>
            noTrim
        }

        /// <summary>
        /// 从SQL中获取TableName
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetTableNameFromSQL(String sql)
        {
            string sTableName = "";
            if (!string.IsNullOrEmpty(sql))
            {
                sTableName = MyString.Right(sql.ToUpper(), " FROM ").Trim();
                if (sTableName.IndexOf(" ") != -1) sTableName = MyString.Left(sTableName, " ");
            }
            return sTableName;
        }

        /// <summary> 
        /// 从父字符串中删除指定的子字符串(考虑分隔符情况) 
        /// </summary> 
        /// <param name="sStr">父字符串</param> 
        /// <param name="sSubStr">子字符串，如果有多个子字符串，以分隔符隔开</param> 
        /// <param name="sSeparator">分隔符</param> 
        /// <param name="bIgnoreCase">是否忽略大小写</param> 
        /// <returns></returns> 
        public static string Remove(string sStr, string sSubStr, string sSeparator = ";", bool bIgnoreCase = false)
        {
            if (sStr == "") return "";
            string sReturn = String.Empty;
            if (sSubStr.IndexOf(sSeparator) != -1)
            {
                sReturn = sStr;
                string[] aTemps = MyString.SplitEx(sSubStr, sSeparator);
                foreach (string sTemp in aTemps)
                {
                    sReturn = MyString.Remove(sReturn, sTemp, sSeparator, bIgnoreCase);
                }
            }
            else
            {
                string[] aSubStrs = MyString.SplitEx(sStr, sSeparator);
                foreach (string SubStr in aSubStrs)
                {
                    if (MyString.StrEqual(SubStr, sSubStr, bIgnoreCase)) continue;
                    sReturn = MyString.Connect(sReturn, SubStr, sSeparator);
                }
            }
            return sReturn;
        }

        /// <summary> 
        /// 转换字符串为合法的格式 
        /// </summary> 
        /// <param name="oStr"></param> 
        /// <returns></returns> 
        public static string TurnToJs(object oStr)
        {
            if (oStr == null)
            {
                return "";
            }
            else
            {
                string sStr = oStr.ToString();
                if (sStr != "")
                {
                    sStr = sStr.Replace("\\", "\\\\");
                    sStr = sStr.Replace("\"", "\\\"");
                    sStr = sStr.Replace(Convert.ToChar(10).ToString(), "\\n");
                    sStr = sStr.Replace(Convert.ToChar(13).ToString(), "\\r");
                    sStr = sStr.Replace("</script>", "\\x3C/script>");
                    sStr = sStr.Replace("<script", "\\x3Cscript>");
                }
                return sStr;
            }
        }

        /// <summary>
        /// 功能: 获取字符串的字节长度
        /// 作者: 刘学亮
        /// </summary>
        /// <param name="sStr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetByteLength(string sStr)
        {
            if (string.IsNullOrEmpty(sStr) == false) return System.Text.Encoding.Default.GetByteCount(sStr);
            else return 0;
        }

        /// <summary>
        /// 功能: 按字节长度截取字符串
        /// </summary>
        /// <param name="str">被截取的字符串</param>
        /// <param name="len">字节长度</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetSubStringB(string str, int len)
        {
            string result = string.Empty;
            // 最终返回的结果 
            int byteLen = MyString.GetByteLength(str);
            // 单字节字符长度 
            int charLen = str.Length;
            // 把字符平等对待时的字符串长度 
            int byteCount = 0;
            // 记录读取进度 
            int pos = 0;
            // 记录截取位置 
            if (byteLen > len)
            {
                for (int i = 0; i <= charLen - 1; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)
                    {
                        // 按中文字符计算加2 
                        byteCount += 2;
                    }
                    else
                    {
                        // 按英文字符计算加1 
                        byteCount += 1;
                    }
                    if (byteCount > len)
                    {
                        // 超出时只记下上一个有效位置 
                        pos = i;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    else if (byteCount == len)
                    {
                        // 记下当前位置 
                        pos = i + 1;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                if (pos >= 0) result = str.Substring(0, pos);
            }
            else
            {
                result = str;
            }
            return result;
        }

        /// <summary>
        /// 数据连成字符串
        /// </summary>
        /// <param name="aArrays"></param>
        /// <param name="sSeparator"></param>
        /// <param name="removeNothing">是否移除空值</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Join(ArrayList aArrays, String sSeparator, Boolean removeNothingAndEmptyString = false)
        {
            return Join(MyArrayList.CopyToArray<String>(aArrays), sSeparator, removeNothingAndEmptyString, null, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 数据连成字符串
        /// </summary>
        /// <param name="aArrays"></param>
        /// <param name="sSeparator"></param>
        /// <param name="removeNothing">是否移除空值</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Join(String[] aArrays, String sSeparator, Boolean removeNothingAndEmptyString = false)
        {
            return Join(aArrays, sSeparator, removeNothingAndEmptyString, null, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 数据连成字符串
        /// </summary>
        /// <param name="aArrays"></param>
        /// <param name="sSeparator"></param>
        /// <param name="removeNothing">是否移除空值</param>
        /// <param name="removeString">拼接的时候，需要移除的子串</param>
        /// <param name="removeSubStringComparison">拼接的时候，需要移除的子串与字符串数组的字符串比较的方式</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Join(String[] aArrays, String sSeparator, Boolean removeNothingAndEmptyString = false, String[] removeSubString = null, StringComparison removeSubStringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            string sReturn = "";
            if (aArrays != null && aArrays.Length > 0)
            {
                Int32 index = 0;
                for (int i = 0; i <= aArrays.Length - 1; i++)
                {
                    //String value = Convert.ToString(aArrays[i]);
                    if (removeNothingAndEmptyString && String.IsNullOrEmpty(aArrays[i])) continue;
                    for (var j = 0; j < removeSubString.Length; j++)
                    {
                        if (String.Equals(removeSubString[j], aArrays[i], removeSubStringComparison)) continue;
                    }
                    if (index == 0)
                    {
                        sReturn = aArrays[i];
                    }
                    else
                    {
                        sReturn += sSeparator + aArrays[i];
                    }
                }
            }
            return sReturn;
        }

        /// <summary>
        /// 删除重复的子字符串
        /// </summary>
        /// <param name="sStr"></param>
        /// <param name="sSeparator"></param>
        /// <param name="bIgnoreCase"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String RemoveRepeateSubString(String sStr, String sSeparator, Boolean bIgnoreCase = false)
        {
            String[] aStrings = MyString.SplitEx(sStr, sSeparator);
            MyString.RemoveRepeatItems(aStrings, bIgnoreCase);
            return MyString.Join(aStrings, sSeparator);
        }

        /// <summary>
        /// 移除空串
        /// </summary>
        /// <param name="sStr"></param>
        /// <param name="sSeparator"></param>
        /// <returns></returns>
        public static string RemoveEmptySubString(string sStr, string sSeparator)
        {
            if (string.IsNullOrEmpty(sStr)) return "";
            string[] aStrings = MyString.SplitEx(sStr, sSeparator);
            StringBuilder sb = new StringBuilder();
            foreach (String s in aStrings)
            {
                if (!String.IsNullOrEmpty(s))
                {
                    MyString.Append(sb,s,sSeparator);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 组合字符串数组为一个字符串
        /// </summary>
        /// <param name="aArrays"></param>
        /// <param name="sSeparator"></param>
        /// <returns></returns>
        public static String Join(String[] aArrays, String sSeparator)
        {
            StringBuilder sb = new StringBuilder();
            if (aArrays != null && aArrays.Length > 0)
            {
                for (int i = 0; i <= aArrays.Length - 1; i++)
                {
                    if (i == 0) sb.Append(aArrays[i]);
                    else sb.Append(sSeparator + aArrays[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary> 
        /// 字符串联接 
        /// </summary> 
        /// <param name="sStrA">被联接的第一个字符串</param> 
        /// <param name="sStrB">被联接的第二个字符串</param> 
        /// <param name="sSeparator">联接分隔符</param> 
        /// <returns></returns> 
        public static String Connect(String sStrA, String sStrB, String sSeparator)
        {
            return String.IsNullOrEmpty(sStrA) ? sStrB : sStrA + sSeparator + sStrB;
        }

        /// <summary> 
        /// 字符串比较 
        /// </summary> 
        /// <param name="sb">StringBuilder对象</param> 
        /// <param name="target">加到Builder对象的字符串</param> 
        /// <param name="divString">字符串分隔符</param> 
        /// <returns></returns> 
        public static StringBuilder Append(StringBuilder sb, string target, String divString)
        {
            if (sb.Length == 0) sb.Append(target);
            else
            {
                sb.Append(divString + target);
            }
            return sb;
        }

        /// <summary> 
        /// 字符串比较 
        /// </summary> 
        /// <param name="sStrA">参加比较的第一个字符串</param> 
        /// <param name="sStrB">参加比较的第二个字符串</param> 
        /// <param name="bIgnoreCase">是否忽略大小写, True=忽略大小写; false=大小写敏感</param> 
        /// <returns></returns> 
        public static bool StrEqual(string sStrA, string sStrB, bool bIgnoreCase = false)
        {
            if (bIgnoreCase)
            {
                //大小写不敏感 
                sStrA = sStrA.ToLower();
                sStrB = sStrB.ToLower();
            }

            return sStrA.CompareTo(sStrB) == 0;
        }

        /// <summary>
        /// EndsWith()方法的多个字符串版本，只要满足其中一个字符串，则为返回true；否则返回false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Boolean EndsWith(String target, String[] values)
        {
            if (values == null || values.Length == 0) return false;
            for (Int32 i = 0; i < values.Length; i++)
                if (target.EndsWith(values[i])) return true;
            return false;
        }

        /// <summary>
        /// StartsWith()方法的多个字符串版本，只要满足其中一个字符串，则为返回true；否则返回false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Boolean EndsWith(String target, List<String> values)
        {
            if (values == null) return false;
            Int32 count = values.Count;
            if (count == 0) return false;
            for (Int32 i = 0; i < count; i++)
                if (target.EndsWith(values[i])) return true;
            return false;
        }

        /// <summary>
        /// StartsWith()方法的多个字符串版本，只要满足其中一个字符串，则为返回true；否则返回false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Boolean StartsWith(String target, String[] values)
        {
            if (values == null || values.Length == 0) return false;
            for (Int32 i = 0; i < values.Length; i++)
                if (target.StartsWith(values[i])) return true;
            return false;
        }

        /// <summary>
        /// StartsWith()方法的多个字符串版本，只要满足其中一个字符串，则为返回true；否则返回false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Boolean StartsWith(String target, List<String> values)
        {
            if (values == null) return false;
            Int32 count = values.Count;
            if (count == 0) return false;
            for (Int32 i = 0; i < count; i++)
                if (target.StartsWith(values[i])) return true;
            return false;
        }

        /// <summary>
        /// 把字符串转换成base64编码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static String ToBase64(String target, Encoding encoding)
        {
            if (String.IsNullOrEmpty(target)) return String.Empty;
            if (encoding == null) encoding = System.Text.Encoding.Default;
            Byte[] b = encoding.GetBytes(target);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 把base64编码转成字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static String ParseFromBase64(String target, Encoding encoding)
        {
            if (String.IsNullOrEmpty(target)) return String.Empty;
            Byte[] b = Convert.FromBase64String(target);
            if (encoding == null) encoding = System.Text.Encoding.Default;
            return encoding.GetString(b);
        }

        /// <summary>
        /// 字符串转换成布尔型
        /// 字符串为空、false、0，获取其他未列举的的情况均返回false；
        /// 字符串为1，true，返回true
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static Boolean ToBoolean(String target)
#else
        public static Boolean ToBoolean(this String target)
#endif
        {
            if (String.IsNullOrEmpty(target)) return false;
            if (String.Compare(target, "true", true) == 0) return true;
            if (String.Compare(target, "false", true) == 0) return false;
            if (String.Compare(target, "0") == 0) return false;
            if (String.Compare(target, "1") == 0) return true;
            return false;
        }

        /// <summary>
        /// 字符串倒转
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String Reverse(String target)
#else
        public static String Reverse(this String target)
#endif
        {
            if (String.IsNullOrEmpty(target)) return String.Empty;
            Char[] value = target.ToCharArray();
            Array.Reverse(value);
            return new String(value);
        }
        
        /// <summary>
        /// 返回一个整数标识比较的内容
        /// 返回0表示两个字符串相等
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="stringComparison">字符串的比较控制</param>
        /// <param name="trimType">指示对去掉空格的处理方法</param>
        /// <returns></returns>
#if NET20
        public static Int32 CompareEx(String str1, String str2, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase, TrimType trimType = TrimType.trimBoth)
#else
        public static Int32 CompareEx(String str1, String str2, StringComparison stringComparison, TrimType trimType= TrimType.trimBoth)
#endif
        {
            if (trimType == TrimType.trimBoth)
            {
                str1 = str1.Trim();
                str2 = str2.Trim();
            }
            else if (trimType == TrimType.trimLeft)
            {
                str1 = str1.TrimStart();
                str2 = str2.TrimStart();
            }
            else if (trimType == TrimType.trimRight)
            {
                str1 = str1.TrimEnd();
                str2 = str2.TrimEnd();
            }
            return String.Compare(str1, str2, stringComparison);
        }

        /// <summary>
        /// 判断target中是否含有subStr(可选择是否大小写敏感) 
        /// </summary>
        /// <param name="target">父字符串</param>
        /// <param name="subStr">子字符串</param>
        /// <param name="separator">字符串分隔符</param>
        /// <param name="stringComparison">比较字符串的方式</param>
        /// <returns></returns>
#if NET20
        public static Boolean ContainsEx(String target, String subStr, String separator, StringComparison stringComparison)
#else
        public static Boolean ContainsEx(this String target, String subStr, String separator = "", StringComparison stringComparison = StringComparison.CurrentCulture)
#endif
        {
            if (String.IsNullOrEmpty(target)) return false;
            target = separator + target + separator;
            subStr = separator + subStr + separator;
            return target.IndexOf(subStr, stringComparison) >= 0;
        }

        /// <summary>
        /// 判断字符串数组中是否包含有目标字符串
        /// </summary>
        /// <param name="sourceArray">字符串数组</param>
        /// <param name="targetString">目标字符串</param>
        /// <param name="comparison">字符串比较的方式</param>
        /// <returns></returns>
        public static Boolean ContainsTargetStringInArray(String[] sourceArray, String targetString,StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (sourceArray == null || sourceArray.Length == 0) return false;
            for (var i = 0; i < sourceArray.Length; i++)
            {
                if (String.IsNullOrEmpty(targetString))
                {
                    if (String.IsNullOrEmpty(sourceArray[i])) return true;
                }
                else
                {
                    if (String.Equals(sourceArray[i], targetString, comparison))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断target中是否含有subStr(可选择是否大小写敏感) 
        /// </summary>
        /// <param name="target">父字符串</param>
        /// <param name="subStr">子字符串</param>
        /// <param name="separator">字符串分隔符</param>
        /// <param name="bIgnoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static Boolean ContainsEx(String target, String subStr, String separator, bool bIgnoreCase = false)
        {
            if (String.IsNullOrEmpty(target)) return false;
            target = separator + target + separator;
            subStr = separator + subStr + separator;
            if (bIgnoreCase)
            {
                //大小写不敏感 
                target = target.ToLower();
                subStr = subStr.ToLower();
            }
            return target.IndexOf(subStr) >= 0;
        }

        /// <summary>
        /// 判断target中是否含有subStr(可选择是否大小写敏感) 
        /// </summary>
        /// <param name="target">父字符串</param>
        /// <param name="subStr">子字符串</param>
        /// <param name="bIgnoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static Boolean ContainsEx(String target, String subStr, bool bIgnoreCase = false)
        {
            return ContainsEx(target, subStr, "", bIgnoreCase);
        }

        /// <summary>
        /// 统计字串在某字符串中出现的次数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="subStr">需要统计的字符串</param>
        /// <param name="bIgnoreCase">是否区分大小写； 默认是区分大小写的</param>
        /// <returns></returns>
#if NET20
        public static Int32 CountSubString(String target, String subStr, Boolean ignoreCase = false)
#else
        public static Int32 CountSubString(this String target, String subStr, Boolean ignoreCase = false)
#endif
        {
            if (ignoreCase)
            {
                target = target.ToLower();
                subStr = subStr.ToLower();
            }
            return MyString.SplitEx(target, subStr).Length - 1;
        }

        /// <summary>
        /// 根据两个字符，求两个字符中间的子字符串内容，所有符合这种匹配的子串
        /// 注意了：如果是贪婪匹配，则符合条件的子串肯定只有一个（数组长度肯定为1）；
        /// 如果是非贪婪匹配，则有可能返回多个符合条件的子串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startStr">起始字符串</param>
        /// <param name="endStr">结束字符串(注意，结束字符是从开始字符往后计算的，否则也没有意义了)</param>
        /// <param name="greedyMatch">是否贪婪匹配，true表示贪婪匹配：也就是最大范围的匹配左右两个字符；false：为相反</param>
        /// <param name="bIgnoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <returns></returns>
#if NET20
        public static String[] SubStringBetweenStr(String target,
                                                   String startStr,
                                                   String endStr,
                                                   Boolean greedyMatch = true,
                                                   Boolean ignoreCase = false)
#else
        public static String[] SubStringBetweenStr(this String target,
                                                  String startStr,
                                                  String endStr,
                                                  Boolean greedyMatch = true,
                                                  Boolean ignoreCase = false)
#endif
        {
            Int32 startIndex = 0;
            Int32 endIndex = 0;
            if (greedyMatch)
            {
                startIndex = target.IndexOf(startStr,
                        ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                endIndex = target.LastIndexOf(endStr,
                        ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                if (startIndex >= 0 && endIndex >= 0 && startIndex != endIndex)
                    return new String[] { target.Substring(startIndex + startStr.Length, endIndex - startIndex - startStr.Length) };
                return null;
            }
            else
            {
                List<String> tempStr = new List<String>();
                String tempTargetStr = target;
                do
                {
                    startIndex = tempTargetStr.IndexOf(startStr,
                        ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                    if (startIndex < 0) break;
                    Int32 countEndStrStartIndex = startIndex + startStr.Length;
                    if (tempTargetStr.Length <= countEndStrStartIndex) break;
                    endIndex = tempTargetStr.IndexOf(endStr,
                        countEndStrStartIndex,
                        ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                    if (endIndex >= 0 && startIndex < endIndex && startIndex != endIndex)
                        tempStr.Add(tempTargetStr.Substring(countEndStrStartIndex, endIndex - countEndStrStartIndex));
                    //继续搜索剩余的字符串
                    tempTargetStr = tempTargetStr.Substring(endIndex + 1);
                } while (!String.IsNullOrEmpty(tempTargetStr) && startIndex >= 0 && endIndex >= 0 && startIndex != endIndex);
                return tempStr.ToArray();
            }
        }

        /// <summary>
        /// 把某两个位置之间的字符串替换成其他指定的字符串
        /// </summary>
        /// <param name="target">目标字符串</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="text">替换的文本</param>
        /// <returns></returns>
#if NET20
        public static String ReplaceEx(String target, Int32 startIndex, Int32 endIndex, String text)
#else
        public static String ReplaceEx(this String target, Int32 startIndex, Int32 endIndex, String text)
#endif
        {
            return target.Replace(target.Substring(startIndex, endIndex - startIndex), text);
        }

        /// <summary>
        /// 替换字符串（支持忽略大小写的操作）
        /// </summary>
        /// <param name="target">需要处理的目标字符串</param>
        /// <param name="subStr">被替换的子字符串</param>
        /// <param name="replaceStr">准备替换的字符串</param>
        /// <param name="ignoreCase">true：忽略大小写；false：区分大小写</param>
        /// <returns></returns>
        public static String ReplaceEx(String target, String subStr, String replaceStr, Boolean ignoreCase = false)
        {
            if (String.IsNullOrEmpty(target) || String.IsNullOrEmpty(subStr) || String.IsNullOrEmpty(replaceStr)) return target;
            if (ignoreCase) 
            {
                List<String> subs = MyString.GetAllSubString(target, subStr);
                if (subs != null)
                {
                    for (Int32 i = 0, j = subs.Count; i < j; i++)
                        target = target.Replace(subs[i], replaceStr);
                }
                return target;
            }
            else 
            {
                return target.Replace(subStr, replaceStr);
            }
        }

        /// <summary>
        /// 把两个字符串之间的字符串替换成其他指定的字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <param name="text"></param>
        /// <param name="maxReplace">是否最大匹配，为true时，表示最大匹配替换，与正则表达式的贪婪匹配概念一样</param>
        /// <param name="ignoreCase">是否忽略大小写，默认为false，也就是大小写敏感的</param>
        /// <returns></returns>
        
#if NET20
        public static String ReplaceEx(String target,
                                       String startStr,
                                       String endStr,
                                       String text,
                                       Boolean maxReplace = true,
                                       Boolean ignoreCase = false)
#else
        public static String ReplaceEx(this String target,
                                       String startStr,
                                       String endStr,
                                       String text,
                                       Boolean maxReplace = true,
                                       Boolean ignoreCase = false)
#endif
        {
            String[] subString = SubStringBetweenStr(target , startStr, endStr, maxReplace, ignoreCase);
            for (Int32 i = 0; i < subString.Length; i++)
                target = target.Replace(subString[i], endStr);
            return target;
        }

        /// <summary>
        /// 拆分字符串为字符串数组，如果目标字符串为空串或者为NULL，则返回NULL；
        /// 否则返回分割后的字符串数组
        /// </summary>
        /// <param name="target"></param>
        /// <param name="separatorStr">分隔符号</param>
        /// <param name="stringSplitOptions"></param>
        /// <param name="trimType">指定去掉空格的方式，默认是不进行去掉空格</param>
        /// <returns></returns>
#if NET20
        public static String[] SplitEx(String target,
                                       String separatorStr,
                                       StringSplitOptions stringSplitOptions = StringSplitOptions.None,
                                       TrimType trimType = TrimType.noTrim)
#else
        public static String[] SplitEx(this String target,
                                       String separatorStr,
                                       StringSplitOptions stringSplitOptions = StringSplitOptions.None,
                                       TrimType trimType = TrimType.noTrim)
#endif
        {
            if (String.IsNullOrEmpty(target)) return null;
            String[] result = target.Split(new String[] { separatorStr }, stringSplitOptions);
            if (trimType == TrimType.noTrim) return result;
            else
            {
                List<String> resultList = new List<String>();
                if (trimType == TrimType.trimBoth)
                {
                    for (Int32 i = 0, j = result.Length; i < j; i++)
                        resultList.Add(result[i].Trim());
                }
                else if (trimType == TrimType.trimLeft)
                {
                    for (Int32 i = 0, j = result.Length; i < j; i++)
                        resultList.Add(result[i].TrimStart());
                }
                else if (trimType == TrimType.trimRight)
                {
                    for (Int32 i = 0, j = result.Length; i < j; i++)
                        resultList.Add(result[i].TrimEnd());
                }
                return resultList.ToArray();
            }
        }

        /// <summary>
        /// 拆分字符串为字符串数组，如果目标字符串为空串或者为NULL，则返回NULL；
        /// 否则返回分割后的字符串数组；最后剩余不足一个单元的字符数组归并为一组
        /// 从左往右进行拆分成字符串数组，每指定的长度拆分成一个单元；例如：AABBCC，指定拆分长度为2时；返回三个数组为：AA;BB;CC
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cellLength">指定的单元的长度</param>
        /// <returns></returns>
#if NET20
        public static String[] SplitEx(String target, Int32 cellLength)
#else
        public static String[] SplitEx(this String target, Int32 cellLength)
#endif
        {
            if (String.IsNullOrEmpty(target)) return null;
            Int32 cellCount = target.Length / cellLength + ((target.Length % cellLength == 0) ? 0 : 1);
            String[] results = new String[cellCount];
            for (Int32 i = 0; i < cellCount; i++)
            {
                if (i == cellCount - 1) results[i] = target.Substring(i * cellLength);
                else results[i] = target.Substring(i * cellLength, cellLength);
            }
            return results;
        }

        /// <summary>
        /// 拆分字符串为字符串数组，如果目标字符串为空串或者为NULL，则返回NULL；
        /// 否则返回分割后的字符串数组；
        /// 从右往左进行拆分成字符串数组，每指定的长度拆分成一个单元；例如：AABBCC，指定拆分长度为2时；返回三个数组为：AA;BB;CC
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cellLength">指定的单元的长度</param>
        /// <returns></returns>
#if NET20
        public static String[] SplitExOfLast(String target, Int32 cellLength)
#else
        public static String[] SplitExOfLast(this String target, Int32 cellLength)
#endif
        {
            return MyString.SplitEx(MyString.Reverse(target), cellLength);
        }

        /// <summary>
        /// 把字符串拆分到dictionary中；
        /// 例如：“黎鉴,1;海清,2”   拆分到dictionary会有两项，前面为key，后面为value；项与项之间用分号隔开，key与value之间用逗号隔开
        /// </summary>
        /// <param name="target">等待拆分的目标字符串</param>
        /// <param name="itemDivChar">项与项之间的分隔符，默认为分号;</param>
        /// <param name="keyValueDivChar">key与value之间的分隔符，默认为逗号,</param>
        /// <param name="defaultValue">当转换失败时（例如传入字符串为null）返回的值，默认为null</param>
        /// <returns></returns>
#if NET20
        public static Dictionary<String, String> SplitToDictionary(String target, String itemDivChar, String keyValueDivChar, Dictionary<String, String> defaultValue)
#else
        public static Dictionary<String, String> SplitToDictionary(String target, String itemDivChar = ";", String keyValueDivChar = ",", Dictionary<String, String> defaultValue = null)
#endif
        {
            if (String.IsNullOrEmpty(target)) return null;
            String[] keyValuePairs = SplitEx(target, itemDivChar, StringSplitOptions.None);
            Dictionary<String, String> result = new Dictionary<String, String>();
            SplitToDictionaryEx(target, itemDivChar, keyValueDivChar, ref result);
            return result;
        }

        /// <summary>
        /// 把字符串拆分到dictionary中；
        /// 例如：“黎鉴,1;海清,2”   拆分到dictionary会有两项，前面为key，后面为value；项与项之间用分号隔开，key与value之间用逗号隔开
        /// </summary>
        /// <param name="target">等待拆分的目标字符串</param>
        /// <param name="itemDivChar">项与项之间的分隔符，默认为分号;</param>
        /// <param name="keyValueDivChar">key与value之间的分隔符，默认为逗号,</param>
        /// <param name="targetDictionary">转转的项保存到的目标字段中</param>
        /// <returns></returns>
        public static void SplitToDictionaryEx(String target, String itemDivChar, String keyValueDivChar, ref Dictionary<String, String> targetDictionary)
        {
            if (String.IsNullOrEmpty(target)) return;
            String[] keyValuePairs = SplitEx(target, itemDivChar, StringSplitOptions.None);
            for (Int32 i = 0; i < keyValueDivChar.Length; i++)
            {
                String[] tempKeyValuePair = SplitEx(keyValuePairs[i], keyValueDivChar, StringSplitOptions.None);
                if (!String.IsNullOrEmpty(tempKeyValuePair[0]))
                    targetDictionary.Add(tempKeyValuePair[0], tempKeyValuePair[1]);
            }
        }

        /// <summary>
        /// 在每一个单元的字符组后面加上某个字符，从左往右开始执行操作，字符串前后不加所指定的字符；
        /// 例如：字符串，fd3dkahfkakdh，希望每隔两个字符插入一个符号“：”，则可用此方法
        /// </summary>
        /// <param name="target"></param>
        /// <param name="insertValue">插入的文本</param>
        /// <param name="cellCount">间隔长度</param>
        /// <returns></returns>
#if NET20
        public static String InsertEx(String target, String insertValue, Int32 cellCount)
#else
        public static String InsertEx(this String target, String insertValue, Int32 cellCount)
#endif
        {
            if (String.IsNullOrEmpty(target)) return null;
            if (String.IsNullOrEmpty(insertValue)) return target;
            //如果单元字符个数小于0或者大于等于原字符的长度，则直接返回原值即可
            if (cellCount <= 0 || cellCount >= target.Length) return target;

            //如果单元字符数为1，那就是一个字符一组了
            if (target.Length == 1) return target;
            Char[] value = target.ToCharArray();

            StringBuilder builder = new StringBuilder();
            //计算分组数【注意：此处最后一个分组不用计算的，因为最后不用加指定字符】
            Int32 groupCount = value.Length / cellCount;
            for (Int32 i = 0; i < groupCount; i++)
            {
                Int32 start = i * cellCount;
                //最后一个分组，不用加指定字符
                if (i == groupCount - 1)
                {
                    builder.Append(target.Substring(start));
                    break;
                }
                //取最小的长度作为分割长度
                Int32 length = cellCount;
                builder.Append(target.Substring(start, length) + insertValue);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsNumber(String value)
        {
            try
            {
                Double result = 0.0;
                return Double.TryParse(value, out result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="target"></param>
        /// <returns>true：字符窜为空；false：字符串非空</returns>
#if NET20
        public static Boolean IsNullOrEmpty(String target)
#else
        public static Boolean IsNullOrEmpty(this String target)
#endif
        {
            return String.IsNullOrEmpty(target);
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="target"></param>
        /// <returns>true：字符串为非空；false：字符串为空</returns>
#if NET20
        public static Boolean IsNotNullOrEmpty(String target)
#else
        public static Boolean IsNotNullOrEmpty(this String target)
#endif
        {
            return !String.IsNullOrEmpty(target);
        }

        /// <summary>
        /// 移除所有空白的字符，例如：\n \t \r
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static String RemoveAllEmptyWords(String target)
        {
            if (!String.IsNullOrEmpty(target)) return target.Replace("\n", "").Replace("\t", "");
            return target;
        }

        /// <summary>
        /// 将html文本转化为 文本内容方法NoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public static String NoHTML(String htmlString)
        {
            //删除脚本   
            htmlString = Regex.Replace(htmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            htmlString = Regex.Replace(htmlString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"-->", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            htmlString = htmlString.Replace("<", "");
            htmlString = htmlString.Replace(">", "");
            htmlString = htmlString.Replace("/r/n", "");
            htmlString = htmlString.Replace("\n", "");
            htmlString = htmlString.Replace("\t", "");
            //返回去掉html标记的字符串
            return htmlString;
        }

        /// <summary>
        /// 格式化字符串；
        /// 默认支持{\n}换行符；
        /// </summary>
        /// <param name="target"></param>
        /// <param name="autoReplaceStandardEscapeSequence">是否自动替换标准的转义字符；默认为自动替换，会自动识别字符串中已有的转义字符（根据.NET的标准识别）</param>
        /// <param name="args"></param>
        /// <returns>格式化后的字符串</returns>
#if NET20
        public static String FormatEx(String target, Boolean autoReplaceStandardEscapeSequence = true, params Object[] args)
#else
        public static String FormatEx(this String target, Boolean autoReplaceStandardEscapeSequence = true, params Object[] args)
#endif
        {
            //自动转换转移字符
            if (autoReplaceStandardEscapeSequence)
            {
                String[] dateFormat = SubStringBetweenStr(target,"{", "}", false, false);
                String[] dateMappingString = { "yyyy", "MM", "dd", "HH", "mm", "ss", "ffff" };
                DateTime nowDate = DateTime.Now;
                for (Int32 i = 0; i < dateFormat.Length; i++)
                {
                    if (MyString.HasTargetStrContainsItem(dateFormat[i],dateMappingString, StringComparison.CurrentCulture))
                    {
                        try
                        {
                            target = target.Replace("{" + dateFormat[i] + "}", nowDate.ToString(dateFormat[i]));
                        }
                        catch (FormatException)
                        { }
                    }
                }
                target = target.Replace("{\\n}", Environment.NewLine); //换行符号
                target = target.Replace("{\\r}", "\r");  //回车
                target = target.Replace("{\\r\\n}", "\r\n");  //文本框的换行符
            }
            if (args != null && args.Length > 0)
                target = String.Format(target, args);
            return target;
        }

        /// <summary>
        /// 取标志字符的右边部分（从最后开始匹配标志字符）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="bIgnoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <param name="defaultValue">当target为NULL或者空串时，返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static String RightOfLast(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String RightOfLast(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.LastIndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(index + value.Length) : defaultValue;
        }

        /// <summary>
        /// 取标志字符的左边部分（从最后开始匹配标志字符）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="bIgnoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <returns></returns>
#if NET20
        public static String LeftOfLast(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String LeftOfLast(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.LastIndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(0, index) : defaultValue;
        }

        /// <summary>
        /// 取标志字符的左边部分
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="bIgnoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <returns></returns>
#if NET20
        public static String Left(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String Left(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.IndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(0, index) : defaultValue;
        }

        /// <summary>
        /// 获取左边Length长度的字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NET20
        public static String Left(String target, Int32 length)
#else
        public static String Left(this String target, Int32 length)
#endif
        {
            if (length <= 0) return "";
            if (length >= target.Length) return target;
            return target.Substring(0, length);
        }

        /// <summary>
        /// 获取右边Length长度的字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NET20
        public static String Right(String target, Int32 length)
#else
        public static String Right(this String target, Int32 length)
#endif
        {
            if (length <= 0) return "";
            if (length >= target.Length) return target;
            return target.Substring(target.Length - length, length);
        }

        /// <summary>
        /// 取标志字符的右边部分
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value">标志字符</param>
        /// <param name="bIgnoreCase">是否忽略大小写，默认为false，也就是默认是大小写敏感的</param>
        /// <returns></returns>
#if NET20
        public static String Right(String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#else
        public static String Right(this String target, String value, Boolean ignoreCase = false, String defaultValue = "")
#endif
        {
            if (String.IsNullOrEmpty(target)) return defaultValue;
            Int32 index = target.IndexOf(value,
                ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            return index >= 0 ? target.Substring(index + value.Length) : defaultValue;
        }

        /// <summary>
        /// 把字符串转换成byte数组
        /// </summary>
        /// <param name="target"></param>
        /// <param name="encoder">采用的编码方式，如果不传入，默认为utf8编码</param>
        /// <returns></returns>
#if NET20
        public static Byte[] ToByte(String target, Encoding encoder = null)
#else
        public static Byte[] ToByte(this String target, Encoding encoder = null)
#endif
        {
            if (encoder == null)
                encoder = Encoding.UTF8;
            Byte[] strBuffer = encoder.GetBytes(target);
            Byte[] buffer = new Byte[strBuffer.Length + 1];
            Buffer.BlockCopy(strBuffer, 0, buffer, 0, strBuffer.Length);
            buffer[strBuffer.Length] = 0;  //最后一个byte标识字符结束
            return buffer;
        }

        /// <summary>
        /// 把字符串转换成ascii码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Int32 ToAsciiCode(String target)
        {
            Byte[] charBytes = Encoding.ASCII.GetBytes(target);
            Int32 aWordIndex = 1;
            Int32.TryParse(Convert.ToString(charBytes[0]), out aWordIndex);
            return aWordIndex;
        }

        /// <summary>
        /// 把字符串转换成ascii码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Int32 ToAsciiCode(Char target)
        {
            Byte[] charBytes = Encoding.ASCII.GetBytes(new Char[] { target });
            Int32 aWordIndex = 1;
            Int32.TryParse(Convert.ToString(charBytes[0]), out aWordIndex);
            return aWordIndex;
        }

        /// <summary>
        /// 把字符转换成ascii码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Int32 ToAsciiCodeEx(Char target)
        {
            return Strings.Asc(target);
        }

        /// <summary>
        /// 把字符串转换成ascii码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Int32 ToAsciiCodeEx(String target)
        {
            return Strings.Asc(target);
        }

        /// <summary>
        /// 把Ascii数字转换成对应的字符（有效范围是0-255）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Char ToChar(Int32 value)
        {
            return Strings.Chr(value);
        }

        /// <summary>
        /// ascii码转成对应的字符串
        /// </summary>
        /// <param name="ascii"></param>
        /// <param name="defaultString">ascii码必须在0-255之间，转换失败后返回的值</param>
        /// <returns></returns>
#if NET20
        public static String RecoverFromAsciiCode(Int32 asciiCode, String defaultString)
#else
        public static String RecoverFromAsciiCode(Int32 asciiCode, String defaultString = "")
#endif
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                Byte[] bytes = new Byte[1] { Convert.ToByte(asciiCode) };
                return Encoding.ASCII.GetString(bytes);
            }
            else
            {
                return defaultString;
            }
        }

        /// <summary>
        /// 把字符格式化为整型
        /// 转换成功返回正确的值，转换失败则返回你传入的转换失败的返回值，如果不传入，则返回默认值（不会因为转换失败而抛出异常）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">转换失败时返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static Int32 ToInt32(String target, Int32 returnValueWhenParseError = default(Int32))
#else
        public static Int32 ToInt32(this String target, Int32 returnValueWhenParseError = default(Int32))
#endif
        {
            Int32 tempInt32 = default(Int32);
            if (Int32.TryParse(target, out tempInt32))
                return tempInt32;
            return returnValueWhenParseError;
        }

        /// <summary>
        /// 把字符格式化为整型
        /// 转换成功返回正确的值，转换失败则返回你传入的转换失败的返回值，如果不传入，则返回默认值（不会因为转换失败而抛出异常）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">转换失败时返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static Int64 ToInt64(String target, Int64 returnValueWhenParseError = default(Int64))
#else
        public static Int64 ToInt64(this String target, Int64 returnValueWhenParseError = default(Int64))
#endif
        {
            Int64 tempInt64 = default(Int64);
            if (Int64.TryParse(target, out tempInt64))
                return tempInt64;
            return returnValueWhenParseError;
        }

        /// <summary>
        /// 把字符格式化为双精度浮点型
        /// 转换成功返回正确的值，转换失败则返回你传入的转换失败的返回值，如果不传入，则返回默认值（不会因为转换失败而抛出异常）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">转换失败时返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static Double ToDouble(String target, Double returnValueWhenParseError = default(Double))
#else
        public static Double ToDouble(this String target, Double returnValueWhenParseError = default(Double))
#endif
        {
            Double tempDouble = default(Double);
            if (Double.TryParse(target, out tempDouble))
                return tempDouble;
            return returnValueWhenParseError;
        }

        /// <summary>
        /// 把字符格式化为单精度浮点型
        /// 转换成功返回正确的值，转换失败则返回你传入的转换失败的返回值，如果不传入，则返回默认值（不会因为转换失败而抛出异常）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">转换失败时返回的默认值</param>
        /// <returns></returns>
#if NET20
        public static Single ToSingle(String target, Single returnValueWhenParseError = default(Single))
#else
        public static Single ToSingle(this String target, Single returnValueWhenParseError = default(Single))
#endif
        {
            Single tempSingle = default(Single);
            if (Single.TryParse(target, out tempSingle))
                return tempSingle;
            return returnValueWhenParseError;
        }

        /// <summary>
        /// 把字符串转换成日期对象（不会因为转换失败而抛出异常）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="returnValueWhenParseError">默认为default(DateTime)</param>
        /// <returns></returns>
#if NET20
        public static DateTime ToDateTime(String target, DateTime returnValueWhenParseError = default(DateTime))
#else
        public static DateTime ToDateTime(this String target, DateTime returnValueWhenParseError = default(DateTime))
#endif
        {
            DateTime tempDate = default(DateTime);
            if (DateTime.TryParse(target, out tempDate))
                return tempDate;
            return returnValueWhenParseError;
        }

        /// <summary>
        /// 把字符串格式化成物理路径，也就是把反斜杠都转换成斜杠符
        /// </summary>
        /// <param name="path"></param>
        /// <returns>如果传入文件路径为null或者空串，则返回空串</returns>
#if NET20
        public static String ToPhysicalPath(String path)
#else
        public static String ToPhysicalPath(this String path)
#endif
        {
            if (String.IsNullOrEmpty(path)) return String.Empty;
            return path.Replace("/", "\\");
        }

        /// <summary>
        /// 把字符串数组转换成SQL语句中，where条件中的in所需要的字符串，转换成： 'a','b','c'的样式
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="isString">如果是字符串形式的，则每一项加上单撇号，否则不加（默认是加的）</param>
        /// <param name="removeEmptyItem">是否移除空串</param>
        /// <returns></returns>
        public static String ToSqlWhereInConditions(String[] inputs, Boolean isString = true,Boolean removeEmptyItem = true )
        {
            if (inputs == null || inputs.Length == 0) return "";
            StringBuilder sb = new StringBuilder();
            Int32 index = 0;
            for (Int32 i = 0; i < inputs.Length; i++)
            {
                if (removeEmptyItem && String.IsNullOrEmpty(inputs[i])) continue;
                if (index != 0) sb.Append(",");
                if (isString) sb.Append(String.Format("'{0}'", inputs[i]));
                else sb.Append(String.Format("{0}", inputs[i]));
                index++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 合并两个数组，并返回新的数组
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NET20
        public static String[] Append(String[] target, String[] value)
#else
        public static String[] Append(this String[] target, String[] value)
#endif
        {
            String[] result = new String[target.Length + value.Length];
            for (Int32 i = 0; i < target.Length; i++)
                result[i] = target[i];
            for (Int32 i = target.Length, j = 0; j < value.Length; j++, i++)
                result[i] = value[j];
            return result;
        }

        /// <summary>
        /// 判断字符串数组中是否含有某项是null或者Empty的;
        /// </summary>
        /// <param name="target"></param>
        /// <returns>如果含有Null或者Empty的项，则返回true；否则返回false；目标字符串数组为null返回false</returns>
#if NET20
        public static Boolean HasItemNullOrEmpty(String[] target)
#else
        public static Boolean HasItemNullOrEmpty(this String[] target)
#endif
        {
            if (target == null) return false;
            List<Int32> index = GetNullOrEmptyItems(target, true);
            return index != null && index.Count > 0;
        }

        /// <summary>
        /// 获取字符串数组中，为NULL或为空串的项的序号
        /// </summary>
        /// <param name="target"></param>
        /// <param name="onlyTopItem">标志是否仅只查找一项即可，true：只查找一项</param>
        /// <returns>如果含有Null或者Empty的项，则返回true；否则返回false；目标字符串数组为null返回false</returns>
#if NET20
        public static List<Int32> GetNullOrEmptyItems(String[] target, Boolean onlyTopItem = true)
#else
        public static List<Int32> GetNullOrEmptyItems(this String[] target, Boolean onlyTopItem = true)
#endif
        {
            if (target == null) return null;
            List<Int32> index = new List<Int32>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (String.IsNullOrEmpty(target[i]))
                {
                    index.Add(i);
                    if (onlyTopItem) return index;
                }
            }
            return index;
        }

        /// <summary>
        /// 从字符串数组中移除空白字符串项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="itemFormatFunc">判断是否为空的操作前，调用每一项字符串的回调方法</param>
        /// <returns></returns>
        public static String[] RemoveNullOrEmptyItems(String[] target, Func<String, String> itemFormatFunc)
        {
            if (target == null) return null;
            List<String> results = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                String itemStr = target[i];
                if (itemFormatFunc != null)
                    itemStr = itemFormatFunc(itemStr);
                if (!String.IsNullOrEmpty(itemStr))
                    results.Add(itemStr);
            }
            return results.ToArray();
        }

        /// <summary>
        /// 在字符串末尾开始处理，移除所有指定的字符串，如果最后不是该指定的字符，则按原样返回字符串
        /// </summary>
        /// <param name="target">需要处理的目标字符串</param>
        /// <param name="targetChar">需要去掉的字符串</param>
        /// <param name="trim">是否去掉字符串两边的空格</param>
        /// <returns></returns>
        public static String RemoveAllLastChar(String target, String targetChar, Boolean trim)
        {
            if (String.IsNullOrEmpty(target)) return target;
            if (trim) target = target.Trim();
            if (target.EndsWith(targetChar))
            {
                do
                {
                    target = target.Substring(0, target.Length - 1);
                } while (target.EndsWith(targetChar));
            }
            return target;
        }

        /// <summary>
        /// 移除最后一个指定的字符，如果最后不是该指定的字符，则按原样返回字符串
        /// </summary>
        /// <param name="target">需要处理的目标字符串</param>
        /// <param name="targetChar">需要去掉的字符串</param>
        /// <param name="trim">是否去掉字符串两边的空格</param>
        /// <returns></returns>
        public static String RemoveLastChar(String target, String targetChar, Boolean trim)
        {
            if (String.IsNullOrEmpty(target)) return target;
            if (trim) target = target.Trim();
            if (target.EndsWith(targetChar)) return target.Substring(0, target.Length - 1);
            return target;
        }

        /// <summary>
        /// 移除重复项
        /// </summary>
        /// <param name="target">目标字符串数组</param>
        /// <param name="bIgnoreCase">是否忽略大小写，true：忽略大小写</param>
        /// <returns></returns>
        public static String[] RemoveRepeatItems(String[] target, Boolean bIgnoreCase = false)
        {
            if (target == null) return null;
            List<String> results = new List<String>();
            Boolean hasSameItem = false;
            for (Int32 i = 0; i < target.Length; i++)
            {
                String itemStr = target[i];
                for (Int32 k = 0; k < results.Count; k++)
                {
                    if (String.Compare(itemStr, results[k], bIgnoreCase) == 0)
                    {
                        hasSameItem = true;
                        break;
                    }
                }
                if (!hasSameItem)
                {
                    //重置标识符
                    hasSameItem = false;
                    results.Add(itemStr);
                }
            }
            return results.ToArray();
        }

        /// <summary>
        /// 移除重复项
        /// </summary>
        /// <param name="target">目标字符串数组</param>
        /// <param name="comparison">自定字符串的比较规则（默认是忽略大小写）</param>
        /// <returns></returns>
        public static String[] RemoveRepeatItems(String[] target,StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (target == null) return null;
            List<String> results = new List<String>();
            Boolean hasSameItem = false;
            for (Int32 i = 0; i < target.Length; i++)
            {
                String itemStr = target[i];
                for (Int32 k = 0; k < results.Count; k++)
                {
                    if (String.Compare(itemStr, results[k], comparison) == 0)
                    {
                        hasSameItem = true;
                        break;
                    }
                }
                if (!hasSameItem)
                {
                    //重置标识符
                    hasSameItem = false;
                    results.Add(itemStr);
                }
            }
            return results.ToArray();
        }

        /// <summary>
        /// 移除重复项
        /// </summary>
        /// <param name="target">目标字符串数组</param>
        /// <param name="itemFormatFunc">在执行该方法的操作前，调用每一项字符串的回调方法</param>
        /// <param name="itemCompareFunc">自定义比较字符串的方法，该方法返回true则删除，返回false则不删除</param>
        /// <returns></returns>
        public static String[] RemoveRepeatItems(String[] target, Func<String, String> itemFormatFunc = null, Func<String, String, Boolean> itemCompareFunc = null)
        {
            if (target == null) return null;
            List<String> results = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                String itemStr = target[i];
                if (itemFormatFunc != null)
                    itemStr = itemFormatFunc(itemStr);
                if (itemCompareFunc != null)
                {
                    for (Int32 k = 0; k < results.Count; k++)
                    {
                        if (itemCompareFunc(results[k], itemStr))
                            results.Add(itemStr);
                    }
                }
                else
                {
                    if (!results.Contains(itemStr))
                        results.Add(itemStr);
                }
            }
            return results.ToArray();
        }

        /// <summary>
        /// 判断字符串是否包含指定字符数组中的项
        /// 例如：给定字符串是：abcdefg；给定数组元素为：a,b,c,ef,abc,ab,ad,aq
        /// 则满足条件的数组项是：a,b,c,ef,abc,ab
        /// </summary>
        /// <param name="target"></param>
        /// <param name="compareStrs">需要查找的字符串数组</param>
        /// <param name="stringComparison">默认是忽略大小写的</param>
        /// <returns>目标字符串为null或空串时返回false</returns>
#if NET20
        public static Boolean HasTargetStrContainsItem(String target,
                                                       String[] compareStrs,
                                                       StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
#else
        public static Boolean HasTargetStrContainsItem(this String target,
                                                       String[] compareStrs,
                                                       StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)

#endif
        {
            if (String.IsNullOrEmpty(target)) return false;
            List<String> temp = GetTargetStrContainsItems(target, compareStrs, true, stringComparison);
            return temp.Count > 0;
        }

        /// <summary>
        /// 判断某个字符串是否属于是字符串数组中某项的开始字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetArray"></param>
        /// <param name="stringComparison"></param>
        /// <returns>目标字符串为null或空串时返回false</returns>
#if NET20
            public static Boolean HasTargetStrStartWithItem(String target,
                                                        String[] targetArray,
                                                        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
#else
            public static Boolean HasTargetStrStartWithItem(this String target,
                                                        String[] targetArray,
                                                        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
#endif
        {
            if (String.IsNullOrEmpty(target)) return false;
            List<String> matchItem = GetTargetStrStartWithItems(target, targetArray, true, stringComparison);
            return matchItem.Count > 0;
        }

        /// <summary>
        /// 获取数组中是某个字符串的开始部分的数组项；
        /// 例如：给定字符串是：abcdefg；给定数组元素为：a,b,c,ef,abc,ab,ad
        /// 则满足条件的数组项是：a,abc,ab
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetArray"></param>
        /// <param name="onlyTopItem">只查找一项即可</param>
        /// <param name="stringComparison"></param>
        /// <returns>目标字符串为null或空串时返回null</returns>
#if NET20
            public static List<String> GetTargetStrStartWithItems(String target,
                                                                  String[] targetArray,
                                                                  Boolean onlyTopItem = true,
                                                                  StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
#else
            public static List<String> GetTargetStrStartWithItems(this String target,
                                                              String[] targetArray,
                                                              Boolean onlyTopItem = true,
                                                              StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
#endif
            {
            if (String.IsNullOrEmpty(target)) return null;
            List<String> matchItem = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (target.StartsWith(targetArray[i], stringComparison))
                {
                    matchItem.Add(targetArray[i]);
                    if (onlyTopItem) break;
                }
            }
            return matchItem;
        }

        /// <summary>
        /// 获取数组中所有属于某个字符串的子串的数组项
        /// 例如：给定字符串是：abcdefg；给定数组元素为：a,b,c,ef,abc,ab,ad,aq
        /// 则满足条件的数组项是：a,b,c,ef,abc,ab
        /// </summary>
        /// <param name="target"></param>
        /// <param name="compareStrs"></param>
        /// <param name="onlyTopItem">是否仅仅是获取第一项即退出循环；默认为false，获取所有符合条件的数组项</param>
        /// <returns>目标字符串为null或空串时返回null</returns>
#if NET20
            public static List<String> GetTargetStrContainsItems(String target,
                                                                 String[] compareStrs,
                                                                 Boolean onlyTopItem = false,
                                                                 StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#else
            public static List<String> GetTargetStrContainsItems(this String target,
                                                                 String[] compareStrs,
                                                                 Boolean onlyTopItem = false,
                                                                 StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)

#endif
            {
                if (String.IsNullOrEmpty(target)) return null;
                List<String> tempList = new List<String>();
                for (Int32 i = 0; i < compareStrs.Length; i++)
                {
                    if (target.IndexOf(compareStrs[i], comparison) >= 0)
                    {
                        tempList.Add(compareStrs[i]);
                        if (onlyTopItem) break;
                    }
                }
                return tempList;
            }

        /// <summary>
        /// 判断数组中的字符串是否有某项是以某个字符串开始的
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <returns>true表示包含有该项；false表示没有包含该项；目标字符串为null或空串时返回false</returns>
#if NET20
            public static Boolean HasItemStartWithTargetStr(String[] target,
                                                            String targetString,
                                                            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#else
            public static Boolean HasItemStartWithTargetStr(this String[] target,
                                                            String targetString,
                                                            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#endif
            {
                if (target == null) return false;
                List<String> matchItem = MyString.GetStartWithTargetStrItems(target, targetString, true, comparison);
                return matchItem != null && matchItem.Count > 0;
            }
        

        /// <summary>
        /// 判断数组中的字符串是否存在了某些项，其项是包含了指定字符串的
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <param name="comparison"></param>
        /// <returns>目标字符串数组为null或空串时返回false</returns>
#if NET20
        public static Boolean HasItemContainsTargetStr(String[] target,
                                                           String targetString,
                                                           StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#else
        public static Boolean HasItemContainsTargetStr(this String[] target,
                                                       String targetString,
                                                       StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#endif
        {
            if (target == null) return false;
            List<String> matchItem = MyString.GetContainsTargetStrItems(target, targetString, true, comparison);
            return matchItem != null && matchItem.Count > 0;
        }

        /// <summary>
        /// 判断数组中的字符串是否有等于某个字符串的项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static Boolean HasItemEqualTargetStr(String[] target,
                                                    String targetString,
                                                    StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (target == null) return false;
            List<String> matchItem = MyString.GetEqualTargetStrItems(target, targetString, true, comparison);
            return matchItem != null && matchItem.Count > 0;
        }

        /// <summary>
        /// 获取数组中的字符串等于某个字符串的所有项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <param name="onlyTopItem"></param>
        /// <param name="comparison"></param>
        /// <returns>目标字符串数组为null或空串时返回false</returns>
        public static List<String> GetEqualTargetStrItems(String[] target,
                                                          String targetString,
                                                          Boolean onlyTopItem = true,
                                                          StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (target == null) return null;
            List<String> matchItem = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (String.Compare(target[i], targetString, comparison) == 0)
                {
                    matchItem.Add(target[i]);
                    if (onlyTopItem) break;
                }
            }
            return matchItem;
        }

        /// <summary>
        /// 获取数组中的字符串包含了某个字符串的所有项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <param name="onlyTopItem"></param>
        /// <param name="comparison"></param>
        /// <returns>目标字符串数组为null或空串时返回false</returns>
#if NET20
        public static List<String> GetContainsTargetStrItems(String[] target,
                                                             String targetString,
                                                             Boolean onlyTopItem = true,
                                                             StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#else
        public static List<String> GetContainsTargetStrItems(this String[] target, 
                                                             String targetString, 
                                                             Boolean onlyTopItem = true,
                                                             StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#endif
        {
            if (target == null) return null;
            List<String> matchItem = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (target[i].IndexOf(targetString, comparison) >= 0)
                {
                    matchItem.Add(target[i]);
                    if (onlyTopItem) break;
                }
            }
            return matchItem;
        }

        /// <summary>
        /// 根据字符串，获取字符串中所有的目标子串（如果target或者subStr为null或者空串，则返回NULL；获取不到指定的空串，也返回空的List对象）
        /// 这个方法是不区分大小写的，感觉区分大小写没意义。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="subStr"></param>
        /// <returns></returns>
        public static List<String> GetAllSubString(String target, String subStr)
        {
            if (String.IsNullOrEmpty(target) || String.IsNullOrEmpty(subStr)) return null;
            Int32 index = 0;
            Int32 startIndex = 0;
            List<String> subs = new List<String>();
            while (startIndex < target.Length && (index = target.IndexOf(subStr, startIndex, StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                subs.Add(target.Substring(index, subStr.Length));
                startIndex = index + subStr.Length;
            }
            return subs;
        }

        /// <summary>
        /// 获取数组中的字符串是以某个字符串开始的项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetString"></param>
        /// <param name="onlyTopItem">仅仅返回首条记录即可</param>
        /// <param name="comparison"></param>
        /// <returns>目标字符串数组为null或空串时返回false</returns>
#if NET20
        public static List<String> GetStartWithTargetStrItems(String[] target,
                                                              String targetString,
                                                              Boolean onlyTopItem = true,
                                                              StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#else
        public static List<String> GetStartWithTargetStrItems(this String[] target, 
                                                              String targetString, 
                                                              Boolean onlyTopItem = true,
                                                              StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
#endif
        {
            if (target == null) return null;
            List<String> matchItem = new List<String>();
            for (Int32 i = 0; i < target.Length; i++)
            {
                if (target[i].StartsWith(targetString, comparison))
                {
                    matchItem.Add(target[i]);
                    if (onlyTopItem) break;
                }
            }
            return matchItem;
        }

        #region 剪短字符串

        /// <summary>
        /// 截断字符串，不考虑中英文编码，并去除非法字符
        /// </summary>
        /// <param name="txt">要截断的字符串</param>
        /// <param name="lenght">要截取的长度</param>
        /// <returns></returns>
        public static String CutAndCheckStr(String txt, Int32 lenght)
        {
            if (txt.Length > lenght)
            {
                return txt.Substring(0, lenght);
            }
            else
            {
                return txt;
            }
        }

        /// <summary>
        /// 截断字符串，考虑中英文编码，中文是英文的两倍宽度
        /// </summary>
        /// <param name="txt">要截断的字符串</param>
        /// <param name="lenght">要截取的长度</param>
        /// <returns></returns>
        public static String Cut(String txt, Int32 lenght)
        {
            //取得自定义长度的字符串  中文123456789
            String outputtext = "";
            if (txt.Length > lenght)
            {
                //int tempnum = 0;
                Int32 tempnum1 = 0; //英文编码长度总计
                Int32 tempnum2 = 0; //中文编码长度总计
                Byte[] byitem = System.Text.ASCIIEncoding.ASCII.GetBytes(txt);
                for (Int32 i = 0; i < txt.Length; i++)
                {
                    if ((Int32)byitem[i] != 63) tempnum1++; else tempnum2++;
                    if (tempnum2 * 2 + tempnum1 >= lenght * 2) break;
                }
                outputtext = txt.Substring(0, tempnum2 + tempnum1);
            }
            else
            {
                outputtext = txt;
            }
            return outputtext;
        }

        /// <summary>
        /// 截断字符串，考虑中英文编码，中文是英文的两倍宽度，并在末尾添加字符串
        /// </summary>
        /// <param name="txt">要截断的字符串</param>
        /// <param name="lenght">要截取的长度</param>
        /// <param name="endstr">要在末尾拼接上的字符串</param>
        /// <returns></returns>
        public static String Cut(String txt, Int32 lenght, String endstr)
        {
            //取得自定义长度的字符串  中文123456789
            String outputtext = "";
            if (txt.Length > lenght)
            {
                //int tempnum = 0;
                Int32 tempnum1 = 0;
                Int32 tempnum2 = 0;
                Byte[] byitem = System.Text.ASCIIEncoding.ASCII.GetBytes(txt);
                for (Int32 i = 0; i < txt.Length; i++)
                {
                    if ((Int32)byitem[i] != 63) tempnum1++; else tempnum2++;
                    if (tempnum2 * 2 + tempnum1 >= lenght * 2) break;
                }
                outputtext = txt.Substring(0, tempnum2 + tempnum1);
            }
            else
            {
                outputtext = txt;
            }
            return outputtext += endstr;
        }

        #endregion

        /// <summary>
        /// 把内存留读成16进制字符串
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static String MemoryToHexString(MemoryStream ms)
        {
            if (ms == null) return String.Empty;
            return ByteArrayToHexString(ms.ToArray());
        }

        /// <summary>
        /// byte数组转换为16进制字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static String ByteArrayToHexString(Byte[] byteArray)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in byteArray)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 把16进制字符串转换为byte数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static Byte[] HexStringToByteArray(String hexString)
        {
            if (String.IsNullOrEmpty(hexString)) return null;
            var inputByteArray = new byte[hexString.Length / 2];
            for (var x = 0; x < hexString.Length / 2; x++)
            {
                var i = (Convert.ToInt32(hexString.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            return inputByteArray;
        }
    }
}
