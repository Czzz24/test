using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.DataBase
{
    public class c_dataBaseManager
    {
        public long Id { get; set; }

        public string projectName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public string dataBaseIP { get; set; }

        public string dataBaseName { get; set; }

        public long? attributeElectricId { get; set; }

        public string ElectricName { get; set; }

        public long? attributeLineId { get; set; }

        public string LineName { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }

        public string DelUserName { get; set; }
    }
}
