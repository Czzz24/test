namespace EF.Application.From.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_DeviceSignalStatus
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public string ModularId { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public long? deviceId { get; set; }
    }
}
