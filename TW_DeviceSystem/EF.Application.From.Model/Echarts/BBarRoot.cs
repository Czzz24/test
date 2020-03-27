using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class BBarRoot
    {
        public List<object> B1ElectricSeriesData { get; set; }

        public List<object> B2ElectricSeriesData { get; set; }

        public List<object> B3ElectricSeriesData { get; set; }

        public List<object> B4ElectricSeriesData { get; set; }

        public List<object> B5ElectricSeriesData { get; set; }

        public List<object> B1frequencySeriesData { get; set; }

        public List<object> B2frequencySeriesData { get; set; }

        public List<object> B3frequencySeriesData { get; set; }

        public List<object> B4frequencySeriesData { get; set; }

        public List<object> B5frequencySeriesData { get; set; }

        public List<object> B1WaveformSeriesData { get; set; }

        public List<object> B2WaveformSeriesData { get; set; }

        public List<object> B3WaveformSeriesData { get; set; }

        public List<object> B4WaveformSeriesData { get; set; }

        public List<object> B5WaveformSeriesData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int totalcount { get; set; }
    }
}
