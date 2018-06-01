using System;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.OleDB2
{
    public static class SqlHelper
    {
        /// <summary> 
        /// 格式化SQL查询语句的变量 
        /// </summary> 
        /// <param name="vValue">变量值</param> 
        /// <param name="ValueType">值类型</param> 
        /// <param name="databaseType">数据库类型</param> 
        /// <returns></returns> 
        public static string FormatSqlValue(object vValue, Type ValueType, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            if (vValue == null) return "NULL";

            string sValue = vValue.ToString();
            string sType = ValueType.Name.ToLower();

            if (sType.IndexOf(".") != -1) sType = MyString.Right(sType, ".");

            //字符串 
            if (sType == "string") return "'" + sValue.Replace("'", "''") + "'";

            //时间日期 
            if (sType == "datetime")
            {
                sValue = MyDatetime.Format(sValue);
                if (!MyDatetime.IsValidateDateString(sValue)) return "NULL";
                switch (databaseType)
                {
                    case DatabaseType.Access:
                        return "#" + sValue + "#";
                    case DatabaseType.Oracle11g:
                    case DatabaseType.Oracle10g:
                    case DatabaseType.Oracle9i:
                        return SqlFuncToDate(sValue, "");
                    default:
                        return "'" + sValue + "'";
                }
            }

            //GUID 
            if (sType == "guid")
            {
                switch (databaseType)
                {
                    case DatabaseType.SqlServer2005:
                    case DatabaseType.SqlServer2000:
                        return "'" + sValue.Replace("{", "").Replace("}", "") + "'";
                    default:
                        return "'" + sValue + "'";
                }
            }

            if (sValue == "") return "NULL";
            else return sValue;
        }

        /// <summary> 
        /// 不同类型的RDB格式化日期的不同方法 
        /// </summary> 
        /// <param name="sDateValue">需要转换为日期的数值</param> 
        /// <param name="sFormat">格式化格式</param> 
        /// <returns></returns> 
        public static string SqlFuncToDate(string sDateValue, string sFormat = "yyyy-mm-dd hh24:mi:ss",DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "Cast(" + sDateValue + " AS datetime)";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    if (sFormat == "")
                    {
                        if (sDateValue.IndexOf("-") >= 0)
                        {
                            sFormat = "yyyy-mm-dd";
                        }

                        if (sDateValue.IndexOf(":") >= 0)
                        {
                            if (sFormat != "")
                            {
                                sFormat += " ";
                            }
                            sFormat += "hh24:mi:ss";
                        }
                    }
                    return "TO_DATE(" + sDateValue + ", '" + sFormat + "')";
                case DatabaseType.Access:
                    return "CDate(" + sDateValue + ")";
                default:
                    return "";
            }
        }

        /// <summary> 
        /// 对不同类型，返回求字串左边n个字符的SQL函数写法 
        /// </summary> 
        /// <param name="sVal">需要运算的字串表达式，此表达式必须是当前RDB类型的正确SQL写法。如：“[Code]”、“'2233001'”、“[name]+'-'+[zone]”</param> 
        /// <param name="iLeftCount">需要求左边子串的字符数</param> 
        /// <returns></returns> 
        public static string SqlFuncStrLeft(string sVal, int iLeftCount, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "Left(" + sVal + ", " + iLeftCount + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "SUBSTR(" + sVal + ", 1, " + iLeftCount + ")";
                case DatabaseType.Access:
                    return "SUBSTR(" + sVal + ", 1, " + iLeftCount + ")";
                default:
                    return "";
            }
        }

        /// <summary> 
        /// 对不同类型，返回求字串右边n个字符的SQL函数写法 
        /// </summary> 
        /// <param name="sVal">需要运算的字串表达式，此表达式必须是当前RDB类型的正确SQL写法。如：“[Code]”、“'2233001'”、“[name]+'-'+[zone]”</param> 
        /// <param name="iLeftCount">需要求右边子串的字符数</param> 
        /// <returns></returns> 
        public static string SqlFuncStrRight(string sVal, int iRightCount, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "Right(" + sVal + ", " + iRightCount + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    iRightCount -= 1;
                    return "SUBSTR(" + sVal + ", LENGTH(" + sVal + ")-" + iRightCount.ToString() + ")";
                case DatabaseType.Access:
                    iRightCount -= 1;
                    return "SUBSTR(" + sVal + ", LENGTH(" + sVal + ")-" + iRightCount.ToString() + ")";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 对不同类型，返回求字串求子串的写法
        /// </summary>
        /// <param name="sVal"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public static string SqlFuncSubStr(string sVal, int iStartIndex, int iCount, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "SUBSTRING(" + sVal + "," + iStartIndex + "," + iCount + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "SUBSTR(" + sVal + "," + iStartIndex + "," + iCount + ")";
                default:
                    return "";
            }
        }

        /// <summary> 
        /// 对不同类型，返回求ASCII码转为字符的SQL函数写法 
        /// </summary> 
        /// <param name="iAscii">需要转化为字符的数字</param> 
        /// <returns></returns> 
        public static string SqlFuncChar(int iAscii, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "Char(" + iAscii + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "Chr(" + iAscii + ")";
                case DatabaseType.Access:
                    return "Chr(" + iAscii + ")";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 将表达式值转化为字串的不同数据库的SQL串写法
        /// </summary>
        /// <param name="sConst"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SqlToString(object sConst, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return string.Format("''{0}({1})", SqlHelper.SqlFuncStradd(databaseType), sConst);
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "TO_CHAR(" + sConst + ")";
                case DatabaseType.Access:
                    return "CStr(" + sConst + ")";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 将表达式值转化为数字的不同数据库的SQL串写法
        /// </summary>
        /// <param name="sConst"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SqlFuncToNumber(object sConst, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return string.Format("CONVERT(DOUBLE, {0})", sConst);
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "TO_NUMBER(" + sConst + ")";
                default:
                    return Convert.ToString(sConst);
            }
        }

        /// <summary> 
        /// 对不同类型，返回转为小写字符的SQL函数写法 
        /// </summary> 
        /// <param name="sSrc">需要转化为小写的值(字符串或者一个字段的名称)</param> 
        /// <returns></returns> 
        public static string SqlFuncLower(string sSrc, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "LOWER(" + sSrc + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "LOWER(" + sSrc + ")";
                case DatabaseType.Access:
                    return "LCase(" + sSrc + ")";
                default:
                    return "";
            }
        }

        /// <summary> 
        /// 不同类型数据库的Instr的不同写法 
        /// </summary> 
        /// <param name="sStr"></param> 
        /// <param name="sFind"></param> 
        /// <returns></returns> 
        public static string SqlFuncInstr(string sStr, string sFind, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "CharIndex(" + sFind + ", " + sStr + ")";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "INSTR(" + sStr + ", " + sFind + ", 1)";
                case DatabaseType.Access:
                    return "InStr(1, " + sStr + ", " + sFind + ")";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 不同类型数据库的Instr的不同写法 
        /// </summary>
        /// <param name="sColumnName"></param>
        /// <param name="sFindStr"></param>
        /// <param name="sSeperator"></param>
        /// <returns></returns>
        public static string SqlFuncInstrEx(string sColumnName, string sFindStr, string sSeperator, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            sFindStr = string.Format("'{0}{1}{0}'", sSeperator, sFindStr);
            sColumnName = string.Format("'{0}'{1}{2}{1}'{0}'", sSeperator, SqlHelper.SqlFuncStradd(databaseType), sColumnName);
            return SqlHelper.SqlFuncInstr(sColumnName, sFindStr, databaseType);
        }

        /// <summary> 
        /// '参数化查询的占位符，都支持“?” 
        /// </summary> 
        /// <returns></returns> 
        public static string QueryHolder()
        {
            return "?";
        }

        /// <summary> 
        /// 返回不同RDB类型的Like操作符所能识别的配匹单字符的符号 
        /// </summary> 
        /// <returns></returns> 
        public static string SqlLikeChar_Single()
        {
            return "_";
        }

        /// <summary> 
        /// 返回不同RDB类型的Like操作符所能识别的配匹任意字符的符号 
        /// </summary> 
        /// <returns></returns> 
        public static string SqlLikeChar_Multi()
        {
            return "%";
        }

        /// <summary> 
        /// 不同RDB类型的字符串连接符的写法 
        /// </summary> 
        /// <returns></returns> 
        public static string SqlFuncStradd(DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer2000:
                case DatabaseType.SqlServer2005:
                    return "+";
                case DatabaseType.Oracle9i:
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                    return "||";
                case DatabaseType.Access:
                    return "+";
                default:
                    return "+";
            }
        }

        /// <summary>
        /// 格式化NULL值, 是NULL的话转化为指定的值
        /// </summary>
        /// <param name="sColumn"></param>
        /// <param name="sNullValue"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SqlFormatNull(string sColumn, object sNullValue, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                case DatabaseType.Oracle9i:
                    return string.Format("NVL({0}, {1})", sColumn, sNullValue);
                default:
                    return string.Format("ISNULL({0}, {1})", sColumn, sNullValue);
            }
        }

        /// <summary>
        /// 判断字符串是否为空的写法
        /// </summary>
        /// <param name="sColumnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object SqlIsNullOrEmpty(string sColumnName, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            switch (databaseType)
            {
                case DatabaseType.Oracle10g:
                case DatabaseType.Oracle11g:
                case DatabaseType.Oracle9i:
                    return string.Format("{0} IS NULL", sColumnName);
                default:
                    return string.Format("{0} IS NULL OR {0}=''", sColumnName);

            }
        }

        /// <summary>
        /// 因为 WHERE CODE IN（1, 2, 3, ...）最多支持1000个值，为了解决这个问题，特设置本函数
        /// </summary>
        /// <param name="sColumnName">搜索列的名称</param>
        /// <param name="sValues">用来搜索的值</param>
        /// <param name="bIsNumberColumn">该列是不是数字</param>
        /// <param name="sValueDivChar">sValues以什么符号隔开各个值</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MakeInWhere(string sColumnName, string sValues, bool bIsNumberColumn, string sValueDivChar = ";")
        {
            string sWhere = "";
            string[] aValues = MyString.SplitEx(sValues, sValueDivChar);
            string sDiv = bIsNumberColumn ? "," : "','";
            string sTemp = "";
            for (int i = 0; i <= aValues.Length - 1; i++)
            {
                sTemp = MyString.Connect(sTemp, aValues[i], sDiv);
                if ((i + 1) % 1000 == 0)
                {
                    if (sWhere.EndsWith(")"))
                    {
                        sWhere += " OR ";
                    }
                    if (bIsNumberColumn)
                    {
                        sWhere += sColumnName + " IN(" + sTemp + ")";
                    }
                    else
                    {
                        sWhere += sColumnName + " IN('" + sTemp + "')";
                    }
                    sTemp = "";
                }
            }

            if (sTemp != "")
            {
                if (sWhere.EndsWith(")"))
                {
                    sWhere += " OR ";
                }
                if (bIsNumberColumn)
                {
                    sWhere += sColumnName + " IN(" + sTemp + ")";
                }
                else
                {
                    sWhere += sColumnName + " IN('" + sTemp + "')";
                }
            }

            return sWhere;
        }

        /// <summary>
        /// SQL字符串处理
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static string SqlFuncNested(string sSQL, DatabaseType databaseType = DatabaseType.Oracle10g)
        {
            if (databaseType == DatabaseType.SqlServer2000  || databaseType == DatabaseType.SqlServer2005)
            {
                string sSqlNested = "";
                string sSQLEx = sSQL;
                sSQL = "";
                string[] aa = sSQLEx.Split('\'');
                for (int n = 0; n <= aa.Length - 1; n++)
                {
                    if ((n & 1) == 0)
                    {
                        if (sSQL == "") sSQL = aa[n];
                        else sSQL = sSQL + "|+-*/||/*-+|" + aa[n];
                    }
                }
                sSQL = sSQL.Replace("COUNT(*)", "CONVERT(VARCHAR,COUNT(*))");
                sSQL = sSQL.Replace("(", " ( ");
                sSQL = sSQL.Replace(")", " ) ");
                sSQL = sSQL.Trim(' ');
                while (sSQL.IndexOf("  ") != -1)
                {
                    sSQL = sSQL.Replace("  ", " ");
                }
                sSQL = sSQL.Replace(" from ", " FROM ");
                if (sSQL.IndexOf(" FROM (") != -1)
                {
                    sSQL = sSQL.Replace(" where ", " WHERE ");
                    sSQL = sSQL.Replace("select ", "SELECT ");
                    sSQL = sSQL.Replace("group by ", "GROUP BY ");
                    sSQL = sSQL.Replace("order by ", "ORDER BY ");
                    sSQL = sSQL.Replace("select ", "SELECT ");
                    string sGroup = "";
                    string sOrder = "";
                    if (sSQL.IndexOf("GROUP BY ") != -1)
                    {
                        sGroup = " GROUP BY " + MyString.Right(sSQL, "GROUP BY ");
                        sSQL = MyString.Left(sSQL, "GROUP BY ");
                    }
                    if (sSQL.IndexOf("ORDER BY ") != -1)
                    {
                        sOrder = " ORDER BY " + MyString.Right(sSQL, "ORDER BY ");
                        sSQL = MyString.Left(sSQL, "ORDER BY ");
                    }
                    string[] aSQL = MyString.SplitEx(sSQL, " FROM (");
                    string[] aSQL1 = aSQL[aSQL.Length - 1].Split(')');
                    string sTableName = "";
                    if (aSQL[aSQL.Length - 1].IndexOf(" FROM ") != -1)
                    {
                        string[] aaa = MyString.SplitEx(aSQL[aSQL.Length - 1], " FROM ");
                        string[] aaa1 = aaa[1].Trim(' ').Split(' ');
                        sTableName = aaa1[0];
                    }
                    else
                    {
                        string[] aaa1 = aSQL[aSQL.Length - 1].Trim(' ').Split(' ');
                        sTableName = aaa1[0];
                    }

                    int i = 0;
                    int j = aSQL.Length - 1;
                    while ((i < aSQL1.Length & j >= 0))
                    {
                        string ss = "";
                        string[] aaa = aSQL1[i].Split('(');
                        for (int k = 0; k <= aaa.Length - 1; k++)
                        {
                            if (ss == "")
                            {
                                ss = aSQL1[i];
                            }
                            else
                            {
                                ss = ss + ")" + aSQL1[i + k + 1];
                            }
                        }
                        if (sSqlNested == "")
                        {
                            sSqlNested = ss;
                            if (sSqlNested.IndexOf("SELECT ") == -1)
                            {
                                sSqlNested = "SELECT * FROM " + sSqlNested;
                            }
                        }
                        else
                        {
                            if (ss.IndexOf(" WHERE ") != -1)
                            {
                                sSqlNested = aSQL[j] + " FROM " + sTableName + ss + " AND EXISTS(" + sSqlNested + ")";
                            }
                            else
                            {
                                sSqlNested = aSQL[j] + " FROM " + sTableName + ss + " WHERE EXISTS(" + sSqlNested + ")";
                            }
                        }
                        i = i + aaa.Length;
                        j = j - 1;
                    }
                    sSQL = sSqlNested + sGroup + sOrder;
                }
                string[] aa1 = MyString.SplitEx(sSQL, "|+-*/||/*-+|");
                for (int m = 0; m <= aa1.Length - 1; m++)
                {
                    if (m * 2 + 1 <= aa.Length - 1)
                    {
                        if (m == 0)
                        {
                            sSQL = aa1[m] + "'" + aa[m * 2 + 1];
                        }
                        else
                        {
                            sSQL = sSQL + "'" + aa1[m] + "'" + aa[m * 2 + 1];
                        }
                    }
                    else
                    {
                        if (m == 0)
                        {
                            sSQL = aa1[m];
                        }
                        else
                        {
                            sSQL = sSQL + "'" + aa1[m];
                        }
                    }
                }
            }
            return sSQL;
        }
    }
}
