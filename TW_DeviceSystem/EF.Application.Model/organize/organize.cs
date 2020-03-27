using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.organize
{
    public class organize
    {
        public long Id { get; set; }

        public string nodePath { get; set; }

        public string name { get; set; }

        public long? parentId { get; set; }

        public bool isElectric { get; set; }

        public bool isLine { get; set; }

        public bool isJoint { get; set; }

        public long? orderNo { get; set; }

        public DateTime CreateTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }

        public string DelUserName { get; set; }
    }
}
