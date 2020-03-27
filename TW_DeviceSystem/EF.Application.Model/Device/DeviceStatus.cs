using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Device
{
    public class DeviceStatus
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 在线数
        /// </summary>
        public int OnlineCount { get; set; }

        /// <summary>
        /// 离线数
        /// </summary>
        public int OffLineCount { get; set; }
    }
}
