namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_freeServicing
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public DateTime? Time { get; set; }

        public decimal? AElectric { get; set; }

        public decimal? BElectric { get; set; }

        public decimal? CElectric { get; set; }

        public decimal? TElectric { get; set; }

        public decimal? BatteryVolt { get; set; }

        public decimal? PRS { get; set; }

        public DateTime? CreateTime { get; set; }

        public long? deviceId { get; set; }
    }
}
