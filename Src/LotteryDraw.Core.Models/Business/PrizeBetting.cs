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
    [Description("奖品投注")]
    public class PrizeBetting : EntityBase<Guid>
    {
        public virtual Member Member { get; set; }

        /// <summary>
        /// 获取或设置 开奖信息
        /// </summary>
        public PrizeOrder PrizeOrder { get; set; }

        /// <summary>
        ///  是否中奖
        /// </summary>
        public bool IsLucky { get; set; }
    }
}