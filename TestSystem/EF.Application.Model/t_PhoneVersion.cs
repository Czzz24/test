namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_PhoneVersion
    {
        public long Id { get; set; }

        public string phoneVersion { get; set; }

        public string address { get; set; }

        public string description { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
