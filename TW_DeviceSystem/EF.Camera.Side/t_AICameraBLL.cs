using EF.Application.Camera.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EF.Camera.Side
{
    public class t_AICameraBLL
    {
        /// <summary>
        /// 分页获取AI智能监控数据
        /// </summary>
        /// <param name="constr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_AICamera> getListPage(string constr,string deviceSerial, int pageIndex, int pageSize, out int totalCount, DateTime? startTime, DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(constr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="time" }
            };
            if (startTime.HasValue && endTime.HasValue)
            {
                List<t_AICamera> list = dal.GetListPaged<t_AICamera>(pageIndex, pageSize, t => t.deviceSerial == deviceSerial && t.time >= startTime && t.time <= endTime, out totalCount, order);
                return list;
            }
            else
            {
                List<t_AICamera> list = dal.GetListPaged<t_AICamera>(pageIndex, pageSize, t => t.deviceSerial == deviceSerial, out totalCount, order);
                return list;
            }
        }


        /// <summary>
        /// 查询最新AI监控设备数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="deviceSerial"></param>
        /// <returns></returns>
        public virtual t_AICamera getSingle(string conStr,string deviceSerial)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 1 * from t_AICamera where deviceSerial=@deviceSerial  order by time desc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@deviceSerial",deviceSerial)
            };
            t_AICamera model=dal.QuerySingle<t_AICamera>(sql, paras);
            return model;
        }


        /// <summary>
        /// 根据AI消息Id查询单个数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public virtual t_AICamera getSingleByMsgId(string conStr,string msgId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_AICamera model=  dal.GetSingle<t_AICamera>(t => t.msgId == msgId);
            return model;
        }
    }
}
