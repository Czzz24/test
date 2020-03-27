using EF.Application.From.Model;
using EF.Application.From.Model.Custom;
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
    public class t_SourceVoltBLL
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
            string sql = "select MAX(Id) as Id from t_SourceVolt where deviceId=" + deviceId + "";
            c_Id model = dal.QuerySingle<c_Id>(sql, null);
            return model.Id;
        }

        /// <summary>
        /// 获取最新三合一数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_SourceVolt getSingle(string conStr, string TerminalId)
        {
            long id = getId(conStr, TerminalId);
            BaseDAL dal = new BaseDAL(conStr);
            t_SourceVolt model = dal.GetSingle<t_SourceVolt>(t => t.Id == id);
            return model;
        }

        /// <summary>
        /// 分页获取三合一板子数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_SourceVolt> getDataPage(string conStr, string TerminalId, int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, out int totalCount)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="Time" }
            };
            if(startTime.HasValue && endTime.HasValue)
            {
                List<t_SourceVolt> list = dal.GetListPaged<t_SourceVolt>(pageIndex, pageSize,t=>t.Time>=startTime && t.Time<=endTime, out totalCount, order);
                return list;
            }
            else
            {
                List<t_SourceVolt> list = dal.GetListPaged<t_SourceVolt>(pageIndex, pageSize, out totalCount, order);
                return list;
            }
        }

        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<t_SourceVolt> GetChartData(string conStr,string TerminalId,DateTime? startTime,DateTime? endTime)
        {
            long deviceId = long.Parse(HexTo.HexToFloat(TerminalId));
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select * from t_SourceVolt where deviceId=@deviceId and Time >=@startTime and Time<=@endTime order by Time";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@deviceId",deviceId)
            };
            List<t_SourceVolt> list = dal.QueryList<t_SourceVolt>(sql, paras);
            return list;
        }
    }
}
