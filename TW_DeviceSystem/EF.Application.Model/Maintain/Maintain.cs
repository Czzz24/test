using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Maintain
{
    public class Maintain
    {
        public long Id { get; set; }

        public string deviceName { get; set; }

        public string TerminalId { get; set; }

        public long? userId { get; set; }

        public string failureCause { get; set; }

        public DateTime? CreateTime { get; set; }

        public string ElectricName{ get; set; }

        public string LineName { get; set; }

        public string JointName { get; set; }

        public string bigTypeName { get; set; }

        public string smallTypeName { get; set; }

    

        public string userName { get; set; }
    }
}
