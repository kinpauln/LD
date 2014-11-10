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
//添加引用
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    [Export]
    public class BusinessController : AdminControllerBase
    {
        [Import]
        protected IPrizeOrderSiteContract PrizeOrderSiteContract { get; set; }

        [Import]
        protected IWhiteListSiteContract WhiteListSiteContract { get; set; }

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
        ///  置頂
        /// </summary>
        [HttpPost]
        public JsonResult Set2Top(string prizeorderIdString)
        {
            if (string.IsNullOrEmpty(prizeorderIdString))
            {
                return Json(new { ErrorString = "prizeorder Id 为空" }, JsonRequestBehavior.AllowGet);
            }
            Guid prizeOrderId = new Guid(prizeorderIdString);

            OperationResult result = PrizeOrderSiteContract.Set2Top(prizeOrderId);

            if (result.ResultType == OperationResultType.Success)
            {
                return Json(new { ErrorString = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ErrorString = result.Message }, JsonRequestBehavior.AllowGet);
            }
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

        public PagedList<PrizeOrderDetailView> GetPagedListOfPrizeOrderDetailView(int pageIndex, string keywords = null)
        {
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("SortOrder") };

            string orderbyString = "SortOrder asc";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetWhereStringOfPrizeOrderDetail(keywords);

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

        private string GetWhereStringOfPrizeOrderDetail(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(PrizeName like '%{0}%' or PrizeDescription like '%{0}%' or UserNickName like '%{0}%' or UserName like '%{0}%')", keywords);
        }

        private string GetWhereStringOfMember(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(UserName like '%{0}%' or Name like '%{0}%' or Email like '%{0}%')", keywords);
        }

        [HttpPost]
        public JsonResult GetPagedListOfMemberView()
        {
            //取数据
            int userid = this.UserId ?? 0;
            int pageIndex = int.Parse(Request["pagenumber"].ToString());
            int pageSize = string.IsNullOrEmpty(Request["pageSize"].ToString()) ? this.PageSize : int.Parse(Request["pageSize"].ToString());
            string prizeOrderIdString = Request["prizeorderId"].ToString();
            if (string.IsNullOrEmpty(Request["prizeorderId"]))
            {
                return Json(new { ErrorString = "prizeorder Id 为空" }, JsonRequestBehavior.AllowGet);
            }
            Guid prizeOrderId = new Guid(prizeOrderIdString);
            string keywords = Request["kword"].ToString();
            Response.ContentType = "text/plain";
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("MemberId") };

            string orderbyString = "";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetWhereStringOfMember(keywords);

            IEnumerable<MemberView> rlist = null;
            OperationResult result = WhiteListSiteContract.GetUsers(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, prizeOrderId);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    rlist = dt.ToMemberViewList();
                    if (dt != null)
                    {
                        //Response.Write(JsonConvert.SerializeObject(dt, new DataTableConverter()));
                        string jsonString = JsonConvert.SerializeObject(new { PageCount = totalPageCount, Data = dt });
                        //Response.Write(jsonString);
                        return Json(new { PageCount = totalPageCount, Data = rlist, ErrorString = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { ErrorString = "没有符合条件的用户" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ErrorString = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddToWhiteList()
        {
            int memberid = int.Parse(Request["memberid"].ToString());

            string prizeOrderIdString = Request["prizeorderId"].ToString();
            if (string.IsNullOrEmpty(Request["prizeorderId"]))
            {
                return Json(new { ErrorString = "prizeorder Id 为空" }, JsonRequestBehavior.AllowGet);
            }
            Guid prizeOrderId = new Guid(prizeOrderIdString);

            OperationResult result = WhiteListSiteContract.Add(
                new WhiteListView(){ PrizeOrderId = prizeOrderId,MemberId =memberid});

            if (result.ResultType == OperationResultType.Success)
            {
                return Json(new { ErrorString = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ErrorString = result.Message }, JsonRequestBehavior.AllowGet);
            }
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
