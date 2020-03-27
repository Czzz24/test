using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.outPartial
{
    public class outPartialRelation
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public string AElectric { get; set; }

        public string AFrequency { get; set; }

        public int? AMaxElectric { get; set; }

        public int? AMaxFrequency { get; set; }

        public string BElectric { get; set; }

        public string BFrequency { get; set; }

        public int? BMaxElectric { get; set; }

        public int? BMaxFrequency { get; set; }

        public string CElectric { get; set; }

        public string CFrequency { get; set; }

        public int? CMaxElectric { get; set; }

        public int? CMaxFrequency { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Astatus { get; set; }

        public int? Bstatus { get; set; }

        public int? Cstatus { get; set; }

        public string dataBaseIp { get; set; }

        public string dataBaseName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public long? ElectricId { get; set; }

        public long? LineId { get; set; }
    }
}
