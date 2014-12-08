// 源文件头信息：
// <copyright file="Member.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core.Models
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:15
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Security;
using System;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Models.Business
{
    /// <summary>
    ///     实体类——奖品信息
    /// </summary>
    [Description("置顶")]
    public class TopOrder : EntityBase<Guid>
    {
        public TopOrder()
        {
            Id = CombHelper.NewComb();
        }

        public virtual PrizeOrder PrizeOrder { get; set; }

        /// <summary>
        /// 获取或设置 客户缴费
        /// </summary>
        public virtual decimal PaymentAmout { get; set; }

        /// <summary>
        /// 获取或设置 排序序列
        /// </summary>
        public virtual int Sequence { get; set; }

        /// <summary>
        /// 获取或设置 截止日期
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 获取或设置 更新日期
        /// </summary>
        public virtual DateTime? UpdateDate { get; set; }
        
        /// <summary>
        /// 获取或设置 操作者
        /// </summary>
        public virtual Member Operator { get; set; }
    }
}