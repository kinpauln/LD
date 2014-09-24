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


namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    [Export]
    public class AccountController : AccountControllerBase
    {
        #region 属性

        public AccountController() {
            base._areaName = "Admin";
        }

        #endregion

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Admin/Views/Shared/InfoPage.cshtml");
        }
    }
}