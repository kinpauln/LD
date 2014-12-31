using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LotteryDraw.Site.Extentions;
using Webdiyer.WebControls.Mvc;
using LotteryDraw.Core;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BusinessController : WebsiteControllerBase
    {
        #region 属性
        #region 受保护的属性

        [Import]
        protected IPrizeOrderSiteContract PrizeOrderSiteContract { get; set; }

        [Import]
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        #endregion
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  定员抽奖
        /// </summary>
        [AuthorizeIgnore]
        public ActionResult Quota(int? id, string keywords)
        {
            int pageIndex = id ?? 1;
            //ViewBag.PageIndex = pageIndex;
            //ViewBag.RevealType = RevealType.Quota.ToInt();
            ViewBag.Keywords = keywords;
            var model = GetLotteries(RevealType.Quota.ToInt(), pageIndex, keywords);
            return View(model);
        }

        /// <summary>
        ///  定时抽奖
        /// </summary>
        [AuthorizeIgnore]
        public ActionResult Timing(int? id, string keywords)
        {
            int pageIndex = id ?? 1;
            //ViewBag.PageIndex = pageIndex;
            //ViewBag.RevealType = RevealType.Quota.ToInt();
            ViewBag.Keywords = keywords;
            var model = GetLotteries(RevealType.Timing.ToInt(), pageIndex, keywords);
            return View(model);
        }

        /// <summary>
        ///  答案抽奖
        /// </summary>
        [AuthorizeIgnore]
        public ActionResult Answer(int? id, string keywords)
        {
            int pageIndex = id ?? 1;
            //ViewBag.PageIndex = pageIndex;
            //ViewBag.RevealType = RevealType.Quota.ToInt();
            ViewBag.Keywords = keywords;
            var model = GetLotteries(RevealType.Answer.ToInt(), pageIndex, keywords);
            return View(model);
        }

        /// <summary>
        ///  现场抽奖
        /// </summary>
        [AuthorizeIgnore]
        public ActionResult Scene(int? id, string keywords)
        {
            int pageIndex = id ?? 1;
            //ViewBag.PageIndex = pageIndex;
            //ViewBag.RevealType = RevealType.Quota.ToInt();
            ViewBag.Keywords = keywords;

            ViewBag.UserId = this.UserId ?? 0;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("SortOrder") };

            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageCount"]);
            string orderbyString = "SortOrder asc";
            int totalCount;
            int totalPageCount;

            string whereString = GetWhereStringOfPrizeOrderDetail(keywords);

            IEnumerable<PrizeOrderDetailView> rlist = null;
            OperationResult result = PrizeOrderSiteContract.GetRevealedSceneLotteries(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, RevealState.Drawn.ToInt());
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    //DataTable dt = ds.Tables[0];
                    //DataRow[] rowarray = new DataRow[dt.Rows.Count];
                    //dt.Rows.CopyTo(rowarray, 0);
                    ////所有
                    //ViewBag.AllPrizeOrders = rowarray;

                    DataTable dt = ds.Tables[0];

                    rlist = dt.ToPrizeOrderDetailList();
                    if (rlist != null)
                    {
                        PagedList<PrizeOrderDetailView> model = new PagedList<PrizeOrderDetailView>(rlist, pageIndex, pageSize, totalCount);
                        return View(model);
                    }
                }
            }
            ViewBag.Message = result.Message;
            return null;
        }

        /// <summary>
        ///  奖单详情
        /// </summary>
        //public ActionResult PrizeOrderDetail(Guid id)
        //{
        //    PrizeOrderView model = PrizeOrderContract.PrizeOrders
        //        .Where(p => p.Id.Equals(id))
        //        .Select(p => new PrizeOrderView()
        //        {
        //            Id = p.Id,
        //            RevealTypeNum = p.RevealTypeNum,
        //            RevealStateNum = p.RevealStateNum,
        //            LaunchTime = p.Extend.LaunchTime,
        //            MinLuckyCount = p.Extend.MinLuckyCount,
        //            LuckyCount = p.Extend.LuckyCount,
        //            LuckyPercent = p.Extend.LuckyPercent,
        //            PoolCount = p.Extend.PoolCount,
        //            Remarks = p.Extend.Remarks,
        //            AddDate = p.AddDate
        //        }).FirstOrDefault();
        //    if (model == null)
        //        ViewBag.Message = string.Format("不存在Id为{0}的奖单", id);
        //    return View(model);
        //} 
        
        /// <summary>
        ///  奖单详情
        /// </summary>
        public ActionResult PrizeOrderDetail()
        {
            return View();
        }

        //public ActionResult PrizeOrders(int revealType, int pageIndex, string keywords)
        //{
        //    var model = GetLotteries(revealType, pageIndex, keywords);
        //    return PartialView(model);
        //}

        /// <summary>
        ///  获取最新的可抽奖信息
        /// </summary>
        private PagedList<PrizeOrderDetailView> GetLotteries(int? rtype, int pageIndex, string keywords = null)
        {
            ViewBag.UserId = this.UserId ?? 0;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("SortOrder") };

            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageCount"]);
            string orderbyString = "SortOrder asc";
            int totalCount;
            int totalPageCount;

            string whereString = GetWhereStringOfPrizeOrderDetail(keywords);

            IEnumerable<PrizeOrderDetailView> rlist = null;
            OperationResult result = PrizeOrderSiteContract.GetLotteries(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, rtype ?? 0);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    //DataTable dt = ds.Tables[0];
                    //DataRow[] rowarray = new DataRow[dt.Rows.Count];
                    //dt.Rows.CopyTo(rowarray, 0);
                    ////所有
                    //ViewBag.AllPrizeOrders = rowarray;

                    DataTable dt = ds.Tables[0];

                    rlist = dt.ToPrizeOrderDetailList();
                    if (rlist != null)
                    {
                        PagedList<PrizeOrderDetailView> model = new PagedList<PrizeOrderDetailView>(rlist, pageIndex, pageSize, totalCount);
                        return model;
                    }
                }
            }
            ViewBag.Message = result.Message;
            return null;
        }

        private string GetWhereStringOfPrizeOrderDetail(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(PrizeName like '%{0}%' or PrizeDescription like '%{0}%' or UserNickName like '%{0}%' or UserName like '%{0}%')", keywords);
        }

        /// <summary>
        ///  联系我们
        /// </summary>
        //[AuthorizeIgnore]
        public ActionResult ContactUs()
        {
            ViewBag.LeftTitleContent = "联系我们";
            return View();
        }
    }
}
