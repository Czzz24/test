using EF.Application.From.Model;
using EF.Application.From.Model.highPartial;
using EF.Application.Model.Dtos;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    public class highPartialController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_highPartialBLL bll = new t_highPartialBLL();

        // GET: highPartial
        public ActionResult Index(string TerminalId, int? ElectricId, int? LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 获取最新外置局放
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetBestNewTable(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            t_highPartial model = bll.GetBestNew(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取历史高频局放
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="currentPage"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetHisPassaGeWay(string TerminalId, int ElectricId, int LineId, int currentPage, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            int totalcount = 0;
            List<t_highPartial> model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime, out totalcount);
            uniteModel<t_highPartial> models = new uniteModel<t_highPartial>
            {
                code=0,
                count=totalcount,
                data=model,
                msg="高频局放历史数据查询成功!",
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取高频局放历史分析
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetPartialAnalysis(string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            List<highPartialAnalysis> list = bll.getHighPartialAnalysis(conFstr, TerminalId, startTime, endTime);
            PartialAnalysisChart model = new PartialAnalysisChart
            {
                MaxElectric = new List<int>(),
                MaxFrequency=new List<int>(),
                xAxisData=new List<string>()
            };
            for (int i = 0; i < list.Count; i++)
            {
                model.MaxElectric.Add(list[i].MaxElectric);
                model.MaxFrequency.Add(list[i].MaxFrequency);
                model.xAxisData.Add(list[i].CreateTime.ToString("yyyy-MM-dd\nHH:mm:ss"));
            }
            return new JsonResult()
            {
                Data = model,
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// 获取高频局放历史趋势
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetPartialTrend(string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            List<object> result = new List<object>();
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            List<highPartialTrend> list = bll.GetHistoryTrend(conFstr, TerminalId, startTime, endTime);
            for (int i = 0; i < list.Count; i++)
            {
                List<string> singData = new List<string>();
                string date = list[i].CreateTime.ToString("yyyy-MM-dd\nHH:mm:ss");
                string MaxElectric = list[i].MaxElectric.ToString();
                singData.Add(date);
                singData.Add(MaxElectric);
                result.Add(singData);
            }
            return new JsonResult()
            {
                Data = result,
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json"
            };
        }
    }
}