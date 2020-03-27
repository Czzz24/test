using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.From.Model.Echarts;
using EF.Application.Model.DataBase;
using EF.Application.Model;
using EF.Application.Model.FreeServicing;
using EF.Application.Model.Custom;
using System.Linq.Expressions;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Application.From.Model.FreeServicing;
using EF.Component.Tools;
using EF.Application.From.Model.Custom;

namespace EF.From.Side
{
    public class t_freeServicingBLL
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
            string sql = "select max(Id) as Id from t_freeServicing where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 查询免维护历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_freeServicing> GetHistoryData(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };
            if(startTime.HasValue && !endTime.HasValue)
            {
                List<t_freeServicing> list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime, out totalCount, order);
                return list;
            }
            if(!startTime.HasValue && endTime.HasValue)
            {
                List<t_freeServicing> list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
            if(!startTime.HasValue && !endTime.HasValue)
            {
                List<t_freeServicing> list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, t => t.deviceId == deviceId, out totalCount, order);
                return list;
            }
            else
            {
                List<t_freeServicing> list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 获取免维护即时数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_freeServicing GetSingle(string conStr, string TerminalId)
        {
            long Id = getId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_freeServicing where Id=" + Id + "";
            t_freeServicing model = dal.QuerySingle<t_freeServicing>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取免维护图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<FreeServicChart> GetChartData(string conStr,string TerminalId,DateTime? startTime,DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select AElectric,BElectric,CElectric,TElectric,BatteryVolt,PRS,Time from t_freeServicing where deviceId=@deviceId and Time >=@startTime and Time<=@endTime order by Time";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<FreeServicChart> list = dal.QueryList<FreeServicChart>(sql, paras);
            return list;
        }

        public virtual List<t_freeServicing> GetFreeServicingData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_freeServicing> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_freeServicing, bool>> seleWhere;
            Expression<Func<t_freeServicing, bool>> seleWhere1 = t => true;

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
                        seleWhere = t => t.Time >= startTime && t.Time <= endTime;
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
                    Expression<Func<t_freeServicing, bool>> res = seleWhere.AndAlso(seleWhere1);
                    list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_freeServicing>();
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
                    seleWhere = t => t.Time >= startTime && t.Time <= endTime;
                }
                list = dal.GetListPaged<t_freeServicing>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        public virtual DataTable GetFreeServicingData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.AElectric,a.BElectric,a.CElectric,a.TElectric,a.BatteryVolt,a.PRS,a.CreateTime from t_freeServicing as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
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
