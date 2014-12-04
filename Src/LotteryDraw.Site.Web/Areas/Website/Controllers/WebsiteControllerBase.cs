using LotteryDraw.Site;
using LotteryDraw.Site.Models;
using LotteryDraw.Site.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    public abstract class WebsiteControllerBase : BaseController
    {
        /// <summary>
        /// 重写分页Size
        /// </summary>
        public override int PageSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageCount"]);
            }
        }

        public override ActionResult InfoPage()
        {
            return View("~/Areas/Website/Views/Shared/InfoPage.cshtml");
        }

        /// <summary>
        ///  用户可发起抽奖的次数
        /// </summary>
        protected int PubishingEnableTimes
        {
            get
            {
                //var cookie = this.ControllerContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                //var ticket = FormsAuthentication.Decrypt(cookie.Value);
                //string dataString = ticket.UserData;
                //if (string.IsNullOrEmpty(dataString))
                //    return 0;

                //string pubtimesString = dataString.Split('|')[2];
                //int pubtimes = 0;
                //bool result = int.TryParse(pubtimesString, out pubtimes);
                //if (result)
                //{
                //    return pubtimes;
                //}
                //return 0;
                string times = string.Empty;
                if (Session["publishTimes"] != null)
                {
                    int timesvalue = 0;
                    if (int.TryParse(Session["publishTimes"].ToString(), out timesvalue))
                    {
                        return timesvalue;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///  更新可发起抽奖的次数
        /// </summary>
        /// <returns></returns>
        protected bool UpdatePubishingEnableTimes()
        {
            string times = string.Empty;
            if (Session["publishTimes"] != null)
            {
                int timesvalue = 0;
                if (int.TryParse(Session["publishTimes"].ToString(), out timesvalue))
                {
                    Session["publishTimes"] = timesvalue - 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        ///// <summary>
        ///// 登录后用户信息
        ///// </summary>
        //public virtual LoginModel LoginModel
        //{
        //    get
        //    {
        //        return null;
        //        //return new LoginModel(){ LoginName = User.Identity.Name,Password = User.Identity.
        //    }
        //}

        ///// <summary>
        ///// 登录后用户信息里的用户权限
        ///// </summary>
        //public virtual List<BusinessPermission> PermissionList
        //{
        //    get
        //    {
        //        var permissionList = new List<BusinessPermission>();

        //        if (this.LoginModel != null)
        //            permissionList = this.LoginInfo.BusinessPermissionList;

        //        return permissionList;
        //    }
        //}

        #region Override controller methods
        /// <summary>
        /// 方法执行前，如果没有登录就调整到登录页面，没有权限就抛出信息
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0)
                return;

            base.OnActionExecuting(filterContext);

            if (!User.Identity.IsAuthenticated)
            {
                filterContext.Result = RedirectToAction("Login", "Account", new { Area = "Website" });
                return;
            }

            //bool hasPermission = true;
            //var permissionAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>();
            //permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>().Union(permissionAttributes);
            //var attributes = permissionAttributes as IList<PermissionAttribute> ?? permissionAttributes.ToList();
            //if (permissionAttributes != null && attributes.Count() > 0)
            //{
            //    hasPermission = true;
            //    foreach (var attr in attributes)
            //    {
            //        foreach (var permission in attr.Permissions)
            //        {
            //            if (!this.LoginInfo.BusinessPermissionList.Contains(permission))
            //            {
            //                hasPermission = false;
            //                break;
            //            }
            //        }
            //    }

            //    if (!hasPermission)
            //    {
            //        if (Request.UrlReferrer != null)
            //            filterContext.Result = this.Stop("没有权限！", Request.UrlReferrer.AbsoluteUri);
            //        else
            //            filterContext.Result = Content("没有权限！");
            //    }
            //}
        }

        /// <summary>
        /// 方法后执行后注入一些视图数据
        /// </summary>
        /// <param name="filterContext">filter context</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (filterContext.ActionDescriptor.ActionName.Contains("Edit") ||
            //    filterContext.ActionDescriptor.ActionName.Contains("Add"))
            //    return;

            //RenderViewData();
            ViewBag.MetHitsVisible = true;
        }

        /// <summary>
        /// 如果是Ajax请求的话，清除浏览器缓存
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
            }

            base.OnResultExecuted(filterContext);
        }

        ///// <summary>
        ///// 注入资源，权限，城市等信息
        ///// </summary>
        //protected override void RenderViewData()
        //{
        //    //var permissions = string.Join(",", this.PermissionList);
        //    //this.ViewData["permissions"] = permissions;
        //}

        #endregion
    }
}
