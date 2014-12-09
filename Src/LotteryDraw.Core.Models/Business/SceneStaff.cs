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
    ///     实体类——现场抽奖人员名单
    /// </summary>
    [Description("现场抽奖人员")]
    public class SceneStaff : EntityBase<Guid>
    {
        public SceneStaff()
        {
            Id = CombHelper.NewComb();
        }

        [Required]
        public string Value { get; set; }

        [DefaultValue(false)]
        public bool IsLucky { get; set; }

        /// <summary>
        /// 现场抽奖中奖人员奖单状态
        /// </summary>
        public LuckySceneStaffState LuckySceneStaffState
        {
            get { return (LuckySceneStaffState)LuckySceneStaffStateNum; }
            set { LuckySceneStaffStateNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 开奖状态的数值表示，用于数据库存储
        /// </summary>
        public int LuckySceneStaffStateNum { get; set; }

        /// <summary>
        /// 获取或设置 奖单信息
        /// </summary>
        public virtual PrizeOrder PrizeOrder { get; set; }
    }
}