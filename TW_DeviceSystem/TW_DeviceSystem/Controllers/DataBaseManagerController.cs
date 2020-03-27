using EF.Application.Model;
using EF.Application.Model.DataBase;
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
    public class DataBaseManagerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_dataBaseManagerBLL bll = new t_dataBaseManagerBLL();
        private readonly static t_organizeBLL organizeBLL = new t_organizeBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            t_dataBaseManager model = bll.GetSingle(conStr, Id);
            return View(model);
        }

        public ActionResult Details(int Id)
        {
            t_dataBaseManager model = bll.GetSingle(conStr, Id);
            return View(model);
        }
        /// <summary>
        /// 数据库列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public virtual JsonResult GetDataBasePage(int page, int limit, int? ElectricId, int? LineId, string projectName)
        {
            int totalcount = 0;
            List<t_dataBaseManager> list = bll.GetListByPage(conStr, page, limit, ElectricId, LineId, projectName, out totalcount);
            List<c_dataBaseManager> result = new List<c_dataBaseManager>();
            foreach (var item in list)
            {
                c_dataBaseManager database = new c_dataBaseManager();
                database.Id = item.Id;
                database.projectName = item.projectName;
                database.dataBaseAccount = item.dataBaseAccount;
                database.dataBasePwd = item.dataBasePwd;
                database.dataBaseIP = item.dataBaseIP;
                database.dataBaseName = item.dataBaseName;
                database.attributeElectricId = item.attributeElectricId;
                database.attributeLineId = item.attributeLineId;
                database.ElectricName = organizeBLL.getSingle(conStr, Convert.ToInt32(item.attributeElectricId)).name;
                database.LineName = organizeBLL.getSingle(conStr, Convert.ToInt32(item.attributeLineId)).name;
                database.CreateTime = item.CreateTime;
                result.Add(database);
            }
            uniteModel<c_dataBaseManager> model = new uniteModel<c_dataBaseManager>
            {
                code = 0,
                msg = "功能列表查询成功!",
                count = totalcount,
                data = result
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据库列表编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult editComit(t_dataBaseManager model)
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
    }
}