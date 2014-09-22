// 源文件头信息：
// <copyright file="AccountController.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Site.Web
// 最后修改：王金鹏
// 最后修改：2014/09/12 0:41
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Helper.Logging;
using LotteryDraw.Site.Impl;
using LotteryDraw.Site.Models;


namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class AccountController : Controller
    {
        #region 属性

        [Import]
        public IAccountSiteContract AccountContract { get; set; }

        #endregion

        #region 视图功能
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index", "Default", new { area = "Website" });
            if (User.Identity.IsAuthenticated) {
                return Redirect(returnUrl);
            }
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            ViewBag.IsPostBack = false;
            return View(model);
        }

        [AuthorizeIgnore]
        public ActionResult Register()
        {
            ViewBag.IsPostBack = false;
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(LoginModel model)
        {
            ViewBag.IsPostBack = true;
            //try
            //{
            //    OperationResult result = AccountContract.Login(model);
            //    string msg = result.Message ?? result.ResultType.ToDescription();
            //    if (result.ResultType == OperationResultType.Success)
            //    {
            //        return Redirect(model.ReturnUrl);
            //    }
            //    ModelState.AddModelError("", msg);
            //    ViewBag.Message = msg;
            //    return View(model);
            //}
            //catch (Exception e)
            //{
            //    ModelState.AddModelError("", e.Message);
            //    ViewBag.Message = e.Message;
            //    return View(model);
            //}
            return View(model);
        }

        public ActionResult Logout()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index", "Default", new { area = "Website" });
            if (User.Identity.IsAuthenticated)
            {
                AccountContract.Logout();
            }
            return Redirect(returnUrl);
        }

        #endregion
    }
}