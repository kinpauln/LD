// 源文件头信息：
// <copyright file="EFRepositoryContext.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Data
// 最后修改：王金鹏
// 最后修改：2013/06/14 23:06
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Tools;


namespace LotteryDraw.Component.Data
{
    /// <summary>
    ///     数据单元操作类
    /// </summary>
    [Export(typeof(IUnitOfWork))]
    public class EFUnitOfWorkContext : UnitOfWorkContextBase
    {
        /// <summary>
        ///     获取 当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get
            {
                bool secondCachingEnabled = ConfigurationManager.AppSettings["EntityFrameworkCachingEnabled"].CastTo(false);
                return secondCachingEnabled ? EFCachingDbContext.Value : EFDbContext.Value;
            }
        }

        [Import("EF", typeof (DbContext))]
        private Lazy<EFDbContext> EFDbContext { get; set; }

        [Import("EFCaching", typeof(DbContext))]
        private Lazy<EFCachingDbContext> EFCachingDbContext { get; set; }
    }
}