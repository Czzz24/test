using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Custom
{
    public class c_Device
    {
        public long Id { get; set; }

        public string deviceName { get; set; }

        public string TerminalId { get; set; }

        public long? deviceId { get; set; }

        public long? bigTypeId { get; set; }

        public long? smallTypeId { get; set; }

        public string simNo { get; set; }

        public long? orderNo { get; set; }

        public string Installer { get; set; }

        public DateTime? InstallDate { get; set; }

        public double? longitude { get; set; }

        public double? latitude { get; set; }

        public string manufacturer { get; set; }

        public string localInstructions { get; set; }

        public long? ElectricId { get; set; }

        public long? LineId { get; set; }

        public long? parentId { get; set; }

        public string nodePath { get; set; }

        public bool? isError { get; set; }

        public bool? isOnline { get; set; }

        public DateTime? createTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }

        public string bigtypeName { get; set; }

        public string smalltypeName { get; set; }

        public string ElectricName { get; set; }

        public string LineName { get; set; }

        public string JointName { get; set; }

        public string DelUserName { get; set; }
    }
}
