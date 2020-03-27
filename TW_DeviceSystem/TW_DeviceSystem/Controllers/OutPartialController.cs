using EF.Application.From.Model;
using EF.Application.From.Model.Echarts;
using EF.Application.From.Model.outPartial;
using EF.Application.Model;
using EF.Component.Data.SQLHelper;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class OutPartialController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_outPartialBLL bll = new t_outPartialBLL();

        public ActionResult Index(string TerminalId,int? ElectricId,int? LineId)
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
            t_outPartial model= bll.GetBestNew(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取A相通道数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual JsonResult GetAPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            APartial model = bll.GetBestNewA(conFstr, TerminalId);
            outABarRoot root = new outABarRoot
            {
                AElectricSeriesData = ConvertToObjectList(model.AElectric),
                AFrequencySeriesData = ConvertToObjectList(model.AFrequency),
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取B相通道数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual JsonResult GetBPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            BPartial model = bll.GetBestNewB(conFstr, TerminalId);
            outBBarRoot root = new outBBarRoot
            {
                BElectricSeriesData=ConvertToObjectList(model.BElectric),
                BFrequencySeriesData=ConvertToObjectList(model.BFrequency),
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取C相通道数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual JsonResult GetCPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            CPartial model = bll.GetBestNewC(conFstr, TerminalId);
            outCBarRoot root = new outCBarRoot
            {
                CElectricSeriesData = ConvertToObjectList(model.CElectric),
                CFrequencySeriesData = ConvertToObjectList(model.CFrequency),
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取外置局放历史分析数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetPartialAnalysis(string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            List<outPartialAnalysis> list = bll.GetHisChartX(conFstr, TerminalId, startTime, endTime);
            PartialAnalysis model = new PartialAnalysis
            {
                AElectricData = new List<int>(),
                AFrequencyData = new List<int>(),
                BElectricData = new List<int>(),
                BFrequencyData = new List<int>(),
                CElectricData = new List<int>(),
                CFrequencyData = new List<int>(),
                xAxisData = new List<string>()
            };
            for(int i = 0; i < list.Count; i++)
            {
                model.AElectricData.Add(list[i].AMaxElectric);
                model.AFrequencyData.Add(list[i].AMaxFrequency);
                model.BElectricData.Add(list[i].BMaxElectric);
                model.BFrequencyData.Add(list[i].BMaxFrequency);
                model.CElectricData.Add(list[i].CMaxElectric);
                model.CFrequencyData.Add(list[i].CMaxFrequency);
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
        /// 获取历史外置局放A相
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="currentPage"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetAHPassaGeWay(string TerminalId, int ElectricId, int LineId, int currentPage, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            int totalcount = 0;
            t_outPartial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime,out totalcount);
            outABarRoot root = new outABarRoot
            {
                AElectricSeriesData = ConvertToObjectList(model.AElectric),
                AFrequencySeriesData = ConvertToObjectList(model.AFrequency),
                CreateTime=model.CreateTime,
                totalcount = totalcount
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取历史外置局放B相
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="currentPage"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetBHPassaGeWay(string TerminalId, int ElectricId, int LineId, int currentPage, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            int totalcount = 0;
            t_outPartial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime,out totalcount);
            outBBarRoot root = new outBBarRoot
            {
                BElectricSeriesData = ConvertToObjectList(model.BElectric),
                BFrequencySeriesData = ConvertToObjectList(model.BFrequency),
                CreateTime=model.CreateTime,
                totalcount = totalcount
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取历史外置局放C相
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="currentPage"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetCHPassaGeWay(string TerminalId, int ElectricId, int LineId, int currentPage, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            int totalcount = 0;
            t_outPartial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime, out totalcount);
            outCBarRoot root = new outCBarRoot
            {
                CElectricSeriesData = ConvertToObjectList(model.CElectric),
                CFrequencySeriesData = ConvertToObjectList(model.CFrequency),
                CreateTime=model.CreateTime,
                totalcount=totalcount
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 字符串转List数据集
        /// </summary>
        /// <param name="longStr"></param>
        /// <returns></returns>
        public virtual List<object> ConvertToObjectList(string longStr)
        {
            List<object> data = new List<object>();
            data.Add(null);
            string[] array = longStr.Split(',');
            object[] result = Array.ConvertAll<string, object>(array, s => int.Parse(s));
            data.AddRange(result);
            return data;
        }

        /// <summary>
        /// 外置局放历史趋势
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
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
            List<outPartialTrend> list = bll.GetHistoryTrend(conFstr, TerminalId, startTime, endTime);
            for (int i=0;i<list.Count;i++)
            {
                List<string> singData = new List<string>();
                string date = list[i].CreateTime.ToString("yyyy-MM-dd\nHH:mm:ss");
                string AMaxElectric = list[i].AMaxElectric.ToString();
                string BMaxElectric = list[i].BMaxElectric.ToString();
                string CMaxElectric = list[i].CMaxElectric.ToString();
                singData.Add(date);
                singData.Add(AMaxElectric);
                singData.Add(BMaxElectric);
                singData.Add(CMaxElectric);
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