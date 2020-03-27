using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Application.Model;
using EF.Component.Data.EFHelper;

namespace EF.Core.Side
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class t_userRoleBLL
    {

        /// <summary>
        /// 获取用户角色分页列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public virtual List<t_userRole> GetListByPage(string conStr, int pageIndex, int pageSize, out int totalcount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" },
            };
            List<t_userRole> list = dal.GetListPaged<t_userRole>(pageIndex, pageSize, out totalcount, orderList);
            return list;
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_userRole model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Edit(string conStr, t_userRole model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(string conStr, t_userRole model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Delete(model);
        }

        /// <summary>
        /// 查询用户角色
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_userRole GetSingle(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_userRole userRole = dal.GetSingleById<t_userRole>(Id);
            return userRole;
        }

        /// <summary>
        /// 查询角色下拉列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<t_userRole> GetSelectList(string conStr, long? roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            if (roleId.Value != 1)
            {
                return dal.GetList<t_userRole>(t => t.Id != 1);
            }
            else
            {
                return dal.GetAll<t_userRole>();
            }
        }
    }
}
