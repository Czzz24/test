using EF.Component.Data.EFHelper;
using EF.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Application.Model.Custom;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace EF.Core.Side
{
    /// <summary>
    /// 用户
    /// </summary>
    public class t_userInfoBLL
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool AddUser(string conStr, t_userInfo user)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool UpdateUser(string conStr,t_userInfo user)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(user);
        }

        /// <summary>
        /// 检测用户账号是否存在
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public virtual bool CheckUserAccount(string conStr,string userAccount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_userInfo> list = dal.GetList<t_userInfo>(t => t.userAccount == userAccount);
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_userInfo> GetUserPage(string conStr, int pageIndex, int pageSize, out int totalcount,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[] {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_userInfo, bool>> seleWhere;
            if (!string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == false && t.userName.Contains(searchText) || t.userAccount.Contains(searchText);
                List<t_userInfo> list = dal.GetListPaged<t_userInfo>(pageIndex, pageSize, seleWhere, out totalcount, orderList);
                return list;
            }
            else
            {
                List<t_userInfo> list = dal.GetListPaged<t_userInfo>(pageIndex, pageSize, t => t.isDel == false, out totalcount, orderList);
                return list;
            }
        }

        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual t_userInfo GetUserById(string conStr,int userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_userInfo user= dal.GetSingle<t_userInfo>(t => t.Id == userId);
            return user;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual bool Delete(string conStr,t_userInfo model,long DelUser)
        {
            BaseDAL dal = new BaseDAL(conStr);
            model.isDel = true;
            model.DelTime = DateTime.Now;
            model.DelUser = DelUser;
            bool isDel = dal.Update(model);
            return isDel;

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userAccount"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public virtual t_userInfo Login(string conStr,string userAccount,string userPwd)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_userInfo user = dal.GetSingle<t_userInfo>(t => t.userAccount == userAccount && t.userPwd == userPwd);
            return user;
        }

        /// <summary>
        /// 获取删除用户列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual List<t_userInfo> GetDelListPage(string conStr, int pageIndex, int pageSize, out int totalCount, string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="DelTime" }
            };
            Expression<Func<t_userInfo, bool>> seleWhere;
            if (string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == true;
                return dal.GetListPaged<t_userInfo>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            else
            {
                seleWhere = t => t.isDel == true && t.userName.Contains(searchText) || t.userAccount.Contains(searchText);
                return dal.GetListPaged<t_userInfo>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
        }

        public virtual bool UpdataDel(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_userInfo model = dal.GetSingleById<t_userInfo>(Id);
            model.isDel = false;
            return dal.Update(model);
        }
    }
}
