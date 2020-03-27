using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class outABarRoot
    {
        public List<object> AElectricSeriesData { get; set; }

        public List<object> AFrequencySeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
