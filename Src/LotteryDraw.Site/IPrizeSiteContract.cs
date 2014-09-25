// 源文件头信息：
// <copyright file="IPrizeSiteContract.cs">
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
    public interface IPrizeSiteContract
    {
        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prize">奖品信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(PrizeView prize);

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Update(PrizeView prize);

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(Guid guid);
    }
}