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


namespace LotteryDraw.Site.Impl
{
    /// <summary>
    ///     账户模块站点业务实现
    /// </summary>
    [Export(typeof(IPrizeBettingSiteContract))]
    internal class PrizeBettingSiteService : IPrizeBettingSiteContract
    {
        [Import]
        protected IPrizeBettingContract PrizeBettingContract { get; set; }

        [Import]
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        protected IAccountContract AccountContract { get; set; }

        /// <summary>
        ///     添加投注
        /// </summary>
        /// <param name="prizebetting">奖单信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeBettingView prizebetting)
        {
            //PublicHelper.CheckArgument(prizebetting, "prizebetting");

            PrizeBetting pmodel = new PrizeBetting
            {
                Phone = prizebetting.Phone,
                Address = prizebetting.Address,
                PrizeOrder = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id==prizebetting.PrizeOrderId),
                Member = AccountContract.Members.SingleOrDefault(m => m.Id == prizebetting.UserId)
            };

            try
            {
                return PrizeBettingContract.Add(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}