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
using LotteryDraw.Core;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    [Export]
    public class AccountController : AccountControllerBase
    {
        #region 属性

        public AccountController() {
            base._areaName = "Admin";
        }

        [Import]
        public IAccountContract AccountContract { get; set; }
        
        #endregion

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Admin/Views/Shared/InfoPage.cshtml");
        }

        //public new ActionResult UserList(int? id, string keywords)
        //{
        //    return base.UserList(id,keywords);
        //}

        public new ActionResult UserList(int? id, string kword)
        {
            int pageIndex = id ?? 1;
            int pageSize = 10;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };
            int total;
            
            var query = AccountContract.Members;
            if (!string.IsNullOrEmpty(kword))
            {
                query = query.Where(m => m.UserName.Contains(kword) || m.Name.Contains(kword) || m.Email.Contains(kword));
            }
            var memberViews = query
                .Where(m=>!m.IsDeleted)
                .Where<Member, Int64>(m => true, pageIndex, pageSize, out total, sortConditions).Select(m => new MemberView
            {
                Id = m.Id,
                UserName = m.UserName,
                Name = m.Name,
                Email = m.Email,
                IsDeleted = m.IsDeleted,
                AddDate = m.AddDate,
                LoginLogCount = m.LoginLogs.Count,
                RoleNames = m.Roles.Select(n => n.Name)
            });
            ViewBag.Keywords = kword;
            ViewBag.TotalCount = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            PagedList<MemberView> model = new PagedList<MemberView>(memberViews, pageIndex, pageSize, total);
            return View(model);
        }

        /// <summary>
        ///  免审核
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public JsonResult NoAudit(long memberid,int? noauditTimes)
        {
            if (memberid==0)
            {
                return Json(new { ErrorString = "用户Id不合法" }, JsonRequestBehavior.AllowGet);
            }

            OperationResult result = AccountSiteContract.NoAudit(memberid, noauditTimes);

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
        ///  删除
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public JsonResult Delete(long memberid)
        {
            if (memberid==0)
            {
                return Json(new { ErrorString = "用户Id不合法" }, JsonRequestBehavior.AllowGet);
            }

            OperationResult result = AccountSiteContract.Delete(memberid);

            if (result.ResultType == OperationResultType.Success)
            {
                return Json(new { ErrorString = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ErrorString = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}