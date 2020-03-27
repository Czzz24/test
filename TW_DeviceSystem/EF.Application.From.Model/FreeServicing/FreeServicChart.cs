using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.FreeServicing
{
    public class FreeServicChart
    {
        public DateTime? Time { get; set; }

        public decimal? AElectric { get; set; }

        public decimal? BElectric { get; set; }

        public decimal? CElectric { get; set; }

        public decimal? TElectric { get; set; }

        public decimal? BatteryVolt { get; set; }

        public decimal? PRS { get; set; }
    }
}
