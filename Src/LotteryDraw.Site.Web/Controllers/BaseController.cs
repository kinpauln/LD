using LotteryDraw.Component.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace LotteryDraw.Site.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public virtual int PageSize
        {
            get { return 10; }
        }

        public long? UserId {
            get
            {
                var cookie = this.ControllerContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string dataString = ticket.UserData;
                if (string.IsNullOrEmpty(dataString))
                    return null;

                string uidString = dataString.Split('|')[0];
                long userid = 0;
                bool result = Int64.TryParse(uidString, out userid);
                if (result)
                {
                    return userid;
                }
                return null;
            }
        }

        public int[] UserRoles
        {
            get
            {
                var cookie = this.ControllerContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string dataString = ticket.UserData;
                if (string.IsNullOrEmpty(dataString))
                    return null;
                string[] stringArray =  dataString.Split('|');
                if(stringArray!=null && stringArray.Length>1){
                    string roleString = stringArray[1];
                    try
                    {
                        return roleString.Split(',').Select(rid => { 
                            int currRid = 0;
                            bool result = int.TryParse(rid, out currRid);
                            return currRid; 
                        }).ToArray();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        public abstract ActionResult InfoPage();

        #region 公共提示画面操作
        /// <summary>
        /// 执行增,删，改操作后跳转到公共提示画面
        /// </summary>
        /// <param name="key">操作信息对应的key</param>
        /// <param name="param">参数数组</param>
        /// <param name="jsParam">通过Js跳转的URL</param>
        /// <returns>公共提示画面</returns>
        public ActionResult OperateTooltip(string key, string[] param, string jsParam = "", string view = "")
        {
            //获取操作信息对象
            OperateMsg msg = OperateMsg.getInstance();

            //获取操作信息
            string operateMsg = "";

            //判断参数
            if (string.IsNullOrEmpty(jsParam))
            {
                if (param == null)
                    operateMsg = msg.GetValue(key);
                else
                    operateMsg = msg.GetValue(key, param);
            }
            else
            {
                param = jsParam.Split(';');
                operateMsg = msg.GetValue(key, param);
            }

            //将操作信息添加到ViewBag中供页面获取
            ViewBag.operateMsg = operateMsg;

            //将html的ID放入viewbag中供画面获取
            ViewBag.ids = msg.GetIds(key); ;

            //判断是操作成功还是失败
            if (key.Contains("Fail"))
            {
                ViewBag.isSuccess = false;
            }
            else
            {
                ViewBag.isSuccess = true;
            }
            if (string.IsNullOrEmpty(view))
            {
                return View("~/Views/Common/OperateMsg.cshtml");
            }
            else
            {
                return View(view);
            }
        }
        #endregion

        protected ContentResult JsonP(string callback, object data)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return this.Content(string.Format("{0}({1})", callback, json));
        }

        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <returns></returns>
        public ContentResult RefreshParent(string alert = null)
        {
            var script = string.Format("<script>{0}; parent.location.reload(1)</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        /// <summary>
        /// 用JS关闭弹窗
        /// </summary>
        /// <returns></returns>
        public ContentResult CloseThickbox()
        {
            return this.Content("<script>top.tb_remove()</script>");
        }

        /// <summary>
        ///  警告并且历史返回
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ContentResult Back(string notice)
        {
            var content = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
                content.AppendFormat("alert('{0}');", notice);
            content.Append("history.go(-1)</script>");
            return this.Content(content.ToString());
        }


        public ContentResult PageReturn(string msg, string url = null)
        {
            var content = new StringBuilder("<script type='text/javascript'>");
            if (!string.IsNullOrEmpty(msg))
                content.AppendFormat("alert('{0}');", msg);
            if (string.IsNullOrWhiteSpace(url))
                url = Request.Url.ToString();
            content.Append("window.location.href='" + url + "'</script>");
            return this.Content(content.ToString());
        }

        /// <summary>
        /// 转向到一个提示页面，然后自动返回指定的页面
        /// </summary>
        /// <param name="notice"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public ContentResult Stop(string notice, string redirect, bool isAlert = false)
        {
            var content = "<meta http-equiv='refresh' content='1;url=" + redirect + "' /><body style='margin-top:0px;color:red;font-size:24px;'>" + notice + "</body>";

            if (isAlert)
                content = string.Format("<script>alert('{0}'); window.location.href='{1}'</script>", notice, redirect);

            return this.Content(content);
        }

        /// <summary>
        /// 当前Http上下文信息，用于写Log或其他作用
        /// </summary>
        public WebExceptionContext WebExceptionContext
        {
            get
            {
                var exceptionContext = new WebExceptionContext
                {
                    IP = Fetch.UserIp,
                    CurrentUrl = Fetch.CurrentUrl,
                    RefUrl = (Request == null || Request.UrlReferrer == null) ? string.Empty : Request.UrlReferrer.AbsoluteUri,
                    IsAjaxRequest = (Request == null) ? false : Request.IsAjaxRequest(),
                    FormData = (Request == null) ? null : Request.Form,
                    QueryData = (Request == null) ? null : Request.QueryString,
                    RouteData = (Request == null || Request.RequestContext == null || Request.RequestContext.RouteData == null) ? null : Request.RequestContext.RouteData.Values
                };

                return exceptionContext;
            }
        }

        /// <summary>
        /// 发生异常写Log
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var e = filterContext.Exception;
            LogException(e, this.WebExceptionContext);
            filterContext.Result = new RedirectResult("/default/error");
        }

        protected virtual void LogException(Exception exception, WebExceptionContext exceptionContext = null)
        {
            var message = new
            {
                exception = exception.Message,
                exceptionContext = exceptionContext,
            };

            //Log4NetHelper.Error(LoggerType.WebExceptionLog, message, exception);
        }
    }

    public class WebExceptionContext
    {
        public string IP { get; set; }
        public string CurrentUrl { get; set; }
        public string RefUrl { get; set; }
        public bool IsAjaxRequest { get; set; }
        public NameValueCollection FormData { get; set; }
        public NameValueCollection QueryData { get; set; }
        public RouteValueDictionary RouteData { get; set; }
    }
}
