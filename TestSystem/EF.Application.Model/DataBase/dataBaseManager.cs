using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.DataBase
{
    public class dataBaseManager
    {
        public string dataBaseIp { get; set; }

        public string dataBaseName { get; set; }

        public string dataBaseAccount { get; set; }

        public string dataBasePwd { get; set; }

        public int ElectricId { get; set; }

        public int LineId { get; set; }
    }
}
