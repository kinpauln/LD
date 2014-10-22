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
using System.Data;


namespace LotteryDraw.Site.Impl
{
    /// <summary>
    ///     账户模块站点业务实现
    /// </summary>
    [Export(typeof(IPrizeOrderSiteContract))]
    internal class PrizeOrderSiteService : IPrizeOrderSiteContract
    {
        [Import]
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        protected IPrizeContract PrizeContract { get; set; }

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeOrderView povmodel)
        {
            PublicHelper.CheckArgument(povmodel, "povmodel");
            int? sortorder = PrizeOrderContract.PrizeOrders.Max(po => po.SortOrder);

            PrizeOrder pmodel = new PrizeOrder
            {
                RevealTypeNum = povmodel.RevealTypeNum,
                RevealState = Component.Tools.RevealState.UnDrawn,
                Prize = PrizeContract.Prizes.SingleOrDefault(m => m.Id.Equals(povmodel.PrizeId)),
                Extend = new PrizeOrderExtend() { Id = povmodel.Id },
                SortOrder = (sortorder ?? 0) + 1
            };

            switch (povmodel.RevealType)
            {
                case RevealType.Timing:
                    pmodel.Extend.LaunchTime = povmodel.LaunchTime;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Quota:
                    pmodel.Extend.PoolCount = povmodel.PoolCount;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Answer:
                    pmodel.Extend.PrizeAsking.Question = povmodel.Question;
                    pmodel.Extend.PrizeAsking.Answer = povmodel.Answer;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;

                    pmodel.Extend.AnswerRevealConditionTypeNum = (int)povmodel.AnswerRevealConditionType;
                    if (povmodel.AnswerRevealConditionType == AnswerRevealConditionType.Timing)
                    {
                        pmodel.Extend.LaunchTime = povmodel.LaunchTime;
                    }
                    else if (povmodel.AnswerRevealConditionType == AnswerRevealConditionType.Quota)
                    {
                        pmodel.Extend.PoolCount = povmodel.PoolCount;
                    }
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Scene:
                    break;
            }
            try
            {
                return PrizeOrderContract.Add(pmodel);
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
        public OperationResult Update(PrizeOrderView pvmodel)
        {
            PublicHelper.CheckArgument(pvmodel, "pvmodel");
            try
            {
                PrizeOrder dbmodel = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id.Equals(pvmodel.Id));
                if (dbmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在要更新的Id为{0}的奖单", pvmodel.Id));
                }

                //dbmodel.Name = pvmodel.Name;
                //dbmodel.Description = pvmodel.Description;
                //if (savePhoto) {
                //    dbmodel.Photo = pvmodel.Photo;
                //}
                //dbmodel.UpdateDate = DateTime.Now;
                return PrizeOrderContract.Update(dbmodel);
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
                PrizeOrder pmodel = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id.Equals(guid));
                if (pmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在Id为{0}的奖单", guid));
                }

                return PrizeOrderContract.Delete(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }


        /// <summary>
        ///     获取Top奖单
        /// </summary>
        /// <returns>奖单信息结果集</returns>
        public OperationResult GetTopPrizeOrders()
        {
            OperationResult result = PrizeOrderContract.GetTopPrizeOrders();
            return result;
        }
    }
}