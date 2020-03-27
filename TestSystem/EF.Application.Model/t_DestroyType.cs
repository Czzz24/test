namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class t_DestroyType
    {
        public long Id { get; set; }

        public string areaTypeName { get; set; }
    }
}
