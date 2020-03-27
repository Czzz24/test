using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Side
{
    /// <summary>
    /// 设备大类
    /// </summary>
    public class t_deviceBigTypeBLL
    {
        /// <summary>
        /// 获取设备大类列表
        /// </summary>
        /// <returns></returns>
        public virtual List<t_deviceBigType> GetAll(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.GetAll<t_deviceBigType>();
        }

        /// <summary>
        /// 添加设备大类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr,t_deviceBigType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 删除设备大类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(string conStr,t_deviceBigType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return  dal.Delete(model);
        }

        /// <summary>
        /// 修改设备大类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(string conStr,t_deviceBigType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 分页获取设备大类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public virtual List<t_deviceBigType> GetAllByPage(string conStr,int pageIndex,int pageSize, int totalcount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                 new OrderModelField {IsDESC=false,PropertyName="Id" },
            };
           return dal.GetListPaged<t_deviceBigType>(pageIndex, pageSize, out totalcount, orderList);
        }

        /// <summary>
        /// 根据Id获取设备大类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_deviceBigType GetSingle(string conStr,int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.GetSingleById<t_deviceBigType>(Id);
        }
    }
}
