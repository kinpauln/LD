using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    public class BusinessController : Controller
    {
        //
        // GET: /Website/Business/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  联系我们
        /// </summary>
        public ActionResult ContactUs()
        {
            ViewBag.LeftTitleContent = "联系我们";
            return View();
        }
    }
}
