using EF.Application.Model;
using EF.Application.Model.Album;
using EF.Application.Model.Custom;
using EF.Application.Model.Dtos;
using EF.Application.Model.Maintain;
using EF.Core.Side;
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
    public class MaintainController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_MaintainBLL maintainbll = new t_MaintainBLL();
        private readonly static t_MaintainPictureBLL maintainpicturebll = new t_MaintainPictureBLL();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_deviceBigTypeBLL bigtypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smalltypebll = new t_deviceSmallTypeBLL();
        private readonly static t_userInfoBLL userbll = new t_userInfoBLL();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int Id)
        {
            t_Device device = devicebll.GetSingle(conStr, Id);
            ViewBag.TerminalId = device.TerminalId;
            ViewBag.deviceName = device.deviceName;
            ViewBag.deviceBigTypeId = device.bigTypeId;
            ViewBag.deviceSmallTypeId = device.smallTypeId;
            ViewBag.ElectricName = organizebll.getSingle(conStr, (int)device.ElectricId).name;
            ViewBag.LineName = organizebll.getSingle(conStr, (int)device.LineId).name;
            ViewBag.JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
            return View();
        }

        public ActionResult List(int ElectricId,int LineId,string TerminalId,int JointId)
        {
            ViewBag.ElectricId = ElectricId;
            ViewBag.LineId = LineId;
            ViewBag.TerminalId = TerminalId;
            ViewBag.JointId = JointId;
            return View();
        }

        /// <summary>
        /// 维护信息添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult conmitAdd(t_Maintain model,List<long> pictureId)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            model.userId = loginUser.Id;
            model.CreateTime = DateTime.Now;
            bool isAdd = maintainbll.Add(conStr, model);
            if (isAdd)
            {
                int upNum = 0;
                if (pictureId != null)
                {
                    for (int i = 0; i < pictureId.Count; i++)
                    {
                        upNum += maintainpicturebll.updateMaintainPicture(conStr, model.Id, pictureId[i]);
                    }
                }
                uniteModel<t_Maintain> models = new uniteModel<t_Maintain>
                {
                    code = 0,
                    msg = "添加维护信息成功!",
                    count = upNum,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<t_Maintain> models = new uniteModel<t_Maintain>
                {
                    code = 1,
                    msg = "添加维护信息失败!",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取维护历史
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual JsonResult getPageList(int page,int limit,int ElectricId,int LineId,int JointId,string TerminalId)
        {
            int totalcount = 0;
            List<t_Maintain> list = maintainbll.GetPage(conStr, page, limit, out totalcount, TerminalId);
            List<Maintain> result = new List<Maintain>();
            t_organize organizeElectric = organizebll.getSingle(conStr, ElectricId);
            t_organize organizeLine = organizebll.getSingle(conStr, LineId);
            t_organize organizeJoint = organizebll.getSingle(conStr, JointId);
            t_Device device = devicebll.GetSingleDevice(conStr, ElectricId, LineId, TerminalId);
            t_deviceBigType devicebig = bigtypebll.GetSingle(conStr, (int)device.bigTypeId);
            t_deviceSmallType devicesmall = smalltypebll.GetSingle(conStr, (int)device.smallTypeId);
            foreach (var maintain in list)
            {
                t_userInfo user = userbll.GetUserById(conStr, (int)maintain.userId);
                Maintain maintainModel = new Maintain
                {
                    deviceName= device.deviceName,
                    CreateTime = maintain.CreateTime,
                    ElectricName = organizeElectric.name,
                    LineName = organizeLine.name,
                    JointName = organizeJoint.name,
                    failureCause = maintain.failureCause,
                    Id = maintain.Id,
                    TerminalId = maintain.TerminalId,
                    userId = maintain.userId,
                    userName = user.userName,
                    bigTypeName= devicebig.typeName,
                    smallTypeName= devicesmall.typeName
                };
                result.Add(maintainModel);
            }
            uniteModel<Maintain> model = new uniteModel<Maintain>
            {
                code = 0,
                msg = "功能列表查询成功!",
                count = totalcount,
                data = result
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetPicture(int maintainId)
        {
            List<t_MaintainPicture> list = maintainpicturebll.GetListByMaintainId(conStr, maintainId);
            List<AlbumContent> data = new List<AlbumContent>();
            foreach (var item in list)
            {
                AlbumContent content = new AlbumContent
                {
                    alt = item.fileName,
                    pid = (int)item.Id,
                    src = item.serverPath + item.filePath,
                    thumb = item.serverPath + item.filePath,
                };
                data.Add(content);
            }
            Album model = new Album
            {
                id = maintainId,
                start = 0,
                title = "故障图片",
                data = data
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}