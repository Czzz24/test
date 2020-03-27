using EF.Application.From.Model;
using EF.Application.From.Model.Alarm;
using EF.Application.Model;
using EF.Application.Model.Alarm;
using EF.Application.Model.Album;
using EF.Application.Model.Custom;
using EF.Application.Model.DataBase;
using EF.Application.Model.Dtos;
using EF.Component.Tools;
using EF.Core.Side;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class AlarmController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_AlarmBLL alarmbll = new t_AlarmBLL();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_dataBaseManagerBLL databasebll = new t_dataBaseManagerBLL();
        private readonly static t_AlarmStatusBLL alarmStatusbll = new t_AlarmStatusBLL();
        private readonly static t_AlarmHandPictureBLL alarmhandPicture = new t_AlarmHandPictureBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendAlarm()
        {
            return View();
        }

        public ActionResult DoAlarm()
        {
            return View();
        }

        public ActionResult EndAlarm()
        {
            return View();
        }

        /// <summary>
        /// 告警处理
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public ActionResult HandleAlarm(long Id, long ElectricId, long LineId, string TerminalId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            t_Alarm items = alarmbll.GetAlarmById(conFstr, (int)Id);
            var item = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, TerminalId);
            AlarmNexus single = new AlarmNexus
            {
                DeviceName = item.deviceName,
                Id = items.Id,
                TerminalId = items.TerminalId,
                Content = items.Content,
                Flag = items.Flag,
                StartTime = items.StartTime,
                AlarmCode = items.AlarmCode,
                CreateTime = items.CreateTime,
                Status = items.Status,
                EndTime = items.EndTime,
                Value = items.Value,
                ElectricId = ElectricId,
                LineId = LineId,
                ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name,
                LineName = organizebll.getSingle(conStr, (int)LineId).name,
                StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status),
                JointId = item.parentId,
                JointName = organizebll.getSingle(conStr, (int)item.parentId).name
            };
            List<t_AlarmStatus> list = alarmStatusbll.GetALL(conFstr);
            List<SelectListItem> alarmStatusSelect = new List<SelectListItem>();
            alarmStatusSelect.Add(new SelectListItem { Disabled = true, Value = "0", Text = "请选择处理状态" });
            foreach (var alarmStatus in list)
            {
                if (single.Status == alarmStatus.Id)
                {
                    SelectListItem select = new SelectListItem()
                    {
                        Selected = true,
                        Value = alarmStatus.Id.ToString(),
                        Text = alarmStatus.StatusName
                    };
                    alarmStatusSelect.Add(select);
                }
                else
                {
                    SelectListItem select = new SelectListItem()
                    {
                        Value = alarmStatus.Id.ToString(),
                        Text = alarmStatus.StatusName
                    };
                    alarmStatusSelect.Add(select);
                }
            }
            ViewData["alarmStatus"] = alarmStatusSelect;
            return View(single);
        }


        /// <summary>
        /// 根据设备终端供电公司和线路Id获取告警数量
        /// </summary>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetAlarmCount(string TerminalId, int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            c_Count model = alarmbll.GetAlarmCount(conFstr, TerminalId);
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 报警历史分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetAlarmPageList(int page, int limit, string TerminalId, int ElectricId, int LineId, DateTime? startTime, DateTime? endTime)
        {
            int totalcount = 0;
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            List<t_Alarm> list = alarmbll.GetAlarm(conFstr, TerminalId, startTime, endTime, page, limit, out totalcount);
            List<AlarmNexus> result = new List<AlarmNexus>();
            var item = devicebll.GetSingleDevice(conStr, ElectricId, LineId, TerminalId);
            string ElectricName = organizebll.getSingle(conStr, ElectricId).name;
            string LineName = organizebll.getSingle(conStr, LineId).name;
            string JointName = organizebll.getSingle(conStr, (int)item.parentId).name;
            foreach (var items in list)
            {
                AlarmNexus single = new AlarmNexus
                {
                    DeviceName = item.deviceName,
                    Id = items.Id,
                    TerminalId = items.TerminalId,
                    Content = items.Content,
                    Flag = items.Flag,
                    StartTime = items.StartTime,
                    AlarmCode = items.AlarmCode,
                    CreateTime = items.CreateTime,
                    Status = items.Status,
                    EndTime = items.EndTime,
                    Value = items.Value,
                    ElectricId = ElectricId,
                    LineId = LineId,
                    ElectricName = ElectricName,
                    LineName = LineName,
                    StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status),
                    JointId = item.parentId,
                    JointName = JointName,
                    Cause = items.Cause,
                    handContent = items.handContent,
                    handEndTime = items.handEndTime,
                    handUser = items.handUser
                };
                result.Add(single);
            }
            uniteModel<AlarmNexus> model = new uniteModel<AlarmNexus>
            {
                code = 0,
                msg = "报警数据列表查询成功!",
                count = totalcount,
                data = result
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认告警
        /// </summary>
        /// <param name="listTerId"></param>
        /// <param name="TerminalId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult confirmAlarm(List<long> listId, string TerminalId, int ElectricId, int LineId)
        {
            int number = 0;
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            foreach (var item in listId)
            {
                number += alarmbll.updateAlarmStatus(conFstr, item);
            }
            uniteModel<object> model = new uniteModel<object>
            {
                code = 0,
                msg = string.Format("确认成功{0}条报警", number.ToString()),
                count = number,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

       
        /// <summary>
        /// 确认告警告警待处理列表
        /// </summary>
        /// <param name="listAlarm"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult confirmAlarmByStatus(List<AlarmNexus> listAlarm)
        {
            int number = 0;
            FromConnection fromCon = new FromConnection();
            foreach (var item in listAlarm)
            {
                string conFstr = fromCon.GetConStr(conStr, (int)item.ElectricId, (int)item.LineId);
                number += alarmbll.updateAlarmStatus(conFstr, item.Id);
            }
            uniteModel<object> model = new uniteModel<object>
            {
                code = 0,
                msg = string.Format("确认成功{0}条报警", number.ToString()),
                count = number,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 告警处理
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="status"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult AlarmHandle(long Id, long ElectricId, long LineId, int status, string Cause, string handContent, string handUser, DateTime? handEndTime, DateTime? endTime, List<long> pictureId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            int flag = 0;
            if (status == 1 || status == 2)
            {
                flag = -1;
            }
            else
            {
                endTime = DateTime.Now;
                flag = 0;
            }
            for(int i = 0; i < pictureId.Count; i++)
            {
                alarmhandPicture.updataAlamHandPicture(conFstr, Id, pictureId[i]);
            }
            int number = alarmbll.AlarmHandToStatus(conFstr, Id, status, flag, endTime, Cause, handContent, handUser, handEndTime);
            uniteModel<object> model = new uniteModel<object>
            {
                code = 0,
                msg = string.Format("处理成功{0}条报警", number.ToString()),
                count = number,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据告警状态获取告警列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="JointId"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="smallTypeId"></param>
        /// <param name="searchText"></param>
        /// <param name="Status"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetAlarmByStatus(int page, int limit, int ElectricId, int LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, int Status,DateTime? startTime,DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            int totalCount = 0;
            List<AlarmNexus> result = new List<AlarmNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, bigTypeId, JointId, smallTypeId, searchText);
                List<t_Alarm> alarmList = alarmbll.GetAlarmByStatus(conFstr, page, limit, Status, startTime, endTime, out totalCount, devicelist);
                foreach (var items in alarmList)
                {
                    var item = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status);
                    string JointName = organizebll.getSingle(conStr, (int)item.parentId).name;
                    AlarmNexus single = new AlarmNexus
                    {
                        DeviceName = item.deviceName,
                        Id = items.Id,
                        TerminalId = items.TerminalId,
                        Content = items.Content,
                        Flag = items.Flag,
                        StartTime = items.StartTime,
                        AlarmCode = items.AlarmCode,
                        CreateTime = items.CreateTime,
                        Status = items.Status,
                        EndTime = items.EndTime,
                        Value = items.Value,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        StatusName = StatusName,
                        JointId = item.parentId,
                        JointName = JointName,
                        Cause=items.Cause,
                        handContent=items.handContent,
                        handEndTime=items.handEndTime,
                        handUser=items.handUser
                    };
                    result.Add(single);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_Alarm> alarmList = alarmbll.GetAlarmByStatus(conFstr, page, limit, Status, startTime, endTime, out totalCount, devicelist);
                foreach (var items in alarmList)
                {
                    var item = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status);
                    string JointName = organizebll.getSingle(conStr, (int)item.parentId).name;
                    AlarmNexus single = new AlarmNexus
                    {
                        DeviceName = item.deviceName,
                        Id = items.Id,
                        TerminalId = items.TerminalId,
                        Content = items.Content,
                        Flag = items.Flag,
                        StartTime = items.StartTime,
                        AlarmCode = items.AlarmCode,
                        CreateTime = items.CreateTime,
                        Status = items.Status,
                        EndTime = items.EndTime,
                        Value = items.Value,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        StatusName = StatusName,
                        JointId = item.parentId,
                        JointName = JointName,
                        Cause = items.Cause,
                        handContent = items.handContent,
                        handEndTime = items.handEndTime,
                        handUser = items.handUser
                    };
                    result.Add(single);
                }
            }

            uniteModel<AlarmNexus> models = new uniteModel<AlarmNexus>
            {
                code = 0,
                msg = "报警数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取告警历史列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="JointId"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="smallTypeId"></param>
        /// <param name="searchText"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual JsonResult GetAlarmHistory(int page, int limit, int ElectricId, int LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            int totalCount = 0;
            List<AlarmNexus> result = new List<AlarmNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, bigTypeId, JointId, smallTypeId, searchText);
                List<t_Alarm> alarmList = alarmbll.GetAlarmHistory(conFstr, page, limit,out totalCount, startTime, endTime, devicelist);
                foreach (var items in alarmList)
                {
                    var item = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status);
                    string JointName = organizebll.getSingle(conStr, (int)item.parentId).name;
                    AlarmNexus single = new AlarmNexus
                    {
                        DeviceName = item.deviceName,
                        Id = items.Id,
                        TerminalId = items.TerminalId,
                        Content = items.Content,
                        Flag = items.Flag,
                        StartTime = items.StartTime,
                        AlarmCode = items.AlarmCode,
                        CreateTime = items.CreateTime,
                        Status = items.Status,
                        EndTime = items.EndTime,
                        Value = items.Value,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        StatusName = StatusName,
                        JointId = item.parentId,
                        JointName = JointName
                    };
                    result.Add(single);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_Alarm> alarmList = alarmbll.GetAlarmHistory(conFstr, page, limit, out totalCount, startTime, endTime, devicelist);
                foreach (var items in alarmList)
                {
                    var item = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string StatusName = alarmbll.GetAlarmsStatus(conFstr, items.Status);
                    string JointName = organizebll.getSingle(conStr, (int)item.parentId).name;
                    AlarmNexus single = new AlarmNexus
                    {
                        DeviceName = item.deviceName,
                        Id = items.Id,
                        TerminalId = items.TerminalId,
                        Content = items.Content,
                        Flag = items.Flag,
                        StartTime = items.StartTime,
                        AlarmCode = items.AlarmCode,
                        CreateTime = items.CreateTime,
                        Status = items.Status,
                        EndTime = items.EndTime,
                        Value = items.Value,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        StatusName = StatusName,
                        JointId = item.parentId,
                        JointName = JointName
                    };
                    result.Add(single);
                }
            }
           
            uniteModel<AlarmNexus> models = new uniteModel<AlarmNexus>
            {
                code = 0,
                msg = "报警数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 告警列表导出
        /// </summary>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="JointId"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="smallTypeId"></param>
        /// <param name="searchText"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public virtual void ExcelExport(int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            t_dataBaseManager model = fromCon.GetModel(conStr, (int)ElectricId, (int)LineId);
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_Device> devicelist;
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                 devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, bigTypeId, JointId, smallTypeId, searchText);
            }
            else
            {
                devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
            }
            string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
            string LineName = organizebll.getSingle(conStr, (int)LineId).name;
            string conFstr = string.Format("server={0};uid={1};pwd={2};database={3};Trusted_Connection=no;connect timeout = 60;", model.dataBaseIP, model.dataBaseAccount, model.dataBasePwd, model.dataBaseName);
            DataTable alarmDtSource = alarmbll.GetAlarmHistory(conFstr, conStr, startTime, endTime, devicelist, ElectricName, LineName, (int)ElectricId, (int)LineId);

            string strHeadText = string.Format("告警数据报表{0}至{1}", startTime, endTime);
            alarmDtSource.Columns["deviceName"].ColumnName = "设备名称";
            alarmDtSource.Columns["TerminalId"].ColumnName = "终端ID";
            alarmDtSource.Columns["ElectricName"].ColumnName = "供电或公司";
            alarmDtSource.Columns["LineName"].ColumnName = "线路名称";
            alarmDtSource.Columns["name"].ColumnName = "接头名称";
            alarmDtSource.Columns["FlagName"].ColumnName = "报警状态";
            alarmDtSource.Columns["StatusName"].ColumnName = "处理状态";
            alarmDtSource.Columns["Value"].ColumnName = "告警值";
            alarmDtSource.Columns["StartTime"].ColumnName = "告警时间";
            alarmDtSource.Columns["EndTime"].ColumnName = "结束时间";
            alarmDtSource.Columns["Content"].ColumnName = "告警详情";

            MemoryStream ms = ExcelHelper.ExportDT_NotUsing(alarmDtSource, strHeadText);
            var FileName = "(告警数据报表)" + Guid.NewGuid().ToString("N");
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
            Response.BinaryWrite(ms.ToArray());
        }

        /// <summary>
        /// 获取告警处理图片
        /// </summary>
        /// <param name="AlarmId"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual JsonResult GetPicture(int AlarmId,int ElectricId, int LineId)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineId);
            List<t_AlarmHandPicture> list = alarmhandPicture.getListByAlarmId(conFstr, AlarmId);
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
                id = AlarmId,
                start = 0,
                title = "告警处理图片",
                data = data
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}