using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Controllers
{
    public class AccountController : Controller
    {

        #region 属性

        [Import]
        public IAccountSiteContract AccountContract { get; set; }

        #endregion

        #region 视图功能
        [AuthorizeIgnore]
        public ActionResult Login(string areaName)
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index", "Home", new { area = areaName });
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
            returnUrl = returnUrl ?? Url.Action("Login", "Account", new { area = "Admin" });
            if (User.Identity.IsAuthenticated)
            {
                AccountContract.Logout();
            }
            return Redirect(returnUrl);
        }

        #endregion
    }
}
