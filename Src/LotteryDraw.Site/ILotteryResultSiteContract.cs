﻿// 源文件头信息：
// <copyright file="IAccountSiteContract.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Site
// 最后修改：王金鹏
// 最后修改：2013/05/20 13:06
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core;
using LotteryDraw.Site.Models;


namespace LotteryDraw.Site
{
    /// <summary>
    ///     账户模块站点业务契约
    /// </summary>
    public interface ILotteryResultSiteContract
    {
        OperationResult UpdateLotteryResult(Guid id, int state);
    }
}