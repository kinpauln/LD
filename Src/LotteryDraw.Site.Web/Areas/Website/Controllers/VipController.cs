using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class VipController : WebsiteControllerBase
    {
        [Import]
        public IPrizeSiteContract PrizeContract { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  发布奖品
        /// </summary>
        public ActionResult PublishPrize()
        {
            PrizeView model = new PrizeView {
                MemberId = this.UserId ?? 0
            };
            return View(model);
        }

        /// <summary>
        ///  发布奖品
        /// </summary>
        [HttpPost]
        public ActionResult PublishPrize(PrizeView model)
        {
            ViewBag.IsPostBack = true;
            if (model.MemberId == 0) {
                ViewBag.Message = "用户Id为0";
                return View(model);
            }
            OperationResult result = PrizeContract.Add(model);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "奖品发布成功。<br /><a href='/Vip/PublishPrize'>继续发布<a><br /><a href='/Vip/ManagePrizes'>奖品管理<a>";
                return RedirectToAction("InfoPage");
            }
            //ModelState.AddModelError("", msg);
            ViewBag.Message = msg;
            return View(model);
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
            ViewBag.MetHitsVisible = false;
            ViewBag.MemberId = this.UserId ?? 0;
        }
    }
}
