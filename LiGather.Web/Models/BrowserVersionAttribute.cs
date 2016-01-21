using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiGather.Util;

namespace LiGather.Web.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BrowserVersionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var browserName = filterContext.RequestContext.HttpContext.Request.Browser.Browser;
            var browserVersion = filterContext.RequestContext.HttpContext.Request.Browser.MajorVersion;
            if (browserName.ToLower().Equals("ie"))
            {
                if (Conv.ToInt(browserVersion) < 10)
                {
                    filterContext.Result = new RedirectResult("/OtherPages/OldBrowser.html");
                }
            }
        }
    }
}