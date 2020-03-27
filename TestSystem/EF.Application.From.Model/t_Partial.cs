namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Partial
    {
        public long Id { get; set; }

        public string PartialId { get; set; }

        public string TerminalId { get; set; }

        public long? deviceId { get; set; }

        public string status1 { get; set; }

        public string status2 { get; set; }

        public string status3 { get; set; }

        public int? A1Voltage1 { get; set; }

        public int? A1Voltage2 { get; set; }

        public int? A1Voltage3 { get; set; }

        public int? A2Voltage1 { get; set; }

        public int? A2Voltage2 { get; set; }

        public int? A2Voltage3 { get; set; }

        public int? B1Voltage1 { get; set; }

        public int? B1Voltage2 { get; set; }

        public int? B1Voltage3 { get; set; }

        public int? B2Voltage1 { get; set; }

        public int? B2Voltage2 { get; set; }

        public int? B2Voltage3 { get; set; }

        public int? C1Voltage1 { get; set; }

        public int? C1Voltage2 { get; set; }

        public int? C1Voltage3 { get; set; }

        public int? C2Voltage1 { get; set; }

        public int? C2Voltage2 { get; set; }

        public int? C2Voltage3 { get; set; }

        public int? d3Voltage1 { get; set; }

        public int? d3Voltage2 { get; set; }

        public int? d3Voltage3 { get; set; }

        public string A1AvgElectric { get; set; }

        public string A1Frequency { get; set; }

        public string A1waveform { get; set; }

        public string A2AvgElectric { get; set; }

        public string A2Frequency { get; set; }

        public string A2waveform { get; set; }

        public string A3AvgElectric { get; set; }

        public string A3Frequency { get; set; }

        public string A3waveform { get; set; }

        public string A4AvgElectric { get; set; }

        public string A4Frequency { get; set; }

        public string A4waveform { get; set; }

        public string A5AvgElectric { get; set; }

        public string A5Frequency { get; set; }

        public string A5waveform { get; set; }

        public string B1AvgElectric { get; set; }

        public string B1Frequency { get; set; }

        public string B1waveform { get; set; }

        public string B2AvgElectric { get; set; }

        public string B2Frequency { get; set; }

        public string B2waveform { get; set; }

        public string B3AvgElectric { get; set; }

        public string B3Frequency { get; set; }

        public string B3waveform { get; set; }

        public string B4AvgElectric { get; set; }

        public string B4Frequency { get; set; }

        public string B4waveform { get; set; }

        public string B5AvgElectric { get; set; }

        public string B5Frequency { get; set; }

        public string B5waveform { get; set; }

        public string C1AvgElectric { get; set; }

        public string C1Frequency { get; set; }

        public string C1waveform { get; set; }

        public string C2AvgElectric { get; set; }

        public string C2Frequency { get; set; }

        public string C2waveform { get; set; }

        public string C3AvgElectric { get; set; }

        public string C3Frequency { get; set; }

        public string C3waveform { get; set; }

        public string C4AvgElectric { get; set; }

        public string C4Frequency { get; set; }

        public string C4waveform { get; set; }

        public string C5AvgElectric { get; set; }

        public string C5Frequency { get; set; }

        public string C5waveform { get; set; }

        public string A1hardversion { get; set; }

        public string A1softversion { get; set; }

        public string A2hardversion { get; set; }

        public string A2softversion { get; set; }

        public string B1hardversion { get; set; }

        public string B1softversion { get; set; }

        public string B2hardversion { get; set; }

        public string B2softversion { get; set; }

        public string C1hardversion { get; set; }

        public string C1softversion { get; set; }

        public string C2hardversion { get; set; }

        public string C2softversion { get; set; }

        public string d3hardversion { get; set; }

        public string d3softversion { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? AElectricMaxValue { get; set; }

        public int? BElectricMaxValue { get; set; }

        public int? CElectricMaxValue { get; set; }

        public int? AFrequencyMaxValue { get; set; }

        public int? BFrequencyMaxValue { get; set; }

        public int? CFrequencyMaxValue { get; set; }

        public int? Astatus { get; set; }

        public int? Bstatus { get; set; }

        public int? Cstatus { get; set; }
    }
}
