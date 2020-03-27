using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.ErrorLocation
{
    public class ErrorLocation
    {
        public long Id { get; set; }

        public string TerminalId { get; set; }

        public double? distance { get; set; }

        public double? longitude { get; set; }

        public double? latitude { get; set; }

        public DateTime? Time { get; set; }

        public DateTime? CreateTime { get; set; }

        public List<PathItem> errorPath { get; set; }
    }

    public class PathItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lng { get; set; }
    }
}
