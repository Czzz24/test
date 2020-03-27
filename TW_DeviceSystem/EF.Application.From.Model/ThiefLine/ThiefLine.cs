using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.ThiefLine
{
    public class ThiefLine
    {
        public DateTime? Time { get; set; }

        public decimal? xAmplitude { get; set; }

        public decimal? yAmplitude { get; set; }

        public decimal? zAmplitude { get; set; }
    }
}
