namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class t_AlarmHandPicture
    {
        public long Id { get; set; }

        public string fileName { get; set; }

        [StringLength(50)]
        public string suffix { get; set; }

        public string serverPath { get; set; }

        public string filePath { get; set; }

        public long? AlarmId { get; set; }

        public DateTime? CreateTime { get; set; }

        public long? deviceId { get; set; }
    }
}
