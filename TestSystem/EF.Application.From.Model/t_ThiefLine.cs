namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class t_ThiefLine
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public decimal? xAmplitude { get; set; }

        public decimal? yAmplitude { get; set; }

        public decimal? zAmplitude { get; set; }

        public string SoftVersion { get; set; }

        public string CPUID { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remark1 { get; set; }

        public string Remark2 { get; set; }

        public string Remark3 { get; set; }

        public long? deviceId { get; set; }
    }
}
