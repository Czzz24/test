using EF.Application.Model;
using EF.Application.Model.Custom;
using EF.Application.Model.Dtos;
using EF.Application.Model.zTree;
using EF.Component.Tools;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;
using TW_DeviceSystem.Enum;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class UserManagerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_userInfoBLL userbll = new t_userInfoBLL();
        private readonly static t_organizeBLL bll = new t_organizeBLL();
        private readonly static t_userRoleBLL userRolebll = new t_userRoleBLL();
        private readonly static t_rolePowerBLL rolePowerbll = new t_rolePowerBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_userRole> list = userRolebll.GetSelectList(conStr, loginUser.Id);
            List<SelectListItem> listRoleTypes = new List<SelectListItem>();
            listRoleTypes.Add(new SelectListItem { Value = "0", Text = "请选择角色" });
            foreach (var item in list)
            {
                SelectListItem select = new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.roleName
                };
                listRoleTypes.Add(select);
            }

            ViewData["userRole"] = listRoleTypes;

            return View();
        }

        public ActionResult Edit(int Id)
        {
            t_userInfo user = userbll.GetUserById(conStr, Id);
            if (user.roleId == 1)
            {
                List<t_userRole> list = userRolebll.GetSelectList(conStr, Id);
                List<SelectListItem> listRoleTypes = new List<SelectListItem>();
                listRoleTypes.Add(new SelectListItem { Disabled = true, Value = "0", Text = "请选择角色" });
                foreach (var item in list)
                {
                    if (item.Id == user.roleId)
                    {
                        SelectListItem select = new SelectListItem()
                        {
                            Selected=true,
                            Value=item.Id.ToString(),
                            Text=item.roleName
                        };
                        listRoleTypes.Add(select);
                    }
                    else
                    {
                        SelectListItem select = new SelectListItem()
                        {
                            Disabled = true,
                            Value = item.Id.ToString(),
                            Text = item.roleName
                        };
                        listRoleTypes.Add(select);
                    }
                }

                ViewData["roleId"] = listRoleTypes;
            }
            else
            {
                List<t_userRole> list = userRolebll.GetSelectList(conStr, Id);
                List<SelectListItem> listRoleTypes = new List<SelectListItem>();
                listRoleTypes.Add(new SelectListItem {Value = "0", Text = "请选择角色" });
                foreach (var item in list)
                {
                    if (item.Id == user.roleId)
                    {
                        SelectListItem select = new SelectListItem()
                        {
                            Selected = true,
                            Value = item.Id.ToString(),
                            Text = item.roleName
                        };
                        listRoleTypes.Add(select);
                    }
                    else
                    {
                        SelectListItem select = new SelectListItem()
                        {
                            Value = item.Id.ToString(),
                            Text = item.roleName
                        };
                        listRoleTypes.Add(select);
                    }
                }

                ViewData["roleId"] = listRoleTypes;
            }
            return View(user);
        }

        [HttpPost]
        public JsonResult EditComit(t_userInfo user)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.updateUser);
            }
            if (authority == false)
            {
                uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                {
                    code = 1,
                    msg = "编辑用户!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (user.roleId == 1 && loginUser.roleId != 1)
                {
                    uniteModel<t_userInfo> model = new uniteModel<t_userInfo>
                    {
                        code = 1,
                        count = 0,
                        data = null,
                        msg = "超级管理员不能编辑!"
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool isbase64 = IsAesDecrypt(user.userPwd);
                    if (isbase64)
                    {
                        user.userPwd = ExtHelper.EncryptAESDe(user.userPwd);
                        user.userPwd = ExtHelper.EncryptAES(user.userPwd);
                    }
                    else
                    {
                        user.userPwd = ExtHelper.EncryptAES(user.userPwd);
                    }
                    bool isEdit = userbll.UpdateUser(conStr, user);
                    if (isEdit)
                    {
                        uniteModel<t_userInfo> model = new uniteModel<t_userInfo>
                        {
                            code = 0,
                            count = 0,
                            data = null,
                            msg = "用户修改成功!"
                        };
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<t_userInfo> model = new uniteModel<t_userInfo>
                        {
                            code = 1,
                            count = 0,
                            data = null,
                            msg = "用户修改失败!"
                        };
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        /// <summary>
        /// 是否经过AES加密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static bool IsAesDecrypt(string toDecrypt)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AesKey.ToLower());
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private const string defaultKey = "Author:xxwgcg;By:http://xxwgcg.cnblogs.com/";
        private static string _aesKey;

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


        [HttpPost]
        public JsonResult CheckLookDetails()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.selectUser);
            }
            if (authority == false)
            {
                uniteModel<t_organize> models = new uniteModel<t_organize>
                {
                    code = 1,
                    msg = "查看用户!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_organize> models = new uniteModel<t_organize>
                {
                    code = 0,
                    msg = "查看用户!,有权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int Id)
        {
            t_userInfo user = userbll.GetUserById(conStr, Id);
            return View(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.delUser);
            }
            if (authority == false)
            {
                uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                {
                    code = 1,
                    msg = "删除用户!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                t_userInfo userModel = userbll.GetUserById(conStr, Id);
                if (userModel.roleId == 1)
                {
                    uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                    {
                        code = 1,
                        msg = "超级管理员不允许删除!",
                        count = 0,
                        data = null,
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool isDel = userbll.Delete(conStr, userModel, loginUser.Id);
                    if (isDel)
                    {
                        uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                        {
                            code = 0,
                            msg = "删除成功!",
                            count = 1,
                            data = null,
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                        {
                            code = 1,
                            msg = "删除失败!",
                            count = 0,
                            data = null,
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddUser(t_userInfo user)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.addUser);
            }
            if (authority == false)
            {
                uniteModel<t_userInfo> models = new uniteModel<t_userInfo>
                {
                    code = 1,
                    msg = "添加用户!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //验证用户是否存在
                bool isExistUser = userbll.CheckUserAccount(conStr, user.userAccount);
                if (isExistUser)
                {
                    uniteModel<t_userInfo> models = new uniteModel<t_userInfo>()
                    {
                        code = 1,
                        msg = "添加失败,存在当前用户账号!",
                        count = 1,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                user.CreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(user.Phone))
                {
                    user.Phone = null;
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    user.Email = null;
                }
                user.userPwd = ExtHelper.EncryptAES(user.userPwd);
                bool isAdd = userbll.AddUser(conStr, user);
                if (isAdd)
                {
                    uniteModel<t_userInfo> models = new uniteModel<t_userInfo>()
                    {
                        code = 0,
                        msg = "添加成功!",
                        count = 1,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_userInfo> models = new uniteModel<t_userInfo>()
                    {
                        code = 1,
                        msg = "添加失败!",
                        count = 1,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 获取用户表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual JsonResult GetUserTable(int page, int limit,string searchText)
        {
            int totalcount = 0;
            List<t_userInfo> list = userbll.GetUserPage(conStr, page, limit, out totalcount, searchText);
            uniteModel<t_userInfo> model = new uniteModel<t_userInfo>
            {
                code = 0,
                msg = "功能列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRoleSelectList()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_userRole> list = userRolebll.GetSelectList(conStr, loginUser.roleId);
            return Json(list,JsonRequestBehavior.AllowGet);
        }
    }
}