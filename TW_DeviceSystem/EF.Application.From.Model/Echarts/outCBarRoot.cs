using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class outCBarRoot
    {
        public List<object> CElectricSeriesData { get; set; }

        public List<object> CFrequencySeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
