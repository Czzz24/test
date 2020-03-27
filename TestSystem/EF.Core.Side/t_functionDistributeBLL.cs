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
    public class t_functionDistributeBLL
    {
        /// <summary>
        /// 添加角色功能
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual void Add(string conStr, List<long> functionId, List<long> roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<string> lsSql = new List<string>();
            List<object[]> lsParas = new List<object[]>();
            for (int i = 0; i < roleId.Count; i++)
            {
                for(int j = 0; j < functionId.Count; j++)
                {
                    string sql = "insert into t_functionDistribute(functionId,roleId,CreateTime)values(@functionId,@roleId,@CreateTime)";
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new SqlParameter("@functionId",functionId[j]),
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
        /// 删除原有功能
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual void Delete(string conStr, List<long> fdId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<String> lsSql = new List<String>();
            List<Object[]> lsParas = new List<object[]>();
            for (int i = 0; i < fdId.Count; i++)
            {
                string sql = "delete from t_functionDistribute where Id=@Id";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@Id",fdId[i])
                };
                lsSql.Add(sql);
                lsParas.Add(paras);
            }
            dal.ExecuteTransaction(lsSql, lsParas);
        }

        /// <summary>
        /// 根据角色标识查询角色已拥有功能
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual List<t_functionDistribute> GetListByRoleId(string conStr, long? roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_functionDistribute> list = dal.GetList<t_functionDistribute>(t => t.roleId == roleId);
            return list;
        }
    }
}
