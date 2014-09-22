// 源文件头信息：
// <copyright file="SortCondition.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Tools
// 最后修改：王金鹏
// 最后修改：2013/07/10 19:01
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace LotteryDraw.Component.Tools
{
    /// <summary>
    ///     属性排序条件信息类
    /// </summary>
    public class PropertySortCondition
    {
        /// <summary>
        ///     构造一个指定属性名称的升序排序的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        public PropertySortCondition(string propertyName)
            : this(propertyName, ListSortDirection.Ascending) { }

        /// <summary>
        ///     构造一个排序属性名称和排序方式的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        /// <param name="listSortDirection">排序方式</param>
        public PropertySortCondition(string propertyName, ListSortDirection listSortDirection)
        {
            PropertyName = propertyName;
            ListSortDirection = listSortDirection;
        }

        /// <summary>
        ///     获取或设置 排序属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///     获取或设置 排序方向
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }
}