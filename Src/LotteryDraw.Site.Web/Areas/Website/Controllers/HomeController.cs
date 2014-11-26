using LotteryDraw.Component.Tools;
using LotteryDraw.Core;
using LotteryDraw.Site.Web.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class HomeController : BaseController
    {
        #region 属性
        #region 受保护的属性

        [Import]
        protected IPrizeOrderSiteContract PrizeOrderSiteContract { get; set; }
        [Import]
        protected ILotteryResultContract LotteryResultContract { get; set; }

        #endregion
        #endregion

        public ActionResult Index()
        {
            OperationResult result = PrizeOrderSiteContract.GetTopPrizeOrders();
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    //DataRow[] arrayDR = dt.Select("RevealType=1");

                    //定时
                    ViewBag.TopTimingPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Timing);
                    //定员
                    ViewBag.TopQuotaPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Quota);
                    //答案
                    ViewBag.TopAnswerPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Answer);
                }
            }

            if (User.Identity.IsAuthenticated) {
                long userid = UserId ?? 0;
                ViewBag.NoticeCount = LotteryResultContract.LotteryResults.Where(lr => 
                    !lr.IsDeleted 
                    && lr.Member.Id == userid 
                    && lr.State == (int)LotteryResultState.Default).Count();
            }
            return View();
        }

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Website/Views/Shared/InfoPage.cshtml");
        }
    }
}
