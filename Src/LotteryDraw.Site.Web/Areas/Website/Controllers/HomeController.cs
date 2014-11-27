using LotteryDraw.Component.Tools;
using LotteryDraw.Core;
using LotteryDraw.Site.Models;
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

        public ActionResult Index(int? rtype)
        {
            // 获取最新的可抽奖信息
            GetGetTopPrizeOrders(rtype);

            // 获取前N个最新中奖用户
            GetTopLuckies();

            if (rtype.HasValue) {
                ViewBag.RTypeParam = rtype.Value;
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

        /// <summary>
        ///  获取前N个最新中奖用户
        /// </summary>
        private void GetTopLuckies() {
            int luckyCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TopCountOfLuckyMember"]);
            var topLuckies = LotteryResultContract.LotteryResults.Where(lr =>
                    !lr.IsDeleted)
                    .OrderByDescending(lr => lr.AddDate).Take(luckyCount).Select(lr => new LotteryResultView
                    { 
                        MemberView = new MemberView(){
                        UserName =lr.Member.UserName,
                        Name = lr.Member.Name,
                        Tel = lr.Member.Extend.Tel
                    }
                    }).ToList();
            ViewBag.TopLuckies = topLuckies;
        }

        /// <summary>
        ///  获取最新的可抽奖信息
        /// </summary>
        private void GetGetTopPrizeOrders(int? rtype)
        {
            int topCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TopCountOfPrizeOrder"]);
            OperationResult result = PrizeOrderSiteContract.GetTopPrizeOrders(topCount,rtype);
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow[] rowarray = new DataRow[dt.Rows.Count];
                    dt.Rows.CopyTo(rowarray, 0);
                    //所有
                    ViewBag.AllPrizeOrders = rowarray;

                    //定时
                    ViewBag.TopTimingPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Timing);
                    //定员
                    ViewBag.TopQuotaPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Quota);
                    //答案
                    ViewBag.TopAnswerPrizeOrders = dt.Select("RevealType=" + (int)RevealType.Answer);
                }
            }
        }

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Website/Views/Shared/InfoPage.cshtml");
        }
    }
}
