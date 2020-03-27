using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Custom
{
    public class c_Count
    {
        [Key]
        public int count { get; set; }
    }
}
