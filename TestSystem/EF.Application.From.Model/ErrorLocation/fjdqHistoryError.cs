using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.ErrorLocation
{
    public class fjdqHistoryError
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string error { get; set; }
    }

    public class ListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int deviceID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receiveTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
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

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> list { get; set; }
    }
}
