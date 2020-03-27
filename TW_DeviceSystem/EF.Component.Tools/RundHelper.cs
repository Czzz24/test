using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public class RundHelper
    {
        #region 公用方法(public)
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="length">要生成随机数字的长度</param>
        /// <param name="Sleep">是否要停顿</param>
        /// <returns>返回已生成的随机数字</returns>
        public static string Number(int length, bool Sleep)
        {
            if (Sleep)
                Thread.Sleep(2);
            string Result = "";
            System.Random Ran = new Random();
            for (int i = 0; i < length; i++)
            {
                Result += Ran.Next(10).ToString();
            }

            return Result;
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="length">生成随机数组的长度</param>
        /// <returns>返回已生成的随机数字</returns>
        public static string Number(int length)
        {
            return Number(length, false);
        }

        /// <summary>
        /// 随机生成随机数字和随机的字母
        /// </summary>
        /// <param name="length">生成随机数字的字符长度</param>
        /// <param name="Sleep">是否要停顿</param>
        /// <returns>返回已经生成的字符串</returns>
        public static string RandomCreateString(int length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(2);
            char[] Pattern = new char[] { '0', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string Result = "";
            System.Random Ran = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Pattern.Length; i++)
            {
                Result += Pattern[Ran.Next(0, i)];
            }
            return Result;
        }

        /// <summary>
        /// 随机生成随机数字和随机的字母
        /// </summary>
        /// <param name="length">生成随机数字的字符长度</param>
        /// <returns>返回已经生成的随机数字</returns>
        public static string RandomCreateString(int length)
        {
            return RandomCreateString(length, false);
        }

        /// <summary>
        /// 随机生成纯字母的指定长度的字符串
        /// </summary>
        /// <param name="length">生成随机数字字符串的长度</param>
        /// <param name="Sleep">是否要启动线程来停止</param>
        /// <returns>返回已经生成的字符串</returns>
        public static string CreateRandomStr(int length, bool Sleep)
        {
            //是否启动线程 
            if (Sleep)
                //用线程暂用2秒的时间
                System.Threading.Thread.Sleep(2);
            string Result = string.Empty;
            char[] Pettern = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            System.Random Ran = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                Result += Pettern[Ran.Next(0, i)];
            }
            return Result;
        }

        /// <summary>
        /// 随机生成纯字母的指定长度的字符串
        /// </summary>
        /// <param name="length">生成随机数字字符串的长度</param>
        /// <returns>返回已经生成的字符串</returns>
        public static string CreateRandomStr(int length)
        {
            return CreateRandomStr(length, false);
        }

        /// <summary>
        /// 随机生成Guid
        /// </summary>
        /// <returns>返回已经生成的Guid字符串</returns>
        public static string CreateGuid()
        {
            return System.Guid.NewGuid().ToString().ToUpper();
        }

        #endregion
    }
}
