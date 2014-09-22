using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    public class BusinessController : AdminControllerBase
    {
        //
        // GET: /Admin/Business/

        public ActionResult Advertisements()
        {
            return View();
        }

        /// <summary>
        ///  置頂
        /// </summary>
        /// <returns></returns>
        public ActionResult Set2Top()
        {
            return View();
        }

        /// <summary>
        ///  人工干預
        /// </summary>
        public ActionResult ManualIntervention()
        {
            return View();
        }

        /// <summary>
        ///  免審核
        /// </summary>
        /// <returns></returns>
        public ActionResult NonAudit()
        {
            return View();
        }
    }
}
