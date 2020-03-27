namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_earthBox
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public DateTime? Time { get; set; }

        public decimal? BoxTemp { get; set; }

        public decimal? BoxHumidity { get; set; }

        public decimal? AElectric { get; set; }

        public decimal? BElectric { get; set; }

        public decimal? CElectric { get; set; }

        public decimal? AVolt { get; set; }

        public decimal? BVolt { get; set; }

        public decimal? CVolt { get; set; }

        public decimal? Volt1 { get; set; }

        public decimal? Volt2 { get; set; }

        public decimal? Volt3 { get; set; }

        public decimal? BatteryVolt { get; set; }

        public decimal? ClockVolt { get; set; }

        public decimal? A1Temp { get; set; }

        public decimal? A2Temp { get; set; }

        public decimal? B1Temp { get; set; }

        public decimal? B2Temp { get; set; }

        public decimal? C1Temp { get; set; }

        public decimal? C2Temp { get; set; }

        public decimal? PTCT { get; set; }

        public string PBUS { get; set; }

        public int? Alarm { get; set; }

        public int? PRS { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? deviceType { get; set; }

        public long? deviceId { get; set; }
    }
}
