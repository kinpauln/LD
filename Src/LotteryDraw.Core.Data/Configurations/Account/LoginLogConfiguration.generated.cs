﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//		如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类实现 LoginLogConfigurationAppend 分部方法。
// </auto-generated>
//
// <copyright file="LoginLogConfiguration.generated.cs">
//		Copyright(c)2013 Kingdon.All rights reserved.
//		CLR版本：4.0.30319.239
//		开发组织：王金鹏@中国
//		公司网站：http://www.wuliubang.net/
//		所属工程：LotteryDraw.Core.Data
//		生成时间：2014-10-19 09:46
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Data.Configurations.Account
{
    /// <summary>
    /// 实体类-数据表映射——登录记录信息
    /// </summary>    
	internal partial class LoginLogConfiguration : EntityTypeConfiguration<LoginLog>, IEntityMapper
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——登录记录信息
        /// </summary>
        public LoginLogConfiguration()
        {
			LoginLogConfigurationAppend();
        }
		
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void LoginLogConfigurationAppend();
		
        /// <summary>
        /// 将当前实体映射对象注册到当前数据访问上下文实体映射配置注册器中
        /// </summary>
        /// <param name="configurations">实体映射配置注册器</param>
        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
