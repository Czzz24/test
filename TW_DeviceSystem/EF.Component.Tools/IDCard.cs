using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public static class IDCard
    {
        /// <summary>
        /// 从身份证号中读生日
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns>YYYY-MM-DD格式,身份证号码错误时返回0</returns>
        public static string GetBirthdayFromIDNO(string IDNO)
        {
            string Birthday = "";
            if (IsIDCard(IDNO))
            {
                if (IDNO.Length == 18)
                    Birthday = IDNO.Substring(6, 4) + "-" + IDNO.Substring(10, 2) + "-" + IDNO.Substring(12, 2);
                if (IDNO.Length == 15)
                    Birthday = "19" + IDNO.Substring(6, 2) + "-" + IDNO.Substring(8, 2) + "-" + IDNO.Substring(10, 2);
            }
            else
                Birthday = "0";
            return Birthday;

        }
        /// <summary>
        /// 从身份证中读取出生年
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns>返回YYYY，身份证号码错误时返回0</returns>
        public static string GetYearFromIDNO(string IDNO)
        {
            string Year = "";
            if (IsIDCard(IDNO))
            {
                if (IDNO.Length == 18)
                    Year = IDNO.Substring(6, 4);
                if (IDNO.Length == 15)
                    Year = "19" + IDNO.Substring(6, 2);
            }
            else
                Year = "0";
            return Year;

        }
        /// <summary>
        /// 从身份证中读取出生月份
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns>返回MM，身份证号码错误时返回0</returns>
        public static string GetMonthFromIDNO(string IDNO)
        {
            string Month = "";
            if (IsIDCard(IDNO))
            {
                if (IDNO.Length == 18)
                    Month = IDNO.Substring(10, 4);
                if (IDNO.Length == 15)
                    Month = IDNO.Substring(8, 2);
            }
            else
                Month = "0";
            return Month;

        }
        /// <summary>
        /// 从身份证中读取出生天
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns>返回DD，身份证号码错误时返回0</returns>
        public static string GetDayFromIDNO(string IDNO)
        {
            string Day = "";
            if (IsIDCard(IDNO))
            {
                if (IDNO.Length == 18)
                    Day = IDNO.Substring(12, 4);
                if (IDNO.Length == 15)
                    Day = IDNO.Substring(10, 2);
            }
            else
                Day = "0";
            return Day;

        }
        /// <summary>
        /// 从身份证号中读取性别
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns></returns>
        public static string GetSexFromIDNO(string IDNO)
        {

            string Sex = "";

            if (IsIDCard(IDNO))
            {
                int Temp = 0;
                if (IDNO.Length == 18)
                    Temp = Convert.ToInt32(IDNO.Substring(16, 1));
                if (IDNO.Length == 15)
                    Temp = Convert.ToInt32(IDNO.Substring(14, 1));
                if (Temp % 2 == 0)
                    Sex = "女";
                else
                    Sex = "男";
            }
            else
                Sex = "0";
            return Sex;

        }
        /// <summary>
        /// 检查是不是身份证号
        /// </summary>
        /// <param name="IDNO">身份证号</param>
        /// <returns>正确时返回TRUE 错误时返回FALES</returns>
        public static bool IsIDCard(string IDNO)
        {
            bool Check = true;
            string wh = "";//身份证最后一位
            string mIDNO = "";//去掉最后一位
            if (string.IsNullOrEmpty(IDNO.Trim()))
            {
                Check = false;
            }

            if (IDNO.Length == 15)
            {
                mIDNO = IDNO.Substring(0, 14);
                wh = IDNO.Substring(14, 1);
                if (Convert.ToInt32(IDNO.Substring(8, 2)) > 12)
                    Check = false;
                if (Convert.ToInt32(IDNO.Substring(10, 2)) > 31)
                    Check = false;
            }
            if (IDNO.Length == 18)
            {
                mIDNO = IDNO.Substring(0, 17);
                wh = IDNO.Substring(17, 1);
                if (Convert.ToInt32(IDNO.Substring(10, 2)) > 12)
                    Check = false;
                if (Convert.ToInt32(IDNO.Substring(12, 2)) > 31)
                    Check = false;
                if (Convert.ToInt32(IDNO.Substring(6, 4)) < 1753)
                    Check = false;
            }
            if (wh != "X")
            {
                if (!IsNumber(wh))
                {
                    Check = false;
                }
            }
            if (!IsNumber(mIDNO))
            {
                Check = false;
            }
            return Check;
        }
        ///检查是不是数字
        public static bool IsNumber(String checkNumber)
        {
            bool isCheck = true;

            if (string.IsNullOrEmpty(checkNumber))
            {
                isCheck = false;
            }
            else
            {
                char[] charNumber = checkNumber.ToCharArray();

                for (int i = 0; i < charNumber.Length; i++)
                {
                    if (!Char.IsNumber(charNumber[i]))
                    {
                        isCheck = false;
                        break;
                    }
                }
            }

            return isCheck;
        }


    }
}
