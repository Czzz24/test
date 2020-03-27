using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ExtHelper
    {
        #region string ext
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// 使用默认key进行aes加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncryptAES(this string s)
        {
            return EncryptHelper.AesEncrypt(s);
        }
        /// <summary>
        /// 自定义key进行aes加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptAES(this string s, string key)
        {
            return EncryptHelper.AesEncrypt(s, key);
        }
        /// <summary>
        /// 使用默认key进行aes解密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncryptAESDe(this string s)
        {
            return EncryptHelper.AesDecrypt(s);
        }
        /// <summary>
        /// 自定义key进行aes解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptAESDe(this string s, string key)
        {
            return EncryptHelper.AesDecrypt(s, key);
        }
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptMD5(this string s)
        {
            return EncryptHelper.MD5SHA1Encrypt2(s, "MD5");
        }
        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncryptSHA1(this string s)
        {
            return EncryptHelper.MD5SHA1Encrypt2(s, "SHA1");
        }
        /// <summary>
        /// 正则检测
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (string.IsNullOrEmpty(s)) return false;
            else return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 正则搜索字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }
        /// <summary>
        /// 是否是int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        /// <summary>
        /// 是否是邮件
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length < 5) return false;
            return Regex.IsMatch(s, "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");
        }
        /// <summary>
        /// 是否是用户名
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsUserName(this string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length < 6 || s.Length > 16) return false;
            return Regex.IsMatch(s, "^\\w{6,16}$");
        }
        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhone(this string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length < 11) return false;
            return Regex.IsMatch(s, "^13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}$");
        }

        /// <summary>
        /// 转换成int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
        /// <summary>
        /// 转换成int,不成功返回-1
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToTryInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i))
                return i;
            return -1;
        }
        /// <summary>
        /// 是否是DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDate(this string s)
        {
            DateTime i;
            return DateTime.TryParse(s, out i);
        }
        /// <summary>
        /// 转换成DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDate(this string s)
        {
            return DateTime.Parse(s);
        }

        /// <summary>
        /// 转换成DateTime,不成功返回DateTime.Now
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToTryDate(this string s)
        {
            DateTime i;
            if (DateTime.TryParse(s, out i))
                return i;
            return DateTime.Now;
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string FormatWith(this string format, object args0)
        {
            return string.Format(format, args0);
        }
        public static string FormatWith(this string format, object args0, object args1)
        {
            return string.Format(format, args0, args1);
        }
        public static string FormatWith(this string format, object args0, object args1, object args2)
        {
            return string.Format(format, args0, args1, args2);
        }
        public static string FormatWith(this string format, object args0, object args1, object args2, object args3)
        {
            return string.Format(format, args0, args1, args2, args3);
        }
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }
        /// <summary>
        /// object to json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new IsoDateTimeConverter());
        }
        #endregion

        #region Random
        /// <summary>
        /// 随机返回 true 或 false
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool NextBool(this Random random)
        {
            return random.NextDouble() > 0.5;
        }
        /// <summary>
        /// 枚举: NextEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <returns></returns>
        public static T NextEnum<T>(this Random random) where T : struct
        {
            Type type = typeof(T);
            if (type.IsEnum == false) throw new InvalidOperationException();

            var array = Enum.GetValues(type);
            var index = random.Next(array.GetLowerBound(0), array.GetUpperBound(0) + 1);
            return (T)array.GetValue(index);
        }
        public static byte[] NextBytes(this Random random, int length)
        {
            var data = new byte[length];
            random.NextBytes(data);
            return data;
        }
        public static UInt16 NextUInt16(this Random random)
        {
            return BitConverter.ToUInt16(random.NextBytes(2), 0);
        }
        public static Int16 NextInt16(this Random random)
        {
            return BitConverter.ToInt16(random.NextBytes(2), 0);
        }
        public static float NextFloat(this Random random)
        {
            return BitConverter.ToSingle(random.NextBytes(4), 0);
        }
        public static DateTime NextDateTime(this Random random, DateTime minValue, DateTime maxValue)
        {
            var ticks = minValue.Ticks + (long)((maxValue.Ticks - minValue.Ticks) * random.NextDouble());
            return new DateTime(ticks);
        }
        public static DateTime NextDateTime(this Random random)
        {
            return NextDateTime(random, DateTime.MinValue, DateTime.MaxValue);
        }
        #endregion
    }
}
