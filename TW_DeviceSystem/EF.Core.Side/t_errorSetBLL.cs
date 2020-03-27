using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Side
{
    public class t_errorSetBLL
    {
        /// <summary>
        /// 添加故障定位基准坐标
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_errorSet entity)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_errorSet models = dal.GetSingle<t_errorSet>(t => t.TerminalId == entity.TerminalId);
            if (models != null)
            {
                models.leftLatitude = entity.leftLatitude;
                models.leftLongitude = entity.leftLongitude;
                models.rightLatitude = entity.rightLatitude;
                models.rightLongitude = entity.rightLongitude;
                return dal.Update<t_errorSet>(models);
            }
            else
            {
                entity.CreateTime = DateTime.Now;
                return dal.Add<t_errorSet>(entity);
            }
        }

        /// <summary>
        /// 根据故障终端查询终端坐标设置
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_errorSet GetSingle(string conStr,string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_errorSet model=  dal.GetSingle<t_errorSet>(t => t.TerminalId == TerminalId);
            return model;
        }
    }
}
