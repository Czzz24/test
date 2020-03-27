using EF.Application.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Side
{
    public class t_DestroyTypeBLL
    {
        /// <summary>
        /// 获取所有防区类型
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<t_DestroyType> GetAll(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_DestroyType> list = dal.GetAll<t_DestroyType>();
            return list;
        }
    }
}
