namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_userRole
    {
        public long Id { get; set; }

        public string roleName { get; set; }

        public string roleDescription { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
