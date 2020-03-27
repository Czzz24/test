using EF.Application.From.Model.Partial;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.From.Model.Page;
using EF.Application.From.Model;
using EF.Application.Model;
using System.Linq.Expressions;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Component.Tools;
using EF.Application.From.Model.Custom;

namespace EF.From.Side
{
    public class t_PartialBLL
    {
        /// <summary>
        /// 获取最新局放数据ID
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual long GetDeviceNewId(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            string sql = "select MAX(Id) as Id from t_Partial where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 最新局放A相通道数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual APartial GetAPassaGeWay(string conStr, string TerminalId)
        {
            long Id = GetDeviceNewId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Id,A1AvgElectric,A1Frequency,A1waveform,A2AvgElectric,A2Frequency,A2waveform,A3AvgElectric,A3Frequency, A3waveform,A4AvgElectric,A4Frequency,A4waveform,A5AvgElectric,A5Frequency,A5waveform from t_Partial where Id=" + Id + "";
            APartial model = dal.QuerySingle<APartial>(sql, null);
            return model;
        }


        /// <summary>
        /// 最新局放B相通道数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual BPartial GetBPassaGeWay(string conStr, string TerminalId)
        {
            long Id = GetDeviceNewId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Id,B1AvgElectric,B1Frequency,B1waveform,B2AvgElectric,B2Frequency,B2waveform,B3AvgElectric,B3Frequency, B3waveform,B4AvgElectric,B4Frequency,B4waveform,B5AvgElectric,B5Frequency,B5waveform from t_Partial where Id=" + Id + "";
            BPartial model = dal.QuerySingle<BPartial>(sql, null);
            return model;
        }


        /// <summary>
        /// 最新局放C相通道数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual CPartial GetCPassaGeWay(string conStr, string TerminalId)
        {
            long Id = GetDeviceNewId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Id,C1AvgElectric,C1Frequency,C1waveform,C2AvgElectric,C2Frequency,C2waveform,C3AvgElectric,C3Frequency, C3waveform,C4AvgElectric,C4Frequency,C4waveform,C5AvgElectric,C5Frequency,C5waveform from t_Partial where Id=" + Id + "";
            CPartial model = dal.QuerySingle<CPartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取局放历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_Partial> GetHistorySingle(string conStr, int pageSize, int currentPage, string TerminalId, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="CreateTime" },
            };
            List<t_Partial> model = dal.GetListPaged<t_Partial>(currentPage, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, orderList).ToList();
            return model;
        }

        /// <summary>
        /// 获取局放历史分析数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<BuiltPartialAnalysis> GetPartialAnalysis(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select AElectricMaxValue,AFrequencyMaxValue,BElectricMaxValue,BFrequencyMaxValue,CElectricMaxValue,CFrequencyMaxValue,CreateTime from t_Partial where deviceId=" + deviceId + " and CreateTime>='" + startTime + "' and CreateTime<='" + endTime + "' order by Id asc";
            List<BuiltPartialAnalysis> list = dal.QueryList<BuiltPartialAnalysis>(sql, null);
            return list;
        }

        /// <summary>
        /// 获取最新局放状态表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual c_Partial GetSingle(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = GetDeviceNewId(conStr, TerminalId);
            string sql = "select Id,AElectricMaxValue,AFrequencyMaxValue,BElectricMaxValue,BFrequencyMaxValue,CElectricMaxValue,CFrequencyMaxValue,Astatus,Bstatus,Cstatus,A1Voltage1,A1Voltage2,A1Voltage3,A2Voltage1,A2Voltage2,A2Voltage3,B1Voltage1,B1Voltage2,B1Voltage3,B2Voltage1,B2Voltage2,B2Voltage3,C1Voltage1,C1Voltage2,C1Voltage3,C2Voltage1,C2Voltage2,C2Voltage3,d3Voltage1,d3Voltage2,d3Voltage3,A1hardversion,A1softversion,A2hardversion,A2softversion,B1hardversion,B1softversion,B2hardversion,B2softversion,C1hardversion,C1softversion,C2hardversion,C2softversion,d3hardversion,d3softversion,CreateTime,status1,status2,status3 from t_Partial where Id=" + Id + "";
            c_Partial model = dal.QuerySingle<c_Partial>(sql, null);
            return model;
        }

        /// <summary>
        /// 局放趋势分析
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<BuilltPartialTrend> GetHistoryTrend(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select AElectricMaxValue,BElectricMaxValue,CElectricMaxValue,CreateTime from t_Partial where deviceId=" + deviceId + " and CreateTime>='" + startTime + "' and CreateTime<='" + endTime + "' order by Id asc";
            List<BuilltPartialTrend> list = dal.QueryList<BuilltPartialTrend>(sql, null);
            return list;
        }


        public virtual List<t_Partial> GetPartialData(string conStr,int pageIndex,int pageSize,out int totalCount, List<t_Device> deviceList,DateTime? startTime,DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Partial> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_Partial, bool>> seleWhere;
            Expression<Func<t_Partial, bool>> seleWhere1 = t => true;
            if (deviceList != null)
            {
                if (deviceList.Count > 0)
                {
                    if (!startTime.HasValue || !endTime.HasValue)
                    {
                        seleWhere = t => true;
                    }
                    else
                    {
                        seleWhere = t => t.CreateTime >= startTime && t.CreateTime <= endTime;
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
                    Expression<Func<t_Partial, bool>> res = seleWhere.AndAlso(seleWhere1);

                    list = dal.GetListPaged<t_Partial>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_Partial>();
                }
            }
            else
            {
                if (!startTime.HasValue || !endTime.HasValue)
                {
                    seleWhere = t => true;
                }
                else
                {
                    seleWhere = t => t.CreateTime >= startTime && t.CreateTime <= endTime;
                }
                list = dal.GetListPaged<t_Partial>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        public virtual DataTable GetPartialData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            SqlParameter[] paras = null;
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select e.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,f.name,b.AStatusName,c.BStatusName,d.CStatusName,a.AElectricMaxValue,a.AFrequencyMaxValue,a.BElectricMaxValue,a.BFrequencyMaxValue,a.CElectricMaxValue,a.CFrequencyMaxValue,a.CreateTime from t_Partial as a inner join t_AStatus as b on a.Astatus=b.StatusId inner join t_BStatus as c on a.Bstatus=c.StatusId inner join t_CStatus as d on a.Cstatus = d.StatusId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as e on a.TerminalId = e.TerminalId inner join openrowset('SQLOLEDB', '" + dataSource + "'; '" + userId + "'; '" + password + "',[" + dataName + "].dbo.t_organize) as f on e.ParentId=f.Id where e.ElectricId=" + ElectricId + " and e.LineId=" + LineId + "");
            if (deviceList != null)
            {
                if (startTime.HasValue && endTime.HasValue)
                {
                    strsql.Append(" and a.CreateTime>=@startTime and a.CreateTime<=@endTime");
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
