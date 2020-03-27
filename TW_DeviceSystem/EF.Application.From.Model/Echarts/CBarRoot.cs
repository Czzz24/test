using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class CBarRoot
    {
        public List<object> C1ElectricSeriesData { get; set; }

        public List<object> C2ElectricSeriesData { get; set; }

        public List<object> C3ElectricSeriesData { get; set; }

        public List<object> C4ElectricSeriesData { get; set; }

        public List<object> C5ElectricSeriesData { get; set; }

        public List<object> C1frequencySeriesData { get; set; }

        public List<object> C2frequencySeriesData { get; set; }

        public List<object> C3frequencySeriesData { get; set; }

        public List<object> C4frequencySeriesData { get; set; }

        public List<object> C5frequencySeriesData { get; set; }

        public List<object> C1WaveformSeriesData { get; set; }

        public List<object> C2WaveformSeriesData { get; set; }

        public List<object> C3WaveformSeriesData { get; set; }

        public List<object> C4WaveformSeriesData { get; set; }

        public List<object> C5WaveformSeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
