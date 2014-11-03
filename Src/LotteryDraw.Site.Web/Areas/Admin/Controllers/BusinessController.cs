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

namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    [Export]
    public class BusinessController : AdminControllerBase
    {
        [Import]
        protected IPrizeOrderSiteContract PrizeOrderSiteContract { get; set; }

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
        public ActionResult ManualIntervention(int? id)
        {
            int userid = this.UserId ?? 0;
            int pageIndex = id ?? 1;
            PagedList<PrizeOrderDetailView> model = GetPagedListOfPrizeOrderDetailView(pageIndex);
            return View(model);
        }

        private PagedList<PrizeOrderDetailView> GetPagedListOfPrizeOrderDetailView(int pageIndex, string keywords = null)
        {
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("SortOrder") };

            string orderbyString = "SortOrder asc";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetWhereString(keywords);

            IEnumerable<PrizeOrderDetailView> rlist = null;
            OperationResult result = PrizeOrderSiteContract.GetLotteries(this.PageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, 0, (int)RevealState.UnDrawn);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    rlist = dt.ToPrizeOrderDetailList();
                    if (rlist != null)
                    {
                        PagedList<PrizeOrderDetailView> model = new PagedList<PrizeOrderDetailView>(rlist, pageIndex, this.PageSize, totalCount);

                        return model;
                    }
                }
            }
            return null;
        }

        private string GetWhereString(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(PrizeName like '%{0}%' or PrizeDescription like '%{0}%' or UserNickName like '%{0}%' or UserName like '%{0}%')", keywords);
        }

        public ActionResult Search(int id = 1, string kword = null)
        {
            int pageIndex = id;
            PagedList<PrizeOrderDetailView> model = GetPagedListOfPrizeOrderDetailView(pageIndex, kword);
            return View("~/Areas/Admin/Views/Business/ManualIntervention.cshtml", model);
        }

        /// <summary>
        ///  免审核
        /// </summary>
        /// <returns></returns>
        public ActionResult NonAudit()
        {
            return View();
        }
    }
}
