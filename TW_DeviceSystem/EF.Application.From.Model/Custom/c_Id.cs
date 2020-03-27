using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Custom
{
    public class c_Id
    {

        [Key]
        public long Id { get; set; }
    }
}
