using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.FreeServicing
{
    public class FreeServicingRelation
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public DateTime? Time { get; set; }

        public decimal? AElectric { get; set; }

        public decimal? BElectric { get; set; }

        public decimal? CElectric { get; set; }

        public decimal? TElectric { get; set; }

        public decimal? BatteryVolt { get; set; }

        public decimal? PRS { get; set; }

        public DateTime? CreateTime { get; set; }

        public string dataBaseIp { get; set; }

        public string dataBaseName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public long? ElectricId { get; set; }

        public long? LineId { get; set; }
    }
}
