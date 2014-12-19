using LotteryDraw.Component.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    /// <summary>
    /// 用户中奖结果模型
    /// </summary>
    public class LotteryResultView : ModelBase
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// 开奖结果状态
        /// </summary>
        public LotteryResultState LotteryResultState
        {
            get { return (LotteryResultState)LotteryResultStateNum; }
            set { LotteryResultStateNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 开奖结果状态的数值表示，用于数据库存储
        /// </summary>
        public int LotteryResultStateNum { get; set; }

        public MemberView MemberView { get; set; }

        public PrizeOrderView PrizeOrderView { get; set; }
    }
}
