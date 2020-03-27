using EF.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace TW_DeviceSystem.Common
{
    public  class CookieUserHelper
    {
        public static t_userInfo getCookieUser()
        {
            //1.登录状态获取用户信息
            string cookievalue = TicketManagerment.GetCookieUser();
            //2.使用FormsAuthentication解密用户凭据
            var ticket = FormsAuthentication.Decrypt(cookievalue);
            t_userInfo loginUser = new t_userInfo();
            //3. 直接解析到用户模型里去
            loginUser = new JavaScriptSerializer().Deserialize<t_userInfo>(ticket.UserData);
            return loginUser;
        }
    }
}