// 源文件头信息：
// <copyright file="AccountService.cs">
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
    [Export(typeof(IPrizeContract))]
    public class PrizeService : CoreServiceBase, IPrizeContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<Prize> Prizes
        {
            get { return PrizeRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prize">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(Prize prize) {
            int rcount = PrizeRepository.Insert(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "发布商品成功。", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "发布商品失败。");
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(Prize prize)
        {
            return null;
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(Prize prize)
        {
            return null;
        }
    }
}