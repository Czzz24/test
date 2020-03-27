namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Alarm
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public int? AlarmCode { get; set; }

        public DateTime? StartTime { get; set; }

        public decimal? Value { get; set; }

        public string Content { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Flag { get; set; }

        public long? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Cause { get; set; }

        public string handContent { get; set; }

        public string handUser { get; set; }

        public DateTime? handEndTime { get; set; }

        public long? deviceId { get; set; }
    }
}
