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
    [Export(typeof(IPrizeBettingContract))]
    public class PrizeBettingService : CoreServiceBase, IPrizeBettingContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeBettingRepository PrizeBettingRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<PrizeBetting> PrizeBettings
        {
            get { return PrizeBettingRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeBetting prizebetting)
        {
            var entity = PrizeBettingRepository.Entities.SingleOrDefault(pb => pb.Member.Id == prizebetting.Member.Id && pb.PrizeOrder.Id == prizebetting.PrizeOrder.Id && !pb.IsDeleted && !pb.IsLucky);
            if (entity != null)
            {
                return new OperationResult(OperationResultType.Warning, "您只能抽奖一次，请勿重复抽奖。", prizebetting);
            }
            int rcount = PrizeBettingRepository.Insert(prizebetting);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "奖单投注成功。", prizebetting);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "奖单投注失败。");
            }
        }

        /// <summary>
        ///     批量添加投注
        /// </summary>
        /// <param name="prizebetting">投注信息集合</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(IEnumerable<PrizeBetting> prizebettings)
        {
            int rcount = PrizeBettingRepository.Insert(prizebettings);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "批量奖单投注成功。");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "批量奖单投注失败。");
            }
        }
    }
}