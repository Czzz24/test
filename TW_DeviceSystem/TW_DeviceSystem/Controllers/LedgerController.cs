using EF.Application.Model;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Common;
using System.Data;
using System.IO;
using EF.Component.Tools;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class LedgerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();

        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 导出故障设备
        /// </summary>
        public virtual void ExcelExport(int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, int? isError, int? isOnline)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            DataTable resulttable = new DataTable();
            if(loginUser.roleId==1 || loginUser.roleId == 3)
            {
                resulttable = devicebll.GetErrorDeviceTab(conStr, null, ElectricId, LineId, JointId, bigTypeId, smallTypeId, searchText, isError, isOnline);
            }
            else
            {
                resulttable = devicebll.GetErrorDeviceTab(conStr, loginUser.Id, ElectricId, LineId, JointId, bigTypeId, smallTypeId, searchText, isError, isOnline);
            }
            resulttable.Columns["ElectricName"].ColumnName = "供电或公司";
            resulttable.Columns["LineName"].ColumnName = "线路名称";
            resulttable.Columns["JointName"].ColumnName = "接头名称";
            resulttable.Columns["deviceName"].ColumnName = "设备名称";
            resulttable.Columns["TerminalId"].ColumnName = "终端Id";
            resulttable.Columns["bigTypeName"].ColumnName = "设备大类";
            resulttable.Columns["smallTypeName"].ColumnName = "设备小类";
            resulttable.Columns["localInstructions"].ColumnName = "位置说明";
            resulttable.Columns["longitude"].ColumnName = "经度";
            resulttable.Columns["latitude"].ColumnName = "纬度";
            resulttable.Columns["manufacturer"].ColumnName = "生产厂家";
            resulttable.Columns["Installer"].ColumnName = "安装用户";
            resulttable.Columns["InstallDate"].ColumnName = "安装日期";
            resulttable.Columns["simNo"].ColumnName = "simNo卡号";
            resulttable.Columns["isError"].ColumnName = "是否故障";
            resulttable.Columns["isOnline"].ColumnName = "是否在线";
            resulttable.Columns["createTime"].ColumnName = "录入日期";
            string headText = string.Format("故障设备表格导出日期{0}", DateTime.Now);
            MemoryStream ms = ExcelHelper.ExportDT_NotUsing(resulttable, headText);
            var fileName = "故障设备表格" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            Response.BinaryWrite(ms.ToArray());
        }
    }
}