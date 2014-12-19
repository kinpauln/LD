// 源文件头信息：
// <copyright file="Member.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core.Models
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:15
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;
using System;


namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     实体类——用户信息
    /// </summary>
    [Description("用户信息")]
    public class Member : EntityBase<Int64>
    {
        public Member()
        {
            Roles = new List<Role>();
            LoginLogs = new List<LoginLog>();
        }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        //[Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 用户类型
        /// </summary>
        public MemberType MemberType
        {
            get { return (MemberType)MemberTypeNum; }
            set { MemberTypeNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 角色类型的数值表示，用于数据库存储
        /// </summary>
        public int MemberTypeNum { get; set; }

        /// <summary>
        /// 获取或设置 用户扩展信息
        /// </summary>
        public virtual MemberExtend Extend { get; set; }

        /// <summary>
        ///  可发布奖品的次数
        /// </summary>
        //[DefaultValue(0)]
        public int PubishingEnableTimes { get; set; }

        /// <summary>
        /// 获取或设置 充值历史
        /// </summary>
        public virtual ICollection<RechargeHistory> RechargeHistories { get; set; }

        /// <summary>
        /// 获取或设置 用户下的奖品
        /// </summary>
        public virtual ICollection<Prize> Prizes { get; set; }

        /// <summary>
        /// 获取或设置 该用户参与的抽奖
        /// </summary>
        public virtual ICollection<PrizeBetting> PrizeBettings { get; set; }

        /// <summary>
        /// 获取或设置 该用户中奖的奖（非现场抽奖）
        /// </summary>
        public virtual ICollection<LotteryResult> LotteryResults { get; set; }

        /// <summary>
        /// 获取或设置 用户拥有的角色信息集合
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 获取或设置 用户登录记录集合
        /// </summary>
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
    }
}