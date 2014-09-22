using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website
{
    public class WebsiteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Website";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Website_default",
                "Website/{controller}/{action}/{id}",
                new { controller="Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
