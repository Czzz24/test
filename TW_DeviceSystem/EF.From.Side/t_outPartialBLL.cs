using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.From.Model.outPartial;
using EF.Application.From.Model.Echarts;
using EF.Application.From.Model.Page;
using EF.Application.Model;
using EF.Application.Model.DataBase;
using EF.Application.Model.Custom;
using EF.Application.Model.outPartial;
using System.Linq.Expressions;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Application.From.Model.Custom;
using EF.Component.Tools;

namespace EF.From.Side
{
    /// <summary>
    /// 外置局放
    /// </summary>
    public class t_outPartialBLL
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
            string sql = "select max(Id) as Id from t_outPartial where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 查询最新外置局放数据根据终端ID
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_outPartial GetBestNew(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 * from t_outPartial where Id="+ Id + "";
            t_outPartial model = dal.QuerySingle<t_outPartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取最新A相数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual APartial GetBestNewA(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 Id,AElectric,AFrequency from t_outPartial where Id=" + Id + "";
            APartial model = dal.QuerySingle<APartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取最新B相数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual BPartial GetBestNewB(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 Id,BElectric,BFrequency from t_outPartial where Id=" + Id + "";
            BPartial model = dal.QuerySingle<BPartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取最新C相数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual CPartial GetBestNewC(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 Id,CElectric,CFrequency from t_outPartial where Id=" + Id + "";
            CPartial model = dal.QuerySingle<CPartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取历史分析X轴数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="columnName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<outPartialAnalysis> GetHisChartX(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select AMaxElectric,AMaxFrequency,BMaxElectric,BMaxFrequency,CMaxElectric,CMaxFrequency,CreateTime from t_outPartial where deviceId=@deviceId and CreateTime>=@startTime and CreateTime<=@endTime order by CreateTime";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<outPartialAnalysis> list = dal.QueryList<outPartialAnalysis>(sql, paras);
            return list;
        }

        /// <summary>
        /// 获取外置局放历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual t_outPartial GetHistorySingle(string conStr, int pageSize, int currentPage, string TerminalId, DateTime? startTime, DateTime? endTime,out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="CreateTime" },
            };
            List<t_outPartial> list = dal.GetListPaged<t_outPartial>(currentPage, pageSize, t => t.CreateTime >= startTime && t.CreateTime <= endTime && t.deviceId == deviceId, out totalCount, orderList);
            return list[0];
        }

        /// <summary>
        /// 外置局放趋势分析
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<outPartialTrend> GetHistoryTrend(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select AMaxElectric,BMaxElectric,CMaxElectric,CreateTime from t_outPartial where deviceId=@deviceId and CreateTime>=@startTime and CreateTime<=@endTime order by CreateTime";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<outPartialTrend> list = dal.QueryList<outPartialTrend>(sql, paras);
            return list;
        }


        public virtual List<t_outPartial> GetOutPartialData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_outPartial> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_outPartial, bool>> seleWhere;
            Expression<Func<t_outPartial, bool>> seleWhere1 = t => true;
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
                    Expression<Func<t_outPartial, bool>> res = seleWhere.AndAlso(seleWhere1);

                    list = dal.GetListPaged<t_outPartial>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_outPartial>();
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
                list = dal.GetListPaged<t_outPartial>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        public virtual DataTable GetOutPartialData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select e.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,f.name,b.AStatusName,c.BStatusName,d.CStatusName,a.AMaxElectric,a.AMaxFrequency,a.BMaxElectric,a.BMaxFrequency,a.CMaxElectric,a.CMaxFrequency,a.CreateTime from t_outPartial as a inner join t_AStatus as b on a.Astatus=b.StatusId inner join t_BStatus as c on a.Bstatus=c.StatusId inner join t_CStatus as d on a.Cstatus = d.StatusId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as e on a.TerminalId = e.TerminalId inner join openrowset('SQLOLEDB', '" + dataSource + "'; '" + userId + "'; '" + password + "',[" + dataName + "].dbo.t_organize) as f on e.ParentId=f.Id where e.ElectricId=" + ElectricId + " and e.LineId=" + LineId + "");
            if (deviceList != null)
            {
                if(startTime.HasValue  && endTime.HasValue)
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
