using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.EarthBox
{
    /// <summary>
    /// 接地箱箱柜门打开图片上传
    /// </summary>
    public class PictureUpload
    {
        /// <summary>
        /// 0代表上传成功,1代表上传失败
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string suffix { get; set; }

        /// <summary>
        /// 服务器网络地址
        /// </summary>
        public string serverPath { get; set; }

        /// <summary>
        /// 服务器路径
        /// </summary>
        public string filePath { get; set; }
    }
}
