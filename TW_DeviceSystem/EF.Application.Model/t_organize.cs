namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_organize
    {
        public long Id { get; set; }

        public string nodePath { get; set; }

        public string name { get; set; }

        public long? parentId { get; set; }

        public bool isElectric { get; set; }

        public bool isLine { get; set; }

        public bool isJoint { get; set; }

        public long? orderNo { get; set; }

        public DateTime CreateTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }
    }
}
