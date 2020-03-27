using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Dtos
{
    public class uniteModel<T>
    {
        /// <summary>
        /// 服务器返回约定状态
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 服务器返回消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public List<T> data { get; set; }
    }
}
