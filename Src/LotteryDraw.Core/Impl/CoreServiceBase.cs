using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Data;
using LotteryDraw.Component.Tools;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    /// 核心业务实现基类
    /// </summary>
    public abstract class CoreServiceBase
    {
        /// <summary>
        /// 获取或设置 工作单元对象，用于处理同步业务的事务操作
        /// </summary>
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        ///     获取 EntityFramework的数据仓储上下文
        /// </summary>
        protected UnitOfWorkContextBase EFContext
        {
            get
            {
                if (UnitOfWork is UnitOfWorkContextBase)
                {
                    return UnitOfWork as UnitOfWorkContextBase;
                }
                throw new DataAccessException(string.Format("数据仓储上下文对象类型不正确，应为UnitOfWorkContextBase，实际为 {0}", UnitOfWork.GetType().Name));
            }
        }
    }
}
