using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.Model.Album
{
    public class Album
    {
        /// <summary>
        /// 相册标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 相册Id
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 初始默认图片序号0
        /// </summary>
        public int start { get; set; }

        /// <summary>
        /// 相册包含图片数组格式
        /// </summary>
        public List<AlbumContent> data { get; set; }
    }

    public class AlbumContent
    {
        /// <summary>
        /// 图片名
        /// </summary>
        public string alt { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 原图地址
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string thumb { get; set; }
    }
}
