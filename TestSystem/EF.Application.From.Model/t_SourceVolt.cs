namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class t_SourceVolt
    {
        public long Id { get; set; }

        public DateTime? Time { get; set; }

        public string ADC { get; set; }

        public string TerminalId { get; set; }

        public long deviceId { get; set; }

        public string hardversion { get; set; }

        public string softversion { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}