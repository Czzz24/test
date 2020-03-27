using EF.Application.From.Model;
using EF.Application.From.Model.Echarts;
using EF.Application.From.Model.Partial;
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
    public class PartialController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_PartialBLL bll = new t_PartialBLL();
        private readonly static t_DeviceSignalStatusBLL netbll = new t_DeviceSignalStatusBLL();

        // GET: Partial
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 获取A相最新数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetAPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            APartial result = bll.GetAPassaGeWay(conFstr, TerminalId);

            ABarRoot root = new ABarRoot
            {
                A1ElectricSeriesData = ConvertToObjectList(result.A1AvgElectric),
                A1frequencySeriesData = ConvertToObjectList(result.A1Frequency),
                A1WaveformSeriesData = ConvertToObjectWave(result.A1waveform),
                A2ElectricSeriesData = ConvertToObjectList(result.A2AvgElectric),
                A2frequencySeriesData = ConvertToObjectList(result.A2Frequency),
                A2WaveformSeriesData = ConvertToObjectWave(result.A2waveform),
                A3ElectricSeriesData = ConvertToObjectList(result.A3AvgElectric),
                A3frequencySeriesData = ConvertToObjectList(result.A3Frequency),
                A3WaveformSeriesData = ConvertToObjectWave(result.A3waveform),
                A4ElectricSeriesData = ConvertToObjectList(result.A4AvgElectric),
                A4frequencySeriesData = ConvertToObjectList(result.A4Frequency),
                A4WaveformSeriesData = ConvertToObjectWave(result.A4waveform),
                A5ElectricSeriesData = ConvertToObjectList(result.A5AvgElectric),
                A5frequencySeriesData = ConvertToObjectList(result.A5Frequency),
                A5WaveformSeriesData = ConvertToObjectWave(result.A5waveform)
            };
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取B相最新数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetBPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            BPartial result = bll.GetBPassaGeWay(conFstr, TerminalId);

            BBarRoot root = new BBarRoot
            {
                B1ElectricSeriesData = ConvertToObjectList(result.B1AvgElectric),
                B1frequencySeriesData = ConvertToObjectList(result.B1Frequency),
                B1WaveformSeriesData = ConvertToObjectWave(result.B1waveform),
                B2ElectricSeriesData = ConvertToObjectList(result.B2AvgElectric),
                B2frequencySeriesData = ConvertToObjectList(result.B2Frequency),
                B2WaveformSeriesData = ConvertToObjectWave(result.B2waveform),
                B3ElectricSeriesData = ConvertToObjectList(result.B3AvgElectric),
                B3frequencySeriesData = ConvertToObjectList(result.B3Frequency),
                B3WaveformSeriesData = ConvertToObjectWave(result.B3waveform),
                B4ElectricSeriesData = ConvertToObjectList(result.B4AvgElectric),
                B4frequencySeriesData = ConvertToObjectList(result.B4Frequency),
                B4WaveformSeriesData = ConvertToObjectWave(result.B4waveform),
                B5ElectricSeriesData = ConvertToObjectList(result.B5AvgElectric),
                B5frequencySeriesData = ConvertToObjectList(result.B5Frequency),
                B5WaveformSeriesData = ConvertToObjectWave(result.B5waveform)
            };

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取C相最新数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetCPassaGeWay(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            CPartial result = bll.GetCPassaGeWay(conFstr, TerminalId);

            CBarRoot root = new CBarRoot
            {
                C1ElectricSeriesData = ConvertToObjectList(result.C1AvgElectric),
                C1frequencySeriesData = ConvertToObjectList(result.C1Frequency),
                C1WaveformSeriesData = ConvertToObjectWave(result.C1waveform),
                C2ElectricSeriesData = ConvertToObjectList(result.C2AvgElectric),
                C2frequencySeriesData = ConvertToObjectList(result.C2Frequency),
                C2WaveformSeriesData = ConvertToObjectWave(result.C2waveform),
                C3ElectricSeriesData = ConvertToObjectList(result.C3AvgElectric),
                C3frequencySeriesData = ConvertToObjectList(result.C3Frequency),
                C3WaveformSeriesData = ConvertToObjectWave(result.C3waveform),
                C4ElectricSeriesData = ConvertToObjectList(result.C4AvgElectric),
                C4frequencySeriesData = ConvertToObjectList(result.C4Frequency),
                C4WaveformSeriesData = ConvertToObjectWave(result.C4waveform),
                C5ElectricSeriesData = ConvertToObjectList(result.C5AvgElectric),
                C5frequencySeriesData = ConvertToObjectList(result.C5Frequency),
                C5WaveformSeriesData = ConvertToObjectWave(result.C5waveform)
            };

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 字符串转数组
        /// </summary>
        /// <param name="longStr"></param>
        /// <returns></returns>
        public virtual List<object> ConvertToObjectList(string longStr)
        {
            List<object> data = new List<object>();
            data.Add(null);
            string[] array = longStr.Split(',');
            object[] result=  Array.ConvertAll<string, object>(array, s=>int.Parse(s));
            data.AddRange(result);
            return data;
        }

        /// <summary>
        /// 波形特殊处理(-2047)
        /// </summary>
        /// <param name="longStr"></param>
        /// <returns></returns>
        public virtual List<object> ConvertToObjectWave(string longStr)
        {
            List<object> data = new List<object>();
            data.Add(null);
            string[] array = longStr.Split(',');
            int[] result = Array.ConvertAll<string, int>(array, s => int.Parse(s));
            if (result.Sum() == 0)
            {
                object[] resultone = Array.ConvertAll<string, object>(array, s => int.Parse(s));
                data.AddRange(resultone);
            }
            else
            {
                object[] resultone = Array.ConvertAll<string, object>(array, s => int.Parse(s) - 2047);
                data.AddRange(resultone);
            }
            return data;
        }

        /// <summary>
        /// 获取历史局放A相
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
                startTime =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            int totalcount = 0;
            t_Partial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime, out totalcount)[0];

            ABarRoot root = new ABarRoot
            {
                A1ElectricSeriesData = ConvertToObjectList(model.A1AvgElectric),
                A1frequencySeriesData = ConvertToObjectList(model.A1Frequency),
                A1WaveformSeriesData = ConvertToObjectWave(model.A1waveform),
                A2ElectricSeriesData = ConvertToObjectList(model.A2AvgElectric),
                A2frequencySeriesData = ConvertToObjectList(model.A2Frequency),
                A2WaveformSeriesData = ConvertToObjectWave(model.A2waveform),
                A3ElectricSeriesData = ConvertToObjectList(model.A3AvgElectric),
                A3frequencySeriesData = ConvertToObjectList(model.A3Frequency),
                A3WaveformSeriesData = ConvertToObjectWave(model.A3waveform),
                A4ElectricSeriesData = ConvertToObjectList(model.A4AvgElectric),
                A4frequencySeriesData = ConvertToObjectList(model.A4Frequency),
                A4WaveformSeriesData = ConvertToObjectWave(model.A4waveform),
                A5ElectricSeriesData = ConvertToObjectList(model.A5AvgElectric),
                A5frequencySeriesData = ConvertToObjectList(model.A5Frequency),
                A5WaveformSeriesData = ConvertToObjectWave(model.A5waveform),
                CreateTime = model.CreateTime,
                totalcount = totalcount
            };

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取历史局放B相
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
            t_Partial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime, out totalcount)[0];

            BBarRoot root = new BBarRoot
            {
                B1ElectricSeriesData = ConvertToObjectList(model.B1AvgElectric),
                B1frequencySeriesData = ConvertToObjectList(model.B1Frequency),
                B1WaveformSeriesData = ConvertToObjectWave(model.B1waveform),
                B2ElectricSeriesData = ConvertToObjectList(model.B2AvgElectric),
                B2frequencySeriesData = ConvertToObjectList(model.B2Frequency),
                B2WaveformSeriesData = ConvertToObjectWave(model.B2waveform),
                B3ElectricSeriesData = ConvertToObjectList(model.B3AvgElectric),
                B3frequencySeriesData = ConvertToObjectList(model.B3Frequency),
                B3WaveformSeriesData = ConvertToObjectWave(model.B3waveform),
                B4ElectricSeriesData = ConvertToObjectList(model.B4AvgElectric),
                B4frequencySeriesData = ConvertToObjectList(model.B4Frequency),
                B4WaveformSeriesData = ConvertToObjectWave(model.B4waveform),
                B5ElectricSeriesData = ConvertToObjectList(model.B5AvgElectric),
                B5frequencySeriesData = ConvertToObjectList(model.B5Frequency),
                B5WaveformSeriesData = ConvertToObjectWave(model.B5waveform),
                CreateTime = model.CreateTime,
                totalcount = totalcount
            };

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取历史局放C相
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
            t_Partial model = bll.GetHistorySingle(conFstr, 1, currentPage, TerminalId, startTime, endTime, out totalcount)[0];

            CBarRoot root = new CBarRoot
            {
                C1ElectricSeriesData = ConvertToObjectList(model.C1AvgElectric),
                C1frequencySeriesData = ConvertToObjectList(model.C1Frequency),
                C1WaveformSeriesData = ConvertToObjectWave(model.C1waveform),
                C2ElectricSeriesData = ConvertToObjectList(model.C2AvgElectric),
                C2frequencySeriesData = ConvertToObjectList(model.C2Frequency),
                C2WaveformSeriesData = ConvertToObjectWave(model.C2waveform),
                C3ElectricSeriesData = ConvertToObjectList(model.C3AvgElectric),
                C3frequencySeriesData = ConvertToObjectList(model.C3Frequency),
                C3WaveformSeriesData = ConvertToObjectWave(model.C3waveform),
                C4ElectricSeriesData = ConvertToObjectList(model.C4AvgElectric),
                C4frequencySeriesData = ConvertToObjectList(model.C4Frequency),
                C4WaveformSeriesData = ConvertToObjectWave(model.C4waveform),
                C5ElectricSeriesData = ConvertToObjectList(model.C5AvgElectric),
                C5frequencySeriesData = ConvertToObjectList(model.C5Frequency),
                C5WaveformSeriesData = ConvertToObjectWave(model.C5waveform),
                CreateTime = model.CreateTime,
                totalcount = totalcount
            };

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取局放历史分析数据
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
            List<BuiltPartialAnalysis> list = bll.GetPartialAnalysis(conFstr, TerminalId, startTime, endTime);
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

            for (int i = 0; i < list.Count; i++)
            {
                model.AElectricData.Add(list[i].AElectricMaxValue);
                model.AFrequencyData.Add(list[i].AFrequencyMaxValue);
                model.BElectricData.Add(list[i].BElectricMaxValue);
                model.BFrequencyData.Add(list[i].BFrequencyMaxValue);
                model.CElectricData.Add(list[i].CElectricMaxValue);
                model.CFrequencyData.Add(list[i].CFrequencyMaxValue);
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
        /// 获取最新局放状态
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
            c_Partial result = bll.GetSingle(conFstr, TerminalId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 局放趋势分析
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
            List<BuilltPartialTrend> list = bll.GetHistoryTrend(conFstr, TerminalId, startTime, endTime);
            for (int i = 0; i < list.Count; i++)
            {
                List<string> singData = new List<string>();
                string date = Convert.ToDateTime(list[i].CreateTime).ToString("yyyy-MM-dd\nHH:mm:ss");
                string AElectricMaxValue = list[i].AElectricMaxValue.ToString();
                string BElectricMaxValue = list[i].BElectricMaxValue.ToString();
                string CElectricMaxValue = list[i].CElectricMaxValue.ToString();
                singData.Add(date);
                singData.Add(AElectricMaxValue);
                singData.Add(BElectricMaxValue);
                singData.Add(CElectricMaxValue);
                result.Add(singData);
            }
            return new JsonResult()
            {
                Data = result,
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// 获取最新通讯状态信息
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetBestNewNetWork(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            t_DeviceSignalStatus model= netbll.GetBestNewNetWork(conFstr, TerminalId);
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新通讯状态趋势图
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetNetWorkTrend(string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            List<object> result = new List<object>();
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = DateTime.Now.AddHours(-24);
                endTime = DateTime.Now;
            }
            List< t_DeviceSignalStatus> list = netbll.GetNetWorkTrend(conFstr, TerminalId, startTime, endTime);
            foreach (var item in list)
            {
                List<string> singData = new List<string>();
                string date = Convert.ToDateTime(item.CreateTime).ToString("yyyy-MM-dd\nHH:mm:ss");
                string Status = item.Status.ToString();
                singData.Add(date);
                singData.Add(Status);
                result.Add(singData);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}