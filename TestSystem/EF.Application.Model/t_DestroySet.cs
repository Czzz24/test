namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_DestroySet
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public long areaTypeId { get; set; }

        public string Path { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
