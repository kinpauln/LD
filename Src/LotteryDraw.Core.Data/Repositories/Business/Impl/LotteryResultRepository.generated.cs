﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="LotteryResultRepository.generated.cs">
//		Copyright(c)2013 Kingdon.All rights reserved.
//		CLR版本：4.0.30319.239
//		开发组织：王金鹏@中国
//		公司网站：http://www.wuliubang.net/
//		所属工程：LotteryDraw.Core.Data
//		生成时间：2014-12-02 17:13
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Linq;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core.Data.Repositories.Business.Impl
{
	/// <summary>
    ///   仓储操作层实现——开奖结果
    /// </summary>
    [Export(typeof(ILotteryResultRepository))]
    public partial class LotteryResultRepository : EFRepositoryBase<LotteryResult, Guid>, ILotteryResultRepository
    { }
}
