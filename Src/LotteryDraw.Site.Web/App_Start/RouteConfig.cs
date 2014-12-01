using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LotteryDraw.Site.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "",
                 "_MvcCaptcha/MvcCaptchaImage/{id}",
                new { controller = "_MvcCaptcha", action = "MvcCaptchaImage", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , namespaces: new string[] { "LotteryDraw.Site.Web.Areas.Website.Controllers" }
                ).DataTokens.Add("Area", "Website");
        }
    }
}