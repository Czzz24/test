using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.EarthBox
{
    public class TempEarthBoxLine
    {
        public decimal? BoxTemp { get; set; }
        public decimal? BoxHumidity { get; set; }
        public decimal? AElectric { get; set; }
        public decimal? BElectric { get; set; }
        public decimal? CElectric { get; set; }
        public decimal? AVolt { get; set; }
        public decimal? BVolt { get; set; }
        public decimal? CVolt { get; set; }
        public decimal? A1Temp { get; set; }
        public decimal? B1Temp { get; set; }
        public decimal? C1Temp { get; set; }
        public decimal? A2Temp { get; set; }
        public decimal? B2Temp { get; set; }
        public decimal? C2Temp { get; set; }
        public decimal? BatteryVolt { get; set; }
        public decimal? Volt1 { get; set; }
        public decimal? Volt2 { get; set; }
        public DateTime? Time { get; set; }
    }
}
