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
using System;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Models.Business
{
    /// <summary>
    ///     实体类——奖品开奖信息
    /// </summary>
    [Description("奖品开奖信息")]
    public class PrizeOrder : EntityBase<Guid>
    {
        public PrizeOrder()
        {
            Id = CombHelper.NewComb();
        }

        /// <summary>
        /// 开奖类型
        /// </summary>
        public RevealType RevealType
        {
            get { return (RevealType)RevealTypeNum; }
            set { RevealTypeNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 抽奖类型的数值表示，用于数据库存储
        /// </summary>
        public int RevealTypeNum { get; set; }

        /// <summary>
        /// 开奖状态
        /// </summary>
        public RevealState RevealState
        {
            get { return (RevealState)RevealStateNum; }
            set { RevealStateNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 开奖状态的数值表示，用于数据库存储
        /// </summary>
        public int RevealStateNum { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int? SortOrder { get; set; }

        public DateTime? RevealDate { get; set; }

        /// <summary>
        /// 获取或设置 该奖单下的所有用户投注
        /// </summary>
        public virtual ICollection<PrizeBetting> PrizeBettings { get; set; }

        /// <summary>
        /// 获取或设置 该奖单下的所有参与现场抽奖的人员（现场抽奖）
        /// </summary>
        public virtual ICollection<SceneStaff> SceneStaffs { get; set; }

        /// <summary>
        /// 获取或设置 该奖单下的抽奖结果（非现场抽奖）
        /// </summary>
        public virtual ICollection<LotteryResult> LotteryResults { get; set; }


        public virtual Prize Prize { get; set; }

        public virtual PrizeOrderExtend Extend { get; set; }
    }
}