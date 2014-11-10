// 源文件头信息：
// <copyright file="AccountService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:08
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories.Account;
using LotteryDraw.Core.Data.Repositories.Security;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Data.Repositories.Business;
using LotteryDraw.Core.Models.Business;
using System.Data.SqlClient;
using System.Data;
using System;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     白名单核心业务契约
    /// </summary>
    [Export(typeof(IWhiteListContract))]
    public class WhiteListService : CoreServiceBase, IWhiteListContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IWhiteListRepository WhiteListRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取白名单查询数据集
        /// </summary>
        public IQueryable<WhiteList> WhiteLists
        {
            get { return WhiteListRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加白名单
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(int memberid, Guid poid)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //用户ID
                SqlParameter paramMemberId = new SqlParameter("@MemberId", SqlDbType.Int);
                paramMemberId.Value = memberid;
                paramList.Add(paramMemberId);
                //PrizeOrder Id
                SqlParameter paramPI = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPI.Value = poid.ToString();
                paramList.Add(paramPI);

                SqlParameter paramerrorcode = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramerrorcode.Direction = ParameterDirection.Output;
                paramList.Add(paramerrorcode);

                SqlCommand command = new SqlCommand();
                DataSet ds = WhiteListRepository.ExecProcdureReturnDataSet("sp_addToWhiteList", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return new OperationResult(OperationResultType.Success, "添加白名单成功。", null);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "白名单数目不能超过中奖人数。", null);
                        case "Error_02":
                            return new OperationResult(OperationResultType.Warning, "该用户已在白名单中，不能重复添加。", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "出错了。", null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     删除白名单
        /// </summary>
        /// <param name="wl">白名单实体</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(WhiteList wl)
        {
            int rcount = WhiteListRepository.Delete(wl);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "删除白名单成功。", wl);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "删除白名单失败。");
            }
        }

        /// <summary>
        ///  取待添加至白名单的用户
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealstate">奖单ID</param>
        /// <returns></returns>
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid)
        {
            totalCount = 0;
            totalPageCount = 0;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //每页输出的记录数
                SqlParameter paramPS = new SqlParameter("@PageSize", SqlDbType.Int);
                paramPS.Value = pageSize;
                paramList.Add(paramPS);
                //当前页数
                SqlParameter paramPI = new SqlParameter("@PageIndex", SqlDbType.Int);
                paramPI.Value = pageIndex;
                paramList.Add(paramPI);
                //where条件字符串
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //排序字符串
                SqlParameter paramOrder = new SqlParameter("@Order", SqlDbType.VarChar, 1000);
                paramOrder.Value = orderbyString;
                paramList.Add(paramOrder);
                //奖单ID
                SqlParameter paramPOId = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPOId.Value = poid.ToString();
                paramList.Add(paramPOId);

                SqlParameter paramtc = new SqlParameter("@TotalCount", SqlDbType.Int);
                paramtc.Direction = ParameterDirection.Output;
                paramList.Add(paramtc);
                SqlParameter paramtpc = new SqlParameter("@TotalPageCount", SqlDbType.Int);
                paramtpc.Direction = ParameterDirection.Output;
                paramList.Add(paramtpc);
                SqlParameter paramerrorcode = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramerrorcode.Direction = ParameterDirection.Output;
                paramList.Add(paramerrorcode);

                SqlCommand command = new SqlCommand();
                DataSet ds = WhiteListRepository.ExecProcdureReturnDataSet("sp_getUsersForWhiteList", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    totalCount = Convert.ToInt32(command.Parameters["@TotalCount"].Value);
                    totalPageCount = Convert.ToInt32(command.Parameters["@TotalPageCount"].Value);
                    return new OperationResult(OperationResultType.Success, "模糊查询用户操作顺利。", ds);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "白名单数目不能超过中奖人数。", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "出错了。", null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}