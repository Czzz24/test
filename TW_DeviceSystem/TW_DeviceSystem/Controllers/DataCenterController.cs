using EF.Application.From.Model;
using EF.Application.From.Model.Destroy;
using EF.Application.From.Model.EarthBox;
using EF.Application.From.Model.ErrorLocation;
using EF.Application.From.Model.FreeServicing;
using EF.Application.From.Model.FreeVolt;
using EF.Application.From.Model.outPartial;
using EF.Application.From.Model.Partial;
using EF.Application.From.Model.ThiefLine;
using EF.Application.Model;
using EF.Application.Model.DataBase;
using EF.Application.Model.Dtos;
using EF.Application.Model.EarthBox;
using EF.Application.Model.FreeServicing;
using EF.Application.Model.outPartial;
using EF.Component.Tools;
using EF.Core.Side;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class DataCenterController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_organizeBLL organizebll = new t_organizeBLL();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_earthBoxBLL earthbll = new t_earthBoxBLL();
        private readonly static t_dataBaseManagerBLL databasebll = new t_dataBaseManagerBLL();
        private readonly static t_freeServicingBLL freebll = new t_freeServicingBLL();
        private readonly static t_outPartialBLL outPartialbll = new t_outPartialBLL();
        private readonly static t_PartialBLL partialbll = new t_PartialBLL();
        private readonly static t_freeVoltServicingBLL voltbll = new t_freeVoltServicingBLL();
        private readonly static t_ThiefLineBLL thiefbll = new t_ThiefLineBLL();
        private readonly static t_DestroyBLL desbll = new t_DestroyBLL();
        private readonly static t_errorLocationBLL errorbll = new t_errorLocationBLL();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取接地箱数据
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
        public virtual JsonResult GetEarthBoxData(int page, int limit, int? ElectricId, int? LineId,int? JointId, int? bigTypeId,int? smallTypeId, string searchText,DateTime? startTime,DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<EarthBoxNexus> result = new List<EarthBoxNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId==3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_earthBox> earthboxList = earthbll.GetEarthBoxData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime, 1);
                foreach (var items in earthboxList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    EarthBoxNexus model = new EarthBoxNexus
                    {
                        Id = items.Id,
                        A1Temp = items.A1Temp,
                        A2Temp = items.A2Temp,
                        AElectric = items.AElectric,
                        Alarm = items.Alarm,
                        AVolt = items.AVolt,
                        B1Temp = items.B1Temp,
                        B2Temp = items.B2Temp,
                        LineId = LineId,
                        PTCT = items.PTCT,
                        TerminalId = items.TerminalId,
                        PRS = items.PRS,
                        PBUS = items.PBUS,
                        BatteryVolt = items.BatteryVolt,
                        BElectric = items.BElectric,
                        BoxHumidity = items.BoxHumidity,
                        BoxTemp = items.BoxTemp,
                        BVolt = items.BVolt,
                        C1Temp = items.C1Temp,
                        C2Temp = items.C2Temp,
                        CElectric = items.CElectric,
                        ClockVolt = items.ClockVolt,
                        CreateTime = items.CreateTime,
                        deviceType = items.deviceType,
                        CVolt = items.CVolt,
                        ElectricId = ElectricId,
                        Volt1 = items.Volt1,
                        Volt2 = items.Volt2,
                        Time = items.Time,
                        Volt3 = items.Volt3,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_earthBox> earthboxList = earthbll.GetEarthBoxData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime, 1);
                foreach (var items in earthboxList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    EarthBoxNexus model = new EarthBoxNexus
                    {
                        Id = items.Id,
                        A1Temp = items.A1Temp,
                        A2Temp = items.A2Temp,
                        AElectric = items.AElectric,
                        Alarm = items.Alarm,
                        AVolt = items.AVolt,
                        B1Temp = items.B1Temp,
                        B2Temp = items.B2Temp,
                        LineId = LineId,
                        PTCT = items.PTCT,
                        TerminalId = items.TerminalId,
                        PRS = items.PRS,
                        PBUS = items.PBUS,
                        BatteryVolt = items.BatteryVolt,
                        BElectric = items.BElectric,
                        BoxHumidity = items.BoxHumidity,
                        BoxTemp = items.BoxTemp,
                        BVolt = items.BVolt,
                        C1Temp = items.C1Temp,
                        C2Temp = items.C2Temp,
                        CElectric = items.CElectric,
                        ClockVolt = items.ClockVolt,
                        CreateTime = items.CreateTime,
                        deviceType = items.deviceType,
                        CVolt = items.CVolt,
                        ElectricId = ElectricId,
                        Volt1 = items.Volt1,
                        Volt2 = items.Volt2,
                        Time = items.Time,
                        Volt3 = items.Volt3,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<EarthBoxNexus> models = new uniteModel<EarthBoxNexus>
            {
                code = 0,
                msg = "接地箱数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取测温数据
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
        public virtual JsonResult GetTempData(int page, int limit, long? ElectricId, long? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<EarthBoxNexus> result = new List<EarthBoxNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_earthBox> earthboxList = earthbll.GetEarthBoxData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime, 2);
                foreach (var items in earthboxList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    EarthBoxNexus model = new EarthBoxNexus
                    {
                        Id = items.Id,
                        A1Temp = items.A1Temp,
                        A2Temp = items.A2Temp,
                        AElectric = items.AElectric,
                        Alarm = items.Alarm,
                        AVolt = items.AVolt,
                        B1Temp = items.B1Temp,
                        B2Temp = items.B2Temp,
                        LineId = LineId,
                        PTCT = items.PTCT,
                        TerminalId = items.TerminalId,
                        PRS = items.PRS,
                        PBUS = items.PBUS,
                        BatteryVolt = items.BatteryVolt,
                        BElectric = items.BElectric,
                        BoxHumidity = items.BoxHumidity,
                        BoxTemp = items.BoxTemp,
                        BVolt = items.BVolt,
                        C1Temp = items.C1Temp,
                        C2Temp = items.C2Temp,
                        CElectric = items.CElectric,
                        ClockVolt = items.ClockVolt,
                        CreateTime = items.CreateTime,
                        deviceType = items.deviceType,
                        CVolt = items.CVolt,
                        ElectricId = ElectricId,
                        Volt1 = items.Volt1,
                        Volt2 = items.Volt2,
                        Time = items.Time,
                        Volt3 = items.Volt3,
                        ElectricName =ElectricName,
                        LineName =LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_earthBox> earthboxList = earthbll.GetEarthBoxData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime, 2);
                foreach (var items in earthboxList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    EarthBoxNexus model = new EarthBoxNexus
                    {
                        Id = items.Id,
                        A1Temp = items.A1Temp,
                        A2Temp = items.A2Temp,
                        AElectric = items.AElectric,
                        Alarm = items.Alarm,
                        AVolt = items.AVolt,
                        B1Temp = items.B1Temp,
                        B2Temp = items.B2Temp,
                        LineId = LineId,
                        PTCT = items.PTCT,
                        TerminalId = items.TerminalId,
                        PRS = items.PRS,
                        PBUS = items.PBUS,
                        BatteryVolt = items.BatteryVolt,
                        BElectric = items.BElectric,
                        BoxHumidity = items.BoxHumidity,
                        BoxTemp = items.BoxTemp,
                        BVolt = items.BVolt,
                        C1Temp = items.C1Temp,
                        C2Temp = items.C2Temp,
                        CElectric = items.CElectric,
                        ClockVolt = items.ClockVolt,
                        CreateTime = items.CreateTime,
                        deviceType = items.deviceType,
                        CVolt = items.CVolt,
                        ElectricId = ElectricId,
                        Volt1 = items.Volt1,
                        Volt2 = items.Volt2,
                        Time = items.Time,
                        Volt3 = items.Volt3,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<EarthBoxNexus> models = new uniteModel<EarthBoxNexus>
            {
                code = 0,
                msg = "测温数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取免维护数据
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
        public virtual JsonResult GetFreeServicingData(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<FreeServicingNexus> result = new List<FreeServicingNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_freeServicing> freeList = freebll.GetFreeServicingData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in freeList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    FreeServicingNexus model = new FreeServicingNexus
                    {
                        Id = items.Id,
                        AElectric = items.AElectric,
                        BatteryVolt = items.BatteryVolt,
                        PRS=items.PRS,
                        BElectric=items.BElectric,
                        CElectric=items.CElectric,
                        CreateTime=items.CreateTime,
                        ElectricId=ElectricId,
                        LineId=LineId,
                        TElectric=items.TElectric,
                        TerminalId=items.TerminalId,
                        Time=items.Time,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_freeServicing> freeList = freebll.GetFreeServicingData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in freeList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    FreeServicingNexus model = new FreeServicingNexus
                    {
                        Id = items.Id,
                        AElectric = items.AElectric,
                        BatteryVolt = items.BatteryVolt,
                        PRS = items.PRS,
                        BElectric = items.BElectric,
                        CElectric = items.CElectric,
                        CreateTime = items.CreateTime,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TElectric = items.TElectric,
                        TerminalId = items.TerminalId,
                        Time = items.Time,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<FreeServicingNexus> models = new uniteModel<FreeServicingNexus>
            {
                code = 0,
                msg = "免维护数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取外置局放数据
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
        public virtual JsonResult GetOutPartialData(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<outPartialNexus> result = new List<outPartialNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_outPartial> outPartialList = outPartialbll.GetOutPartialData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in outPartialList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    outPartialNexus model = new outPartialNexus
                    {
                        Id = items.Id,
                        AElectric=items.AElectric,
                        AFrequency=items.AFrequency,
                        AMaxElectric=items.AMaxElectric,
                        AMaxFrequency=items.AMaxFrequency,
                        Astatus=items.Astatus,
                        BElectric=items.BElectric,
                        BFrequency=items.BFrequency,
                        BMaxElectric=items.BMaxElectric,
                        BMaxFrequency=items.BMaxFrequency,
                        Bstatus=items.Bstatus,
                        CElectric=items.CElectric,
                        CFrequency=items.CFrequency,
                        CMaxElectric=items.CMaxElectric,
                        CMaxFrequency=items.CMaxFrequency,
                        CreateTime=items.CreateTime,
                        Cstatus=items.Cstatus,
                        ElectricId=ElectricId,
                        LineId=LineId,
                        TerminalId=items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_outPartial> outPartialList = outPartialbll.GetOutPartialData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in outPartialList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    outPartialNexus model = new outPartialNexus
                    {
                        Id = items.Id,
                        AElectric = items.AElectric,
                        AFrequency = items.AFrequency,
                        AMaxElectric = items.AMaxElectric,
                        AMaxFrequency = items.AMaxFrequency,
                        Astatus = items.Astatus,
                        BElectric = items.BElectric,
                        BFrequency = items.BFrequency,
                        BMaxElectric = items.BMaxElectric,
                        BMaxFrequency = items.BMaxFrequency,
                        Bstatus = items.Bstatus,
                        CElectric = items.CElectric,
                        CFrequency = items.CFrequency,
                        CMaxElectric = items.CMaxElectric,
                        CMaxFrequency = items.CMaxFrequency,
                        CreateTime = items.CreateTime,
                        Cstatus = items.Cstatus,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<outPartialNexus> models = new uniteModel<outPartialNexus>
            {
                code = 0,
                msg = "外置局放数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取免维护环压数据
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
        public virtual JsonResult GetFreeVoltData(int page,int limit,int? ElectricId,int? LineId,int? JointId,int? bigTypeId,int? smallTypeId,string searchText,DateTime? startTime,DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<FreeVoltServiceNexus> result = new List<FreeVoltServiceNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_freeVoltServicing> list = voltbll.GetFreeVoltData(conFstr, page, limit,out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    FreeVoltServiceNexus model = new FreeVoltServiceNexus
                    {
                        Id = items.Id,
                        AVolt = items.AVolt,
                        BVolt = items.BVolt,
                        CVolt = items.CVolt,
                        CPUID = items.CPUID,
                        SoftVersion = items.SoftVersion,
                        PRS = items.PRS,
                        BatteryVolt = items.BatteryVolt,
                        CreateTime = items.CreateTime,
                        TVolt = items.TVolt,
                        Remark1 = items.Remark1,
                        Remark2 = items.Remark2,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_freeVoltServicing> list = voltbll.GetFreeVoltData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach(var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    FreeVoltServiceNexus model = new FreeVoltServiceNexus
                    {
                        Id = items.Id,
                        AVolt=items.AVolt,
                        BVolt=items.BVolt,
                        CVolt=items.CVolt,
                        CPUID=items.CPUID,
                        SoftVersion=items.SoftVersion,
                        PRS=items.PRS,
                        BatteryVolt=items.BatteryVolt,
                        CreateTime=items.CreateTime,
                        TVolt=items.TVolt,
                        Remark1=items.Remark1,
                        Remark2=items.Remark2,
                        Time=items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<FreeVoltServiceNexus> models = new uniteModel<FreeVoltServiceNexus>
            {
                code=0,
                count=totalCount,
                data=result,
                msg="免维护环压数据查询成功"
            };
            return Json(models,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取线缆防盗数据
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
        public virtual JsonResult GetThiefLineData(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<ThiefLineNexus> result = new List<ThiefLineNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_ThiefLine> list = thiefbll.GetThiefLineData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach(var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    ThiefLineNexus model = new ThiefLineNexus
                    {
                        Id=items.Id,
                        SoftVersion=items.SoftVersion,
                        CPUID=items.CPUID,
                        CreateTime=items.CreateTime,
                        Remark1=items.Remark1,
                        Remark2=items.Remark2,
                        Remark3=items.Remark3,
                        xAmplitude=items.xAmplitude,
                        yAmplitude=items.yAmplitude,
                        zAmplitude=items.zAmplitude,
                        Time=items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_ThiefLine> list = thiefbll.GetThiefLineData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    ThiefLineNexus model = new ThiefLineNexus
                    {
                        Id = items.Id,
                        SoftVersion = items.SoftVersion,
                        CPUID = items.CPUID,
                        CreateTime = items.CreateTime,
                        Remark1 = items.Remark1,
                        Remark2 = items.Remark2,
                        Remark3 = items.Remark3,
                        xAmplitude = items.xAmplitude,
                        yAmplitude = items.yAmplitude,
                        zAmplitude = items.zAmplitude,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<ThiefLineNexus> models = new uniteModel<ThiefLineNexus>
            {
                code = 0,
                count = totalCount,
                data = result,
                msg = "线缆防盗数据查询成功"
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 故障定位数据
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
        public virtual JsonResult GetErrorLocationData(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {

            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<ErrorLocationNexus> result = new List<ErrorLocationNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_errorLocation> list = errorbll.GetErrorData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    ErrorLocationNexus model = new ErrorLocationNexus
                    {
                        Id = items.Id,
                        distance=items.distance,
                        latitude=items.latitude,
                        longitude=items.longitude,
                        CreateTime = items.CreateTime,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_errorLocation> list = errorbll.GetErrorData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    ErrorLocationNexus model = new ErrorLocationNexus
                    {
                        Id = items.Id,
                        distance = items.distance,
                        latitude = items.latitude,
                        longitude = items.longitude,
                        CreateTime = items.CreateTime,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<ErrorLocationNexus> models = new uniteModel<ErrorLocationNexus>
            {
                code = 0,
                count = totalCount,
                data = result,
                msg = "故障定位数据查询成功"
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 光纤防外破数据
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
        public virtual JsonResult GetDestroyData(int page, int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<DestroyNexus> result = new List<DestroyNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_Destroy> list = desbll.GetDestroyData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    DestroyNexus model = new DestroyNexus
                    {
                        Id = items.Id,
                        areaone=items.areaone,
                        areatwo=items.areatwo,
                        CreateTime = items.CreateTime,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_Destroy> list = desbll.GetDestroyData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in list)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    DestroyNexus model = new DestroyNexus
                    {
                        Id = items.Id,
                        areaone = items.areaone,
                        areatwo = items.areatwo,
                        CreateTime = items.CreateTime,
                        Time = items.Time,
                        ElectricId = ElectricId,
                        LineId = LineId,
                        TerminalId = items.TerminalId,
                        ElectricName = ElectricName,
                        LineName = LineName,
                        JointId = device.parentId,
                        JointName = JointName,
                        DeviceName = device.deviceName,
                    };
                    result.Add(model);
                }
            }
            uniteModel<DestroyNexus> models = new uniteModel<DestroyNexus>
            {
                code = 0,
                count = totalCount,
                data = result,
                msg = "光纤防外破数据查询成功"
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取内置局放数据
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
        public virtual JsonResult GetPartialData(int page,int limit, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            string conFstr = fromCon.GetConStr(conStr, (int)ElectricId, (int)LineId);
            List<PartialNexus> result = new List<PartialNexus>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            int totalCount = 0;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                List<t_Device> devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
                List<t_Partial> partialList = partialbll.GetPartialData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in partialList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    PartialNexus model = new PartialNexus
                    {
                        status1 = items.status1,
                        status2 = items.status2,
                        status3 = items.status3,
                        A1softversion = items.A1softversion,
                        A2softversion = items.A2softversion,
                        B1softversion = items.B1softversion,
                        B2softversion = items.B2softversion,
                        C1softversion = items.C1softversion,
                        C2softversion = items.C2softversion,
                        d3softversion = items.d3softversion,
                        d3Voltage1 = items.d3Voltage1,
                        A1AvgElectric = items.A1AvgElectric,
                        A1Frequency = items.A1Frequency,
                        A1hardversion = items.A1hardversion,
                        A1Voltage1 = items.A1Voltage1,
                        A1Voltage2 = items.A1Voltage2,
                        A1Voltage3 = items.A1Voltage3,
                        A1waveform = items.A1waveform,
                        A2AvgElectric = items.A2AvgElectric,
                        A2Frequency = items.A2Frequency,
                        A2hardversion = items.A2hardversion,
                        A2Voltage1 = items.A2Voltage1,
                        A2Voltage2 = items.A2Voltage2,
                        A2Voltage3 = items.A2Voltage3,
                        A2waveform = items.A2waveform,
                        A3AvgElectric = items.A3AvgElectric,
                        A3Frequency = items.A3Frequency,
                        A3waveform = items.A3waveform,
                        A4AvgElectric = items.A4AvgElectric,
                        A4Frequency = items.A4Frequency,
                        A4waveform = items.A4waveform,
                        A5AvgElectric = items.A5AvgElectric,
                        A5Frequency = items.A5Frequency,
                        A5waveform = items.A5waveform,
                        AElectricMaxValue = items.AElectricMaxValue,
                        AFrequencyMaxValue = items.AFrequencyMaxValue,
                        Astatus = items.Astatus,
                        B1AvgElectric = items.B1AvgElectric,
                        B2AvgElectric = items.B2AvgElectric,
                        B3AvgElectric = items.B3AvgElectric,
                        B4AvgElectric = items.B4AvgElectric,
                        B5AvgElectric = items.B5AvgElectric,
                        C1AvgElectric = items.C1AvgElectric,
                        C2AvgElectric = items.C2AvgElectric,
                        C3AvgElectric = items.C3AvgElectric,
                        C4AvgElectric = items.C4AvgElectric,
                        C1Frequency = items.C1Frequency,
                        C1hardversion = items.C1hardversion,
                        C1Voltage1 = items.C1Voltage1,
                        C1Voltage2 = items.C1Voltage2,
                        C1Voltage3 = items.C1Voltage3,
                        C1waveform = items.C1waveform,
                        C2Frequency = items.C2Frequency,
                        C2hardversion = items.C2hardversion,
                        C2Voltage1 = items.C2Voltage1,
                        C2Voltage2 = items.C2Voltage2,
                        C2Voltage3 = items.C2Voltage3,
                        C2waveform = items.C2waveform,
                        C3Frequency = items.C3Frequency,
                        C3waveform = items.C3waveform,
                        C4Frequency = items.C4Frequency,
                        C5AvgElectric = items.C5AvgElectric,
                        C5Frequency = items.C5Frequency,
                        CreateTime = items.CreateTime,
                        Bstatus = items.Bstatus,
                        B1Frequency = items.B1Frequency,
                        B1hardversion = items.B1hardversion,
                        B1Voltage1 = items.B1Voltage1,
                        B1Voltage2 = items.B1Voltage2,
                        B1Voltage3 = items.B1Voltage3,
                        B1waveform = items.B1waveform,
                        B2Frequency = items.B2Frequency,
                        B2hardversion = items.B2hardversion,
                        B2Voltage1 = items.B2Voltage1,
                        B2Voltage2 = items.B2Voltage2,
                        B2Voltage3 = items.B2Voltage3,
                        B2waveform = items.B2waveform,
                        B3Frequency = items.B3Frequency,
                        B3waveform = items.B3waveform,
                        B4Frequency = items.B4Frequency,
                        B4waveform = items.B4waveform,
                        B5Frequency = items.B5Frequency,
                        B5waveform = items.B5waveform,
                        BElectricMaxValue = items.BElectricMaxValue,
                        BFrequencyMaxValue = items.BFrequencyMaxValue,
                        C4waveform = items.C4waveform,
                        C5waveform = items.C5waveform,
                        CElectricMaxValue = items.CElectricMaxValue,
                        Id = items.Id,
                        CFrequencyMaxValue = items.CFrequencyMaxValue,
                        Cstatus = items.Cstatus,
                        d3hardversion = items.d3hardversion,
                        d3Voltage2 = items.d3Voltage2,
                        d3Voltage3 = items.d3Voltage3,
                        TerminalId = items.TerminalId,
                        PartialId = items.PartialId,
                        LineId = LineId,
                        ElectricId = ElectricId,
                        JointId = device.parentId,
                        ElectricName = ElectricName,
                        JointName = JointName,
                        LineName = LineName,
                        DeviceName = device.deviceName
                    };
                    result.Add(model);
                }
            }
            else
            {
                List<t_Device> devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
                List<t_Partial> partialList = partialbll.GetPartialData(conFstr, page, limit, out totalCount, devicelist, startTime, endTime);
                foreach (var items in partialList)
                {
                    t_Device device = devicebll.GetSingleDevice(conStr, (int)ElectricId, (int)LineId, items.TerminalId);
                    string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
                    string LineName = organizebll.getSingle(conStr, (int)LineId).name;
                    string JointName = organizebll.getSingle(conStr, (int)device.parentId).name;
                    PartialNexus model = new PartialNexus
                    {
                        status1 = items.status1,
                        status2 = items.status2,
                        status3 = items.status3,
                        A1softversion = items.A1softversion,
                        A2softversion = items.A2softversion,
                        B1softversion = items.B1softversion,
                        B2softversion = items.B2softversion,
                        C1softversion = items.C1softversion,
                        C2softversion = items.C2softversion,
                        d3softversion = items.d3softversion,
                        d3Voltage1 = items.d3Voltage1,
                        A1AvgElectric = items.A1AvgElectric,
                        A1Frequency = items.A1Frequency,
                        A1hardversion = items.A1hardversion,
                        A1Voltage1 = items.A1Voltage1,
                        A1Voltage2 = items.A1Voltage2,
                        A1Voltage3 = items.A1Voltage3,
                        A1waveform = items.A1waveform,
                        A2AvgElectric = items.A2AvgElectric,
                        A2Frequency = items.A2Frequency,
                        A2hardversion = items.A2hardversion,
                        A2Voltage1 = items.A2Voltage1,
                        A2Voltage2 = items.A2Voltage2,
                        A2Voltage3 = items.A2Voltage3,
                        A2waveform = items.A2waveform,
                        A3AvgElectric = items.A3AvgElectric,
                        A3Frequency = items.A3Frequency,
                        A3waveform = items.A3waveform,
                        A4AvgElectric = items.A4AvgElectric,
                        A4Frequency = items.A4Frequency,
                        A4waveform = items.A4waveform,
                        A5AvgElectric = items.A5AvgElectric,
                        A5Frequency = items.A5Frequency,
                        A5waveform = items.A5waveform,
                        AElectricMaxValue = items.AElectricMaxValue,
                        AFrequencyMaxValue = items.AFrequencyMaxValue,
                        Astatus = items.Astatus,
                        B1AvgElectric = items.B1AvgElectric,
                        B2AvgElectric = items.B2AvgElectric,
                        B3AvgElectric = items.B3AvgElectric,
                        B4AvgElectric = items.B4AvgElectric,
                        B5AvgElectric = items.B5AvgElectric,
                        C1AvgElectric = items.C1AvgElectric,
                        C2AvgElectric = items.C2AvgElectric,
                        C3AvgElectric = items.C3AvgElectric,
                        C4AvgElectric = items.C4AvgElectric,
                        C1Frequency = items.C1Frequency,
                        C1hardversion = items.C1hardversion,
                        C1Voltage1 = items.C1Voltage1,
                        C1Voltage2 = items.C1Voltage2,
                        C1Voltage3 = items.C1Voltage3,
                        C1waveform = items.C1waveform,
                        C2Frequency = items.C2Frequency,
                        C2hardversion = items.C2hardversion,
                        C2Voltage1 = items.C2Voltage1,
                        C2Voltage2 = items.C2Voltage2,
                        C2Voltage3 = items.C2Voltage3,
                        C2waveform = items.C2waveform,
                        C3Frequency = items.C3Frequency,
                        C3waveform = items.C3waveform,
                        C4Frequency = items.C4Frequency,
                        C5AvgElectric = items.C5AvgElectric,
                        C5Frequency = items.C5Frequency,
                        CreateTime = items.CreateTime,
                        Bstatus = items.Bstatus,
                        B1Frequency = items.B1Frequency,
                        B1hardversion = items.B1hardversion,
                        B1Voltage1 = items.B1Voltage1,
                        B1Voltage2 = items.B1Voltage2,
                        B1Voltage3 = items.B1Voltage3,
                        B1waveform = items.B1waveform,
                        B2Frequency = items.B2Frequency,
                        B2hardversion = items.B2hardversion,
                        B2Voltage1 = items.B2Voltage1,
                        B2Voltage2 = items.B2Voltage2,
                        B2Voltage3 = items.B2Voltage3,
                        B2waveform = items.B2waveform,
                        B3Frequency = items.B3Frequency,
                        B3waveform = items.B3waveform,
                        B4Frequency = items.B4Frequency,
                        B4waveform = items.B4waveform,
                        B5Frequency = items.B5Frequency,
                        B5waveform = items.B5waveform,
                        BElectricMaxValue = items.BElectricMaxValue,
                        BFrequencyMaxValue = items.BFrequencyMaxValue,
                        C4waveform = items.C4waveform,
                        C5waveform = items.C5waveform,
                        CElectricMaxValue = items.CElectricMaxValue,
                        Id = items.Id,
                        CFrequencyMaxValue = items.CFrequencyMaxValue,
                        Cstatus = items.Cstatus,
                        d3hardversion = items.d3hardversion,
                        d3Voltage2 = items.d3Voltage2,
                        d3Voltage3 = items.d3Voltage3,
                        TerminalId = items.TerminalId,
                        PartialId = items.PartialId,
                        LineId = LineId,
                        ElectricId = ElectricId,
                        JointId = device.parentId,
                        ElectricName = ElectricName,
                        JointName = JointName,
                        LineName = LineName,
                        DeviceName = device.deviceName
                    };
                    result.Add(model);
                }
            }
            uniteModel<PartialNexus> models = new uniteModel<PartialNexus>
            {
                code = 0,
                msg = "内置局放数据列表查询成功!",
                count = totalCount,
                data = result
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 报表导出
        /// </summary>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="JointId"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="smallTypeId"></param>
        /// <param name="searchText"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual void ExcelExport(int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, DateTime? startTime, DateTime? endTime)
        {
            FromConnection fromCon = new FromConnection();
            t_dataBaseManager model = fromCon.GetModel(conStr, (int)ElectricId, (int)LineId);
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_Device> devicelist;
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                devicelist = devicebll.GetDeviceBySuperUser(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, JointId, smallTypeId, searchText);
            }
            else
            {
                devicelist = devicebll.GetDeviceByUserOrganize(conStr, (int)ElectricId, (int)LineId, (int)bigTypeId, (int)loginUser.Id, JointId, smallTypeId, searchText);
            }
            string ElectricName = organizebll.getSingle(conStr, (int)ElectricId).name;
            string LineName = organizebll.getSingle(conStr, (int)LineId).name;
            string conFstr = string.Format("server={0};uid={1};pwd={2};database={3};Trusted_Connection=no;connect timeout = 60;", model.dataBaseIP, model.dataBaseAccount, model.dataBasePwd, model.dataBaseName);
            int bigTypeValue = (int)bigTypeId;
            switch (bigTypeValue)
            {
                case 1:
                    DataTable dtPartialSource = partialbll.GetPartialData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextPartial = string.Format("内置局放数据{0}至{1}", startTime, endTime);
                    dtPartialSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtPartialSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtPartialSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtPartialSource.Columns["LineName"].ColumnName = "线路名称";
                    dtPartialSource.Columns["name"].ColumnName = "接头名称";
                    dtPartialSource.Columns["AStatusName"].ColumnName = "A相状态";
                    dtPartialSource.Columns["BStatusName"].ColumnName = "B相状态";
                    dtPartialSource.Columns["CStatusName"].ColumnName = "C相状态";
                    dtPartialSource.Columns["AElectricMaxValue"].ColumnName = "A相放电量最大值";
                    dtPartialSource.Columns["AFrequencyMaxValue"].ColumnName = "A相放电频率最大值";
                    dtPartialSource.Columns["BElectricMaxValue"].ColumnName = "B相放电量最大值";
                    dtPartialSource.Columns["BFrequencyMaxValue"].ColumnName = "B相放电频率最大值";
                    dtPartialSource.Columns["CElectricMaxValue"].ColumnName = "C相放电量最大值";
                    dtPartialSource.Columns["CFrequencyMaxValue"].ColumnName = "C相放电频率最大值";
                    dtPartialSource.Columns["CreateTime"].ColumnName = "数据录入时间";
                    MemoryStream msPartial = ExcelHelper.ExportDT_NotUsing(dtPartialSource, strHeadTextPartial);
                    var FileNamePartial = "(内置局放数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNamePartial));
                    Response.BinaryWrite(msPartial.ToArray());
                    break;
                case 2:
                    DataTable dtSource = earthbll.GetEarthBoxData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadText = string.Format("接地箱数据{0}至{1}", startTime, endTime);
                    dtSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtSource.Columns["LineName"].ColumnName = "线路名称";
                    dtSource.Columns["name"].ColumnName = "接头名称";
                    dtSource.Columns["Time"].ColumnName = "采集时间";
                    dtSource.Columns["BoxTemp"].ColumnName = "箱内温度";
                    dtSource.Columns["BoxHumidity"].ColumnName = "箱内湿度";
                    dtSource.Columns["AElectric"].ColumnName = "A相电流(A)";
                    dtSource.Columns["BElectric"].ColumnName = "B相电流(A)";
                    dtSource.Columns["CElectric"].ColumnName = "C相电流(A)";
                    dtSource.Columns["AVolt"].ColumnName = "A相电压(V)";
                    dtSource.Columns["BVolt"].ColumnName = "B相电压(V)";
                    dtSource.Columns["CVolt"].ColumnName = "C相电压(V)";
                    dtSource.Columns["Volt1"].ColumnName = "电缆取电电压(V)";
                    dtSource.Columns["Volt2"].ColumnName = "太阳能取电电压(V)";
                    dtSource.Columns["Volt3"].ColumnName = "供电3电压(V)";
                    dtSource.Columns["BatteryVolt"].ColumnName = "铅酸电池电压(V)";
                    dtSource.Columns["ClockVolt"].ColumnName = "时钟电池电压(V)";
                    dtSource.Columns["A1Temp"].ColumnName = "A相接头温度℃";
                    dtSource.Columns["A2Temp"].ColumnName = "A相表皮温度℃";
                    dtSource.Columns["B1Temp"].ColumnName = "B相接头温度℃";
                    dtSource.Columns["B2Temp"].ColumnName = "B相表皮温度℃";
                    dtSource.Columns["C1Temp"].ColumnName = "C相接头温度℃";
                    dtSource.Columns["C2Temp"].ColumnName = "C相表皮温度℃";
                    dtSource.Columns["PTCT"].ColumnName = "PTCT状态字";
                    dtSource.Columns["PBUS"].ColumnName = "PBUS状态字";
                    dtSource.Columns["Alarm"].ColumnName = "报警状态字";
                    dtSource.Columns["PRS"].ColumnName = "GPRS信号强度";
                    dtSource.Columns["CreateTime"].ColumnName = "数据录入时间";
                    MemoryStream ms = ExcelHelper.ExportDT_NotUsing(dtSource, strHeadText);
                    var FileName = "(接地箱数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
                    Response.BinaryWrite(ms.ToArray());
                    break;
                case 3:
                    DataTable dtFreeSource = freebll.GetFreeServicingData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextFree = string.Format("免维护数据{0}至{1}", startTime, endTime);
                    dtFreeSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtFreeSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtFreeSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtFreeSource.Columns["LineName"].ColumnName = "线路名称";
                    dtFreeSource.Columns["name"].ColumnName = "接头名称";
                    dtFreeSource.Columns["Time"].ColumnName = "采集时间";
                    dtFreeSource.Columns["AElectric"].ColumnName = "A相电流(A)";
                    dtFreeSource.Columns["BElectric"].ColumnName = "B相电流(A)";
                    dtFreeSource.Columns["CElectric"].ColumnName = "C相电流(A)";
                    dtFreeSource.Columns["TElectric"].ColumnName = "总电流(A)";
                    dtFreeSource.Columns["BatteryVolt"].ColumnName = "蓄电池电压(V)";
                    dtFreeSource.Columns["PRS"].ColumnName = "信号强度";
                    dtFreeSource.Columns["CreateTime"].ColumnName = "数据录入时间";
                    MemoryStream msFree = ExcelHelper.ExportDT_NotUsing(dtFreeSource, strHeadTextFree);
                    var FileNameFree = "(免维护数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameFree));
                    Response.BinaryWrite(msFree.ToArray());
                    break;
                case 4:
                    DataTable dtTempSource = earthbll.GetEarthBoxTempData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextTemp = string.Format("测温数据{0}至{1}", startTime, endTime);
                    dtTempSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtTempSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtTempSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtTempSource.Columns["LineName"].ColumnName = "线路名称";
                    dtTempSource.Columns["name"].ColumnName = "接头名称";
                    dtTempSource.Columns["Time"].ColumnName = "采集时间";
                    dtTempSource.Columns["A1Temp"].ColumnName = "A相接头温度℃";
                    dtTempSource.Columns["B1Temp"].ColumnName = "B相接头温度℃";
                    dtTempSource.Columns["C1Temp"].ColumnName = "C相接头温度℃";
                    dtTempSource.Columns["A2Temp"].ColumnName = "A相表皮温度℃";
                    dtTempSource.Columns["B2Temp"].ColumnName = "B相表皮温度℃";
                    dtTempSource.Columns["C2Temp"].ColumnName = "C相表皮温度℃";
                    dtTempSource.Columns["Volt1"].ColumnName = "电缆取电电压(V)";
                    dtTempSource.Columns["Volt2"].ColumnName = "太阳能取电电压(V)";
                    dtTempSource.Columns["BatteryVolt"].ColumnName = "铅酸电池电压(V)";
                    dtTempSource.Columns["CreateTime"].ColumnName = "数据录入时间";
                    MemoryStream msTemp = ExcelHelper.ExportDT_NotUsing(dtTempSource, strHeadTextTemp);
                    var FileNameTemp = "(测温数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameTemp));
                    Response.BinaryWrite(msTemp.ToArray());
                    break;
                case 5:
                    DataTable dtOutPartialSource = outPartialbll.GetOutPartialData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextOutPartial = string.Format("外置局放数据{0}至{1}", startTime, endTime);
                    dtOutPartialSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtOutPartialSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtOutPartialSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtOutPartialSource.Columns["LineName"].ColumnName = "线路名称";
                    dtOutPartialSource.Columns["name"].ColumnName = "接头名称";
                    dtOutPartialSource.Columns["AStatusName"].ColumnName = "A相状态";
                    dtOutPartialSource.Columns["BStatusName"].ColumnName = "B相状态";
                    dtOutPartialSource.Columns["CStatusName"].ColumnName = "C相状态";
                    dtOutPartialSource.Columns["AMaxElectric"].ColumnName = "A相最大放电量";
                    dtOutPartialSource.Columns["AMaxFrequency"].ColumnName = "A相最大放电次数";
                    dtOutPartialSource.Columns["BMaxElectric"].ColumnName = "B相最大放电";
                    dtOutPartialSource.Columns["BMaxFrequency"].ColumnName = "B相最大放电次数";
                    dtOutPartialSource.Columns["CMaxElectric"].ColumnName = "C相最大放电";
                    dtOutPartialSource.Columns["CMaxFrequency"].ColumnName = "C相最大放电次数";
                    dtOutPartialSource.Columns["CreateTime"].ColumnName = "数据录入时间";
                    MemoryStream msOutPartial = ExcelHelper.ExportDT_NotUsing(dtOutPartialSource, strHeadTextOutPartial);
                    var FileNameOutPartial = "(外置局放数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameOutPartial));
                    Response.BinaryWrite(msOutPartial.ToArray());
                    break;
                case 6:
                    DataTable dtVoltSource = voltbll.GetFreeVoltData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextVolt = string.Format("环压数据{0}至{1}", startTime, endTime);
                    dtVoltSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtVoltSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtVoltSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtVoltSource.Columns["LineName"].ColumnName = "线路名称";
                    dtVoltSource.Columns["name"].ColumnName = "接头名称";
                    dtVoltSource.Columns["Time"].ColumnName = "采集时间";
                    dtVoltSource.Columns["AVolt"].ColumnName = "A相电压";
                    dtVoltSource.Columns["BVolt"].ColumnName = "B相电压";
                    dtVoltSource.Columns["CVolt"].ColumnName = "C相电压";
                    dtVoltSource.Columns["TVolt"].ColumnName = "总电压";
                    dtVoltSource.Columns["BatteryVolt"].ColumnName = "电池电压";
                    dtVoltSource.Columns["SoftVersion"].ColumnName = "软件版本号";
                    dtVoltSource.Columns["CPUID"].ColumnName = "CPUID";
                    dtVoltSource.Columns["PRS"].ColumnName = "信号强度";
                    dtVoltSource.Columns["CreateTime"].ColumnName = "录入时间";
                    dtVoltSource.Columns["Remark1"].ColumnName = "备注一";
                    dtVoltSource.Columns["Remark2"].ColumnName = "备注二";
                    MemoryStream msVolt = ExcelHelper.ExportDT_NotUsing(dtVoltSource, strHeadTextVolt);
                    var FileNameVolt = "(环压数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameVolt));
                    Response.BinaryWrite(msVolt.ToArray());
                    break;
                case 7:
                    DataTable dtthiefSource = thiefbll.GetThiefLineData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextThief = string.Format("线缆防盗{0}至{1}", startTime, endTime);
                    dtthiefSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtthiefSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtthiefSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtthiefSource.Columns["LineName"].ColumnName = "线路名称";
                    dtthiefSource.Columns["name"].ColumnName = "接头名称";
                    dtthiefSource.Columns["Time"].ColumnName = "采集时间";
                    dtthiefSource.Columns["xAmplitude"].ColumnName = "X轴振动值";
                    dtthiefSource.Columns["yAmplitude"].ColumnName = "Y轴振动值";
                    dtthiefSource.Columns["zAmplitude"].ColumnName = "Z轴振动值";
                    dtthiefSource.Columns["SoftVersion"].ColumnName = "软件版本号";
                    dtthiefSource.Columns["CPUID"].ColumnName = "CPUID";
                    dtthiefSource.Columns["CreateTime"].ColumnName = "录入时间";
                    dtthiefSource.Columns["Remark1"].ColumnName = "备注一";
                    dtthiefSource.Columns["Remark2"].ColumnName = "备注二";
                    dtthiefSource.Columns["Remark3"].ColumnName = "备注三";
                    MemoryStream msThief = ExcelHelper.ExportDT_NotUsing(dtthiefSource, strHeadTextThief);
                    var FileNameThief = "(线缆防盗数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameThief));
                    Response.BinaryWrite(msThief.ToArray());
                    break;
                case 8:
                    DataTable dtDestroySource = desbll.GetDestroyData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextDes= string.Format("光纤防外破{0}至{1}", startTime, endTime);
                    dtDestroySource.Columns["deviceName"].ColumnName = "设备名称";
                    dtDestroySource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtDestroySource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtDestroySource.Columns["LineName"].ColumnName = "线路名称";
                    dtDestroySource.Columns["name"].ColumnName = "接头名称";
                    dtDestroySource.Columns["Time"].ColumnName = "采集时间";
                    dtDestroySource.Columns["areaone"].ColumnName = "防区一";
                    dtDestroySource.Columns["areatwo"].ColumnName = "防区二";
                    dtDestroySource.Columns["CreateTime"].ColumnName = "录入时间";
                    MemoryStream msDes = ExcelHelper.ExportDT_NotUsing(dtDestroySource, strHeadTextDes);
                    var FileNameDes = "(光纤防外破数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameDes));
                    Response.BinaryWrite(msDes.ToArray());
                    break;
                case 9:
                    DataTable dtErrorSource = desbll.GetDestroyData(conFstr, conStr, devicelist, startTime, endTime, ElectricName, LineName, (int)ElectricId, (int)LineId);
                    string strHeadTextError= string.Format("故障定位{0}至{1}", startTime, endTime);
                    dtErrorSource.Columns["deviceName"].ColumnName = "设备名称";
                    dtErrorSource.Columns["TerminalId"].ColumnName = "终端ID";
                    dtErrorSource.Columns["ElectricName"].ColumnName = "供电或公司";
                    dtErrorSource.Columns["LineName"].ColumnName = "线路名称";
                    dtErrorSource.Columns["name"].ColumnName = "接头名称";
                    dtErrorSource.Columns["Time"].ColumnName = "采集时间";
                    dtErrorSource.Columns["latitude"].ColumnName = "基点纬度";
                    dtErrorSource.Columns["longitude"].ColumnName = "基点经度";
                    dtErrorSource.Columns["distance"].ColumnName = "距离";
                    dtErrorSource.Columns["CreateTime"].ColumnName = "录入时间";
                    MemoryStream msError = ExcelHelper.ExportDT_NotUsing(dtErrorSource, strHeadTextError);
                    var FileNameError = "(故障定位数据)" + Guid.NewGuid().ToString("N");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileNameError));
                    Response.BinaryWrite(msError.ToArray());
                    break;
                default:
                    break;
            }
        }
    }
}