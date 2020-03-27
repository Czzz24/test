using EF.Application.Camera.Model;
using EF.Application.Model;
using EF.Application.Model.Dtos;
using EF.Camera.Side;
using EF.Component.Tools;
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
    public class CameraController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrCamera"].ToString();
        private readonly static string constrMain = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private t_AICameraBLL bll = new t_AICameraBLL();
        private t_AIRectBLL rectbll = new t_AIRectBLL();
        private t_DeviceBLL deivcebll = new t_DeviceBLL();

        // GET: Camera
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        public ActionResult AIResult(string msgId)
        {
            ViewBag.msgId = msgId;
            return View();
        }

        /// <summary>
        /// 获取AI智能监控历史数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual JsonResult GetHistoryData(int page, int limit, string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            int totalcount = 0;
            List<t_AICamera> list = bll.getListPage(conStr, TerminalId, page, limit, out totalcount, startTime, endTime);
            uniteModel<t_AICamera> model = new uniteModel<t_AICamera>
            {
                code = 0,
                msg = "AI智能防外破数据查询成功",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取AI分析矩形数据
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public virtual JsonResult getRectData(string msgId)
        {
            List<t_AIRect> list = rectbll.getRectbyMsgId(conStr, msgId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新AI监控数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual JsonResult GetSingByBestNew(string TerminalId)
        {
            t_AICamera model = bll.getSingle(conStr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据AI消息Id获取单条数据
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public virtual JsonResult getAIByMsgId(string msgId)
        {
            t_AICamera model = bll.getSingleByMsgId(conStr, msgId);
            string filepath = model.diskPath;
            string base64img = ImageHelper.decodeImgToBase64(filepath);
            return Json(base64img, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取图片资源
        /// </summary>
        /// <param name="diskPath"></param>
        /// <returns></returns>
        public virtual JsonResult getStrImgByDisPath(string diskPath)
        {
            string base64img = ImageHelper.decodeImgToBase64(diskPath);
            return Json(base64img, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual JsonResult getDevice(string TerminalId,int ElectricId,int LineId)
        {
            t_Device model = deivcebll.GetSingleDevice(constrMain, ElectricId, LineId, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}