using EF.Application.From.Model;
using EF.Application.From.Model.Echarts;
using EF.Application.Model.Dtos;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    public class MainboardController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_SourceVoltBLL bll = new t_SourceVoltBLL();

        // GET: Mainboard
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 获取历史数据
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
            List<t_SourceVolt> list = bll.getDataPage(conFstr, TerminalId, page, limit, startTime, endTime, out totalcount);
            uniteModel<t_SourceVolt> model = new uniteModel<t_SourceVolt>
            {
                code = 0,
                msg = "三合一主板历史数据列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新三合一数据
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
            t_SourceVolt model = bll.getSingle(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取三合一图表数据
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
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            if (!startTime.HasValue || !endTime.HasValue)
            {
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                endTime = DateTime.Now;
            }
            List<t_SourceVolt> list = bll.GetChartData(conFstr, TerminalId, startTime, endTime);
            string[] arrayValue = list[0].ADC.Split(',');
            //图例颜色
            List<string> color = new List<string>();
            //图例名称
            List<string> legendName = new List<string>();

            for (int i = 0; i < arrayValue.Length; i++)
            {
                //随机颜色
                string hexColor = GetRandomColor();
                color.Add(hexColor);
                legendName.Add("ADC" + (i + 1));
            }

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

            List<string> xaxisdata = new List<string>();
            for (int h = 0; h < list.Count; h++)
            {
                xaxisdata.Add(Convert.ToDateTime(list[h].Time).ToString("yyyy-MM-dd\nHH:mm:ss"));
                string[] adcArray = list[h].ADC.Split(',');
                for(int j = 0; j < adcArray.Length; j++)
                {
                    series[j].data.Add(decimal.Parse(adcArray[j]));
                }
            }


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

            return new JsonResult
            {
                ContentType = "application/json",
                Data= root,
                MaxJsonLength = int.MaxValue,
            };
        }


        /// <summary>
        /// 获取随机hex颜色
        /// </summary>
        /// <returns></returns>
        public virtual string GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            Color color = Color.FromArgb(int_Red, int_Green, int_Blue);
            string strColor = "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2, 6);
            return strColor;
        }
    }
}