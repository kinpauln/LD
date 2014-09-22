using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Website/Default/

        public ActionResult Index()
        {
            return View();
        }

    }
}
