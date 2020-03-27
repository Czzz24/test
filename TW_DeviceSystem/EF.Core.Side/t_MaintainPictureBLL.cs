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
    public class t_MaintainPictureBLL
    {
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddMaintainPicture(string conStr, t_MaintainPicture model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 更新图片库维护Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="maintainId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual int updateMaintainPicture(string conStr, long maintainId, long Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_MaintainPicture set maintainId=@maintainId where Id=@Id";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@maintainId",maintainId),
                new SqlParameter("@Id",Id)
            };
            return dal.ExecuteSql(sql, paras);
        }

        /// <summary>
        /// 获取维护图片信息
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="maintainId"></param>
        /// <returns></returns>
        public virtual List<t_MaintainPicture> GetListByMaintainId(string conStr, int maintainId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_MaintainPicture> list = dal.GetList<t_MaintainPicture>(t => t.maintainId == maintainId);
            return list;
        }
    }
}
