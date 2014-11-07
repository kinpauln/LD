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
using LotteryDraw.Core.Models.Business;
using LotteryDraw.Core;


namespace LotteryDraw.Site
{
    /// <summary>
    ///     账户模块站点业务实现
    /// </summary>
    public interface IWhiteListSiteContract
    {
        /// <summary>
        ///     添加白名单
        /// </summary>
        /// <param name="prizebetting">白名单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(WhiteListView pvmodel);

        /// <summary>
        ///     删除白名单
        /// </summary>
        /// <param name="member">白名单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(Guid guid);
        
        /// <summary>
        ///  取待添加至白名单的用户
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealstate">奖单ID</param>
        /// <returns></returns>
        OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid);
    }
}