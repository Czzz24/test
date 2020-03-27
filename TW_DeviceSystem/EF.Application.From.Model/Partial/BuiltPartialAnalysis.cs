using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Partial
{
    public class BuiltPartialAnalysis
    {
        public int AElectricMaxValue { get; set; }

        public int AFrequencyMaxValue { get; set; }

        public int BElectricMaxValue { get; set; }
        public int BFrequencyMaxValue { get; set; }

        public int CElectricMaxValue { get; set; }

        public int CFrequencyMaxValue { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
