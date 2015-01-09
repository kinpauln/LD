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
    [Description("开奖结果")]
    public class LotteryResult : EntityBase<Guid>
    {
        public LotteryResult()
        {
            Id = CombHelper.NewComb();
        }

        public virtual Member Member { get; set; }

        /// <summary>
        /// 获取或设置 开奖信息
        /// </summary>
        public virtual PrizeOrder PrizeOrder { get; set; }

        public LotteryResultState LotteryResultState
        {
            get { return (LotteryResultState)LotteryResultStateNum; }
            set { LotteryResultStateNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 中奖状态
        /// </summary>
        public int LotteryResultStateNum { get; set; }

        public int State { get; set; }
        
        /// <summary>
        /// 获取或设置 中奖感言
        /// </summary>
        public string SpeechAfterWinning { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}