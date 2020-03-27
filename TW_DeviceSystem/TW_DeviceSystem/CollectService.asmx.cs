using EF.Application.From.Model.EarthBox;
using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TW_DeviceSystem
{
    /// <summary>
    /// CollectService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class CollectService : System.Web.Services.WebService
    {
        private readonly static string serverPath = ConfigurationManager.AppSettings["ImageServerPath"].ToString();
        private readonly static string basePath = "~/exboxPicture/Images/";

        [WebMethod]
        public string CameraUpload(string pictureImage)
        {
            try
            {
                byte[] imgbyte = ImageHelper.HexToBytes(pictureImage);
                Image image = ImageHelper.BinaryToImg(imgbyte);
                string savaPath = (basePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(basePath) : basePath;
                if (!Directory.Exists(savaPath))
                    Directory.CreateDirectory(savaPath);
                string fileName = Guid.NewGuid().ToString("N");
                string suffix = ".JPEG";
                string filePath = "/exboxPicture/Images/" + fileName + ".JPEG";
                string full = savaPath + fileName + ".JPEG";
                image.Save(full, ImageFormat.Jpeg);
                PictureUpload model = new PictureUpload
                {
                    code = 0,
                    msg = "接地箱箱柜门打开图片上传成功!",
                    fileName = fileName,
                    suffix = suffix,
                    serverPath = serverPath,
                    filePath = filePath,
                };
                string result = JsonHelper.JsonSerializer<PictureUpload>(model);
                return result;
            }
            catch (Exception ex)
            {
                PictureUpload model = new PictureUpload
                {
                    code = 1,
                    msg = "接地箱箱柜门打开图片上传失败!"+ex.ToString(),
                    fileName = null,
                    suffix = null,
                    serverPath = null,
                    filePath = null,
                };
                string result = JsonHelper.JsonSerializer<PictureUpload>(model);
                return result;
            }
        }
    }
}
