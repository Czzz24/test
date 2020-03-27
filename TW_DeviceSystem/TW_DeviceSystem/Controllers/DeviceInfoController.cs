using EF.Application.Model;
using EF.Application.Model.Custom;
using EF.Application.Model.Dtos;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;
using TW_DeviceSystem.Enum;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class DeviceInfoController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_deviceBigTypeBLL bigtypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smalltypebll = new t_deviceSmallTypeBLL();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_rolePowerBLL rolePowerbll = new t_rolePowerBLL();
        private readonly static t_MaintainBLL mbll = new t_MaintainBLL();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getDeviceList(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText,int? isError,int? isOnline)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();

            List<c_Device> list = new List<c_Device>();
            int totalcount = 0;
            //如果是超级管理员
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                list = devicebll.GetListPage(conStr, null, page, limit, out totalcount, ElectricId, LineId, JointId, bigTypeId, smallTypeId, searchText, isError, isOnline);
            }
            else
            {
                list = devicebll.GetListPage(conStr, loginUser.Id, page, limit, out totalcount, ElectricId, LineId, JointId, bigTypeId, smallTypeId, searchText, isError, isOnline);
            }

            uniteModel<c_Device> model = new uniteModel<c_Device>
            {
                code = 0,
                msg = "设备列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckLookDetails()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.selectDevice);
            }
            if (authority == false)
            {
                uniteModel<t_organize> models = new uniteModel<t_organize>
                {
                    code = 1,
                    msg = "查看设备!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_organize> models = new uniteModel<t_organize>
                {
                    code =0,
                    msg = "查看设备!,有权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Details(int Id)
        {
            t_Device model = devicebll.GetSingle(conStr, Id);
            t_deviceBigType devicebigType = bigtypebll.GetSingle(conStr, Convert.ToInt32(model.bigTypeId));
            t_deviceSmallType deviceSmallType = smalltypebll.GetSingle(conStr, Convert.ToInt32(model.smallTypeId));
            t_organize organizeElectric = organizebll.getSingle(conStr, Convert.ToInt32(model.ElectricId));
            t_organize organizeLine = organizebll.getSingle(conStr, Convert.ToInt32(model.LineId));
            t_organize organizeJoint = organizebll.getSingle(conStr, Convert.ToInt32(model.parentId));
            c_Device device = new c_Device();
            device.Id = model.Id;
            device.deviceName = model.deviceName;
            device.TerminalId = model.TerminalId;
            device.bigTypeId = model.bigTypeId;
            device.smallTypeId = model.smallTypeId;
            device.simNo = model.simNo;
            device.orderNo = model.orderNo;
            device.Installer = model.Installer;
            device.InstallDate = model.InstallDate;
            device.longitude = model.longitude;
            device.latitude = model.latitude;
            device.manufacturer = model.manufacturer;
            device.localInstructions = model.localInstructions;
            device.ElectricId = model.ElectricId;
            device.LineId = model.LineId;
            device.nodePath = model.nodePath;
            device.isOnline = model.isOnline;
            device.createTime = model.createTime;
            device.bigtypeName = devicebigType.typeName;
            device.smalltypeName = deviceSmallType.typeName;
            device.ElectricName = organizeElectric.name;
            device.LineName = organizeLine.name;
            device.JointName = organizeJoint.name;
            device.deviceId = model.deviceId;
            return View(device);
        }

        public ActionResult Edit(int Id)
        {
            t_Device model = devicebll.GetSingle(conStr, Id);
            c_Device device = new c_Device();
            device.Id = model.Id;
            device.deviceName = model.deviceName;
            device.TerminalId = model.TerminalId;
            device.bigTypeId = model.bigTypeId;
            device.smallTypeId = model.smallTypeId;
            device.simNo = model.simNo;
            device.orderNo = model.orderNo;
            device.Installer = model.Installer;
            device.InstallDate = model.InstallDate;
            device.longitude = model.longitude;
            device.latitude = model.latitude;
            device.manufacturer = model.manufacturer;
            device.localInstructions = model.localInstructions;
            device.ElectricId = model.ElectricId;
            device.LineId = model.LineId;
            device.parentId = model.parentId;
            device.nodePath = model.nodePath;
            device.isOnline = model.isOnline;
            device.createTime = model.createTime;
            device.deviceId = model.deviceId;
            return View(device);
        }

        [HttpPost]
        public JsonResult editComit(c_Device model)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.updataDevice);
            }
            if (authority == false)
            {
                uniteModel<t_Device> models = new uniteModel<t_Device>
                {
                    code = 1,
                    msg = "编辑设备失败!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            t_Device device = devicebll.GetSingle(conStr, int.Parse(model.Id.ToString()));
            int numbers = mbll.Update(conStr, model.TerminalId, device.TerminalId);
            device.Id = model.Id;
            device.deviceName = model.deviceName;
            device.TerminalId = model.TerminalId;
            device.deviceId = model.deviceId;
            device.bigTypeId = model.bigTypeId;
            device.smallTypeId = model.smallTypeId;
            device.simNo = model.simNo;
            device.orderNo = model.orderNo;
            device.Installer = model.Installer;
            device.InstallDate = Convert.ToDateTime(model.InstallDate);
            device.longitude = model.longitude;
            device.latitude = model.latitude;
            device.manufacturer = model.manufacturer;
            device.localInstructions = model.localInstructions;
            device.isOnline = model.isOnline;
            device.LineId = model.LineId;
            device.parentId = model.parentId;
            string[] nodePath = device.nodePath.Split('\\');
            int lineIndex = nodePath.Length - 4;
            int jointIndex = nodePath.Length - 3;
            nodePath[lineIndex] = model.LineId.ToString();
            nodePath[jointIndex] = model.parentId.ToString();
            string path = null;
            for(int i = 0; i < nodePath.Length; i++)
            {
                if (nodePath[i] != "")
                {
                    path += nodePath[i] + "\\";
                }
            }
            device.nodePath = path;
            bool isEdit = devicebll.Update(conStr, device);
            if (isEdit)
            {
                uniteModel<t_Device> models = new uniteModel<t_Device>
                {
                    code = 0,
                    msg = "编辑设备成功",
                    count = 1,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_Device> models = new uniteModel<t_Device>
                {
                    code = 1,
                    msg = "编辑设备失败",
                    count = 1,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.delDevice);
            }
            if (authority == false)
            {
                uniteModel<t_organize> models = new uniteModel<t_organize>
                {
                    code = 1,
                    msg = "删除失败!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            t_Device model = devicebll.GetSingle(conStr, Id);
            bool isDel = devicebll.Delete(conStr, model, loginUser.Id);
            if (isDel)
            {
                uniteModel<t_Device> models = new uniteModel<t_Device>
                {
                    code = 0,
                    msg = "删除成功!",
                    count = 1,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_Device> models = new uniteModel<t_Device>
                {
                    code = 1,
                    msg = "删除失败!",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取所有供电
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetElectric()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                list = organizebll.GetAllElectric(conStr);
            }
            else
            {
                list = organizebll.GetAllElectric(conStr, loginUser.Id);
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据供电获取线路
        /// </summary>
        /// <param name="electricId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLine(int electricId)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                list = organizebll.GetAllLine(conStr, electricId);
            }
            else
            {
                list = organizebll.GetAllLine(conStr, electricId, loginUser.Id);
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据线路获取接头
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetJoint(int LineId)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                list = organizebll.GetAllJoint(conStr, LineId);
            }
            else
            {
                list = organizebll.GetAllJoint(conStr, LineId, loginUser.Id);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}