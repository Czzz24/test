namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_organizeDistribute
    {
        public long Id { get; set; }

        public long? nodeId { get; set; }

        public long? userId { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
