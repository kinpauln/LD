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
using Webdiyer.WebControls.Mvc;
using LotteryDraw.Core;

namespace LotteryDraw.Site.Web.Controllers
{
    [Export]
    public abstract class AccountControllerBase : BaseController
    {
        protected string _areaName = string.Empty;

        #region 属性

        [Import]
        public IAccountContract AccountContract { get; set; }

        [Import]
        public IAccountSiteContract AccountSiteContract { get; set; }

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

            if (TempData["AlertMessage"] != null)
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }

            ViewBag.IsPostBack = false;
            return View(model);
        }

        [HttpPost]
        [AuthorizeIgnore]
        [ValidateMvcCaptcha]
        public ActionResult Login(LoginModel model)
        {
            ViewBag.IsPostBack = true;
            try
            {
                if (_areaName == "Website")
                {
                    if (ModelState.IsValid)
                    {
                        //验证码验证通过
                    }
                    else
                    {
                        //验证码验证失败
                        //ModelState.AddModelError("", e.Message);
                        ViewBag.Message = "验证码输入不正确";
                        return View(model);
                    }
                }

                OperationResult result = AccountSiteContract.Login(model);
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
                AccountSiteContract.Logout();
            }
            return Redirect(returnUrl);
        }

        #endregion

        //后台数据库分页
        [HttpPost]
        public JsonResult AjaxGettingUsers()
        {
            //取数据
            long userid = this.UserId ?? 0;
            int pageIndex = int.Parse(Request["pagenumber"].ToString());
            int pageSize = string.IsNullOrEmpty(Request["pageSize"].ToString()) ? this.PageSize : int.Parse(Request["pageSize"].ToString());
            string keywords = Request["kword"].ToString();
            Response.ContentType = "text/plain";
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("MemberId") };

            string orderbyString = "";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetUserWhereString(keywords);

            OperationResult result = AccountSiteContract.GetUsers(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = totalPageCount;
            IEnumerable<MemberView> rlist = GettingUsersLogic(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);
            if (result.ResultType == OperationResultType.Success)
            {
                DataSet ds = (DataSet)result.AppendData;
                if (rlist != null)
                {
                    return Json(new { PageCount = totalPageCount, Data = rlist }, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        //后台数据库分页
        protected ActionResult UserList(int? id, string keywords)
        {
            //取数据
            long userid = this.UserId ?? 0;
            int pageIndex = id ?? 1;
            int pageSize = this.PageSize;
            Response.ContentType = "text/plain";
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("MemberId") };

            string orderbyString = "";
            int totalCount;
            int totalPageCount;
            string whereString = string.Empty;
            whereString = GetUserWhereString(keywords);

            IEnumerable<MemberView> rlist = GettingUsersLogic(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);

            PagedList<MemberView> model = new PagedList<MemberView>(rlist, pageIndex, pageSize, totalPageCount);
            return View(model);
        }

        private IEnumerable<MemberView> GettingUsersLogic(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount)
        {
            IEnumerable<MemberView> rlist = null;
            OperationResult result = AccountSiteContract.GetUsers(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);
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
                }
            }
            return rlist;
        }

        private string GetUserWhereString(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return null;
            return string.Format("(UserName like '%{0}%' or Name like '%{0}%' or Email like '%{0}%')", keywords);
        }
    }
}
