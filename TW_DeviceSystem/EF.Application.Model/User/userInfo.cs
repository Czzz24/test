using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.User
{
    public class userInfo
    {

        public long Id { get; set; }

        public string userName { get; set; }

        public string userAccount { get; set; }

        public string userPwd { get; set; }

        public string userDescription { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public long? roleId { get; set; }

        public long? orderNo { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? isDel { get; set; }

        public DateTime? DelTime { get; set; }

        public long? DelUser { get; set; }

        public string DelUserName { get; set; }
    }
}
