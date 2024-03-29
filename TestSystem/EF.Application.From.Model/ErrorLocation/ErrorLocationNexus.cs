﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.ErrorLocation
{
    [NotMapped]
    public class ErrorLocationNexus:t_errorLocation
    {
        public long? ElectricId { get; set; }

        public long? LineId { get; set; }

        public string ElectricName { get; set; }

        public string LineName { get; set; }

        public long? JointId { get; set; }

        public string JointName { get; set; }

        public string DeviceName { get; set; }
    }
}
