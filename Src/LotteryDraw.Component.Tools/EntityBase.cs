// 源文件头信息：
// <copyright file="Entity.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Tools
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:04
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace LotteryDraw.Component.Tools
{
    /// <summary>
    ///     可持久到数据库的领域模型的基类。
    /// </summary>
    [Serializable]
    public abstract class EntityBase<TKey>
    {
        #region 构造函数

        /// <summary>
        ///     数据实体基类
        /// </summary>
        protected EntityBase()
        {
            IsDeleted = false;
            AddDate = DateTime.Now;
        }

        #endregion

        #region 属性

        [Key]
        public TKey Id { get; set; }

        /// <summary>
        ///     获取或设置 获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     获取或设置 添加时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime AddDate { get; set; }

        #endregion
    }
}