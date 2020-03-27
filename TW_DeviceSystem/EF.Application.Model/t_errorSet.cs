namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_errorSet
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public double? leftLongitude { get; set; }

        public double? leftLatitude { get; set; }

        public double? rightLongitude { get; set; }

        public double? rightLatitude { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
