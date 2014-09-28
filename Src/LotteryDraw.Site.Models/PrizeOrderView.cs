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
        public RevealType RevealType { get; set; }

        /// <summary>
        /// 开奖状态
        /// </summary>
        [Display(Name = "开奖状态")]
        public RealState RealState
        {
            get;
            set;
        }

        [Display(Name = "描述")]
        public string Description { get; set; }

        public Guid PrizeId { get; set; }

        /// <summary>
        ///  开奖时间
        /// </summary>
        [Display(Name = "开奖时间")]
        public DateTime? LaunchTime { get; set; }

        /// <summary>
        ///  中奖最低人数
        /// </summary>
        [Display(Name = "中奖最低人数")]
        public int MinLuckyCount { get; set; }

        /// <summary>
        ///  开奖率
        /// </summary>
        [Display(Name = "开奖率")]
        public float? LuckyPercent { get; set; }

        /// <summary>
        ///  奖池大小
        /// </summary>
        [Display(Name = "奖池大小")]
        public int? PoolCount { get; set; }

        /// <summary>
        ///  中奖人数
        /// </summary>
        [Display(Name = "中奖人数")]
        public int? LuckyCount { get; set; }

        [Display(Name = "问题")]
        public string Question { get; set; }

        [Display(Name = "答案")]
        public string Answer { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
