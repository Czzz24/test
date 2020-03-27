using EF.Application.Model;
using EF.Component.Tools;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TW_DeviceSystem
{
    /// <summary>
    /// fileupload 的摘要说明
    /// </summary>
    public class fileupload : IHttpHandler
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static string ImageServerPath = ConfigurationManager.AppSettings["ImageServerPath"].ToString();
        private readonly static t_MaintainPictureBLL bll = new t_MaintainPictureBLL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request["REQUEST_METHOD"] == "OPTIONS")
            {
                context.Response.End();
            }
            SaveFile();
        }

        private void SaveFile(string basePath="~/Upload/Images/")
        {
            var name = string.Empty;
            string savaPath = (basePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(basePath) : basePath;
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (!Directory.Exists(savaPath))
                Directory.CreateDirectory(savaPath);
            var suffix = files[0].ContentType.Split('/');

           // var _suffix = suffix[1].Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) ? "" : suffix[1];
            //文件名
            var _temp = HttpContext.Current.Request["name"];

            var _suffix = _temp.Split('.')[1];

            //if (!string.IsNullOrEmpty(_temp))
            //{
            //    name = _temp;
            //}
            //else
            //{
            //    Random rand = new Random(24 * (int)DateTime.Now.Ticks);
            //    name = rand.Next() + "." + _suffix;
            //}
            /*唯一标识*/
            name = Guid.NewGuid().ToString("N");
            var full = savaPath + name + "." + _suffix;
            files[0].SaveAs(full);

            t_MaintainPicture model = new t_MaintainPicture
            {
                suffix = _suffix,
                fileName = name,
                CreateTime = DateTime.Now,
                serverPath = ImageServerPath,
                filePath = "/Upload/Images/" + name + "." + _suffix,
            };
            bll.AddMaintainPicture(conStr, model);
            var _result = JsonHelper.JsonSerializer(model);
            HttpContext.Current.Response.Write(_result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}