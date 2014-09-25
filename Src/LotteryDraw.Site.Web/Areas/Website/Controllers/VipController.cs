using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    public class VipController : WebsiteControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        ///  发布奖品
        /// </summary>
        public ActionResult PublishPrize()
        {
            return View();
        }

        /// <summary>
        ///  奖品管理
        /// </summary>
        public ActionResult ManagePrizes()
        {
            return View();
        }

        /// <summary>
        ///  发起抽奖
        /// </summary>
        public ActionResult LaunchPrize()
        {
            return View();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.LeftTitleContent = "管理面板";
            ViewBag.OptionName = "会员选项";
        }
    }
}
