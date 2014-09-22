// 源文件头信息：
// <copyright file="IEntityMapper.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Data
// 最后修改：王金鹏
// 最后修改：2013/06/14 22:00
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;


namespace LotteryDraw.Component.Data
{
    /// <summary>
    ///     实体映射接口
    /// </summary>
    [InheritedExport]
    public interface IEntityMapper
    {
        /// <summary>
        ///     将当前实体映射对象注册到当前数据访问上下文实体映射配置注册器中
        /// </summary>
        /// <param name="configurations">实体映射配置注册器</param>
        void RegistTo(ConfigurationRegistrar configurations);
    }
}