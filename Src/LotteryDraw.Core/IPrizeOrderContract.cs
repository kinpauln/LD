// 源文件头信息：
// <copyright file="IPrizeOrderContract.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core
// 最后修改：王金鹏
// 最后修改：2013/05/27 23:06
// </copyright>

using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories;
using LotteryDraw.Core.Models;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;
using System.Collections.Generic;
using System;


namespace LotteryDraw.Core
{
    /// <summary>
    ///     账户模块核心业务契约
    /// </summary>
    public interface IPrizeOrderContract
    {
        #region 属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        IQueryable<PrizeOrder> PrizeOrders { get; }

        #endregion

        #region 公共方法

        /// <summary>
        ///     添加奖单
        /// </summary>
        /// <param name="prizebetting">奖单信息</param>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(PrizeOrder prizeorder, bool shouldMinus = false);

        /// <summary>
        ///  批量添加奖单
        /// </summary>
        /// <param name="prizeorders">奖单集合</param>
        /// <returns></returns>
        OperationResult Add(IEnumerable<PrizeOrder> prizeorders);

        /// <summary>
        ///     更新奖单
        /// </summary>
        /// <param name="prizebetting">奖单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Update(PrizeOrder prizeorder);

        /// <summary>
        ///     删除奖单
        /// </summary>
        /// <param name="prizebetting">奖单信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(PrizeOrder prizeorder);

        /// <summary>
        ///     获取奖单
        /// </summary>
        /// <returns>奖单信息结果集</returns>
        OperationResult GetTopPrizeOrders(int topCount, int? rtype);

        /// <summary>
        ///  开奖
        /// </summary>
        /// <param name="interval">访问数据库频率</param>
        /// <param name="errorString">错误信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult RevealLottery(int interval, out string errorString);
        
        /// <summary>
        ///  “死”奖单
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <returns></returns>
        OperationResult GetDeadLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0);
        
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
        OperationResult GetLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0, int revealstate = 0, string tableName = "");

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
        OperationResult GetRevealedSceneLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealstate = 0);
        
        /// <summary>
        ///  置顶
        /// </summary>
        /// <param name="poid">奖单ID</param> 
        /// <param name="moneyvalue">用户缴费金额</param>
        /// <param name="datelong">置顶时长</param>
        /// <param name="operatorid">操作者Id</param>
        OperationResult Set2Top(Guid poid, decimal moneyvalue, int datelong, long operatorid);

        /// <summary>
        ///  获取奖单实体
        /// </summary>
        /// <param name="poid">奖单ID</param>
        OperationResult GetPrizeOrderById(Guid poid);

        OperationResult GetPrizeOrderDetail(Guid poid);

        /// <summary>
        ///  同时发布奖品、发起抽奖
        /// </summary>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        OperationResult BatchAdd(PrizeOrder porder, bool shouldMinus = false);

        /// <summary>
        ///  后知答案“竞猜开奖”
        /// </summary>
        /// <param name="id">奖单Id</param>
        /// <param name="answer">竞猜答案</param>
        OperationResult RevealManualAnswerLottery(Guid id, string answer);

        /// <summary>
        ///  手动开奖
        /// </summary>
        /// <param name="poid">奖单Id</param>
        /// <param name="rtype">开奖类型</param>
        OperationResult ManualRevealLottery(Guid poid, int rtype);

        /// <summary>
        ///  关闭奖单
        /// </summary>
        /// <param name="poid">奖单Id</param>
        /// <param name="state">奖单状态</param>
        OperationResult UpdateLotteryState(Guid poid, RevealState state);

        #endregion
    }
}