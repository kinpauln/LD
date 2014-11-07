using LotteryDraw.Component.Tools;
using LotteryDraw.Component.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class WhiteListView : ModelBase
    {
        public Guid Id { get; set; }

        public int MemberId { get; set; }

        public Guid PrizeOrderId { get; set; }
    }
}
