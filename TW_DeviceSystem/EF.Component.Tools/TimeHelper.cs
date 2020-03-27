using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// TimeHelper
    /// </summary>
    public class TimeHelper
    {
        // Methods
        public TimeHelper() { }
        /// <summary>
        /// 计算差;...之前
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string str = null;
            try
            {
                TimeSpan span = (TimeSpan)(DateTime2 - DateTime1);
                if (span.Days >= 1) return (DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日");
                if (span.Hours > 1) return (span.Hours.ToString() + "小时前");
                str = span.Minutes.ToString() + "分钟前";
            }
            catch
            {
            }
            return str;
        }

        /// <summary>
        /// 两个日期的间隔
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan span = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts = new TimeSpan(DateTime2.Ticks);
            return span.Subtract(ts).Duration();
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateMode"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");

                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");

                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");

                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");

                case "4":
                    return dateTime1.ToString("MM-dd");

                case "5":
                    return dateTime1.ToString("MM/dd");

                case "6":
                    return dateTime1.ToString("MM月dd日");

                case "7":
                    return dateTime1.ToString("yyyy-MM");

                case "8":
                    return dateTime1.ToString("yyyy/MM");

                case "9":
                    return dateTime1.ToString("yyyy年MM月");
            }
            return dateTime1.ToString();
        }

        /// <summary>
        /// 将时间格式化成 年月日 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public string GetFormatDate(DateTime dt, char Separator)
        {
            if (!dt.Equals(DBNull.Value))
            {
                string format = string.Format("yyyy{0}MM{1}dd", Separator, Separator);
                return dt.ToString(format);
            }
            return this.GetFormatDate(DateTime.Now, Separator);
        }

        /// <summary>
        /// 将时间格式化成 时分秒 的形式,如果时间为null，返回当前系统时间 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public string GetFormatTime(DateTime dt, char Separator)
        {
            if (!dt.Equals(DBNull.Value))
            {
                string format = string.Format("hh{0}mm{1}ss", Separator, Separator);
                return dt.ToString(format);
            }
            return this.GetFormatDate(DateTime.Now, Separator);
        }

        /// <summary>
        /// 返回某年某月最后一天 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime time = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
            return time.Day;
        }

        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime time = new DateTime();
            TimeSpan span = new TimeSpan(time1.Ticks - time2.Ticks);
            double totalSeconds = span.TotalSeconds;
            int num2 = 0;
            if (totalSeconds > 2147483647.0)
                num2 = 0x7fffffff;
            else if (totalSeconds < -2147483648.0)
                num2 = -2147483648;
            else
                num2 = (int)totalSeconds;
            if (num2 > 0)
                time = time2;
            else if (num2 < 0)
                time = time1;
            else
                return time1;
            int num3 = num2;
            if (num2 <= -2147483648) num3 = -2147483647;
            int num4 = random.Next(Math.Abs(num3));
            return time.AddSeconds((double)num4);
        }

        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal d = Second / 60M;
            return Convert.ToInt32(Math.Ceiling(d));
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
