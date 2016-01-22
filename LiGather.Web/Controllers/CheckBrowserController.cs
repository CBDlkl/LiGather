using System.Web.Mvc;
using LiGather.Web.Models;

namespace LiGather.Web.Controllers
{
    [BrowserVersion]
    public class CheckBrowserController : Controller
    {
        public ActionResult OldBrowser()
        {
            return View();
        }
    }
}