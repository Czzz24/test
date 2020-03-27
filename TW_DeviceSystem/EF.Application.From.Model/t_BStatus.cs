namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_BStatus
    {
        public long Id { get; set; }

        public int? StatusId { get; set; }

        [StringLength(50)]
        public string BStatusName { get; set; }
    }
}
