using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EF.Application.Model;

namespace EF.Core.Side
{
    public class t_organizeDistributeBLL
    {
        /// <summary>
        /// 事务方式添加用户树形权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="nodeId"></param>
        /// <param name="roleId"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        public virtual void Add(string conStr, List<long> nodeId, List<long> userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<string> lsSql = new List<string>();
            List<object[]> lsParas = new List<object[]>();
            for (int i = 0; i < userId.Count; i++)
            {
                for(var j = 0; j < nodeId.Count; j++)
                {
                    string sql = "insert into t_organizeDistribute(nodeId,userId,CreateTime)values(@nodeId,@userId,@CreateTime)";
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new SqlParameter("@nodeId",nodeId[j]),
                        new SqlParameter("@userId",userId[i]),
                        new SqlParameter("@CreateTime",DateTime.Now)
                    };
                    lsSql.Add(sql);
                    lsParas.Add(paras);
                }
            }
            dal.ExecuteTransaction(lsSql, lsParas);
        }

        /// <summary>
        /// 事务方式删除用户树形权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual void Delete(string conStr,List<long> organizeId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<string> lsSql = new List<string>();
            List<object[]> lsParas = new List<object[]>();
            for (int i = 0; i < organizeId.Count; i++)
            {
                string sql = "delete from t_organizeDistribute where Id=@Id";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@Id",organizeId[i])
                };
                lsSql.Add(sql);
                lsParas.Add(paras);
            }
            dal.ExecuteTransaction(lsSql, lsParas);
        }

        /// <summary>
        /// 根据用户获取用户权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organizeDistribute> GetUserOrganize(string conStr,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_organizeDistribute> list = dal.GetList<t_organizeDistribute>(t => t.userId == userId);
            return list;
        }
    }
}
