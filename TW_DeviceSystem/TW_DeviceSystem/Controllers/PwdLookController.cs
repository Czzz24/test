using EF.Component.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class PwdLookController : Controller
    {
        // GET: PwdLook
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual JsonResult DecryptString(string originalText)
        {
            string result = ExtHelper.EncryptAESDe(originalText);
            return Json(result);
        }
    }
}