using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LotteryResultView
    {
        public MemberView MemberView { get; set; }

        public PrizeOrderView PrizeOrderView { get; set; }
    }
}
