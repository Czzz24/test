using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class ABarRoot
    {
        public List<object> A1ElectricSeriesData { get; set; }

        public List<object> A2ElectricSeriesData { get; set; }

        public List<object> A3ElectricSeriesData { get; set; }

        public List<object> A4ElectricSeriesData { get; set; }

        public List<object> A5ElectricSeriesData { get; set; }

        public List<object> A1frequencySeriesData { get; set; }

        public List<object> A2frequencySeriesData { get; set; }

        public List<object> A3frequencySeriesData { get; set; }

        public List<object> A4frequencySeriesData { get; set; }

        public List<object> A5frequencySeriesData { get; set; }

        public List<object> A1WaveformSeriesData { get; set; }

        public List<object> A2WaveformSeriesData { get; set; }

        public List<object> A3WaveformSeriesData { get; set; }

        public List<object> A4WaveformSeriesData { get; set; }

        public List<object> A5WaveformSeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
