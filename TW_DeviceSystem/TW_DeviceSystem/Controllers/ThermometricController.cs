using EF.Application.From.Model;
using EF.Application.From.Model.EarthBox;
using EF.Application.From.Model.Echarts;
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
    public class ThermometricController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_earthBoxBLL bll = new t_earthBoxBLL();

        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 接地箱历史数据分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetPageList(int page, int limit, string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            int totalcount = 0;
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            List<t_earthBox> list = bll.GetListByPage(conFstr, TerminalId, startTime, endTime, page, limit, out totalcount);
            uniteModel<t_earthBox> model = new uniteModel<t_earthBox>
            {
                code = 0,
                msg = "接地箱历史数据列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新测温数据
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetEarthBoxData(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            t_earthBox model = bll.GetSingle(conFstr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取历史图表
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
            color.Add("#D50390");
            color.Add("#66E638");
            color.Add("#2B98DC");
            color.Add("#04DD98");
            color.Add("#36A15D");
            color.Add("#F87F38");
            color.Add("#EEE8AB");
            color.Add("#E5B5B5");
            color.Add("#04DD98");
            color.Add("#36A15D");
            color.Add("#F87F38");
            color.Add("#EEE8AB");
            color.Add("#E5B5B5");
            color.Add("#F2DF7D");

            //图例名称
            List<string> legendName = new List<string>();
            legendName.Add("箱內溫度(°C)");
            legendName.Add("箱内湿度(%)");
            legendName.Add("A相电流(A)");
            legendName.Add("B相电流(A)");
            legendName.Add("C相电流(A)");
            legendName.Add("A相电压(V)");
            legendName.Add("B相电压(V)");
            legendName.Add("C相电压(V)");
            legendName.Add("A相接头温度(℃)");
            legendName.Add("B相接头温度(℃)");
            legendName.Add("C相接头温度(℃)");
            legendName.Add("A相表皮温度(℃)");
            legendName.Add("B相表皮温度(℃)");
            legendName.Add("C相表皮温度(℃)");
            legendName.Add("蓄电池电压(V)");
            legendName.Add("电缆取电电压(V)");
            legendName.Add("太阳能取电电压(V)");

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
            List<TempEarthBoxLine> list = bll.GetTempEarthBoxChartData(conFstr, TerminalId, startTime, endTime);

            List<string> xaxisdata = new List<string>();
            List<decimal?> BoxTemp = new List<decimal?>();
            List<decimal?> BoxHumidity = new List<decimal?>();
            List<decimal?> AElectric = new List<decimal?>();
            List<decimal?> BElectric = new List<decimal?>();
            List<decimal?> CElectric = new List<decimal?>();
            List<decimal?> AVolt = new List<decimal?>();
            List<decimal?> BVolt = new List<decimal?>();
            List<decimal?> CVolt = new List<decimal?>();
            List<decimal?> A1Temp = new List<decimal?>();
            List<decimal?> B1Temp = new List<decimal?>();
            List<decimal?> C1Temp = new List<decimal?>();
            List<decimal?> A2Temp = new List<decimal?>();
            List<decimal?> B2Temp = new List<decimal?>();
            List<decimal?> C2Temp = new List<decimal?>();
            List<decimal?> BatteryVolt = new List<decimal?>();
            List<decimal?> Volt1 = new List<decimal?>();
            List<decimal?> Volt2 = new List<decimal?>();

            for (int h = 0; h < list.Count; h++)
            {
                xaxisdata.Add(Convert.ToDateTime(list[h].Time).ToString("yyyy-MM-dd\nHH:mm:ss"));
                BoxTemp.Add(list[h].BoxTemp);
                BoxHumidity.Add(list[h].BoxHumidity);
                AElectric.Add(list[h].AElectric);
                BElectric.Add(list[h].BElectric);
                CElectric.Add(list[h].CElectric);
                AVolt.Add(list[h].AVolt);
                BVolt.Add(list[h].BVolt);
                CVolt.Add(list[h].CVolt);
                A1Temp.Add(list[h].A1Temp);
                B1Temp.Add(list[h].B1Temp);
                C1Temp.Add(list[h].C1Temp);
                A2Temp.Add(list[h].A2Temp);
                B2Temp.Add(list[h].B2Temp);
                C2Temp.Add(list[h].C2Temp);
                BatteryVolt.Add(list[h].BatteryVolt);
                Volt1.Add(list[h].Volt1);
                Volt2.Add(list[h].Volt2);
            }

            series[0].data.AddRange(BoxTemp);
            series[1].data.AddRange(BoxHumidity);
            series[2].data.AddRange(AElectric);
            series[3].data.AddRange(BElectric);
            series[4].data.AddRange(CElectric);
            series[5].data.AddRange(AVolt);
            series[6].data.AddRange(BVolt);
            series[7].data.AddRange(CVolt);
            series[8].data.AddRange(A1Temp);
            series[9].data.AddRange(B1Temp);
            series[10].data.AddRange(C1Temp);
            series[11].data.AddRange(A2Temp);
            series[12].data.AddRange(B2Temp);
            series[13].data.AddRange(C2Temp);
            series[14].data.AddRange(BatteryVolt);
            series[15].data.AddRange(Volt1);
            series[16].data.AddRange(Volt2);

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