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
    ///     实体类——奖品开奖扩展信息
    /// </summary>
    [Description("奖品开奖扩展信息")]
    public class PrizeOrderExtend : EntityBase<Guid>
    {
        public PrizeOrderExtend()
        {
            //Id = CombHelper.NewComb();
            PrizeAsking = new PrizeAsking();
        }

        /// <summary>
        /// 抽奖范围类别
        /// </summary>
        public ScopeType ScopeType
        {
            get { return (ScopeType)ScopeTypeNum; }
            set { ScopeTypeNum = (int)value; }
        }

        /// <summary>
        /// 抽奖范围类别
        /// </summary>
        public int ScopeTypeNum { get; set; }

        /// <summary>
        /// 抽奖范围(城市名)
        /// </summary>
        public string ScopeCity { get; set; }

        /// <summary>
        ///  开奖时间
        /// </summary>
        public DateTime? LaunchTime { get; set; }

        /// <summary>
        ///  中奖最低人数
        /// </summary>
        [DefaultValue(1)]
        public int MinLuckyCount { get; set; }

        /// <summary>
        ///  开奖率
        /// </summary>
        public float? LuckyPercent { get; set; }

        /// <summary>
        ///  奖池大小
        /// </summary>
        public int? PoolCount { get; set; }

        /// <summary>
        ///  中奖人数
        /// </summary>
        public int? LuckyCount { get; set; }
        
        /// <summary>
        /// 获取或设置 运费
        /// </summary>
        public virtual decimal Freight { get; set; }

        /// <summary>
        ///  备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 获取或设置 竞猜开奖开奖条件类型的数值表示，用于数据库存储
        /// </summary>
        public int AnswerRevealConditionTypeNum { get; set; }

        /// <summary>
        /// 竞猜开奖开奖条件
        /// </summary>
        public AnswerRevealConditionType AnswerRevealConditionType
        {
            get { return (AnswerRevealConditionType)AnswerRevealConditionTypeNum; }
            set { AnswerRevealConditionTypeNum = (int)value; }
        }

        /// <summary>
        ///  问答
        /// </summary>
        public PrizeAsking PrizeAsking { get; set; }
        
        public virtual PrizeOrder PrizeOrder { get; set; }
    }
}