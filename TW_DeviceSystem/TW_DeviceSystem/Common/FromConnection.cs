using EF.Application.Model;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TW_DeviceSystem.Common
{
    public class FromConnection
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_dataBaseManagerBLL bll = new t_dataBaseManagerBLL();

        /// <summary>
        /// 根据线路和供电获取从库EF方式链接字符串
        /// </summary>
        /// <param name="conStr">主库链接字符串</param>
        /// <param name="ElectricId">供电Id</param>
        /// <param name="LineId">线路Id</param>
        /// <returns>返回从库链接字符串</returns>
        public virtual string GetConStr(string conStr, int ElectricId, int LineId)
        {
            t_dataBaseManager model = bll.GetModel(conStr, ElectricId, LineId);
            string confStr = "data source=" + model.dataBaseIP + ";initial catalog=" + model.dataBaseName + ";user id=" + model.dataBaseAccount + ";password=" + model.dataBasePwd + ";MultipleActiveResultSets=True;App=EntityFramework";
            return confStr;
        }

        public virtual t_dataBaseManager GetModel(string conStr,int ElectricId,int LineId)
        {
            t_dataBaseManager model = bll.GetModel(conStr, ElectricId, LineId);
            return model;
        }
    }
}