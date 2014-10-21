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
        /// <returns>业务操作结果</returns>
        OperationResult Add(PrizeOrder prizeorder);

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
        OperationResult GetTopPrizeOrders();

        /// <summary>
        ///  开奖
        /// </summary>
        /// <returns>业务操作结果</returns>
        OperationResult RevealLottery(out string errorString);

        #endregion
    }
}