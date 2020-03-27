namespace EF.Application.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_userInfo
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
    }
}
