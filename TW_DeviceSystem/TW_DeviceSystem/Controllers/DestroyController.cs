using EF.Application.From.Model;
using EF.Application.Model;
using EF.Application.Model.DestroySet;
using EF.Application.Model.Dtos;
using EF.Core.Side;
using EF.From.Side;
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
    public class DestroyController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DestroyBLL bll = new t_DestroyBLL();
        private readonly static t_DestroyTypeBLL typebll = new t_DestroyTypeBLL();
        private readonly static t_DestroySetBLL setbll = new t_DestroySetBLL();

        // GET: Destroy
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 光纤防外破历史数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetHistoryData(int page, int limit, string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            int totalcount = 0;
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            List<t_Destroy> list = bll.GetHistoryData(conFstr, TerminalId, page, limit, startTime, endTime, out totalcount);
            uniteModel<t_Destroy> model = new uniteModel<t_Destroy>
            {
                code = 0,
                msg = "光纤防外破历史数据列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询最新光纤防外破数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetSingle(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            t_Destroy model = bll.GetSingle(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取防区类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetDestroyType()
        {
            List<t_DestroyType> list = typebll.GetAll(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 防区设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult AddDestorySet(t_DestroySet entity)
        {
            bool status = setbll.Add(conStr, entity);
            if (status)
            {
                uniteModel<t_DestroySet> model = new uniteModel<t_DestroySet>
                {
                    code = 0,
                    count = 1,
                    msg = "防区设置成功!",
                    data = null,
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_DestroySet> model = new uniteModel<t_DestroySet>
                {
                    code = 1,
                    count = 0,
                    msg = "防区设置失败!",
                    data = null,
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取防区
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetDestoryPath(string TerminalId)
        {
            List<DestroySet> list = setbll.GetDestroyByTerId(conStr, TerminalId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}