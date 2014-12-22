using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizeBettingView : ModelBase
    {
        public PrizeBettingView()
        {
            PrizeOrderDetailView = new PrizeOrderDetailView();
        }

        public long? UserId { get; set; }

        public string ExchangeCode { get; set; }

        public PrizeOrderDetailView PrizeOrderDetailView { get; set; }

        public string UserAnswer { get; set; }
        //public string AnswerOptionsString { get; set; }
    }
}
