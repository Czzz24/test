using EF.Application.From.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.From.Side
{
    public class t_AlarmStatusBLL
    {
        /// <summary>
        /// 获取报警处理状态
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<t_AlarmStatus> GetALL(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_AlarmStatus> list=  dal.GetAll<t_AlarmStatus>();
            return list;
        }
    }
}
