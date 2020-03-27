using EF.Application.From.Model;
using EF.Application.From.Model.ErrorLocation;
using EF.Application.Model;
using EF.Application.Model.Dtos;
using EF.Component.Tools;
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
    public class ErrorController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_errorLocationBLL bll = new t_errorLocationBLL();
        private readonly static t_errorSetBLL eSetbll = new t_errorSetBLL();

        // GET: Error
        public ActionResult Index(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        public ActionResult fjdq(string TerminalId, int ElectricId, int LineId)
        {
            ViewBag.TerminalId = TerminalId;
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            return View();
        }

        /// <summary>
        /// 伏佳安达电气故障定位最新数据
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public virtual JsonResult getfjdqErrorData(int deviceId)
        {
            WebClient client = new WebClient();
            string url = "http://cas-realtime.whfjad.cn/gatherdata/device-status?program=default&deviceId=" + deviceId + "";
            string recive = client.GetHtml(url);
            fjdqErrorRoot model = JsonHelper.JsonDeserialize<fjdqErrorRoot>(recive);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 伏佳安达电气故障定位历史数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public virtual JsonResult fetfjdqHistoryData(int page,int limit,int deviceId,DateTime? start,DateTime? end)
        {
            WebClient client = new WebClient();
            int pageGap = page - 1;
            string url;
            if (start.HasValue && end.HasValue)
            {
                string startTime = Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm:ss");
                url = "http://cas-realtime.whfjad.cn/gatherdata/data-history-query?program=default&deviceId=" + deviceId + "&start=" + startTime + "&end=" + endTime + "&page=" + pageGap + "&count=" + limit + "";
            }
            else
            {
                url = "http://cas-realtime.whfjad.cn/gatherdata/data-history-query?program=default&deviceId=" + deviceId + "&start=" + start + "&end=" + end + "&page=" + pageGap + "&count=" + limit + "";
            }
            string recive = client.GetHtml(url);
            fjdqHistoryError model = JsonHelper.JsonDeserialize<fjdqHistoryError>(recive);
            uniteModel<ListItem> result = new uniteModel<ListItem>
            {
                code = 0,
                msg= "伏佳安达电气故障定位历史数据查询成功!",
                count=model.data.totalCount,
                data=model.data.list
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取故障定位历史数据
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
            List<t_errorLocation> list = bll.GetHistoryData(conFstr, TerminalId, page, limit, startTime, endTime, out totalcount);
            uniteModel<t_errorLocation> model = new uniteModel<t_errorLocation>
            {
                code = 0,
                msg = "故障定位历史数据列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取最新故障定位数据
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
            t_errorLocation model = bll.GetSingle(conFstr, TerminalId);
            t_errorSet errorset = eSetbll.GetSingle(conStr, TerminalId);
            if (model!=null)
            {
                //故障设备基点Y
                double ay = (double)model.latitude;
                //故障设备基点X
                double ax = (double)model.longitude;
                //故障距离
                double distance = (double)model.distance;
                if (distance > 0)
                {
                    double by = (double)errorset.rightLatitude;
                    double bx = (double)errorset.rightLongitude;
                    double tdistance = GetDistance(ay, ax, by, bx);
                    double[] cPoint = GetCpoint(ax, bx, ay, by, tdistance, distance);
                    PathItem Item = new PathItem
                    {
                        lat = ay,
                        lng = ax
                    };
                    PathItem eItem = new PathItem
                    {
                        lat = cPoint[1],
                        lng = cPoint[0]
                    };
                    List<PathItem> errorPath = new List<PathItem>();
                    errorPath.Add(Item);
                    errorPath.Add(eItem);
                    ErrorLocation result = new ErrorLocation
                    {
                        Id = model.Id,
                        distance = model.distance,
                        CreateTime = model.CreateTime,
                        latitude = model.latitude,
                        longitude = model.longitude,
                        TerminalId = model.TerminalId,
                        Time = model.CreateTime,
                        errorPath = errorPath,
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    double by = (double)errorset.leftLatitude;
                    double bx = (double)errorset.leftLongitude;
                    double tdistance = GetDistance(ay, ax, by, bx);
                    double[] cPoint = GetCpoint(ax, bx, ay, by, tdistance, distance);
                    PathItem Item = new PathItem
                    {
                        lat = ay,
                        lng = ax
                    };
                    PathItem eItem = new PathItem
                    {
                        lat = cPoint[1],
                        lng = cPoint[0]
                    };
                    List<PathItem> errorPath = new List<PathItem>();
                    errorPath.Add(Item);
                    errorPath.Add(eItem);
                    ErrorLocation result = new ErrorLocation
                    {
                        Id = model.Id,
                        distance = model.distance,
                        CreateTime = model.CreateTime,
                        latitude = model.latitude,
                        longitude = model.longitude,
                        TerminalId = model.TerminalId,
                        Time = model.CreateTime,
                        errorPath = errorPath,
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                ErrorLocation result = new ErrorLocation
                {
                    Id = 0,
                    distance = 0,
                    CreateTime = DateTime.Now,
                    latitude = 0,
                    longitude = 0,
                    TerminalId = TerminalId,
                    Time = DateTime.Now,
                    errorPath = null,
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 故障定位坐标设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult AddErrorSet(t_errorSet entity)
        {
            bool status = eSetbll.Add(conStr, entity);
            if (status)
            {
                uniteModel<t_DestroySet> model = new uniteModel<t_DestroySet>
                {
                    code = 0,
                    count = 1,
                    msg = "故障定位坐标设置成功!",
                    data = null,
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_DestroySet> model = new uniteModel<t_DestroySet>
                {
                    code = 0,
                    count = 1,
                    msg = "故障定位坐标设置失败!",
                    data = null,
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 查询坐标设置
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetErrorSet(string TerminalId)
        {
            t_errorSet model = eSetbll.GetSingle(conStr, TerminalId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 地球半径
        /// </summary>
        const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// 计算故障位置坐标
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <param name="tdistance"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public double[] GetCpoint(double ax, double bx, double ay, double by, double tdistance, double distance)
        {
            double[] cPoint = new double[2];
            double cx = (ax - bx) * distance / tdistance + ax;
            double cy = (ay - by) * distance / tdistance + ay;
            cPoint[0] = cx;
            cPoint[1] = cy;
            return cPoint;
        }

        /// <summary>
        /// 两点计算距离
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public double GetDistance(double lat1,double lng1,double lat2,double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;

        }

        /// <summary>
        /// 经度转弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

    }
}