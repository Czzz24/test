namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_rolePower
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string powerName { get; set; }
    }
}
