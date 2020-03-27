using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.From.Side
{
    public class t_AlarmPictureBLL
    {
        /// <summary>
        /// 获取接地箱箱柜门打开告警图片
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public virtual List<t_AlarmPicture> GetPicture(string conStr, long alarmId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_AlarmPicture> list=  dal.GetList<t_AlarmPicture>(t => t.AlarmId == alarmId);
            return list;
        }
    }
}
