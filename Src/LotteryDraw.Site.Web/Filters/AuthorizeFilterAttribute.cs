using System;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web
{
    /// <summary>
    /// Attribute for power Authorize
    /// </summary>
    public class AuthorizeFilterAttribute  : ActionFilterAttribute
    {
        public string Name { get; set; }

        public AuthorizeFilterAttribute()
        {
        }

        public AuthorizeFilterAttribute(string name)
        {
            this.Name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Authorize(filterContext, this.Name))
            {
                //filterContext.Result = new ContentResult { Content = "<script>alert('抱歉,你不具有当前操作的权限！');history.go(-1)</script>" };

                //跳转到登陆页
                filterContext.Result = new RedirectResult("/Admin/Account/Login");     
            }

            ////未登录
            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    if (!filterContext.RouteData.Values["Controller"].ToString().ToLower().Equals("account"))
            //    {
            //        //跳转到登陆页
            //        filterContext.Result = new RedirectResult("/Account/Login");
            //    }
            //}
            ////已登陆
            //else
            //{
            //    if (filterContext.RouteData.Values["Controller"].ToString().ToLower().Equals("account"))
            //    {
            //        //跳转到主页
            //        filterContext.Result = new RedirectResult("/Home/Index");
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }

        protected virtual bool Authorize(ActionExecutingContext filterContext, string permissionName)
        {
            if (filterContext.HttpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                return false;

            return true;
        }
    }
}