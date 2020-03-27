using EF.Application.Model;
using EF.Application.Model.Dtos;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{

    [AccountAuthorize]
    public class FunctionController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_functionBLL bll = new t_functionBLL();
        private readonly static t_functionDistributeBLL disbll = new t_functionDistributeBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Distribute()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 菜单添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult addComit(t_function model)
        {
            model.CreateTime = DateTime.Now;
            bool isSuccess = bll.Add(conStr, model);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 菜单编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            t_function model = bll.GetSingle(conStr, Id);
            return View(model);
        }

        /// <summary>
        /// 菜单编辑提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult editComit(t_function model)
        {
            model.CreateTime = DateTime.Now;
            bool isSuccess = bll.update(conStr, model);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 菜单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Details(int Id)
        {
            t_function model = bll.GetSingle(conStr, Id);
            return View(model);
        }


        /// <summary>
        /// 菜单分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual JsonResult GetPageList(int page, int limit)
        {
            int totalcount = 0;
            List<t_function> list = bll.GetListByPage(conStr, page, limit, out totalcount);
            uniteModel<t_function> model = new uniteModel<t_function>
            {
                code = 0,
                msg = "功能列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        public virtual JsonResult GetAllList()
        {
            List<t_function> list = bll.GetAllList(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult AddRoleFunction(List<long> roleId, List<long> functionId)
        {
            List<long> fdId = new List<long>();
            for (int i = 0; i < roleId.Count; i++)
            {
                List<t_functionDistribute> list = disbll.GetListByRoleId(conStr, roleId[i]);
                for(int j = 0; j < list.Count; j++)
                {
                    fdId.Add(list[j].Id);
                }
                disbll.Delete(conStr, fdId);
            }
            disbll.Add(conStr, functionId, roleId);
            uniteModel<t_functionDistribute> model = new uniteModel<t_functionDistribute>
            {
                code = 0,
                msg =string.Format("角色菜单分配成功{0}项",functionId.Count),
                count = functionId.Count,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据角色查询已拥有菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetRoleFunction(long roleId)
        {
            List<t_functionDistribute> list = disbll.GetListByRoleId(conStr, roleId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询单个菜单
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual  JsonResult GetFunctionName(int functionId)
        {
            t_function function = bll.GetSingle(conStr, functionId);
            return Json(function, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据登录用户查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetFunctionByUser()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_function> list;
            if (loginUser.roleId == 1)
            {
                list = bll.GetAllList(conStr);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                list = bll.GetFunctionByUserRole(conStr, loginUser.roleId);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

    }
}