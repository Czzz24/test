using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Alarm
{
    public class AlarmRelation
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public int? AlarmCode { get; set; }

        public DateTime? StartTime { get; set; }

        public decimal? Value { get; set; }

        public string Content { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Flag { get; set; }

        public long? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public string dataBaseIp { get; set; }

        public string dataBaseName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public long? ElectricId { get; set; }

        public long? LineId { get; set; }
    }
}
