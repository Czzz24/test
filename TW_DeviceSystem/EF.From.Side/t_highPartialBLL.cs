using EF.Application.From.Model;
using EF.Application.From.Model.Custom;
using EF.Application.From.Model.highPartial;
using EF.Component.Data.EFHelper;
using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.From.Side
{
    public class t_highPartialBLL
    {
        /// <summary>
        /// 获取模型Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual long getId(string conStr, string TerminalId)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select max(Id) as Id from t_highPartial where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 获取最新超高频局放数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_highPartial GetBestNew(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            long Id = getId(conStr, TerminalId);
            string sql = "select top 1 * from t_highPartial where Id=" + Id + "";
            t_highPartial model = dal.QuerySingle<t_highPartial>(sql, null);
            return model;
        }

        /// <summary>
        /// 获取历史超高频局放数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_highPartial> GetHistorySingle(string conStr,int pageSize,int currentPage,string TerminalId,DateTime? startTime,DateTime? endTime,out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="CreateTime" },
            };
            List<t_highPartial> list = dal.GetListPaged<t_highPartial>(currentPage, pageSize, t => t.CreateTime >= startTime && t.CreateTime <= endTime && t.deviceId == deviceId, out totalCount, orderList);
            return list;
        }

        /// <summary>
        /// 获取高频局放历史分析
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<highPartialAnalysis> getHighPartialAnalysis(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select MaxElectric,MaxFrequency,CreateTime from t_highPartial where deviceId=@deviceId and CreateTime>=@startTime and CreateTime<=@endTime order by CreateTime";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<highPartialAnalysis> list = dal.QueryList<highPartialAnalysis>(sql, paras);
            return list;
        }

        /// <summary>
        /// 获取高频局放历史趋势
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<highPartialTrend> GetHistoryTrend(string conStr, string TerminalId, DateTime? startTime, DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select MaxElectric,CreateTime from t_highPartial where deviceId=@deviceId and CreateTime>=@startTime and CreateTime<=@endTime order by CreateTime";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<highPartialTrend> list = dal.QueryList<highPartialTrend>(sql, paras);
            return list;
        }
    }
}
