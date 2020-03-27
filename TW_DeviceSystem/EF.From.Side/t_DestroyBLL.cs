using EF.Application.From.Model;
using EF.Application.From.Model.Custom;
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
    public class t_DestroyBLL
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
            string sql = "select max(Id) as Id from t_Destroy where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 查询光纤防外破历史数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_Destroy> GetHistoryData(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };
            if (startTime.HasValue && endTime.HasValue)
            {
                List<t_Destroy> list = dal.GetListPaged<t_Destroy>(pageIndex, pageSize, t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalCount, order);
                return list;
            }
            else
            {
                List<t_Destroy> list = dal.GetListPaged<t_Destroy>(pageIndex, pageSize, t => t.deviceId == deviceId, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 查询最新光纤防外破数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_Destroy GetSingle(string conStr, string TerminalId)
        {
            long Id = getId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_Destroy where Id=" + Id + "";
            t_Destroy model = dal.QuerySingle<t_Destroy>(sql, null);
            return model;
        }

        /// <summary>
        /// 光纤防外破数据中心数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="deviceList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_Destroy> GetDestroyData(string conStr, int pageIndex, int pageSize, out int totalCount, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Destroy> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_Destroy, bool>> seleWhere;
            Expression<Func<t_Destroy, bool>> seleWhere1 = t => true;
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
                    Expression<Func<t_Destroy, bool>> res = seleWhere.AndAlso(seleWhere1);
                    list = dal.GetListPaged<t_Destroy>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_Destroy>();
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
                list = dal.GetListPaged<t_Destroy>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 光纤防外破报表导出
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
        public virtual DataTable GetDestroyData(string conStr, string mainConStr, List<t_Device> deviceList, DateTime? startTime, DateTime? endTime, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select b.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,c.name,a.Time,a.areaone,a.areatwo,a.CreateTime from t_Destroy as a inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_Device) as b on a.TerminalId = b.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as c on b.ParentId=c.Id where b.ElectricId=" + ElectricId + " and b.LineId=" + LineId + "");
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
