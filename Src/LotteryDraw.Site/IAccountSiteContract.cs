// 源文件头信息：
// <copyright file="IAccountSiteContract.cs">
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
using System.Linq;
using System.Text;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core;
using LotteryDraw.Site.Models;


namespace LotteryDraw.Site
{
    /// <summary>
    ///     账户模块站点业务契约
    /// </summary>
    public interface IAccountSiteContract
    {
        /// <summary>
        ///     用户登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Login(LoginModel model);

        /// <summary>
        ///     用户退出
        /// </summary>
        void Logout();

        /// <summary>
        ///     用户注册
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Register(MemberView model);
        
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
        OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount);

        /// <summary>
        ///  免审核
        /// </summary>
        /// <param name="memberid">用户Id</param>
        /// <param name="noauditTimes">免审核次数</param>
        OperationResult NoAudit(long memberid, int? noauditTimes);

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="memberid">用户Id</param>
        OperationResult Delete(long memberid);

        /// <summary>
        ///  重置密码
        /// </summary>
        /// <param name="memberid">用户Id</param>
        OperationResult ResetPassword(long memberid);

        /// <summary>
        ///  修改密码
        /// </summary>
        /// <param name="memberid">用户Id</param>
        /// <param name="password">新密码</param>
        OperationResult ChangePassword(long memberid, string password);
    }
}