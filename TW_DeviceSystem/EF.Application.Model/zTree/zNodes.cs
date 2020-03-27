using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.zTree
{
    public  class zNodes
    {
        public long Id { get; set; }

        public string name { get; set; }

        public string icon { get; set; }

        public string iconOpen { get; set; }

        public string iconClose { get; set; }

        public string path { get; set; }

        public bool open { get; set; }

        public string actionAddress { get; set; }

        public long? parentId { get; set; }

        public bool isElectric { get; set; }


        public bool isLine { get; set; }

        public bool isJoint { get; set; }

        public bool isDevice { get; set; }

        public bool isUser { get; set; }

        public string CreateTime { get; set; }

        public long count { get; set; }

        public string TerminalId { get; set; }

        public long? ElectricId { get; set; }

        public long? LineId { get; set; }

        public List<zNodes> children { get; set; }
    }
}
