using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Component.Tools;
using EF.Application.From.Model.Custom;

namespace EF.From.Side
{
    public class t_DeviceSignalStatusBLL
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
            string sql = "select max(Id) as Id from t_DeviceSignalStatus where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            if (model != null)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 获取最新通讯状态信息
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_DeviceSignalStatus GetBestNewNetWork(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 * from t_DeviceSignalStatus where Id=" + Id + "";
            return dal.QuerySingle<t_DeviceSignalStatus>(sql, null);
        }

        /// <summary>
        /// 获取通讯状态历史曲线
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_DeviceSignalStatus> GetNetWorkTrend(string conStr,string TerminalId,DateTime? startTime,DateTime? endTime)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" },
            };
            List<t_DeviceSignalStatus> list = dal.GetList<t_DeviceSignalStatus>(t => t.deviceId == deviceId && t.CreateTime >= startTime && t.CreateTime <= endTime);
            return list;
        }
    }
}
