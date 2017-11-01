using System;
using System.Globalization;
using Microsoft.VisualBasic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyDatetime
    {
        #region "属性"

        /// <summary>
        /// 获取系统当前的年月日时间（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public static String NowTimeyyyyMMddHHmmss
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 获取系统当前的年月日时间(yyyy-MM-dd)
        /// </summary>
        public static String NowTimeyyyyMMdd
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 获取系统当前的年月时间(yyyy-MM)
        /// </summary>
        public static String NowTimeyyyyMM
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM");
            }
        }

        /// <summary>
        /// 求当前时间是今天的第几个毫秒
        /// </summary>
        public static long NowMillisecond
        {
            get
            {
                DateTime oNow = DateTime.Now;
                return oNow.Hour * 60 * 60 * 1000 + oNow.Minute * 60 * 1000 + oNow.Second * 1000 + oNow.Millisecond;
            }
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 功能: 格式化时间长度(考虑工作日)
        /// </summary>
        /// <param name="iTimeLength">时间长度(秒钟数)</param>
        /// <param name="iWorkDayLength">一天是否要以24小时来计算</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string FormatTimeLengthStr(long iTimeLength, int iWorkDayLength = 24)
        {
            int iDay = 0;
            int iHour = 0;
            int iMinute = 0;
            int iSecond = 0;

            if (iTimeLength == 0) return "";

            String resultStr = "";

            if (iTimeLength < 0) resultStr = "-";
            else resultStr = "";

            //转换为正数
            if (iTimeLength < 0) iTimeLength = 0 - iTimeLength;

            //秒钟
            iSecond = (Int32)iTimeLength % 60;
            iTimeLength = iTimeLength - iSecond;
            if (iTimeLength <= 0)
            {
                goto TheEnd;
            }

            //分钟
            iTimeLength = iTimeLength / 60;
            iMinute = (Int32)iTimeLength % 60;
            iTimeLength = iTimeLength - iMinute;
            if (iTimeLength <= 0)
            {
                goto TheEnd;
            }

            //小时
            iTimeLength = iTimeLength / 60;
            iHour = (Int32)iTimeLength % iWorkDayLength;
            iTimeLength = iTimeLength - iHour;
            if (iTimeLength <= 0)
            {
                goto TheEnd;
            }

            //天
            iDay = (Int32)iTimeLength / iWorkDayLength;
        TheEnd:
            if (iDay > 0) resultStr = resultStr + iDay + "天";
            if (iHour > 0) resultStr = resultStr + iHour + "小时";

            if (iMinute > 0)
            {
                if (resultStr == "") resultStr = resultStr + iMinute + "分钟";
                else resultStr = resultStr + iMinute + "分";
            }

            if (iSecond > 0)
            {
                if (resultStr == "") resultStr = resultStr + iSecond + "秒钟";
                else resultStr = resultStr + iSecond + "秒";
            }
            return resultStr;
        }

        /// <summary>
        /// 求sTime1与sTime2之间，相差多少小时(注意：计算方式是sTime1减去sTime2，所以如果sTime2较大，则为负数)
        /// </summary>
        /// <param name="sTime1"></param>
        /// <param name="sTime2">如果为空,则取当前时间</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double CalculateTimeSpan_Hour(string sTime1, string sTime2 = "")
        {
            try
            {
                DateTime oTime1 = DateTime.Parse(sTime1);
                DateTime oTime2;
                if (string.IsNullOrEmpty(sTime2)) oTime2 = DateTime.Now;
                else oTime2 = DateTime.Parse(sTime2);
                TimeSpan oTimeSpan = oTime1 - oTime2;
                return oTimeSpan.TotalHours;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取时间日期的日期部分
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDatePart(string sDateTime)
        {
            try
            {
                DateTime oDateTime = DateTime.Parse(sDateTime);
                return oDateTime.ToString("yyyy-MM");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取本周周日的日期
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetDateOfSundayInCurWeek()
        {
            DateTime oDateTime = DateTime.Now.Date;
            oDateTime = oDateTime.AddDays(0 - Microsoft.VisualBasic.Conversion.Val(oDateTime.DayOfWeek));
            return oDateTime;
        }

        /// <summary>
        /// 获取本周周六的日期
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetDateOfSaturdayInCurWeek()
        {
            DateTime oDateTime = DateTime.Now.Date;
            oDateTime = oDateTime.AddDays(6 - Conversion.Val(oDateTime.DayOfWeek));
            return oDateTime;
        }

        /// <summary>
        /// 判断字符串是否属于日期格式
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public static bool IsValidateDateString(string sDate)
        {
            DateTime date;
            return System.DateTime.TryParse(sDate, out date);
        }

        /// <summary>
        /// 计算时间部分的值（转换成秒）
        /// </summary>
        /// <param name="sTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int TimePartValue(String sTime)
        {
            try
            {
                sTime = sTime.Trim();
                if (sTime.IndexOf(" ") != -1) sTime = MyString.Right(sTime, " ");
                string[] aParts = MyString.SplitEx(sTime, ":");
                return Convert.ToInt32(aParts[0]) * 60 * 60 + Convert.ToInt32(aParts[1]) * 60 + Convert.ToInt32(aParts[2]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 时间比较: 如果sDateTime1>sDateTime2,返回1; 如果sDateTime1=sDateTime2: 返回0; 如果sDateTime1小于sDateTime2: 返回-1
        /// </summary>
        /// <param name="sDateTime1"></param>
        /// <param name="sDateTime2"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32 TimeCompare(String sDateTime1, String sDateTime2)
        {
            try
            {
                int iResult;
                if (sDateTime1 == "" | sDateTime2 == "")
                {
                    if (sDateTime1 == "" & sDateTime2 == "") iResult = 0;
                    else iResult = -1;
                }
                else
                {
                    sDateTime1 = Strings.Replace(sDateTime1, "年", "-");
                    sDateTime1 = Strings.Replace(sDateTime1, "月", "-");
                    sDateTime1 = Strings.Replace(sDateTime1, "日", "");
                    sDateTime1 = Strings.Replace(sDateTime1, "号", "");

                    sDateTime2 = Strings.Replace(sDateTime2, "年", "-");
                    sDateTime2 = Strings.Replace(sDateTime2, "月", "-");
                    sDateTime2 = Strings.Replace(sDateTime2, "日", "");
                    sDateTime2 = Strings.Replace(sDateTime2, "号", "");

                    string[] aTemp;

                    if (Strings.InStr(sDateTime1, " ") == 0)
                    {
                        //处理只精确到月的日期
                        aTemp = Microsoft.VisualBasic.Strings.Split(sDateTime1, "-");
                        if (Information.UBound(aTemp) == 0)
                        {
                            sDateTime1 = sDateTime1 + "-01-01";
                        }
                        else if (Information.UBound(aTemp) == 1)
                        {
                            sDateTime1 = sDateTime1 + "-01";
                        }
                        sDateTime1 = sDateTime1 + " 00:00:00";
                    }
                    if (Strings.InStr(sDateTime2, " ") == 0)
                    {
                        //处理只精确到月的日期
                        aTemp = Strings.Split(sDateTime2, "-");
                        if (Information.UBound(aTemp) == 0)
                        {
                            sDateTime2 = sDateTime2 + "-01-01";
                        }
                        else if (Information.UBound(aTemp) == 1)
                        {
                            sDateTime2 = sDateTime2 + "-01";
                        }
                        sDateTime2 = sDateTime2 + " 00:00:00";
                    }

                    DateTime oDate1 = DateTime.Parse(sDateTime1);
                    DateTime oDate2 = DateTime.Parse(sDateTime2);

                    iResult = System.DateTime.Compare(oDate1, oDate2);
                }
                return iResult;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog("TimeCompare()发生错误：" + ex.Message);
                MyLog.MakeLog("sDateTime1=" + sDateTime1);
                MyLog.MakeLog("sDateTime2=" + sDateTime2);
            }
            return 0;
        }

        /// <summary>
        /// 比较两个日期变量的日期是否一致，也就是是否同一天（不算时分秒，只比较到天）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="compareTarget"></param>
        /// <returns></returns>
#if NET20
        public static Boolean EqualsEx(DateTime target, DateTime compareTarget)
#else
        public static Boolean EqualsEx(this DateTime target, DateTime compareTarget)
#endif
        {
            return target.ToString("yyyy-MM-dd") == compareTarget.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 比较两个日期变量的日期是否一致；比较的格式由用户自定义
        /// </summary>
        /// <param name="target"></param>
        /// <param name="compareTarget"></param>
        /// <param name="format">比较的日期格式，例如：可以选择比较是否同一个小时；同一分钟；同一天等需求</param>
        /// <returns></returns>
#if NET20
        public static Boolean EqualsEx(DateTime target, DateTime compareTarget, String format)
#else
        public static Boolean EqualsEx(this DateTime target, DateTime compareTarget, String format)
#endif
        {
            return target.ToString(format) == compareTarget.ToString(format);
        }

        /// <summary>
        /// 把日期格式化成字符串
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String Format(String dateString, String format = "yyyy-MM-dd")
        {
            DateTime date = new DateTime();
            DateTime.TryParse(dateString, out date);
            return date.ToString(format);
        }

        /// <summary>
        /// 把日期格式化成字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="format"></param>
        /// <returns></returns>
#if NET20
        public static String Format(DateTime target, String format = "yyyy-MM-dd")
#else
        public static String Format(this DateTime target, String format = "yyyy-MM-dd")
#endif
        {
            return target.ToString(format);
        }

        /// <summary>
        /// 把日期格式化到天，例如：2008-08-08
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String FormatToDay(DateTime target)
#else
        public static String FormatToDay(this DateTime target)
#endif
        {
            return MyDatetime.Format(target);
        }

        /// <summary>
        /// 把日期格式化到月份，例如：2008-08
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String FormatToMonth(DateTime target)
#else
        public static String FormatToMonth(this DateTime target)
#endif
        {
            return target.ToString("yyyy-MM");
        }

        /// <summary>
        /// 把日期格式化到秒，例如：2012-07-20 8:25:29 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String FormatToSecond(DateTime target)
#else
        public static String FormatToSecond(this DateTime target)
#endif
        {
            return target.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取当前日期属于某年的第几个星期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
#if NET20
        public static Int32 GetWeekOfYear(DateTime date)
#else
        public static Int32 GetWeekOfYear(this DateTime date)
#endif
        {
            //该年1月1日所在的周，定义为第一周，sunday为0，monday为1   
            DateTime dtFirst = new DateTime(date.Year, 1, 1);
            //目标日期距离第一天的天数   
            Int32 daysCount = Convert.ToInt32((date - dtFirst).TotalDays);
            //将第一天的日期补齐   
            daysCount += Convert.ToInt32(dtFirst.DayOfWeek);
            //目标日期所在的周   
            return daysCount / 7 + 1;
        }

        /// <summary>
        /// 获取日期的星期天，返回中文的格式，例如：星期一
        /// </summary>
        /// <param name="dTargetDateTime"></param>
        /// <param name="chWeeks">
        /// 中文星期数组，数组序号从0-6，按顺序对应星期一到星期天
        /// 如果不传此参数，则默认数据位："星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"
        /// </param>
        /// <returns></returns>
        public static String GetDayOfWeek_CH(String dTargetDateTime, String[] chWeeks = null)
        {
            DateTime inputDate = DateTime.Parse(dTargetDateTime);
            chWeeks = chWeeks == null ? new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" } : chWeeks;
            switch (inputDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return chWeeks[0];
                case DayOfWeek.Tuesday:
                    return chWeeks[1];
                case DayOfWeek.Wednesday:
                    return chWeeks[2];
                case DayOfWeek.Thursday:
                    return chWeeks[3];
                case DayOfWeek.Friday:
                    return chWeeks[4];
                case DayOfWeek.Saturday:
                    return chWeeks[5];
                default:
                    return chWeeks[6];
            }
        }

        /// <summary>
        /// 获取日期的星期天，返回中文的格式，例如：星期一
        /// </summary>
        /// <param name="dTargetDateTime"></param>
        /// <param name="chWeeks">
        /// 中文星期数组，数组序号从0-6，按顺序对应星期一到星期天
        /// 如果不传此参数，则默认数据位："星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"
        /// </param>
        /// <returns></returns>
#if NET20
        public static String GetDayOfWeek_CH(DateTime dTargetDateTime, String[] chWeeks = null)
#else
        public static String GetDayOfWeek_CH(this DateTime dTargetDateTime, String[] chWeeks = null)
#endif
        {
            chWeeks = chWeeks == null ? new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" } : chWeeks;
            switch (dTargetDateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return chWeeks[0];
                case DayOfWeek.Tuesday:
                    return chWeeks[1];
                case DayOfWeek.Wednesday:
                    return chWeeks[2];
                case DayOfWeek.Thursday:
                    return chWeeks[3];
                case DayOfWeek.Friday:
                    return chWeeks[4];
                case DayOfWeek.Saturday:
                    return chWeeks[5];
                default:
                    return chWeeks[6];
            }
        }

        /// <summary>
        /// 获取日期的农历形式
        /// </summary>
        /// <param name="date"></param>
        /// <returns>返回整型数组，[年月日]</returns>
#if NET20
        public static Int32[] GetChineseCalendar(DateTime date)
#else
        public static Int32[] GetChineseCalendar(this DateTime date)
#endif
        {
            ChineseLunisolarCalendar china = new ChineseLunisolarCalendar();
            /** GetLeapMonth(int year)方法返回一个1到13之间的数字， 
             * 比如：1、该年阴历2月有闰月，则返回3 
             * 如果：2、该年阴历8月有闰月，则返回9 
             * GetMonth(DateTime dateTime)返回是当前月份，忽略是否闰月 
             * 比如：1、该年阴历2月有闰月，2月返回2，闰2月返回3 
             * 如果：2、该年阴历8月有闰月，8月返回8，闰8月返回9 
             */

            Int32 year = china.GetYear(date);
            Int32 month = china.GetMonth(date);
            Int32 day = china.GetDayOfMonth(date);

            //获取第几个月是闰月,等于0表示本年无闰月  
            Int32 leapMonth = china.GetLeapMonth(year);

            Int32[] chineseDate = new Int32[3] { year, month, day };

            //如果今年有闰月  
            if (leapMonth > 0)
            {
                //闰月数等于当前月份  
                if (month >= leapMonth)
                    chineseDate[1] = month - 1;  //月份
            }
            return chineseDate;
        }

        /// <summary>
        /// 获得农历时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayTemplate">农历描述日期的文本内容</param>
        /// <param name="mouthTemplate">农历描述月份的文本内容</param>
        /// <returns></returns>
#if NET20
        public static String FormatToChineseCalendar(DateTime date,
                                                     String dayTemplate = "初一;初二;初三;初四;初五;初六;初七;初八;初九;初十;十一;十二;十三;十四;十五;十六;十七;十八;十九;二十;廿一;廿二;廿三;廿四;廿五;廿六;廿七;廿八;廿九;三十",
                                                     String mouthTemplate = "正;二;三;四;五;六;七;八;九;十;冬;腊")
#else
            public static String FormatToChineseCalendar(this DateTime date,
                                                     String dayTemplate = "初一;初二;初三;初四;初五;初六;初七;初八;初九;初十;十一;十二;十三;十四;十五;十六;十七;十八;十九;二十;廿一;廿二;廿三;廿四;廿五;廿六;廿七;廿八;廿九;三十",
                                                     String mouthTemplate = "正;二;三;四;五;六;七;八;九;十;冬;腊")
#endif
        {
            Int32[] chineseDay = GetChineseCalendar(date);
            String[] dayTemplates = MyString.SplitEx(dayTemplate, ";");
            String[] mouthTemplates = MyString.SplitEx(mouthTemplate, ";");
            return mouthTemplates[chineseDay[1] - 1] + "月" + dayTemplates[chineseDay[2] - 1]; ;
        }
        #endregion
    }
}
