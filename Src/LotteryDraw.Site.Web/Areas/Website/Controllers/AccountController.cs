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
using LotteryDraw.Site.Web.Controllers;


namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class AccountController : AccountControllerBase
    {
        #region 属性

        public AccountController() {
            base._areaName = "Website";
        }

        #endregion

        #region 视图功能

        [AuthorizeIgnore]
        public ActionResult Register()
        {
            ViewBag.IsPostBack = false;
            MemberView model = new MemberView {
                
            };
            return View(model);
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Register(MemberView model,string psnl,string ent)
        {
            ViewBag.IsPostBack = true;
            if (!string.IsNullOrEmpty(psnl)) {
                model.MemberType = MemberType.Personal;
            }
            else if (!string.IsNullOrEmpty(ent))
            {
                model.MemberType = MemberType.Enterprise;
            }
            try
            {
                OperationResult result = AccountContract.Register(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    TempData["Message"] = "注册成功。现在<a href='/Account/Login'>登录<a>";
                    return RedirectToAction("InfoPage");
                }
                //ModelState.AddModelError("", msg);
                ViewBag.Message = msg;
                return View(model);
            }
            catch (Exception e)
            {
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = e.Message;
                return View(model);
            }
        }

        #endregion

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Website/Views/Shared/InfoPage.cshtml");
        }
    }
}