using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.ErrorLocation
{
    public class fjdqError
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receiveTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int deviceID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int network { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string monitorID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string channelID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string position { get; set; }
    }

    public class fjdqErrorRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public fjdqError data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string error { get; set; }
    }
}
