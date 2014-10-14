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


namespace LotteryDraw.Core
{
    /// <summary>
    ///     奖单投注模块核心业务契约
    /// </summary>
    public interface IPrizeBettingContract
    {
        #region 属性

        /// <summary>
        /// 获取 奖单投注信息查询数据集
        /// </summary>
        IQueryable<PrizeBetting> PrizeBettings { get; }

        #endregion

        #region 公共方法

        /// <summary>
        ///     添加投注
        /// </summary>
        /// <param name="prizebetting">投注信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(PrizeBetting prizebetting);

        #endregion
    }
}