using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class PartialAnalysis
    {
        public List<int> AElectricData { get; set; }

        public List<int> AFrequencyData { get; set; }

        public List<int> BElectricData { get; set; }

        public List<int> BFrequencyData { get; set; }

        public List<int> CElectricData { get; set; }

        public List<int> CFrequencyData { get; set; }

        public List<string> xAxisData { get; set; }
    }
}
