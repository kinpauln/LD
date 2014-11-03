using LotteryDraw.Component.Tools;
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
        /// 答案开奖开奖条件类型
        /// </summary>
        [Display(Name = "答案开奖开奖条件类型")]
        public AnswerRevealConditionType AnswerRevealConditionType
        {
            get { return (AnswerRevealConditionType)AnswerRevealConditionTypeNum; }
            set { AnswerRevealConditionTypeNum = (int)value; }
        }

        public int AnswerRevealConditionTypeNum { get; set; }

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
        [DefaultValue(100)]
        public int? PoolCount { get; set; }

        /// <summary>
        ///  中奖人数
        /// </summary>
        [Display(Name = "中奖人数")]
        [DefaultValue(5)]
        public int? LuckyCount { get; set; }

        [Display(Name = "问题")]
        public string Question { get; set; }

        [Display(Name = "答案选项")]
        public string AnswerOptions { get; set; }

        [Display(Name = "答案")]
        public string Answer { get; set; }
                
        public DateTime? UpdateDate { get; set; }
    }
}
