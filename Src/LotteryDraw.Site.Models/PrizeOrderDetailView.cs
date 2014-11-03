
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
    public class PrizeOrderDetailView : ModelBase
    {
        public PrizeOrderView PrizeOrderView { get; set; }

        public MemberView MemberView { get; set; }

        public PrizeView PrizeView { get; set; }
    }
}
