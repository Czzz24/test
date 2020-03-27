namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_freeVoltServicing
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public decimal? AVolt { get; set; }

        public decimal? BVolt { get; set; }

        public decimal? CVolt { get; set; }

        public decimal? TVolt { get; set; }

        public decimal? BatteryVolt { get; set; }

        public string SoftVersion { get; set; }

        public string CPUID { get; set; }

        public decimal? PRS { get; set; }

        public DateTime? Time { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark1 { get; set; }

        public string Remark2 { get; set; }

        public long? deviceId { get; set; }
    }
}
