﻿// 源文件头信息：
// <copyright file="IRepository.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Data
// 最后修改：王金鹏
// 最后修改：2013/05/27 18:53
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using LotteryDraw.Component.Tools;
using System.Data;
using System.Data.SqlClient;


namespace LotteryDraw.Component.Data
{
    /// <summary>
    ///     定义仓储模型中的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : EntityBase<TKey>
    {
        #region 属性

        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(TEntity entity, bool isSave = true);

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(TKey id, bool isSave = true);

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(TEntity entity, bool isSave = true);

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true);

        /// <summary>
        ///     更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Update(TEntity entity, bool isSave = true);

        /// <summary>
        /// 使用附带新值的实体信息更新指定实体属性的值
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="isSave">是否执行保存</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        /// <returns>操作影响的行数</returns>
        int Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity, bool isSave = true);

        /// <summary>
        ///     查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        TEntity GetByKey(TKey key);

        /// <summary>
        /// EF SQL 语句返回 dataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        System.Data.DataTable SqlQueryForDataTatable(string sql,
                System.Data.SqlClient.SqlParameter[] parameters);

        /// <summary>
        /// 执行存储过程,返回DataSet对象
        /// </summary>
        /// <param name="pname">存储过程名字</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>数据集</returns>
        DataSet ExecProcdureReturnDataSet(string pname, params SqlParameter[] parameters);

        /// <summary>
        /// 执行存储过程,影响的行数
        /// </summary>
        /// <param name="pname">存储过程名字</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>数据集</returns>
        int ExecProcdureReturnRowCount(string pname, params SqlParameter[] parameters);

        /// <summary>
        /// 执行存储过程,带着SqlCommand输出参数
        /// </summary>
        /// <param name="pname">存储过程名字</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>影响的行数</returns>
        int ExecProcdure(string pname, out SqlCommand command, params SqlParameter[] parameters);

        #endregion

    }
}