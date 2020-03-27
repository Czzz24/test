using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EF.Core.Side
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    public class t_dataBaseManagerBLL
    {

        /// <summary>
        /// 数据库配置项
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_dataBaseManager model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 根据供电Id和线路Id查找数据库配置项
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="attributeElectricId"></param>
        /// <param name="attributeLineId"></param>
        /// <returns></returns>
        public virtual t_dataBaseManager GetModel(string conStr, int attributeElectricId, int attributeLineId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_dataBaseManager model = dal.GetSingle<t_dataBaseManager>(t => t.attributeElectricId == attributeElectricId && t.attributeLineId == attributeLineId && t.isDel==false);
            return model;
        }

        /// <summary>
        /// 查询数据库配置项列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_dataBaseManager> GetListByPage(string conStr, int pageIndex, int pageSize, int? ElectricId, int? LineId, string projectName, out int totalCount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_dataBaseManager> list = new List<t_dataBaseManager>();
            Expression<Func<t_dataBaseManager, bool>> seleWhere;
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            if (ElectricId.HasValue && !LineId.HasValue && string.IsNullOrEmpty(projectName))
            {
                seleWhere = t => t.isDel == false && t.attributeElectricId == ElectricId;
            }
            else if (ElectricId.HasValue && LineId.HasValue && string.IsNullOrEmpty(projectName))
            {
                seleWhere = t => t.isDel == false && t.attributeElectricId == ElectricId && t.attributeLineId == LineId;
            }
            else if (ElectricId.HasValue && LineId.HasValue && !string.IsNullOrEmpty(projectName))
            {
                seleWhere = t => t.isDel == false && t.attributeElectricId == ElectricId && t.attributeLineId == LineId && t.projectName.Contains(projectName);
            }
            else if (!ElectricId.HasValue && !LineId.HasValue && !string.IsNullOrEmpty(projectName))
            {
                seleWhere = t => t.isDel == false && t.projectName.Contains(projectName);
            }
            else
            {
                seleWhere = t => t.isDel == false;
            }
            if (seleWhere != null)
            {
                list = dal.GetListPaged(pageIndex, pageSize, seleWhere, out totalCount, orderList);
            }
            else
            {
                list = dal.GetListPaged<t_dataBaseManager>(pageIndex, pageSize, out totalCount, orderList);
            }
            return list;
        }

        /// <summary>
        /// 根据供电(或公司)线路删除数据库配置
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="attributeElectricId"></param>
        /// <param name="attributeLineId"></param>
        /// <returns></returns>
        public virtual bool DeleteDataBaseManager(string conStr, t_dataBaseManager dataBaseModel,long DelUser)
        {
            BaseDAL dal = new BaseDAL(conStr);
            dataBaseModel.isDel = true;
            dataBaseModel.DelTime = DateTime.Now;
            dataBaseModel.DelUser = DelUser;
            return dal.Update(dataBaseModel);
        }

        public virtual List<t_dataBaseManager> GetRandData(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select top 10 * from t_dataBaseManager where isDel=0 order by NEWID()";
            return dal.QueryList<t_dataBaseManager>(sql, null);
        }

        /// <summary>
        /// 查询单个数据配置对象
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_dataBaseManager GetSingle(string conStr,int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_dataBaseManager model=  dal.GetSingleById<t_dataBaseManager>(Id);
            return model;
        }

        /// <summary>
        /// 数据库配置编辑
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Edit(string conStr,t_dataBaseManager model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return  dal.Update(model);
        }

        /// <summary>
        /// 获取已删除数据库配置
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual List<t_dataBaseManager> GetDelListPage(string conStr, int pageIndex, int pageSize, out int totalCount, string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="DelTime" }
            };
            Expression<Func<t_dataBaseManager, bool>> seleWhere;
            if (string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == true;
                return dal.GetListPaged<t_dataBaseManager>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            else
            {
                seleWhere = t => t.isDel == true && t.projectName.Contains(searchText);
                return dal.GetListPaged<t_dataBaseManager>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }

        }

        public virtual bool UpdataDel(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_dataBaseManager model = dal.GetSingleById<t_dataBaseManager>(Id);
            model.isDel = false;
            return dal.Update(model);
        }
    }
}
