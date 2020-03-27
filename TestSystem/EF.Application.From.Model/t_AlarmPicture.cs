namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_AlarmPicture
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public string fileName { get; set; }

        public string suffix { get; set; }

        public string serverPath { get; set; }

        public string filePath { get; set; }

        public long? AlarmId { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
