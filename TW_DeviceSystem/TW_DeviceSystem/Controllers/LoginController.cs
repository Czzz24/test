using EF.Application.Model;
using EF.Application.Model.Dtos;
using EF.Component.Tools;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using TW_DeviceSystem.App_Start;

namespace TW_DeviceSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_userInfoBLL bll = new t_userInfoBLL();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult userLogin(string userAccount,string userPwd)
        {
            bool status = Request.IsAuthenticated;

            userPwd = ExtHelper.EncryptAES(userPwd);

            //1.验证用户名和密码
            t_userInfo user = bll.Login(conStr, userAccount, userPwd);

            //2.用它来序列化要对象
            JavaScriptSerializer serial = new JavaScriptSerializer();

            //3.判断用户是否存在
            if (user != null)
            {
                //4.户基本信息
                t_userInfo userInfo = new t_userInfo()
                {
                    Id = user.Id,
                    userName = user.userName,
                    roleId = user.roleId
                };
                //5.生成初始化凭据
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    user.userName,
                    DateTime.Now,
                    DateTime.Now.AddDays(7),
                    false,
                    serial.Serialize(userInfo)
                );
                //6.加密
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                CookieHelper.AddCookies(FormsAuthentication.FormsCookieName, encryptedTicket);

                uniteModel<object> models = new uniteModel<object>
                {
                    code = 0,
                    msg = "登录成功!",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<object> models = new uniteModel<object>
                {
                    code = 1,
                    msg = "登录失败,账号或密码错误",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 注销退出登录
        /// </summary>
        public void logOut()
        {
            CookieHelper.DelCookies(FormsAuthentication.FormsCookieName);
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}