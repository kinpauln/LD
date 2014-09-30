// 源文件头信息：
// <copyright file="PrizeOrderService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:08
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories.Account;
using LotteryDraw.Core.Data.Repositories.Security;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Data.Repositories.Business;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    [Export(typeof(IPrizeOrderContract))]
    public class PrizeOrderService : CoreServiceBase, IPrizeOrderContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeOrderRepository PrizeOrderRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<PrizeOrder> PrizeOrders
        {
            get { return PrizeOrderRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizeorder">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeOrder prizeorder) {
            int rcount = PrizeOrderRepository.Insert(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "发布奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "发布奖单失败。");
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Update(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "更新奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "更新奖单失败。");
            }
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Delete(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "删除奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "删除奖单失败。");
            }
        }
    }
}