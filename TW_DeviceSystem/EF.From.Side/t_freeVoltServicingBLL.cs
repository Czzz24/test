using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.From.Model.FreeVolt;
using EF.Application.Model;
using System.Linq.Expressions;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Component.Tools;
using EF.Application.From.Model.Custom;

namespace EF.From.Side
{
    public class t_freeVoltServicingBLL
    {


        /// <summary>
        /// 获取模型Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual long getId(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            string sql = "select max(Id) as Id from t_freeVoltServicing where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 查询免维护环压历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_freeVoltServicing> GetHistoryData(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };

            if (startTime.HasValue && !endTime.HasValue)
            {
                List<t_freeVoltServicing> list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && endTime.HasValue)
            {
                List<t_freeVoltServicing> list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && !endTime.HasValue)
            {
                List<t_freeVoltServicing> list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, t => t.deviceId == deviceId, out totalCount, order);
                return list;
            }
            else
            {
                List<t_freeVoltServicing> list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 查询最新免维护环压数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_freeVoltServicing GetSingle(string conStr, string TerminalId)
        {
            long Id = getId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_freeVoltServicing where Id="+ Id + "";
            t_freeVoltServicing model= dal.QuerySingle<t_freeVoltServicing>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取免维护环压图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<FreeVoltServiceChart> GetChartData(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Time,AVolt,AVolt,BVolt,CVolt,TVolt,BatteryVolt,PRS from t_freeVoltServicing where deviceId=@deviceId and Time >=@startTime and Time<=@endTime order by Time";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<FreeVoltServiceChart> list = dal.QueryList<FreeVoltServiceChart>(sql, paras);
            return list;
        }

        /// <summary>
        /// 免维护环压数据中心数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="deviceList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_freeVoltServicing> GetFreeVoltData(string conStr,int pageIndex,int pageSize,out int totalCount,List<t_Device> deviceList,DateTime? startTime,DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_freeVoltServicing> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_freeVoltServicing, bool>> seleWhere;
            Expression<Func<t_freeVoltServicing, bool>> seleWhere1 = t => true;
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
                    Expression<Func<t_freeVoltServicing, bool>> res = seleWhere.AndAlso(seleWhere1);
                    list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_freeVoltServicing>();
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
                list = dal.GetListPaged<t_freeVoltServicing>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 线缆电压数据导出
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="mainConStr"></param>
        /// <param name="deviceList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ElectricName"></param>
        /// <param name="LineName"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual DataTable GetFreeVoltData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.AVolt,a.BVolt,a.CVolt,a.TVolt,a.BatteryVolt,a.SoftVersion,a.CPUID,a.PRS,a.CreateTime,a.Remark1,a.Remark2 from t_freeVoltServicing as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
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
