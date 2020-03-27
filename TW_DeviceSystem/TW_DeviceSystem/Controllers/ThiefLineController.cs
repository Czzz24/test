using EF.Application.From.Model;
using EF.Application.From.Model.Echarts;
using EF.Application.From.Model.ThiefLine;
using EF.Application.Model.Dtos;
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
    public class ThiefLineController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_ThiefLineBLL bll = new t_ThiefLineBLL();

        // GET: ThiefLine
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 线缆防盗历史数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetHistoryData(int page, int limit, string TerminalId, int ElectricId, int LineId ,DateTime? startTime, DateTime? endTime)
        {
            int totalcount = 0;
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            List<t_ThiefLine> list = bll.GetHistoryData(conFstr, TerminalId, page, limit, startTime, endTime, out totalcount);
            uniteModel<t_ThiefLine> model = new uniteModel<t_ThiefLine>
            {
                code = 0,
                msg = "线缆防盗历史数据列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新线缆防盗数据
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
            t_ThiefLine model = bll.GetSingle(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取线缆防盗历史图表
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetChartData(string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            //图例颜色
            List<string> color = new List<string>();
            color.Add("#F2DF7D");
            color.Add("#D50390");
            color.Add("#8F1EC2");

            //图例名称
            List<string> legendName = new List<string>();
            legendName.Add("X轴振动幅值");
            legendName.Add("Y轴振动幅值");
            legendName.Add("Z轴振动幅值");

            Legend legend = new Legend();
            legend.data = legendName;

            List<Series> series = new List<Series>();
            for (int i = 0; i < legendName.Count; i++)
            {
                Series seriesSingle = new Series
                {
                    name = legendName[i],
                    smooth = true,
                    type = "line",
                    data = new List<decimal?>()
                };
                series.Add(seriesSingle);
            }

            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            List<ThiefLine> list = bll.GetChartData(conFstr, TerminalId, startTime, endTime);

            List<string> xaxisdata = new List<string>();
            List<decimal?> xAmplitude = new List<decimal?>();
            List<decimal?> yAmplitude = new List<decimal?>();
            List<decimal?> zAmplitude = new List<decimal?>();

            for (int h = 0; h < list.Count; h++)
            {
                xaxisdata.Add(Convert.ToDateTime(list[h].Time).ToString("yyyy-MM-dd\nHH:mm:ss"));
                xAmplitude.Add(list[h].xAmplitude);
                yAmplitude.Add(list[h].yAmplitude);
                zAmplitude.Add(list[h].zAmplitude);
            }

            series[0].data.AddRange(xAmplitude);
            series[1].data.AddRange(yAmplitude);
            series[2].data.AddRange(zAmplitude);

            XAxis xAxis = new XAxis
            {
                type = "category",
                boundaryGap = false,
                data = xaxisdata,
            };

            YAxis yAxis = new YAxis
            {
                type = "value",
            };

            Tooltip tooltip = new Tooltip
            {
                trigger = "axis",
            };

            Grid grid = new Grid
            {
                left = "3%",
                right = "4%",
                bottom = "3%",
                containLabel = true,
            };

            List<DataZoom> dataZoom = new List<DataZoom>();
            DataZoom zoom1 = new DataZoom
            {
                type = "slider",
                xAxisIndex = 0,
                filterMode = "empty",
                handleIcon = "M10.7,11.9H9.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4h1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z",
            };
            DataZoom zoom2 = new DataZoom
            {
                type = "inside",
                xAxisIndex = 0,
                filterMode = "empty",
            };
            dataZoom.Add(zoom1);
            dataZoom.Add(zoom2);

            CurveRoot root = new CurveRoot
            {
                tooltip = tooltip,
                color = color,
                legend = legend,
                grid = grid,
                xAxis = xAxis,
                yAxis = yAxis,
                dataZoom = dataZoom,
                series = series,
            };

            return new JsonResult()
            {
                Data = root,
                MaxJsonLength = int.MaxValue,
                ContentType = "application/json"
            };
        }
    }
}