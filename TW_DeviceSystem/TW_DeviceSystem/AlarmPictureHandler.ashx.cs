using EF.Application.From.Model;
using EF.Component.Tools;
using EF.From.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem
{
    /// <summary>
    /// AlarmPictureHandler 的摘要说明
    /// </summary>
    public class AlarmPictureHandler : IHttpHandler
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static string ImageServerPath = ConfigurationManager.AppSettings["ImageServerPath"].ToString();
        private readonly static t_AlarmHandPictureBLL bll = new t_AlarmHandPictureBLL();
        private readonly static FromConnection fromCon = new FromConnection();

        private readonly static string basePath = "~/Upload/AlarmImages/";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request["REQUEST_METHOD"] == "OPTIONS")
            {
                context.Response.End();
            }
            int ElectricId = int.Parse(context.Request["ElectricId"]);
            int LineID = int.Parse(context.Request["LineID"]);
            string conFstr = fromCon.GetConStr(conStr, ElectricId, LineID);
            SaveFile(conFstr);
        }

        private void SaveFile(string conFStr)
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

            t_AlarmHandPicture model = new t_AlarmHandPicture
            {
                suffix = _suffix,
                fileName = name,
                CreateTime = DateTime.Now,
                serverPath = ImageServerPath,
                filePath = "/Upload/AlarmImages/" + name + "." + _suffix,
            };
            bll.Add(conFStr, model);
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