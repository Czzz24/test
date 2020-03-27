using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class outBBarRoot
    {
        public List<object> BElectricSeriesData { get; set; }

        public List<object> BFrequencySeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
