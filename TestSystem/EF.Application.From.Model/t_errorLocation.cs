namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_errorLocation
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public double? distance { get; set; }

        public double? longitude { get; set; }

        public double? latitude { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
