using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EF.From.Side
{
    public class t_AlarmHandPictureBLL
    {
        /// <summary>
        /// 添加告警处理图片
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_AlarmHandPicture entity)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(entity);
        }

        /// <summary>
        /// 更新告警处理图片报警Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="AlarmId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual int updataAlamHandPicture(string conStr,long AlarmId,long Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_AlarmHandPicture set AlarmId=@AlarmId where Id=@Id";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@AlarmId",AlarmId),
                new SqlParameter("@Id",Id)
            };
            return dal.ExecuteSql(sql, paras);
        }

        /// <summary>
        /// 获取告警处理照片
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="AlarmId"></param>
        /// <returns></returns>
        public virtual List<t_AlarmHandPicture> getListByAlarmId(string conStr,int AlarmId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_AlarmHandPicture> list = dal.GetList<t_AlarmHandPicture>(t => t.AlarmId == AlarmId);
            return list;
        }
    }
}
