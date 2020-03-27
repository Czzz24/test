namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_deviceSmallType
    {
        public long Id { get; set; }

        public long? bigTypeId { get; set; }

        public string typeName { get; set; }

        public string actionAddress { get; set; }

        public string locationIconUrl { get; set; }

        public string IconUrl { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
