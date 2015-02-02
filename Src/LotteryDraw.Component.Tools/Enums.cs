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
        [Description("竞猜开奖")]
        Answer = 3,
        [Description("现场开奖")]
        Scene = 4,
        [Description("电视开奖")]
        TV = 5
    }

    /// <summary>
    /// 开奖类型
    /// </summary>
    [Description("开奖状态")]
    public enum RevealState
    {
        [Description("未开奖")]
        UnDrawn = 1,
        [Description("开奖中")]
        Drawing = 2,
        [Description("开奖结束")]
        Drawn = 3,
        [Description("取消")]
        Canceled = 4,
        [Description("开奖失败")]
        Failed = 5
    }

    /// <summary>
    /// 竞猜开奖
    /// </summary>
    [Description("竞猜开奖开奖方式")]
    public enum RevealTypeOfAnswer
    {
        [Description("未知")]
        Unknown = 0,
        [Description("自动")]
        Auto = 1,
        [Description("手动")]
        Manual = 2
    }

    /// <summary>
    /// 竞猜开奖开奖条件类型
    /// </summary>
    [Description("竞猜开奖开奖条件类型")]
    public enum AnswerRevealConditionType
    {
        [Description("定时")]
        Timing = 1,
        [Description("定员")]
        Quota = 2
    }

    /// <summary>
    /// 开奖结果状态
    /// </summary>
    [Description("开奖结果状态")]
    public enum LotteryResultState
    {
        [Description("未读")]
        Default = 0,
        [Description("已读")]
        Noticed = 1,
        [Description("已付款")]
        Paid = 2,
        [Description("用户付款已到账")]
        PaymentReceived = 3,
        [Description("用户付款未到账")]
        PaymentNotReceived = 4,
        [Description("已发货")]
        Posting = 5,
        [Description("已收货")]
        Reveived = 6,
        [Description("交易成功")]
        Finished = 7,
        [Description("已关闭")]
        Closed = 8
    }

    /// <summary>
    /// 现场开奖开奖条件类型
    /// </summary>
    [Description("现场抽奖中奖人员奖单状态")]
    public enum LuckySceneStaffState
    {
        [Description("默认")]
        Default = 0,
        [Description("已通知")]
        Noticed = 1,
        [Description("已发奖")]
        Awarded = 2
    }

    /// <summary>
    /// 奖品图片类型
    /// </summary>
    [Description("奖品图片类型")]
    public enum PhotoType
    {
        [Description("默认")]
        Original = 0,
        [Description("缩略图")]
        Thumbnail = 1
    }

    /// <summary>
    /// 抽奖范围
    /// </summary>
    [Description("抽奖范围")]
    public enum ScopeType
    {
        [Description("全国")]
        Global = 1,
        [Description("地级城市")]
        AreaCity = 2,
        [Description("县级城市")]
        Town = 3
    }

    /// <summary>
    /// 现场抽奖人员录入方式
    /// </summary>
    public enum InputTypeOfStaff
    {
        [Description("手动录入")]
        Manual = 1,
        [Description("文件导入")]
        File = 2
    }
}
