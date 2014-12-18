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
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System;
using LotteryDraw.Component.Data;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     现场抽奖人员核心业务实现
    /// </summary>
    [Export(typeof(ISceneStaffContract))]
    public class SceneStaffService : CoreServiceBase, ISceneStaffContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected ISceneStaffRepository SceneStaffRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 参与现场抽奖人员的查询数据集
        /// </summary>
        public IQueryable<SceneStaff> SceneStaffs
        {
            get { return SceneStaffRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加参与抽奖的人员
        /// </summary>
        /// <param name="staffs">抽奖人员信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(SceneStaff staff)
        {
            int rcount = SceneStaffRepository.Insert(staff);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "现场抽奖人员添加成功。", staff);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "现场抽奖人员添加失败。");
            }
        }

        /// <summary>
        ///     添加参与抽奖的人员
        /// </summary>
        /// <param name="staffs">抽奖人员集合信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(IEnumerable<SceneStaff> staffs)
        {
            int rcount = SceneStaffRepository.Insert(staffs);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "批量添加现场抽奖人员成功。");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "批量添加添加现场抽奖人员失败。");
            }
        }
    }
}