using EF.Application.Model;
using EF.Application.Model.Custom;
using EF.Application.Model.DataBase;
using EF.Application.Model.Dtos;
using EF.Application.Model.organize;
using EF.Application.Model.User;
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
    public class RecoveryController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_dataBaseManagerBLL databasebll = new t_dataBaseManagerBLL();
        private readonly static t_userInfoBLL userbll = new t_userInfoBLL();
        private readonly static t_deviceBigTypeBLL bigtypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smalltypebll = new t_deviceSmallTypeBLL();

        public ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult GetDelUserList(int page, int limit, string searchText)
        {
            int totalCount = 0;
            List<t_userInfo> list = userbll.GetDelListPage(conStr, page, limit, out totalCount, searchText);
            List<userInfo> result = new List<userInfo>();
            foreach(var item in list)
            {
                userInfo user = new userInfo();
                user.Id = item.Id;
                user.orderNo = item.orderNo;
                user.Phone = item.Phone;
                user.roleId = item.roleId;
                user.userAccount = item.userAccount;
                user.userDescription = item.userDescription;
                user.userName = item.userName;
                user.userPwd = item.userPwd;
                user.DelTime = item.DelTime;
                user.CreateTime = item.CreateTime;
                user.DelUser = item.DelUser;
                user.DelUserName = userbll.GetUserById(conStr, (int)item.DelUser).userName;
                user.isDel = item.isDel;
                user.Email = item.Email;
                result.Add(user);
            }

            uniteModel<userInfo> models = new uniteModel<userInfo>
            {
                code = 0,
                msg = "用户回收站列表查询成功!",
                data = result,
                count = totalCount,
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetDelOrganizeList(int page,int limit,string searchText)
        {
            int totalCount = 0;
            List<t_organize> list = organizebll.GetDelListPage(conStr, page, limit, out totalCount, searchText);
            List<organize> result = new List<organize>();
            foreach (var item in list)
            {
                organize organize = new organize();
                organize.Id = item.Id;
                organize.name = item.name;
                organize.nodePath = item.nodePath;
                organize.orderNo = item.orderNo;
                organize.parentId = item.parentId;
                organize.isLine = item.isLine;
                organize.isJoint = item.isJoint;
                organize.isElectric = item.isElectric;
                organize.isDel = item.isDel;
                organize.DelUser = item.DelUser;
                organize.CreateTime = item.CreateTime;
                organize.DelTime = item.DelTime;
                organize.DelUserName = userbll.GetUserById(conStr, (int)item.DelUser).userName;
                result.Add(organize);
            }
            uniteModel<organize> models = new uniteModel<organize>
            {
                code = 0,
                msg = "组织架构回收站列表查询成功!",
                data = result,
                count = totalCount,
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetDelDataBaseList(int page,int limit,string searchText)
        {
            int totalCount = 0;
            List<t_dataBaseManager> list = databasebll.GetDelListPage(conStr, page, limit, out totalCount, searchText);
            List<c_dataBaseManager> result = new List<c_dataBaseManager>();
            foreach (var item in list)
            {
                c_dataBaseManager database = new c_dataBaseManager();
                database.Id = item.Id;
                database.projectName = item.projectName;
                database.dataBaseAccount = item.dataBaseAccount;
                database.dataBasePwd = item.dataBasePwd;
                database.dataBaseIP = item.dataBaseIP;
                database.dataBaseName = item.dataBaseName;
                database.attributeElectricId = item.attributeElectricId;
                database.attributeLineId = item.attributeLineId;
                database.ElectricName = organizebll.getSingle(conStr, Convert.ToInt32(item.attributeElectricId)).name;
                database.LineName = organizebll.getSingle(conStr, Convert.ToInt32(item.attributeLineId)).name;
                database.isDel = item.isDel;
                database.DelTime = item.DelTime;
                database.DelUser = item.DelUser;
                database.DelUserName = userbll.GetUserById(conStr, (int)item.DelUser).userName;
                database.CreateTime = item.CreateTime;

                result.Add(database);
            }
            uniteModel<c_dataBaseManager> models = new uniteModel<c_dataBaseManager>
            {
                code = 0,
                msg = "数据库配置回收站列表查询成功!",
                data = result,
                count = totalCount,
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetDelDeviceList(int page,int limit,string searchText)
        {
            int totalCount = 0;
            List<t_Device> list = devicebll.GetDelListPage(conStr, page, limit, out totalCount, searchText);
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
                device.isDel = item.isDel;
                device.DelTime = item.DelTime;
                device.DelUser = item.DelUser;
                device.DelUserName = userbll.GetUserById(conStr, (int)item.DelUser).userName;
                result.Add(device);
            }
            uniteModel<c_Device> models = new uniteModel<c_Device>
            {
                code = 0,
                msg = "设备回收站列表查询成功!",
                data = result,
                count = totalCount,
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult reductionData(int typeValue,int Id)
        {
            if (typeValue == 1)
            {
                bool isSuc = userbll.UpdataDel(conStr, Id);
                if (isSuc)
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 0,
                        count = 0,
                        msg = "用户数据还原成功!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 1,
                        count = 0,
                        msg = "用户数据还原失败!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else if (typeValue == 2)
            {
                bool isSuc = organizebll.UpdataDel(conStr, Id);
                if (isSuc)
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 0,
                        count = 0,
                        msg = "组织架构数据还原成功!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 1,
                        count = 0,
                        msg = "组织架构数据还原失败!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else if (typeValue == 3)
            {
                bool isSuc = devicebll.UpdataDel(conStr, Id);
                if (isSuc)
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 0,
                        count = 0,
                        msg = "设备数据还原成功!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 1,
                        count = 0,
                        msg = "设备数据还原失败!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else if (typeValue == 4)
            {
                bool isSuc = databasebll.UpdataDel(conStr, Id);
                if (isSuc)
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 0,
                        count = 0,
                        msg = "数据库配置数据还原成功!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<object> model = new uniteModel<object>
                    {
                        code = 1,
                        count = 0,
                        msg = "数据库配置数据还原失败!",
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                uniteModel<object> model = new uniteModel<object>
                {
                    code = 0,
                    count = 0,
                    msg = "请选择数据类型!",
                    data = null
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}