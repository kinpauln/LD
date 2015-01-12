// 源文件头信息：
// <copyright file="MemberExtend.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core.Models
// 最后修改：王金鹏
// 最后修改：2013/05/20 13:43
// </copyright>

using System;
using System.ComponentModel;

using LotteryDraw.Component.Tools;


namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     实体类——用户扩展信息
    /// </summary>
    [Description("用户扩展信息")]
    public class MemberExtend : EntityBase<Guid>
    {
        /// <summary>
        /// 初始化一个 用户扩展实体类 的新实例
        /// </summary>
        public MemberExtend()
        {
            Id = CombHelper.NewComb();
            Address = new MemberAddress();
        }

        public string Tel { get; set; }

        /// <summary>
        ///  头像
        /// </summary>
        public string HeadPortrait { get; set; }

        /// <summary>
        ///  广告Url
        /// </summary>
        public string AdvertisingUrl { get; set; }

        public MemberAddress Address { get; set; }

        public virtual Member Member { get; set; }
    }
}