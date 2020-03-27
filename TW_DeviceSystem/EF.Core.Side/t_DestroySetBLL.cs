using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.Model.DestroySet;

namespace EF.Core.Side
{
    public class t_DestroySetBLL
    {
        /// <summary>
        /// 防区添加
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_DestroySet entity)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_DestroySet models = dal.GetSingle<t_DestroySet>(t => t.areaTypeId == entity.areaTypeId && t.TerminalId == entity.TerminalId);
            if (models != null)
            {
                models.Path = entity.Path;
                return dal.Update<t_DestroySet>(models);
            }
            else
            {
                entity.CreateTime = DateTime.Now;
                return dal.Add<t_DestroySet>(entity);
            }
        }

        /// <summary>
        /// 读取防区设置根据设备终端
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual List<DestroySet> GetDestroyByTerId(string conStr, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select a.*,b.areaTypeName from t_DestroySet as a inner join t_DestroyType as b on a.areaTypeId=b.Id where a.TerminalId=@TerminalId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@TerminalId",TerminalId)
            };
            List<DestroySet> list = dal.QueryList<DestroySet>(sql, paras);
            return list;
        }
    }
}
