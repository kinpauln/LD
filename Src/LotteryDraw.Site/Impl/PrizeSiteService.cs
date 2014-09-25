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
    [Export(typeof(IPrizeSiteContract))]
    internal class PrizeSiteService : IPrizeSiteContract
    {
        [Import]
        protected IPrizeContract PrizeContract { get; set; }

        [Import]
        protected IAccountContract AccountContract { get; set; }

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prize">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeView pvmodel)
        {
            PublicHelper.CheckArgument(pvmodel, "pvmodel");
            Prize pmodel = new Prize
            {
                Name = pvmodel.Name,
                Description = pvmodel.Description,
                Member = AccountContract.Members.SingleOrDefault(m => m.Id.Equals(pvmodel.MemberId))
            };
            try
            {
                return PrizeContract.Add(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(PrizeView pvmodel)
        {
            PublicHelper.CheckArgument(pvmodel, "pvmodel");
            try
            {
                Prize dbmodel = PrizeContract.Prizes.SingleOrDefault(m => m.Id.Equals(pvmodel.Id));
                if (dbmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在要更新的Id为{0}的奖品", pvmodel.Id));
                }

                dbmodel.Name = pvmodel.Name;
                dbmodel.Description = pvmodel.Description;
                dbmodel.UpdateDate = DateTime.Now;
                return PrizeContract.Update(dbmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(Guid guid)
        {
            try
            {
                Prize pmodel = PrizeContract.Prizes.SingleOrDefault(m => m.Id.Equals(guid));
                if (pmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在Id为{0}的奖品", guid));
                }

                return PrizeContract.Delete(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}