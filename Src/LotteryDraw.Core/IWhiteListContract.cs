// 源文件头信息：
// <copyright file="IAccountService.cs">
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
using System;


namespace LotteryDraw.Core
{
    /// <summary>
    ///     白名单核心业务契约
    /// </summary>
    public interface IWhiteListContract
    {
        #region 属性

        /// <summary>
        /// 获取白名单查询数据集
        /// </summary>
        IQueryable<WhiteList> WhiteLists { get; }

        #endregion

        #region 公共方法
        /// <summary>
        ///     添加白名单
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Add(int memberid, Guid poid);

        /// <summary>
        ///     删除白名单
        /// </summary>
        /// <param name="wl">白名单实体</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(WhiteList wl);
        
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
        OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid);
        #endregion
    }
}