namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_outPartial
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public long? deviceId { get; set; }

        public string AElectric { get; set; }

        public string AFrequency { get; set; }

        public int? AMaxElectric { get; set; }

        public int? AMaxFrequency { get; set; }

        public string BElectric { get; set; }

        public string BFrequency { get; set; }

        public int? BMaxElectric { get; set; }

        public int? BMaxFrequency { get; set; }

        public string CElectric { get; set; }

        public string CFrequency { get; set; }

        public int? CMaxElectric { get; set; }

        public int? CMaxFrequency { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Astatus { get; set; }

        public int? Bstatus { get; set; }

        public int? Cstatus { get; set; }
    }
}
