using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// 验证类
    /// </summary>
    public static class ValidateUtil
    {
        /// <summary>
        /// 验证是否是 Email 地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            return Regex.IsMatch(str, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
        }
    }
}
