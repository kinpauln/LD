// 源文件头信息：
// <copyright file="IPrizeOrderSiteContract.cs">
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
    ///     奖单模块站点业务契约
    /// </summary>
    public interface IPrizeOrderSiteContract
    {
        /// <summary>
        ///     添加奖单
        /// </summary>
        /// <param name="prizebetting">奖单信息</param>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(PrizeOrderView prizeorder, bool shouldMinus = false);

        /// <summary>
        ///     更新奖单
        /// </summary>
        /// <param name="member">奖单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Update(PrizeOrderView prizeorder);

        /// <summary>
        ///     删除奖单
        /// </summary>
        /// <param name="member">奖单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(Guid guid);

        /// <summary>
        ///     获取奖单
        /// </summary>
        /// <returns>奖单信息结果集</returns>
        OperationResult GetTopPrizeOrders(int topCount, int? rtype);

        /// <summary>
        ///  取奖单
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <param name="revealstate">奖单状态</param>
        /// <returns></returns>
        OperationResult GetLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0, int revealstate = 0);

        /// <summary>
        ///  置顶
        /// </summary>
        /// <param name="poid">奖单ID</param>
        OperationResult Set2Top(Guid poid);

        OperationResult GetPrizeAsking(Guid poid);
    }
}