using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.DestroySet
{
    public class DestroySet
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public long areaTypeId { get; set; }

        public string Path { get; set; }

        public DateTime? CreateTime { get; set; }

        public string areaTypeName { get; set; }
    }
}
