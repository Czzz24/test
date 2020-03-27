namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Maintain
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public long? userId { get; set; }

        public string failureCause { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
