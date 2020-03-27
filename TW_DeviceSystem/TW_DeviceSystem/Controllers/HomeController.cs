using EF.Application.Model;
using EF.Application.Model.Alarm;
using EF.Application.Model.Custom;
using EF.Application.Model.Device;
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
using System.Web.Security;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class HomeController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_deviceBigTypeBLL bigtypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smalltypebll = new t_deviceSmallTypeBLL();
        private readonly static t_AlarmBLL alarmbll = new t_AlarmBLL();
        private readonly static t_dataBaseManagerBLL databasebll = new t_dataBaseManagerBLL();

        public ActionResult Index(string userName)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            ViewData["userName"] = loginUser.userName;
            return View();
        }

        public ActionResult Chart()
        {
            return View();
        }

        /// <summary>
        /// 获取在线设备比例
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetOnline()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                int deviceCount = devicebll.GetAllCount(conStr);
                int deviceOnlineCount = devicebll.GetAllOnline(conStr);
                DeviceStatus model = new DeviceStatus
                {
                    DeviceCount = deviceCount,
                    OnlineCount = deviceOnlineCount,
                    OffLineCount = (deviceCount - deviceOnlineCount)
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int deviceCount = devicebll.GetAllCountByUser(conStr, loginUser.Id);
                int deviceOnlineCount = devicebll.GetAllOnlineByUser(conStr, loginUser.Id);
                DeviceStatus model = new DeviceStatus
                {
                    DeviceCount = deviceCount,
                    OnlineCount = deviceOnlineCount,
                    OffLineCount = (deviceCount - deviceOnlineCount)
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取设备在线柱状图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetOnlineBar()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {

                List<t_deviceBigType> bigtype = bigtypebll.GetAll(conStr);
                barData model = new barData();
                model.xAxisData = new List<string>();
                model.onLine = new List<int>();
                model.offLine = new List<int>();
                foreach (var item in bigtype)
                {
                    int onLineNumber = devicebll.GetOnlineCountType(conStr, (int)item.Id);
                    int offLineNumber = devicebll.GetOfflineCountType(conStr, (int)item.Id);
                    model.xAxisData.Add(item.typeName);
                    model.onLine.Add(onLineNumber);
                    model.offLine.Add(offLineNumber);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<t_deviceBigType> bigtype = bigtypebll.GetAll(conStr);
                barData model = new barData();
                model.xAxisData = new List<string>();
                model.onLine = new List<int>();
                model.offLine = new List<int>();
                foreach (var item in bigtype)
                {
                    int onLineNumber = devicebll.GetOnlineCountTypeByUser(conStr, (int)item.Id, loginUser.Id);
                    int offLineNumber = devicebll.GetOfflineCountTypeByUser(conStr, (int)item.Id, loginUser.Id);
                    model.xAxisData.Add(item.typeName);
                    model.onLine.Add(onLineNumber);
                    model.offLine.Add(offLineNumber);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual JsonResult GetErrorDevice(int page,int limit)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_Device> list;
            int totalcount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                list = devicebll.GetDeviceErrorList(conStr, page, limit, out totalcount);
            }
            else
            {
                list = devicebll.GetDeviceErrorListByUser(conStr, page, limit, out totalcount, loginUser.Id);
            }
            List<c_Device> result = new List<c_Device>();
            foreach (var item in list)
            {
                c_Device device = new c_Device();
                device.Id = item.Id;
                device.deviceName = item.deviceName;
                device.TerminalId = item.TerminalId;
                device.bigTypeId = item.bigTypeId;
                device.smallTypeId = item.smallTypeId;
                device.simNo = item.simNo;
                device.orderNo = item.orderNo;
                device.Installer = item.Installer;
                device.InstallDate = item.InstallDate;
                device.longitude = item.longitude;
                device.latitude = item.latitude;
                device.manufacturer = item.manufacturer;
                device.localInstructions = item.localInstructions;
                device.ElectricId = item.ElectricId;
                device.LineId = item.LineId;
                device.nodePath = item.nodePath;
                device.isOnline = item.isOnline;
                device.isError = item.isError;
                device.createTime = item.createTime;
                t_deviceBigType devicebigType = bigtypebll.GetSingle(conStr, Convert.ToInt32(item.bigTypeId));
                device.bigtypeName = devicebigType.typeName;
                t_deviceSmallType deviceSmallType = smalltypebll.GetSingle(conStr, Convert.ToInt32(item.smallTypeId));
                device.smalltypeName = deviceSmallType.typeName;
                t_organize organizeElectric = organizebll.getSingle(conStr, Convert.ToInt32(item.ElectricId));
                device.ElectricName = organizeElectric.name;
                t_organize organizeLine = organizebll.getSingle(conStr, Convert.ToInt32(item.LineId));
                device.LineName = organizeLine.name;
                device.JointName = organizebll.getSingle(conStr, Convert.ToInt32(item.parentId)).name;
                result.Add(device);
            }

            uniteModel<c_Device> model = new uniteModel<c_Device>
            {
                code = 0,
                msg = "设备故障列表查询成功!",
                count = totalcount,
                data = result
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult GetBestNewAlarm()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_dataBaseManager> list;
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                list = databasebll.GetRandData(conStr);
                List<AlarmRelation> result = alarmbll.GetBestNewAlarm(conStr, list, null);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<t_organize> organizelist = organizebll.GetUserLine(conStr, loginUser.Id);
                list = new List<t_dataBaseManager>();
                List<t_Device> dlist = new List<t_Device>();
                for (int i = 0; i < organizelist.Count; i++)
                {
                    t_dataBaseManager model = databasebll.GetModel(conStr, (int)organizelist[i].parentId, (int)organizelist[i].Id);
                    if (model != null)
                    {
                        list.Add(model);
                    }
                }
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            List<t_Device> deviceList = devicebll.GetListByEIdAndLId(conStr, list[j].attributeElectricId, list[j].attributeLineId);
                            dlist.AddRange(deviceList);
                        }
                        List<AlarmRelation> result = alarmbll.GetBestNewAlarm(conStr, list, dlist);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        List<AlarmRelation> result = new List<AlarmRelation>();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    List<AlarmRelation> result = new List<AlarmRelation>();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public virtual JsonResult GetDeviceAction(string TerminalId, int ElectricId, int LineId)
        {
            t_Device model = devicebll.GetSingleDevice(conStr, ElectricId, LineId, TerminalId);
            string[] path = model.nodePath.Split('\\');
            string name = "";
            for(int i = 0; i < path.Length; i++)
            {
                if (i < path.Length - 2)
                {
                    t_organize organize = organizebll.getSingle(conStr, int.Parse(path[i]));
                    name += organize.name + ">";
                }
            }
            name += model.deviceName;
            t_deviceSmallType deviceSmallType = smalltypebll.GetSingle(conStr, Convert.ToInt32(model.smallTypeId));
            string url = deviceSmallType.actionAddress + "?TerminalId=" + model.TerminalId + "&ElectricId=" + model.ElectricId + "&LineId=" + model.LineId;
            jsonData data = new jsonData
            {
                title = name,
                url = url
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        class jsonData
        {
            public string title { get; set; }

            public string url { get; set; }
        }


        class barData
        {
            /// <summary>
            /// 横坐标名称
            /// </summary>
            public List<string> xAxisData { get; set; }

            /// <summary>
            /// 在线数
            /// </summary>
            public List<int> onLine { get; set; }

            /// <summary>
            /// 离线数
            /// </summary>
            public List<int> offLine { get; set; }
        }
    }
}