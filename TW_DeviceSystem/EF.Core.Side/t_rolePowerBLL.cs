using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EF.Core.Side
{
    public class t_rolePowerBLL
    {
        /// <summary>
        /// 获取所有角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<t_rolePower> GetAll(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_rolePower> list = dal.GetAll<t_rolePower>();
            return list;
        }

        /// <summary>
        /// 根据角色Id获取角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual List<t_rolePower> GetList(string conStr, long? roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select b.* from t_rolePowerDistribute as a inner join t_rolePower as b on a.powerId=b.Id where roleId = @roleId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@roleId",roleId)
            };
            return dal.QueryList<t_rolePower>(sql, paras);
        }

        public virtual t_rolePower GetSingle(string conStr,int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_rolePower model= dal.GetSingleById<t_rolePower>(Id);
            return model;
        }

        /// <summary>
        /// 判断用户角色是否具有权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <param name="powerId"></param>
        /// <returns></returns>
        public virtual bool GetUserRolePower(string conStr,long roleId,long powerId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select b.* from t_rolePowerDistribute as a inner join t_rolePower as b on a.powerId=b.Id where a.roleId=@roleId and a.powerId=@powerId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@roleId",roleId),
                new SqlParameter("@powerId",powerId)
            };
            t_rolePower model = dal.QuerySingle<t_rolePower>(sql, paras);
            if (model != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
