namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class t_Destroy
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public int areaone { get; set; }

        public int areatwo { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? CreateTime { get; set; }

        public long? deviceId { get; set; }
    }
}
