namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_functionDistribute
    {
        public long Id { get; set; }

        public long? functionId { get; set; }

        public long? roleId { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
