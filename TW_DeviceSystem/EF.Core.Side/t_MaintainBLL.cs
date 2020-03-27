using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Side
{
    public class t_MaintainBLL
    {
        /// <summary>
        /// 维护信息添加
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr,t_Maintain model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 更改设备终端Id维护信息跟随
        /// </summary>
        /// <param name="conStr">链接</param>
        /// <param name="TerminalId">更改的终端Id</param>
        /// <param name="fTerminalId">原来终端Id</param>
        /// <returns></returns>
        public virtual int Update(string conStr, string TerminalId, string fTerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_Maintain set TerminalId=@TerminalId where TerminalId=@fTerminalId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@TerminalId",TerminalId),
                new SqlParameter("@fTerminalId",fTerminalId)
            };
            int number = dal.ExecuteSql(sql, paras);
            return number;
        }

        /// <summary>
        /// 获取历史免维护
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_Maintain> GetPage(string conStr,int pageIndex,int pageSize,out int totalCount,string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="CreateTime" }
            };
            List<t_Maintain> list = dal.GetListPaged<t_Maintain>(pageIndex, pageSize, t=>t.TerminalId == TerminalId, out totalCount, order);
            return list;
        }
    }
}
