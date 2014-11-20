// 源文件头信息：
// <copyright file="AccountSiteService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Site
// 最后修改：王金鹏
// 最后修改：2013/05/20 13:06
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Impl;
using LotteryDraw.Core.Models;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Site.Models;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core;


namespace LotteryDraw.Site.Impl
{
    /// <summary>
    ///     账户模块站点业务实现
    /// </summary>
    [Export(typeof(IAccountSiteContract))]
    internal class AccountSiteService : IAccountSiteContract
    {
        [Import]
        public IAccountContract AccountContract { get; set; }

        /// <summary>
        ///     用户登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Login(LoginModel model)
        {
            PublicHelper.CheckArgument(model, "model");
            LoginInfo loginInfo = new LoginInfo
            {
                Account = model.Account,
                Password = model.Password,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            OperationResult result = AccountContract.Login(loginInfo);
            if (result.ResultType == OperationResultType.Success)
            {
                Member member = (Member)result.AppendData;
                DateTime expiration = model.IsRememberLogin
                    ? DateTime.Now.AddDays(7)
                    : DateTime.Now.Add(FormsAuthentication.Timeout);
                string useridString = member.Id.ToString();
                string roleIdString = member.Roles == null ? string.Empty : string.Join(",", member.Roles.Select(r => r.RoleTypeNum.ToString()));
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.UserName, DateTime.Now, expiration,
                    true, string.Join("|", new string[] { useridString, roleIdString }), FormsAuthentication.FormsCookiePath);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                if (model.IsRememberLogin)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                }
                HttpContext.Current.Response.Cookies.Set(cookie);
                result.AppendData = null;
            }
            return result;
        }

        /// <summary>
        ///     用户退出
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        ///     用户注册
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Register(MemberView model)
        {
            PublicHelper.CheckArgument(model, "model");
            Member member = new Member
            {
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                Name = model.Name,
                Extend = new MemberExtend() { Tel = model.Tel },
                //Roles = new Role[]{new Role(){ RoleType= RoleType.User,Description = ""}},
                MemberTypeNum = (int)model.MemberType
            };
            OperationResult result = AccountContract.Register(member);
            return result;
        }

        /// <summary>
        ///  取用户
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <returns></returns>
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount)
        {
            OperationResult result = AccountContract.GetUsers(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount);
            return result;
        }

        /// <summary>
        ///  免审核
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public OperationResult NoAudit(long memberid)
        {
            try
            {
                bool result = AccountContract.NoAudit(memberid);
                if (result)
                {
                    return new OperationResult(OperationResultType.Success, "免审核成功");
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "免审核失败");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public OperationResult Delete(long memberid)
        {
            try
            {
                bool result = AccountContract.Delete(memberid);
                if (result)
                {
                    return new OperationResult(OperationResultType.Success, "用户删除成功");
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "用户删除失败");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  重置密码
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public OperationResult ResetPassword(long memberid)
        {
            try
            {
                bool result = AccountContract.ResetPassword(memberid);
                if (result)
                {
                    return new OperationResult(OperationResultType.Success, "重置密码成功");
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "重置密码失败");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}