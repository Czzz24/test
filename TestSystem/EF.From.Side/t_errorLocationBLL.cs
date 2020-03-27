using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq.Expressions;
using EF.Application.Model;
using System.Data;
using EF.Component.Data.SQLHelper;

namespace EF.From.Side
{
    public class t_errorLocationBLL
    {
        /// <summary>
        /// 查询故障定位历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_errorLocation> GetHistoryData(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };
            if (startTime.HasValue && !endTime.HasValue)
            {
                List<t_errorLocation> list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, t => t.TerminalId == TerminalId && t.CreateTime >= startTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && endTime.HasValue)
            {
                List<t_errorLocation> list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, t => t.TerminalId == TerminalId && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
            if (!startTime.HasValue && !endTime.HasValue)
            {
                List<t_errorLocation> list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, t => t.TerminalId == TerminalId, out totalCount, order);
                return list;
            }
            else
            {
                List<t_errorLocation> list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, t => t.TerminalId == TerminalId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 查询最新故障定位数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_errorLocation GetSingle(string conStr,string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_errorLocation where TerminalId=@TerminalId order by Id desc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@TerminalId",TerminalId)
            };
            t_errorLocation model= dal.QuerySingle<t_errorLocation>(sql, paras);
            return model;
        }

        /// <summary>
        /// 故障定位数据中心数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="deviceList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_errorLocation> GetErrorData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_errorLocation> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_errorLocation, bool>> seleWhere;
            Expression<Func<t_errorLocation, bool>> seleWhere1 = t => true;
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
                        string TerminalId = deviceList[i].TerminalId;
                        if (i == 0)
                        {
                            seleWhere1 = seleWhere1.AndAlso(t => t.TerminalId == TerminalId);
                        }
                        else
                        {
                            seleWhere1 = seleWhere1.OrElse(t => t.TerminalId == TerminalId);
                        }
                    }
                    Expression<Func<t_errorLocation, bool>> res = seleWhere.AndAlso(seleWhere1);
                    list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_errorLocation>();
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
                list = dal.GetListPaged<t_errorLocation>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 故障定位数据导出
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
        public virtual DataTable GetErrorData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.latitude,a.longitude,a.distance,a.CreateTime from t_errorLocation as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
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
                            strsql.Append(" and (a.TerminalId='" + deviceList[j].TerminalId + "'");
                        }
                        else
                        {
                            strsql.Append(" or a.TerminalId='" + deviceList[j].TerminalId + "'");
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
