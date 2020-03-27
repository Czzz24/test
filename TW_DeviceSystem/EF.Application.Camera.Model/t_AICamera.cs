namespace EF.Application.Camera.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_AICamera
    {
        [Key]
        [StringLength(200)]
        public string msgId { get; set; }

        public string msgType { get; set; }

        public string traceId { get; set; }

        public string taskId { get; set; }

        public string taskName { get; set; }

        public string taskType { get; set; }

        public string trainId { get; set; }

        public string channelId { get; set; }

        public string deviceId { get; set; }

        public string deviceSerial { get; set; }

        public string deviceName { get; set; }

        public int? channelNo { get; set; }

        public string channelName { get; set; }

        public string groupId { get; set; }

        public string groupName { get; set; }

        public long? captureTime { get; set; }

        public string resultUrl { get; set; }

        public string width { get; set; }

        public string height { get; set; }

        public string ruleId { get; set; }

        public string ruleName { get; set; }

        public string diskPath { get; set; }

        public DateTime? time { get; set; }

        public DateTime? createTime { get; set; }
    }
}
