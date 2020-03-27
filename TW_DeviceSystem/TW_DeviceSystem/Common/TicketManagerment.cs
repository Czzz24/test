using EF.Application.Model;
using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TW_DeviceSystem.Common
{
    public class TicketManagerment
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public static string GetCookieUser()
        {
            string cookieValue = CookieHelper.GetCookieValue(FormsAuthentication.FormsCookieName);
            return cookieValue;
        }
    }
}