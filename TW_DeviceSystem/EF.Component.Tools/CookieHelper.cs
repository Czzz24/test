using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EF.Component.Tools
{
    /// <summary>
    /// CookieHelper
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 添加一个新的Cookie到HttpCookes集合
        /// </summary>
        /// <param name="strCookName">Cookie的名称</param>
        /// <param name="strCookValue">Cookie的值</param>
        public static void AddCookies(string strCookName, string strCookValue)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                HttpCookie Cookies = new HttpCookie(strCookName, strCookValue);
                System.Web.HttpContext.Current.Response.AppendCookie(Cookies);
            }
            else
            {
                System.Web.HttpContext.Current.Session[strCookName] = strCookValue;
            }
        }

        /// <summary>
        /// 添加一个Cookie到HttpCookes集合并设置其过期时间
        /// </summary>
        /// <param name="strCookName"></param>
        /// <param name="strCookValue"></param>
        /// <param name="dtExpires"></param>
        public static void AddCookies(string strCookName, string strCookValue, DateTime dtExpires)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                HttpCookie myCookies = new HttpCookie(strCookName);
                myCookies.Value = strCookValue;
                myCookies.Expires = dtExpires;
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookies);
            }
            else
            {
                System.Web.HttpContext.Current.Session[strCookName] = strCookValue;
            }
        }
        /// <summary>
        /// 删除指定的Cookie
        /// </summary>
        /// <param name="strCookName">Cookie名称</param>
        public static void DelCookies(string strCookName)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                if (System.Web.HttpContext.Current.Request.Cookies[strCookName] != null)
                {
                    HttpCookie myCookie = new HttpCookie(strCookName);
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session.Contents.Remove(strCookName);
            }
        }

        public static string GetCookieValue(string strCookName)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
                {
                    if (HttpContext.Current.Request.Cookies[strCookName] != null)
                    {
                        return HttpContext.Current.Request.Cookies[strCookName].Value;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    if (HttpContext.Current.Session[strCookName] != null)
                    {
                        return System.Web.HttpContext.Current.Session[strCookName].ToString();
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
