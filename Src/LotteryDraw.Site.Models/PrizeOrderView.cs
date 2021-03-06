﻿using LotteryDraw.Component.Tools;
using LotteryDraw.Component.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizeOrderView : ModelBase
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 开奖类型
        /// </summary>
        [Display(Name = "开奖类型")]
        public RevealType RevealType
        {
            get { return (RevealType)RevealTypeNum; }
            set { RevealTypeNum = (int)value; }
        }

        public int RevealTypeNum { get; set; }


        /// <summary>
        /// 开奖状态
        /// </summary>
        [Display(Name = "开奖状态")]
        public RevealState RevealState
        {
            get { return (RevealState)RevealStateNum; }
            set { RevealStateNum = (int)value; }
        }

        public int RevealStateNum { get; set; }

        /// <summary>
        /// 抽奖范围
        /// </summary>
        [Display(Name = "抽奖范围")]
        public ScopeType ScopeType
        {
            get { return (ScopeType)ScopeTypeNum; }
            set { ScopeTypeNum = (int)value; }
        }

        /// <summary>
        ///  运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        ///  预售价格
        /// </summary>
        public decimal PresalePrice { get; set; }

        /// <summary>
        ///  伪造参与抽奖者数目
        /// </summary>
        public int ForgedParticipantCount { get; set; }

        public int ScopeTypeNum { get; set; }

        /// <summary>
        ///  抽奖省份
        /// </summary>
        public string ScopeProvince { get; set; }

        /// <summary>
        ///  抽奖城市
        /// </summary>
        public string ScopeAreaCity { get; set; }

        /// <summary>
        /// 抽奖范围(县城名，区名)
        /// </summary>
        public string ScopeTown { get; set; }

        /// <summary>
        /// 竞猜开奖开奖条件类型
        /// </summary>
        //[Display(Name = "竞猜开奖开奖条件类型")]
        public AnswerRevealConditionType AnswerRevealConditionType
        {
            get { return (AnswerRevealConditionType)AnswerRevealConditionTypeNum; }
            set { AnswerRevealConditionTypeNum = (int)value; }
        }

        public int AnswerRevealConditionTypeNum { get; set; }

        /// <summary>
        /// 获取或设置 竞竞猜开奖开奖方式的数值表示，用于数据库存储
        /// </summary>
        public int RevealTypeOfAnswerNum { get; set; }

        /// <summary>
        /// 竞猜开奖开奖方式
        /// </summary>
        public RevealTypeOfAnswer RevealTypeOfAnswer
        {
            get { return (RevealTypeOfAnswer)RevealTypeOfAnswerNum; }
            set { RevealTypeOfAnswerNum = (int)value; }
        }

        [Display(Name = "备注")]
        public string Remarks { get; set; }

        public Guid PrizeId { get; set; }

        public int SortOrder { get; set; }

        /// <summary>
        ///  开奖时间
        /// </summary>
        [Display(Name = "开奖时间")]
        public DateTime? LaunchTime { get; set; }

        /// <summary>
        ///  开奖执行时间
        /// </summary>
        [Display(Name = "开奖执行时间")]
        public DateTime? RevealDate { get; set; }

        /// <summary>
        ///  中奖最低人数
        /// </summary>
        [Display(Name = "中奖最低人数")]
        [DefaultValue(1)]
        public int MinLuckyCount { get; set; }

        /// <summary>
        ///  开奖率
        /// </summary>
        [Display(Name = "开奖率")]
        [DefaultValue(1)]
        public float? LuckyPercent { get; set; }

        /// <summary>
        ///  奖池大小
        /// </summary>
        [Display(Name = "奖池大小")]
        //[DefaultValue(100)]
        public int? PoolCount { get; set; }

        /// <summary>
        ///  中奖人数
        /// </summary>
        [Display(Name = "中奖人数")]
        //[DefaultValue(5)]
        public int? LuckyCount { get; set; }

        [Display(Name = "问题")]
        public string Question { get; set; }

        [Display(Name = "答案选项")]
        public string AnswerOptions { get; set; }

        [Display(Name = "答案")]
        public string Answer { get; set; }

        public string StaffsOfScenceString { get; set; }

        public string LuckyStaffsOfScenceString { get; set; }

        public int StaffTotalCount { get; set; }

        public bool Is2Top { get; set; }

        public bool IsNeedExchangeCode { get; set; }

        public InputTypeOfStaff InputTypeOfStaff
        {
            get { return (InputTypeOfStaff)InputTypeOfStaffNum; }
            set { InputTypeOfStaffNum = (int)value; }
        }

        public int InputTypeOfStaffNum { get; set; }

        public DateTime? UpdateDate { get; set; }

        public PrizeView PrizeView { get; set; }

        /// <summary>
        /// 已抽奖人数
        /// </summary>
        public int BettingCount { get; set; }

        /// <summary>
        /// 白名单人数
        /// </summary>
        public int WhiteListCount { get; set; }

        /// <summary>
        /// 已参与抽奖人数
        /// </summary>
        public int JoinedCount
        {
            get { return BettingCount + WhiteListCount; }
        }
    }
}
