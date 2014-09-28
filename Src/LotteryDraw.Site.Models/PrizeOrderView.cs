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
    public class PrizeOrderView
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 开奖类型
        /// </summary>
        public RevealType RevealType { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        public Guid PrizeId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
