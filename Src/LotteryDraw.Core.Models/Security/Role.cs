// 源文件头信息：
// <copyright file="Role.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core.Models
// 最后修改：王金鹏
// 最后修改：2014/08/07 15:26
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Models.Security
{
    /// <summary>
    ///     实体类——角色信息
    /// </summary>
    [Description("角色信息")]
    public class Role : EntityBase<Int64>
    {
        public Role()
        {
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置 角色类型
        /// </summary>
        public RoleType RoleType
        {
            get { return (RoleType)RoleTypeNum; }
            set { RoleTypeNum = (int)value; }
        }

        /// <summary>
        /// 获取或设置 角色类型的数值表示，用于数据库存储
        /// </summary>
        public int RoleTypeNum { get; set; }

        /// <summary>
        ///     获取或设置 拥有此角色的用户信息集合
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }
    }
}
