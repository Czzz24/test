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
    /// 设备小类
    /// </summary>
    public class t_deviceSmallTypeBLL
    {
        /// <summary>
        /// 添加设备小类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_deviceSmallType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 删除设备小类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(string conStr, t_deviceSmallType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Delete(model);
        }

        /// <summary>
        /// 修改设备小类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(string conStr, t_deviceSmallType model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 获取单个设备小类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_deviceSmallType GetSingle(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.GetSingleById<t_deviceSmallType>(Id);
        }

        /// <summary>
        /// 获取设备小类
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="bigTypeId">设备大类Id</param>
        /// <returns></returns>
        public virtual List<t_deviceSmallType> GetByBigTypeId(string conStr, int bigTypeId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_deviceSmallType> list = dal.GetList<t_deviceSmallType>(t => t.bigTypeId == bigTypeId);
            return list;
        }
    }
}
