using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

/*******************************************************************
 * * 函数名称： EncryptHelper
 * * 功    能： 加密帮助类
 * * 作    者： xxwgcg
 * * 博    客： http://xxwgcg.cnblogs.com/
 * * 电子邮箱： ai_lemony@126.com
 * * 日    期： 2011年2月15日
 * *****************************************************************/

namespace EF.Component.Tools
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// 默认key
        /// </summary>
        private const string defaultKey = "Author:xxwgcg;By:http://xxwgcg.cnblogs.com/";

        private static string _aesKey;

        /// <summary>
        /// key
        /// </summary>
        public static string AesKey
        {
            get
            {
                if (string.IsNullOrEmpty(_aesKey))
                {
                    AesKey = defaultKey;
                }
                return _aesKey;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _aesKey = defaultKey.Substring(0, 32);
                }
                else
                {
                    int valLength = value.Length;
                    if (valLength <= 16)
                    {
                        _aesKey = (value + defaultKey).Substring(0, 16);
                    }
                    else if (valLength <= 24)
                    {
                        _aesKey = (value + defaultKey).Substring(0, 24);
                    }
                    else
                    {
                        _aesKey = (value + defaultKey).Substring(0, 32);
                    }
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt">待加密的字符串</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string AesEncrypt(string toEncrypt, string key)
        {
            AesKey = key;
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AesKey.ToLower());
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// 加密--使用默认key
        /// </summary>
        /// <param name="toEncrypt">待加密的字符串</param>
        /// <returns></returns>
        public static string AesEncrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AesKey.ToLower());
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">待解密的字符串</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string AesDecrypt(string toDecrypt, string key)
        {
            AesKey = key;
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AesKey.ToLower());
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt">待解密的字符串</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string AesDecrypt(string toDecrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AesKey.ToLower());
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 对字符串进行MD5或SHA1加密操作,带'-'
        /// </summary>
        /// <param name="cleanString">明文字符串</param>
        /// <param name="strPasswordFormat">加密方式--MD5 OR SHA1</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5SHA1Encrypt1(string cleanString, string strPasswordFormat)
        {
            //19-A2-85-41-44-B6-3A-8F-76-17-A6-F2-25-01-9B-12                 MD5 admin
            //7C-87-54-1F-D3-F3-EF-50-16-E1-2D-41-19-00-C8-7A-60-46-A8-E8     SHA1 admin
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes;
            switch (strPasswordFormat.ToUpper())
            {
                case "MD5":
                    hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                    break;
                case "SHA1":
                    hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(clearBytes);
                    break;
                default:
                    hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                    break;
            }
            //string str = FormsAuthentication.HashPasswordForStoringInConfigFile(PWD, "MD5");
            return BitConverter.ToString(hashedBytes);
        }

        /// <summary>
        /// 对字符串进行MD5或SHA1加密操作,不带'-'
        /// </summary>
        /// <param name="cleanString">明文字符串</param>
        /// <param name="strPasswordFormat">加密方式--MD5 OR SHA1</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5SHA1Encrypt2(string cleanString, string strPasswordFormat)
        {
            //21232F297A57A5A743894A0E4A801FC3            MD5 admin
            //D033E22AE348AEB5660FC2140AEC35850C4DA997    SHA1 admin
            switch (strPasswordFormat.ToUpper())
            {
                case "MD5":
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(cleanString, "MD5");
                case "SHA1":
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(cleanString, "SHA1");
                default:
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(cleanString, "SHA1");
            }
        }
    }
}
