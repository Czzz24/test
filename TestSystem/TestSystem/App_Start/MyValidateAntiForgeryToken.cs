﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Helpers;

namespace TestSystem.App_Start
{
    public class MyValidateAntiForgeryToken:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;

                    try
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
        }
    }
}