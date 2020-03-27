using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq.Expressions;
using EF.Application.Model.Custom;
using EF.Application.Model.DataBase;
using EF.Application.Model.Alarm;
using EF.Application.Model;
using System.Data;
using EF.Component.Data.SQLHelper;
using EF.Component.Tools;

namespace EF.From.Side
{
    public class t_AlarmBLL
    {
        /// <summary>
        /// 查询报警历史数据根据终端标识、所属供电、所属线路
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public virtual List<t_Alarm> GetAlarm(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, out int totalcount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="StartTime" },
            };
            List<t_Alarm> list = new List<t_Alarm>();
            if (startTime.HasValue && !endTime.HasValue)
            {
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, t => t.deviceId == deviceId && t.Status != 3 && t.CreateTime >= startTime, out totalcount, orderList);
                return list;
            }
            if (!startTime.HasValue && endTime.HasValue)
            {
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, t => t.deviceId == deviceId && t.Status != 3 && t.CreateTime <= endTime, out totalcount, orderList);
                return list;
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, t => t.deviceId == deviceId && t.Status != 3 && t.CreateTime >= startTime && t.CreateTime <= endTime, out totalcount, orderList);
                return list;
            }
            else
            {
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, t => t.deviceId == deviceId && t.Status != 3, out totalcount, orderList);
                return list;
            }
        }

        /// <summary>
        /// 获取报警状态
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        public virtual string GetAlarmsStatus(string conStr, long? StatusId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_AlarmStatus model = dal.GetSingle<t_AlarmStatus>(t => t.Id == StatusId);
            return model.StatusName;
        }

        /// <summary>
        /// 确认告警
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual int updateAlarmStatus(string conStr, long alarmId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_Alarm set Flag=0,Status=3,EndTime=@EndTime where Id=@Id";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@Id",alarmId),
                new SqlParameter("@EndTime",DateTime.Now)
            };
            int numbers = dal.ExecuteSql(sql, paras);
            return numbers;
        }

        /// <summary>
        /// 告警处理
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="alrmId"></param>
        /// <param name="status"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public virtual int AlarmHandToStatus(string conStr, long alrmId, int status, int flag, DateTime? endTime, string Cause, string handContent, string handUser, DateTime? handEndTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "";
            SqlParameter[] paras;
            if (!endTime.HasValue)
            {
                sql = "update t_Alarm set Flag=@Flag,Status=@Status,Cause=@Cause,handContent=@handContent,handUser=@handUser,handEndTime=@handEndTime where Id=@Id";
                paras = new SqlParameter[]
                {
                    new SqlParameter("@Flag",flag),
                    new SqlParameter("@Status",status),
                    new SqlParameter("@Id",alrmId),
                    new SqlParameter("@Cause",Cause),
                    new SqlParameter("@handContent",handContent),
                    new SqlParameter("@handUser",handUser),
                    new SqlParameter("@handEndTime",handEndTime)
                };
            }
            else
            {
                sql = "update t_Alarm set Flag=@Flag,Status=@Status,EndTime=@EndTime,Cause=@Cause,handContent=@handContent,handUser=@handUser,handEndTime=@handEndTime where Id=@Id";
                paras = new SqlParameter[]
                {
                    new SqlParameter("@Flag",flag),
                    new SqlParameter("@Status",status),
                    new SqlParameter("@Id",alrmId),
                    new SqlParameter("@EndTime",endTime),
                    new SqlParameter("@Cause",Cause),
                    new SqlParameter("@handContent",handContent),
                    new SqlParameter("@handUser",handUser),
                    new SqlParameter("@handEndTime",handEndTime)
                };
            }
            int number = dal.ExecuteSql(sql, paras);
            return number;
        }

        /// <summary>
        /// 根据告警状态获取报警
        /// </summary>
        /// <param name="conStr">连接字符串</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="Status">告警状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="totalCount">条数</param>
        /// <returns></returns>
        public virtual List<t_Alarm> GetAlarmByStatus(string conStr, int pageIndex, int pageSize, int Status, DateTime? startTime, DateTime? endTime, out int totalCount, List<t_Device> deviceList)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Alarm> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="StartTime" }
            };
            Expression<Func<t_Alarm, bool>> seleWhere;
            Expression<Func<t_Alarm, bool>> seleWhere1 = t => true;
            if (deviceList != null)
            {
                if (deviceList.Count > 0)
                {
                    if (!startTime.HasValue || !endTime.HasValue)
                    {
                        seleWhere = t => t.Status == Status;
                    }
                    else
                    {
                        seleWhere = t => t.StartTime >= startTime && t.StartTime <= endTime && t.Status == Status;
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
                    Expression<Func<t_Alarm, bool>> res = seleWhere.AndAlso(seleWhere1);

                    list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_Alarm>();
                }
            }
            else
            {
                if (!startTime.HasValue || !endTime.HasValue)
                {
                    seleWhere = t => t.Status == Status;
                }
                else
                {
                    seleWhere = t => t.StartTime >= startTime && t.StartTime <= endTime && t.Status == Status;
                }
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 获取历史报警
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual List<t_Alarm> GetAlarmHistory(string conStr, int pageIndex, int pageSize, out int totalCount, DateTime? startTime, DateTime? endTime,List<t_Device> deviceList)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Alarm> list;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="StartTime" }
            };
            Expression<Func<t_Alarm, bool>> seleWhere;
            Expression<Func<t_Alarm, bool>> seleWhere1 = t => true;
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
                        seleWhere = t => t.StartTime >= startTime && t.StartTime <= endTime;
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
                    Expression<Func<t_Alarm, bool>> res = seleWhere.AndAlso(seleWhere1);

                    list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, res, out totalCount, order);
                }
                else
                {
                    totalCount = 0;
                    list = new List<t_Alarm>();
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
                    seleWhere = t => t.StartTime >= startTime && t.StartTime <= endTime;
                }
                list = dal.GetListPaged<t_Alarm>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            return list;
        }

        /// <summary>
        /// 获取历史告警Data信息报表导出
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="mainConStr"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="deviceList"></param>
        /// <param name="ElectricName"></param>
        /// <param name="LineName"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual DataTable GetAlarmHistory(string conStr,string mainConStr, DateTime? startTime, DateTime? endTime, List<t_Device> deviceList, string ElectricName, string LineName, int ElectricId, int LineId)
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
            strsql.Append("select d.deviceName,a.TerminalId,CAST('" + ElectricName + "'as nvarchar(max))as ElectricName,CAST('" + LineName + "'as nvarchar(max))as LineName,e.name,c.FlagName,b.StatusName,a.Value,a.StartTime,a.EndTime,a.Content from t_Alarm as a inner join t_AlarmStatus as b on a.Status = b.Id inner join t_AlarmFlagStatus as c on a.Flag = c.FlagId inner join openrowset('SQLOLEDB', '" + dataSource + "'; '" + userId + "'; '" + password + "',[" + dataName + "].dbo.t_Device) as d on a.TerminalId=d.TerminalId inner join openrowset('SQLOLEDB','" + dataSource + "';'" + userId + "';'" + password + "',[" + dataName + "].dbo.t_organize) as e on d.parentId=e.Id where d.ElectricId=" + ElectricId + " and d.LineId=" + LineId + "");
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


        /// <summary>
        /// 获取告警详情
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual t_Alarm GetAlarmById(string conStr,int id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_Alarm model= dal.GetSingleById<t_Alarm>(id);
            return model;
        }

        /// <summary>
        /// 获取最新10条报警信息
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual List<AlarmRelation> GetBestNewAlarm(string conStr, List<t_dataBaseManager> list,List<t_Device> dlist)
        {
            BaseDAL dal = new BaseDAL(conStr);
            StringBuilder strsql = new StringBuilder();
            List<AlarmRelation> result = new List<AlarmRelation>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    strsql.Append("select top 10 *,CAST('" + list[i].dataBaseName + "' as nvarchar(max)) as dataBaseName,CAST('" + list[i].dataBaseAccount + "' as nvarchar(max)) as dataBaseAccount,CAST('" + list[i].dataBasePwd + "' as nvarchar(max)) as dataBasePwd,CAST(" + list[i].attributeElectricId + " as bigint) as ElectricId,CAST(" + list[i].attributeLineId + " as bigint) as LineId  from openrowset('SQLOLEDB','" + list[i].dataBaseIP + "';'" + list[i].dataBaseAccount + "';'" + list[i].dataBasePwd + "',[" + list[i].dataBaseName + "].dbo.t_Alarm) where CreateTime>=@todayTime");
                    if (dlist != null)
                    {
                        if (dlist.Count > 0)
                        {
                            for(int j = 0; j < dlist.Count; j++)
                            {
                                if (j == 0)
                                {
                                    strsql.Append(" and (TerminalId='" + dlist[j].TerminalId + "'");
                                }
                                else
                                {
                                    strsql.Append(" or TerminalId='" + dlist[j].TerminalId + "'");
                                }
                            }
                            strsql.Append(")");
                        }
                    }
                }
                else
                {
                    strsql.Append(" union all select top 10 *,CAST('" + list[i].dataBaseName + "' as nvarchar(max)) as dataBaseName,CAST('" + list[i].dataBaseAccount + "' as nvarchar(max)) as dataBaseAccount,CAST('" + list[i].dataBasePwd + "' as nvarchar(max)) as dataBasePwd,CAST(" + list[i].attributeElectricId + " as bigint) as ElectricId,CAST(" + list[i].attributeLineId + " as bigint) as LineId  from openrowset('SQLOLEDB','" + list[i].dataBaseIP + "';'" + list[i].dataBaseAccount + "';'" + list[i].dataBasePwd + "',[" + list[i].dataBaseName + "].dbo.t_Alarm) where CreateTime>=@todayTime");
                    if (dlist != null)
                    {
                        if (dlist.Count > 0)
                        {
                            for (int j = 0; j < dlist.Count; j++)
                            {
                                if (j == 0)
                                {
                                    strsql.Append(" and (TerminalId='" + dlist[j].TerminalId + "'");
                                }
                                else
                                {
                                    strsql.Append(" or TerminalId='" + dlist[j].TerminalId + "'");
                                }
                            }
                            strsql.Append(")");
                        }
                    }
                }
            }
            string sql = strsql.ToString();
            DateTime todayTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@todayTime",todayTime)
            };
            result = dal.QueryList<AlarmRelation>(sql, paras);
            return result;
        }

        /// <summary>
        /// 获取未处理完告警
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual c_Count GetAlarmCount(string conStr,string TerminalId)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(Id) as count from t_Alarm where Status!=3 and deviceId=" + deviceId + "";
            c_Count model = dal.QuerySingle<c_Count>(sql, null);
            return model;
        }
    }
}
