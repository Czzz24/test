using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public class IPAddress
    {
        /// <summary>
        /// 设置IP地址格式
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static string SetIP(string strIP)
        {
            string[] str;
            str = strIP.Split('.');
            int j;
            string tempIP = "", IP = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                tempIP = str[i];
                if (str[i].Length < 3)
                    for (j = 1; j <= 3 - str[i].Length; j++)
                        tempIP = '0' + tempIP;
                IP = tempIP + '.' + IP;
            }
            return IP.Substring(0, IP.Length - 1);
        }

        /// <summary>
        /// 返回用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            return SetIP(System.Web.HttpContext.Current.Request.UserHostAddress);
        }

        /// <summary>
        /// 将IP地址转换成10进制表示形式
        /// </summary>
        /// <param name="strIP">IP地址</param>
        /// <returns>10进制表示形式</returns>
        public static long ConvertIPToNumber(string strIP)
        {
            //取出IP地址去掉'.'后的string数组
            string[] Ip_List = strIP.Split(".".ToCharArray());

            string X_Ip = "";

            //循环数组，把数据转换成十六进制数，并合并数组(3dafe81e)
            foreach (string ip in Ip_List)
            {
                X_Ip += Convert.ToInt16(ip).ToString("x");
            }

            //将十六进制数转换成十进制数(1034938398)
            long N_Ip = long.Parse(X_Ip, System.Globalization.NumberStyles.HexNumber);

            return N_Ip;
        }
    }
}
