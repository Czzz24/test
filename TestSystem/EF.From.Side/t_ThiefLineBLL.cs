using EF.Application.From.Model;
using EF.Application.From.Model.ThiefLine;
using EF.Application.Model;
using EF.Component.Data.EFHelper;
using EF.Component.Data.SQLHelper;
using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EF.From.Side
{
    public class t_ThiefLineBLL
    {
        /// <summary>
        /// 查询线缆防盗历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_ThiefLine> GetHistoryData(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };
            if (startTime.HasValue && !endTime.HasValue)
            {
                List<t_ThiefLine> list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && endTime.HasValue)
            {
                List<t_ThiefLine> list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && !endTime.HasValue)
            {
                List<t_ThiefLine> list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, t => t.deviceId == deviceId, out totalCount, order);
                return list;
            }
            else
            {
                List<t_ThiefLine> list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 查询最新线缆防盗数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_ThiefLine GetSingle(string conStr,string TerminalId)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_ThiefLine where deviceId=@deviceId order by Id desc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@deviceId",deviceId)
            };
            t_ThiefLine model = dal.QuerySingle<t_ThiefLine>(sql, paras);
            return model;
        }

        /// <summary>
        /// 查询线缆防盗历史图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<ThiefLine> GetChartData(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Time,xAmplitude,yAmplitude,zAmplitude from t_ThiefLine where deviceId=@deviceId and Time >=@startTime and Time<=@endTime order by Time asc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<ThiefLine> list = dal.QueryList<ThiefLine>(sql, paras);
            return list;
        }

        /// <summary>
        /// 线缆防盗数据中心数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="deviceList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_ThiefLine> GetThiefLineData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_ThiefLine> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_ThiefLine, bool>> seleWhere;
            Expression<Func<t_ThiefLine, bool>> seleWhere1 = t => true;
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
                    Expression<Func<t_ThiefLine, bool>> res = seleWhere.AndAlso(seleWhere1);
                    list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_ThiefLine>();
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
                list = dal.GetListPaged<t_ThiefLine>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 线缆防盗数据导出
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
        public virtual DataTable GetThiefLineData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.xAmplitude,a.yAmplitude,a.zAmplitude,a.SoftVersion,a.CPUID,a.CreateTime,a.Remark1,a.Remark2,a.Remark3 from t_ThiefLine as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
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
