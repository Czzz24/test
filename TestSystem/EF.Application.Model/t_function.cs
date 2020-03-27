namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_function
    {
        public long Id { get; set; }

        public string functionName { get; set; }

        public string actionAddress { get; set; }

        public string iconFont { get; set; }

        public string functionDescription { get; set; }

        public long? orderNo { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
