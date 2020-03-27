namespace EF.Application.Camera.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_AIRect
    {
        [Key]
        public long tid { get; set; }

        public int? confidence { get; set; }

        public int? id { get; set; }

        public int? type { get; set; }

        public string modelID { get; set; }

        public string x { get; set; }

        public string y { get; set; }

        public string w { get; set; }

        public string h { get; set; }

        [StringLength(200)]
        public string msgId { get; set; }
    }
}
