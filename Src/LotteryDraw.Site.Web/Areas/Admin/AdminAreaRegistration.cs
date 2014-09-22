﻿using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default1",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                , new string[] { "LotteryDraw.Site.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
