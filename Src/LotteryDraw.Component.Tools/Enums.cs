using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LotteryDraw.Component.Tools
{
    public class Enums
    {
    }

    /// <summary>
    /// 表示用户类型的枚举
    /// </summary>
    [Description("用户类型")]
    public enum MemberType
    {
        /// <summary>
        /// 个人类型
        /// </summary>
        [Description("个人用户")]
        Personal = 0,

        /// <summary>
        /// 企业用户类型
        /// </summary>
        [Description("企业用户")]
        Enterprise = 1
    }

    /// <summary>
    /// 开奖类型
    /// </summary>
    [Description("开奖类型")]
    public enum RevealType
    {
        [Description("定时开奖")]
        Timing = 1,
        [Description("定员开奖")]
        Quota = 2,
        [Description("答案开奖")]
        Answer = 3,
        [Description("现场开奖")]
        Scene = 4
    }
}
