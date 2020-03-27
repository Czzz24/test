using EF.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.App_Start
{
    public class AccountAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;

            ActionResult actionResult = null;
            string message = string.Empty;

            //1.登录状态获取用户信息
            string cookievalue = TicketManagerment.GetCookieUser();

            if (!string.IsNullOrEmpty(cookievalue))
            {
                //2.使用FormsAuthentication解密用户凭据
                var ticket = FormsAuthentication.Decrypt(cookievalue);

                t_userInfo loginUser = new t_userInfo();

                //3. 直接解析到用户模型里去
                loginUser = new JavaScriptSerializer().Deserialize<t_userInfo>(ticket.UserData);

                if (loginUser == null)
                {
                    String url = request.RawUrl;
                    UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                    //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                    string returnUrl = urlHelper.Action("Login", "Login", new { returnUrl = "", message = message });
                    actionResult = new RedirectResult(returnUrl);
                }

                filterContext.Result = actionResult;
            }
            else
            {
                String url = request.RawUrl;
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Login", "Login", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            base.OnAuthorization(filterContext);
        }
    }
}