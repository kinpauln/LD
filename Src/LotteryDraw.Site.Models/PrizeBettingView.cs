using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizeBettingView : ModelBase
    {
        public int UserId { get; set; }
        public Guid? PrizeOrderId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
