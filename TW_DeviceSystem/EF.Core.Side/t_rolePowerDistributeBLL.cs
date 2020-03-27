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
    public class t_rolePowerDistributeBLL
    {
        /// <summary>
        /// 根据角色Id删除角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual void Delete(string conStr, List<long> rpId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<string> lsSql = new List<string>();
            List<object[]> lsParas = new List<object[]>();
            for (int i = 0; i < rpId.Count; i++)
            {
                string sql = "delete t_rolePowerDistribute where Id=@Id";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@Id",rpId[i])
                };
                lsSql.Add(sql);
                lsParas.Add(paras);
            }
            dal.ExecuteTransaction(lsSql, lsParas);
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual void Add(string conStr,List<long> powerId, List<long> roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<string> lsSql = new List<string>();
            List<object[]> lsParas = new List<object[]>();
            for (int i = 0; i < roleId.Count; i++)
            {
                for(int j = 0; j < powerId.Count; j++)
                {
                    string sql = "insert into t_rolePowerDistribute(powerId,roleId,CreateTime)values(@powerId,@roleId,@CreateTime)";
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new SqlParameter("@powerId",powerId[j]),
                        new SqlParameter("@roleId",roleId[i]),
                        new SqlParameter("@CreateTime",DateTime.Now)
                    };
                    lsSql.Add(sql);
                    lsParas.Add(paras);
                }
            }
            dal.ExecuteTransaction(lsSql, lsParas);
        }

        /// <summary>
        /// 根据角色获取角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual List<t_rolePowerDistribute> GetByRole(string conStr,long roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_rolePowerDistribute> list = dal.GetList<t_rolePowerDistribute>(t => t.roleId == roleId);
            return list;
        }
    }
}
