namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_MaintainPicture
    {
        public long Id { get; set; }

        public string fileName { get; set; }

        [StringLength(50)]
        public string suffix { get; set; }

        public string serverPath { get; set; }

        public string filePath { get; set; }

        public long? maintainId { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
