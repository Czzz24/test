namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_highPartial
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }


        public string Electric { get; set; }

        public string Frequency { get; set; }

        public int? MaxElectric { get; set; }


        public int? MaxFrequency { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? status { get; set; }

        public long? deviceId { get; set; }
    }
}
