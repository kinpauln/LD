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
using Webdiyer.WebControls.Mvc;
using LotteryDraw.Site.Extentions;


namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class AccountController : AccountControllerBase
    {
        #region 属性

        public AccountController()
        {
            base._areaName = "Website";
        }

        #endregion

        #region 视图功能

        [AuthorizeIgnore]
        //[OutputCache(Duration = 600, VaryByParam = "none", VaryByHeader = "none")]
        public ActionResult Register()
        {
            ViewBag.IsPostBack = false;
            MemberView model = new MemberView
            {

            };
            return View(model);
        }

        [HttpPost]
        [AuthorizeIgnore]
        [ValidateMvcCaptcha]
        public ActionResult Register(MemberView model, string psnl, string ent)
        {
            ViewBag.IsPostBack = true;
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
            if (!string.IsNullOrEmpty(psnl))
            {
                model.MemberType = MemberType.Personal;
            }
            else if (!string.IsNullOrEmpty(ent))
            {
                model.MemberType = MemberType.Enterprise;
            }
            try
            {
                string province = Request.Form["hidn_province"].ToString();
                string city = Request.Form["hidn_city"].ToString();
                string town = Request.Form["hidn_town"].ToString();
                string suffix = Request.Form["addr_suffix"].ToString();
                model.Province = province;
                model.City = city;
                model.Town = town;
                model.AddrSuffix = suffix;
                OperationResult result = AccountSiteContract.Register(model);
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

        public ActionResult Edit(long? id)
        {
            ViewBag.IsPostBack = false;

            return View(GetModelView(id));
        }

        [HttpPost]
        public ActionResult Edit(MemberView model)
        {
            ViewBag.IsPostBack = true;

            return View(model);
        }

        public ActionResult ChangePassword(long? id)
        {
            ViewBag.IsPostBack = false;

            MemberView model = new MemberView() { Id = id ?? 0 };
            return View(model);
        }

        [HttpPost]
        [ValidateMvcCaptcha]
        public ActionResult ChangePassword(MemberView model)
        {
            ViewBag.IsPostBack = true;

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

            if (!ChangePasswordValidate(model))
            {
                return View(model);
            }

            OperationResult result = AccountSiteContract.ChangePassword(model.Id, model.NewPassword);
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = string.Format("密码修改成功。");
                return RedirectToAction("InfoPage");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public JsonResult ChangePasswordAjax(FormCollection formdata)
        {
            return Json(new { OK = true, Message = "" });
        }
        public ActionResult Detail(long? id)
        {
            ViewBag.IsPostBack = false;
            return View(GetModelView(id));
        }

        #endregion

        #region 私有方法
        /// <summary>
        ///  修改密码后台验证
        /// </summary>
        private bool ChangePasswordValidate(MemberView model)
        {
            if (model.Id == 0)
            {
                ViewBag.Message = string.Format("用户Id莫名的为0，无法完成密码的修改。", model.Id);
                return false;
            }

            MemberView dbmodel = GetModelView(model.Id);
            if (dbmodel == null)
            {
                ViewBag.Message = string.Format("不能存在用户Id为{0}的用户，修改密码失败", model.Id);
                return false;
            }

            if (!model.NewPassword.ToLower().Equals(model.ConfirmNewPassword.ToLower()))
            {
                ViewBag.Message = "两次输入的新密码不一致";
                return false;
            }

            if (!LotteryDraw.Component.Utility.Encrypt.Encode(model.Password).Equals(dbmodel.Password.Trim()))
            {
                ViewBag.Message = "初始密码不正确";
                return false;
            }
            return true;
        }

        private MemberView GetModelView(long? id)
        {
            MemberView model = null;
            if (id.HasValue)
            {
                model = AccountContract.Members
                    .Where(mb => mb.Id == id.Value && !mb.IsDeleted)
                    .FirstOrDefault()
                    .ToSiteViewModel();
            }
            return model;
        } 
        #endregion

        #region override
        public override ActionResult InfoPage()
        {
            return View("~/Areas/Website/Views/Shared/InfoPage.cshtml");
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.LeftTitleContent = "管理面板";
            ViewBag.OptionName = "我的账号";
            ViewBag.MetHitsVisible = false;
            ViewBag.MemberId = this.UserId ?? 0;
        }
        #endregion
    }
}