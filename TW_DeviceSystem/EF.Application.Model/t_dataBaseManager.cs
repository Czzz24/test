namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_dataBaseManager
    {
        public long Id { get; set; }

        public string projectName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public string dataBaseIP { get; set; }

        public string dataBaseName { get; set; }

        public long? attributeElectricId { get; set; }

        public long? attributeLineId { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }
    }
}
