using EF.Application.Model;
using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace TW_DeviceSystem.App_Start
{
    public class AuthenticationAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 登录身份验证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                //未登录的时候,此处加了一个判断,判断同步请求还是异步请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //异步请求，返回JSON数据
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Status = -1,
                            Message = "登录已过期,请刷新页面后操作!"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    //非异步请求,则跳转登录页
                    FormsAuthentication.RedirectToLoginPage();//重定向会登录页
                }
            }
            else
            {
                //1.登录状态获取用户信息
                var cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                //2.使用FormsAuthentication解密用户凭据
                var ticket = FormsAuthentication.Decrypt(cookie.Value);

                t_userInfo loginUser = new t_userInfo();

                //3. 直接解析到用户模型里去
                loginUser = new JavaScriptSerializer().Deserialize<t_userInfo>(ticket.UserData);

                //4. 将要使用的数据放到ViewData 里，方便页面使用
                filterContext.Controller.ViewData["userName"] = loginUser.userName;
                filterContext.Controller.ViewData["userId"] = loginUser.Id;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}