using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.highPartial
{
    public class PartialAnalysisChart
    {
        public List<int> MaxElectric { get; set; }
        public List<int> MaxFrequency { get; set; }

        public List<string> xAxisData { get; set; }
    }
}
