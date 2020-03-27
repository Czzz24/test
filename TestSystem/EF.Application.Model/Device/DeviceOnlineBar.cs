using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Device
{
    public class DeviceOnlineBar
    {
        /// <summary>
        /// 内置局放在线数
        /// </summary>
        public int PartialOnline { get; set; }

        /// <summary>
        /// 接地箱在线数
        /// </summary>
        public int EarthBoxOnline { get; set; }

        /// <summary>
        /// 免维护在线数
        /// </summary>
        public int FreeOnline { get; set; }

        /// <summary>
        /// 测温在线数
        /// </summary>
        public int TempOnline { get; set; }

        /// <summary>
        /// 外置局放在线数
        /// </summary>
        public int outPartialOnline { get; set; }

        /// <summary>
        /// 内置局放离线数
        /// </summary>
        public int PartialOffline { get; set; }

        /// <summary>
        /// 接地箱离线数
        /// </summary>
        public int EarthBoxOffline { get; set; }

        /// <summary>
        /// 免维护离线数
        /// </summary>
        public int FreeOffline { get; set; }

        /// <summary>
        /// 测温离线数
        /// </summary>
        public int TempOffline { get; set; }

        /// <summary>
        /// 外置局放离线数
        /// </summary>
        public int outPartialOffline { get; set; }

        /// <summary>
        /// 内置局放未切入在线数
        /// </summary>
        public int PartialCutInOnline { get; set; }

        /// <summary>
        /// 接地箱未切入在线数
        /// </summary>
        public int EarthBoxCutInOnline { get; set; }

        /// <summary>
        /// 免维护未切入在线数
        /// </summary>
        public int FreeCutInOnline { get; set; }

        /// <summary>
        /// 测温未切入在线数
        /// </summary>
        public int TempCutInOnline { get; set; }

        /// <summary>
        /// 外置局放未切入在线数
        /// </summary>
        public int outPartialCutInOnline { get; set; }

        /// <summary>
        /// 内置局放未切入离线数
        /// </summary>
        public int PartialCutInoffline { get; set; }

        /// <summary>
        /// 接地箱未切入离线数
        /// </summary>
        public int EarthBoxCutInoffline { get; set; }

        /// <summary>
        /// 免维护未切入离线数
        /// </summary>
        public int FreeCutInoffline { get; set; }

        /// <summary>
        /// 测温未切入离线数
        /// </summary>
        public int TempCutInoffline { get; set; }

        /// <summary>
        /// 外置局放未切入离线数
        /// </summary>
        public int outPartialCutInoffline { get; set; }
    }
}
