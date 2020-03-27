using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.From.Model.Echarts;
using EF.Application.Model.EarthBox;
using EF.Application.Model.DataBase;
using EF.Application.Model.Custom;
using EF.Application.Model;
using System.Linq.Expressions;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Application.From.Model.EarthBox;
using EF.Component.Tools;
using EF.Application.From.Model.Custom;

namespace EF.From.Side
{
    public class t_earthBoxBLL
    {
        /// <summary>
        /// 获取模型Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual long getId(string conStr,string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            string sql = "select max(Id) as Id from t_earthBox where deviceId="+ deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;

        }

        /// <summary>
        /// 获取接地箱设备历史数据分页
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public virtual List<t_earthBox> GetListByPage(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, out int totalcount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" },
            };
            if (startTime.HasValue && endTime.HasValue)
            {
                List<t_earthBox> list = dal.GetListPaged<t_earthBox>(pageIndex, pageSize, t => t.deviceId == deviceId && t.Time >= startTime && t.Time <= endTime, out totalcount, orderList);
                return list;
            }
            else
            {
                List<t_earthBox> list = dal.GetListPaged<t_earthBox>(pageIndex, pageSize, t => t.deviceId == deviceId, out totalcount, orderList);
                return list;
            }
        }

        /// <summary>
        /// 查询历史图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<EarthBoxLine> GetEarthBoxChartData(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select BoxTemp,BoxHumidity,AElectric,BElectric,CElectric,AVolt,BVolt,CVolt,BatteryVolt,Volt1,Volt2,Time from t_earthBox where deviceId=@deviceId and CreateTime>=@startTime and CreateTime<=@endTime order by Time";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<EarthBoxLine> list = dal.QueryList<EarthBoxLine>(sql, paras);
            return list;
        }

        /// <summary>
        /// 获取带测温接地箱历史图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<TempEarthBoxLine> GetTempEarthBoxChartData(string conStr,string TerminalId,DateTime? startTime,DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select BoxTemp,BoxHumidity,AElectric,BElectric,CElectric,AVolt,BVolt,CVolt,A1Temp,B1Temp,C1Temp,A2Temp,B2Temp,C2Temp,BatteryVolt,Volt1,Volt2,Time from t_earthBox where TerminalId=@TerminalId and Time>=@startTime and Time<=@endTime order by Time";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@TerminalId",TerminalId)
            };
            List<TempEarthBoxLine> list = dal.QueryList<TempEarthBoxLine>(sql, paras);
            return list;
        }

        /// <summary>
        /// 获取温度数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public virtual t_earthBox GetSingle(string conStr, string TerminalId)
        {
            long Id = getId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql= "SELECT * from t_earthBox where Id=" + Id + "";
            t_earthBox model = dal.QuerySingle<t_earthBox>(sql, null);
            return model;
        }


        public virtual List<t_earthBox> GetEarthBoxData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, int deviceType)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_earthBox> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_earthBox, bool>> seleWhere;
            Expression<Func<t_earthBox, bool>> seleWhere1 = t => true;
            if (deviceList != null)
            {
                if (deviceList.Count > 0)
                {
                    if (!startTime.HasValue || !endTime.HasValue)
                    {
                        seleWhere = t => t.deviceType == deviceType;
                    }
                    else
                    {
                        seleWhere = t => t.deviceType == deviceType && t.Time >= startTime && t.Time <= endTime;
                    }
                    for (int i = 0; i < deviceList.Count; i++)
                    {
                        long? deviceId = deviceList[i].deviceId;
                        if (i == 0)
                        {
                            seleWhere1 = seleWhere1.AndAlso(t => t.deviceId == deviceId);
                        }
                        else
                        {
                            seleWhere1 = seleWhere1.OrElse(t => t.deviceId == deviceId);
                        }
                    }
                    Expression<Func<t_earthBox, bool>> res = seleWhere.AndAlso(seleWhere1);

                    list = dal.GetListPaged<t_earthBox>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_earthBox>();
                }
            }
            else
            {
                if (!startTime.HasValue || !endTime.HasValue)
                {
                    seleWhere = t => t.deviceType == deviceType;
                }
                else
                {
                    seleWhere = t => t.deviceType == deviceType && t.Time >= startTime && t.Time <= endTime;
                }
                list = dal.GetListPaged<t_earthBox>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        public virtual DataTable GetEarthBoxData(string conStr,string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime,string ElectricName,string LineName,int ElectricId,int LineId)
        {
            SqlHelper helper = new SqlHelper(conStr);
            string[] arycon = mainConStr.Split(';');
            int i, li_index;
            string dataSource=null, dataName=null, userId=null, password=null;
            for (i = 0; i < arycon.Length; i++)
            {
                if (arycon[i].IndexOf("data source") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataSource = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("initial catalog") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataName = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("user id") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    userId = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("password") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    password = arycon[i].Substring(li_index + 1);
                }
            }
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.BoxTemp,a.BoxHumidity,a.AElectric,a.BElectric,a.CElectric,a.AVolt,a.BVolt,a.CVolt,a.Volt1,a.Volt2,a.Volt3,a.BatteryVolt,a.ClockVolt, a.A1Temp, a.A2Temp, a.B1Temp, a.B2Temp, a.C1Temp, a.C2Temp, a.PTCT, a.PBUS, a.Alarm, a.PRS, a.CreateTime from t_earthBox as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where a.deviceType=1 and b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
            SqlParameter[] paras = null;
            if (deviceList != null)
            {
                if (startTime.HasValue && endTime.HasValue)
                {
                    strsql.Append(" and a.Time>=@startTime and a.Time<=@endTime");
                    paras = new SqlParameter[]
                    {
                        new SqlParameter("@startTime",startTime),
                        new SqlParameter("@endTime",endTime)
                    };
                }
                if (deviceList.Count > 0)
                {
                    for(int j = 0; j < deviceList.Count; j++)
                    {
                        if (j == 0)
                        {
                            strsql.Append(" and (a.deviceId=" + deviceList[j].deviceId + "");
                        }
                        else
                        {
                            strsql.Append(" or a.deviceId=" + deviceList[j].deviceId + "");
                        }
                    }
                    strsql.Append(")");
                }
                
                string sql = strsql.ToString();
                DataTable table = helper.ExcuteDataTable(sql, paras);
                return table;
            }
            else
            {
                return null;
            }
        }

        public virtual DataTable GetEarthBoxTempData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
        {
            SqlHelper helper = new SqlHelper(conStr);
            string[] arycon = mainConStr.Split(';');
            int i, li_index;
            string dataSource = null, dataName = null, userId = null, password = null;
            for (i = 0; i < arycon.Length; i++)
            {
                if (arycon[i].IndexOf("data source") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataSource = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("initial catalog") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataName = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("user id") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    userId = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("password") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    password = arycon[i].Substring(li_index + 1);
                }
            }
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.A1Temp,a.B1Temp,a.C1Temp,a.A2Temp,a.B2Temp,a.C2Temp,a.Volt1,a.Volt2,a.BatteryVolt,a.CreateTime from t_earthBox as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where a.deviceType=2 and b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
            SqlParameter[] paras = null;
            if (deviceList != null)
            {
                if (startTime.HasValue && endTime.HasValue)
                {
                    strsql.Append(" and a.Time>=@startTime and a.Time<=@endTime");
                    paras = new SqlParameter[]
                    {
                        new SqlParameter("@startTime",startTime),
                        new SqlParameter("@endTime",endTime)
                    };
                }
                if (deviceList.Count > 0)
                {
                    for (int j = 0; j < deviceList.Count; j++)
                    {
                        if (j == 0)
                        {
                            strsql.Append(" and (a.deviceId=" + deviceList[j].deviceId + "");
                        }
                        else
                        {
                            strsql.Append(" or a.deviceId=" + deviceList[j].deviceId + "");
                        }
                    }
                    strsql.Append(")");
                }
                string sql = strsql.ToString();
                DataTable table = helper.ExcuteDataTable(sql, paras);
                return table;
            }
            else
            {
                return null;
            }
        }
    }
}
