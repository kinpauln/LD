using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Site.Models;

using Webdiyer.WebControls.Mvc;
using LotteryDraw.Core;


namespace LotteryDraw.Site.Web.Areas.Admin.Controllers
{
    [Export]
    public class HomeController : AdminControllerBase
    {
        [Import]
        public IAccountSiteContract AccountSiteContract { get; set; }

        [Import]
        public IAccountContract AccountContract { get; set; }

        public ActionResult Index(int? id, string keywords)
        {
            int pageIndex = id ?? 1;
            int pageSize = 10;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };
            int total;
            var memberViews = AccountContract.Members.Where(m => m.UserName.Contains(keywords) || m.Name.Contains(keywords) || m.Email.Contains(keywords)).Where<Member, Int64>(m => true, pageIndex, pageSize, out total, sortConditions).Select(m => new MemberView
            {
                UserName = m.UserName,
                Name = m.Name,
                Email = m.Email,
                IsDeleted = m.IsDeleted,
                AddDate = m.AddDate,
                LoginLogCount = m.LoginLogs.Count,
                RoleNames = m.Roles.Select(n => n.Name)
            });
            ViewBag.TotalCount = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageCount = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            PagedList<MemberView> model = new PagedList<MemberView>(memberViews, pageIndex, pageSize, total);
            return View(model);
        }
    }
}
