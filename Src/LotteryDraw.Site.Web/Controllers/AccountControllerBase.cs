using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LotteryDraw.Site.Extentions;

namespace LotteryDraw.Site.Web.Controllers
{
    [Export]
    public abstract class AccountControllerBase : BaseController
    {
        protected string _areaName = string.Empty;

        #region 属性

        [Import]
        public IAccountSiteContract AccountContract { get; set; }

        #endregion

        #region 视图功能
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index", "Home", new { area = _areaName });
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            ViewBag.IsPostBack = false;
            return View(model);
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(LoginModel model)
        {
            ViewBag.IsPostBack = true;
            try
            {
                OperationResult result = AccountContract.Login(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return Redirect(model.ReturnUrl);
                }
                ModelState.AddModelError("", msg);
                ViewBag.Message = msg;
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                ViewBag.Message = e.Message;
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            string returnUrl = Request.Params["returnUrl"];
            if (_areaName.Trim().ToLower().Equals("Admin"))
            {
                returnUrl = returnUrl ?? Url.Action("Login", "Account", new { area = "Admin" });
            }
            else
            {
                returnUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "Website" });
            }
            if (User.Identity.IsAuthenticated)
            {
                AccountContract.Logout();
            }
            return Redirect(returnUrl);
        }

        #endregion


        //后台数据库分页
        [HttpPost]
        public JsonResult li()
        {
            //取数据
            int userid = this.UserId ?? 0;
            int pageIndex = int.Parse(Request["pagenumber"].ToString());
            string keywords = Request["kword"].ToString();
            Response.ContentType = "text/plain";
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("MemberId") };

            string orderbyString = "";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetUserWhereString(keywords);

            IEnumerable<PrizeOrderDetailView> rlist = null;
            OperationResult result = AccountContract.GetUsers(this.PageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (dt != null)
                    {
                        //Response.Write(JsonConvert.SerializeObject(dt, new DataTableConverter()));
                        string jsonString = JsonConvert.SerializeObject(new { PageCount = totalPageCount, Data = dt });
                        //Response.Write(jsonString);
                        return Json(new { PageCount = totalPageCount, Data = dt.ToMemberViewList() },JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return null;
        }


        private string GetUserWhereString(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(UserName like '%{0}%' or Name like '%{0}%' or Email like '%{0}%')", keywords);
        }
        //记录总数
        [HttpGet]
        public ActionResult count()
        {
            Response.ContentType = "text/plain";
            string sql = "select count(id) as count from Members";
            //DataTable tb = DB.DBHelper.GetDataSet(sql);
            int pagecount = 50;
            if (pagecount % this.PageSize == 0)
            {
                pagecount = pagecount / this.PageSize;
            }
            else
            {
                pagecount = pagecount / this.PageSize + 1;
            }
            Response.Write("[{count:" + pagecount + "}]");
            return null;
        }
    }
}
