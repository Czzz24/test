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

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class RoleManagerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_userRoleBLL bll = new t_userRoleBLL();
        private readonly static t_rolePowerBLL rolePowerBLL = new t_rolePowerBLL();
        private readonly static t_rolePowerDistributeBLL rolePowerAttrBLL = new t_rolePowerDistributeBLL();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual JsonResult GetPageList(int page,int limit)
        {
            int totalcount = 0;
            List<t_userRole> list = bll.GetListByPage(conStr, page, limit, out totalcount);
            uniteModel<t_userRole> model = new uniteModel<t_userRole>
            {
                code = 0,
                msg = "功能列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult RolePower()
        {
            return View();
        }

        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult addComit(t_userRole model)
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
        /// 角色详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Details(int Id)
        {
            t_userRole userRole = bll.GetSingle(conStr, Id);
            return View(userRole);
        }
        
        /// <summary>
        /// 角色编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            t_userRole userRole = bll.GetSingle(conStr, Id);
            return View(userRole);
        }
        
        /// <summary>
        /// 角色编辑提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult editComit(t_userRole model)
        {
            bool isSuccess = bll.Edit(conStr, model);
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
        /// 添加角色权限
        /// </summary>
        /// <param name="roleArray"></param>
        /// <param name="powerArray"></param>
        /// <returns></returns>
        public virtual JsonResult AddRolePower(List<long> roleArray,List<long> powerArray)
        {
            List<long> rpId = new List<long>();
            for (int i = 0; i < roleArray.Count; i++)
            {
                List<t_rolePowerDistribute> list = rolePowerAttrBLL.GetByRole(conStr, roleArray[i]);
                for(int j = 0; j < list.Count; j++)
                {
                    rpId.Add(list[j].Id);
                }
                rolePowerAttrBLL.Delete(conStr, rpId);
            }

            rolePowerAttrBLL.Add(conStr, powerArray, roleArray);
            uniteModel<t_rolePowerDistribute> model = new uniteModel<t_rolePowerDistribute>
            {
                code = 0,
                msg = string.Format("角色权限分配成功{0}项!", powerArray.Count),
                count = powerArray.Count,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有角色权限
        /// </summary>
        /// <returns></returns>
        public virtual JsonResult GetRolePower()
        {
            List<t_rolePower> list = rolePowerBLL.GetAll(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetRolePower(long roleId)
        {
            List<t_rolePowerDistribute> list = rolePowerAttrBLL.GetByRole(conStr, roleId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult GetPowerName(int powerId)
        {
            t_rolePower model= rolePowerBLL.GetSingle(conStr, powerId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}