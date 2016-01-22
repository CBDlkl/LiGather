using System;
using System.Web.Mvc;
using LiGather.Util;

namespace LiGather.Web.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BrowserVersionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var pageUrl = filterContext.RequestContext.HttpContext.Request.Url?.AbsolutePath ?? "";
            var browserName = filterContext.RequestContext.HttpContext.Request.Browser.Browser;
            var browserVersion = filterContext.RequestContext.HttpContext.Request.Browser.MajorVersion;

            if (pageUrl.Contains("/CheckBrowser/OldBrowser"))
            {
                if (!browserName.ToLower().Equals("ie"))
                {
                    filterContext.Result = new RedirectResult("/");
                }
            }
            else
            {
                if (browserName.ToLower().Equals("ie"))
                {
                    if (Conv.ToInt(browserVersion) < 10)
                    {
                        filterContext.Result = new RedirectResult("/CheckBrowser/OldBrowser");
                    }
                }
            }
            
        }
    }
}