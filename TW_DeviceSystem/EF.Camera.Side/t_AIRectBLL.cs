using EF.Application.Camera.Model;
using EF.Component.Data.EFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Camera.Side
{
    public class t_AIRectBLL
    {

        /// <summary>
        /// 获取AI分析矩形数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public virtual List<t_AIRect> getRectbyMsgId(string conStr,string msgId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_AIRect> list = dal.GetList<t_AIRect>(t => t.msgId == msgId);
            return list;
        }
    }
}
