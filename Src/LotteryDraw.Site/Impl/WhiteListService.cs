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
    [Export(typeof(IWhiteListSiteContract))]
    internal class WhiteListSiteService : IWhiteListSiteContract
    {
        [Import]
        protected IWhiteListContract WhiteListContract { get; set; }

        [Import]
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        protected IAccountContract AccountContract { get; set; }

        /// <summary>
        ///     添加白名单
        /// </summary>
        /// <param name="prizebetting">白名单信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(WhiteListView pvmodel)
        {
            PublicHelper.CheckArgument(pvmodel, "pvmodel");
            WhiteList pmodel = new WhiteList
            {
                PrizeOrder = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id == pvmodel.PrizeOrderId),
                Member = AccountContract.Members.SingleOrDefault(m => m.Id.Equals(pvmodel.MemberId))
            };
            try
            {
                return WhiteListContract.Add(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     删除白名单
        /// </summary>
        /// <param name="member">白名单信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(Guid guid)
        {
            try
            {
                WhiteList pmodel = WhiteListContract.WhiteLists.SingleOrDefault(m => m.Id == guid);
                if (pmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在Id为{0}的白名单项", guid));
                }

                return WhiteListContract.Delete(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

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
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid)
        {
            OperationResult result = WhiteListContract.GetUsers(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount,poid);
            return result;
        }
    }
}