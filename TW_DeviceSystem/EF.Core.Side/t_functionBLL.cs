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
    /// <summary>
    /// 功能项
    /// </summary>
    public class t_functionBLL
    {

        /// <summary>
        /// 功能列表分页
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回分页列表</returns>
        public virtual List<t_function> GetListByPage(string conStr, int pageIndex, int pageSize,out int totalcount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="orderNo" },
            };
            List<t_function> list = dal.GetListPaged<t_function>(pageIndex, pageSize, out totalcount, orderList);
            return list;
        }

        /// <summary>
        /// 功能添加
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="model">对象实体</param>
        /// <returns>返回true成功false失败</returns>
        public virtual bool Add(string conStr, t_function model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 功能修改
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="model">对象实体</param>
        /// <returns>返回true成功false失败</returns>
        public virtual bool update(string conStr, t_function model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 查询单个功能对象
        /// </summary>
        /// <param name="conStr">连接字符串</param>
        /// <param name="Id">唯一标识</param>
        /// <returns></returns>
        public virtual t_function GetSingle(string conStr,int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_function model = dal.GetSingleById<t_function>(Id);
            return model;
        }

        /// <summary>
        /// 查询所有功能项
        /// </summary>
        /// <returns></returns>
        public virtual List<t_function> GetAllList(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="orderNo" }
            };
            List<t_function> list = dal.GetAll<t_function>(order);
            return list;
        }

        /// <summary>
        /// 根据角色Id获取角色权限
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual List<t_function> GetFunctionByUserRole(string conStr,long? roleId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select b.* from t_functionDistribute as a inner join t_function as b on a.functionId=b.Id where roleId=@roleId order by orderNo asc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@roleId",roleId)
            };
            List<t_function> list=  dal.QueryList<t_function>(sql, paras);
            return list;
        }
    }
}
