namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_AlarmStatus
    {
        public long Id { get; set; }

        public string StatusName { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
