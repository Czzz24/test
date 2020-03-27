using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.FreeVolt
{
    public class FreeVoltServiceChart
    {
        public DateTime? Time { get; set; }

        public decimal? AVolt { get; set; }

        public decimal? BVolt { get; set; }

        public decimal? CVolt { get; set; }

        public decimal? TVolt { get; set; }

        public decimal? BatteryVolt { get; set; }

        public decimal? PRS { get; set; }
    }
}
