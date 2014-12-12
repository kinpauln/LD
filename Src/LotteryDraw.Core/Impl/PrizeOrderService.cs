// 源文件头信息：
// <copyright file="PrizeOrderService.cs">
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
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System;
using LotteryDraw.Component.Data;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    [Export(typeof(IPrizeOrderContract))]
    public class PrizeOrderService : CoreServiceBase, IPrizeOrderContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeOrderRepository PrizeOrderRepository { get; set; }

        [Import]
        protected IMemberRepository MemberRepository { get; set; }

        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<PrizeOrder> PrizeOrders
        {
            get { return PrizeOrderRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeOrder prizeorder, bool shouldMinus = false)
        {
            int rcount = PrizeOrderRepository.Insert(prizeorder);
            if (rcount > 0)
            {
                if (shouldMinus)
                {
                    var member = MemberRepository.Entities.Where(m => m.Id == prizeorder.Prize.Member.Id).FirstOrDefault();
                    if (member != null)
                    {
                        member.PubishingEnableTimes--;
                        MemberRepository.Update(member);
                    }
                }
                return new OperationResult(OperationResultType.Success, "发布奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "发布奖单失败。");
            }
        }

        /// <summary>
        ///  批量添加奖单
        /// </summary>
        /// <param name="prizeorders">奖单集合</param>
        /// <returns></returns>
        public OperationResult Add(IEnumerable<PrizeOrder> prizeorders)
        {
            int rcount = PrizeOrderRepository.Insert(prizeorders);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "批量添加奖单成功。");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "批量添加奖单成功。");
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Update(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "更新奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "更新奖单失败。");
            }
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Delete(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "删除奖单成功。", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "删除奖单失败。");
            }
        }

        /// <summary>
        ///     获取奖单
        /// </summary>
        /// <returns>奖单信息结果集</returns>
        public OperationResult GetTopPrizeOrders(int topCount, int? rtype)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter paramTopCount = new SqlParameter("@topCount", SqlDbType.Int);
                paramTopCount.Value = topCount;
                paramList.Add(paramTopCount);

                SqlParameter paramRType = new SqlParameter("@revealType", SqlDbType.Int);
                paramRType.Value = rtype ?? 0;
                paramList.Add(paramRType);

                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_getTopPrizeOrders", paramList.ToArray());
                return new OperationResult(OperationResultType.Success, "获取Top奖单成功。", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  开奖
        /// </summary>
        /// <param name="interval">访问数据库频率</param>
        /// <param name="errorString">错误信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult RevealLottery(int interval, out string errorString)
        {
            errorString = string.Empty;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter paramInterval = new SqlParameter("@interval", SqlDbType.Int);
                paramInterval.Value = interval;
                paramList.Add(paramInterval);

                SqlParameter paramErrorString = new SqlParameter("@errorString", SqlDbType.VarChar, -1); //-1代表max
                paramErrorString.Direction = ParameterDirection.Output;
                paramList.Add(paramErrorString);


                SqlCommand command = new SqlCommand();
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_revealLottery", out command, paramList.ToArray());
                errorString = command.Parameters["@errorString"].Value.ToString();
                return new OperationResult(OperationResultType.Success, "开奖过程数据库操作顺利。", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  取奖单
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <param name="revealstate">奖单状态</param>
        /// <returns></returns>
        public OperationResult GetLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0, int revealstate = 0)
        {
            totalCount = 0;
            totalPageCount = 0;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //开奖类型
                SqlParameter paramRT = new SqlParameter("@RevealType", SqlDbType.Int);
                paramRT.Value = revealtype;
                paramList.Add(paramRT);
                //奖单状态
                SqlParameter paramRS = new SqlParameter("@RevealState", SqlDbType.Int);
                paramRS.Value = revealstate;
                paramList.Add(paramRS);
                //每页输出的记录数
                SqlParameter paramPS = new SqlParameter("@PageSize", SqlDbType.Int);
                paramPS.Value = pageSize;
                paramList.Add(paramPS);
                //当前页数
                SqlParameter paramPI = new SqlParameter("@PageIndex", SqlDbType.Int);
                paramPI.Value = pageIndex;
                paramList.Add(paramPI);
                //排序字符串
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //排序字符串
                SqlParameter paramOrder = new SqlParameter("@Order", SqlDbType.VarChar, 1000);
                paramOrder.Value = orderbyString;
                paramList.Add(paramOrder);

                SqlParameter paramtc = new SqlParameter("@TotalCount", SqlDbType.Int);
                paramtc.Direction = ParameterDirection.Output;
                paramList.Add(paramtc);
                SqlParameter paramtpc = new SqlParameter("@TotalPageCount", SqlDbType.Int);
                paramtpc.Direction = ParameterDirection.Output;
                paramList.Add(paramtpc);


                SqlCommand command = new SqlCommand();
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_getLotteries", out command, paramList.ToArray());
                totalCount = Convert.ToInt32(command.Parameters["@TotalCount"].Value);
                totalPageCount = Convert.ToInt32(command.Parameters["@TotalPageCount"].Value);
                return new OperationResult(OperationResultType.Success, "开奖过程数据库操作顺利。", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  置顶
        /// </summary>
        /// <param name="poid">奖单ID</param>
        /// <param name="moneyvalue">用户缴费金额</param>
        /// <param name="datelong">置顶时长</param>
        /// <param name="operatorid">操作者Id</param>
        public OperationResult Set2Top(Guid poid, decimal moneyvalue, int datelong, long operatorid)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                //排序字符串
                SqlParameter paramPoId = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPoId.Value = poid.ToString();
                paramList.Add(paramPoId);

                //置顶时长
                SqlParameter paramdl = new SqlParameter("@DateLong", SqlDbType.Int);
                paramdl.Value = datelong;
                paramList.Add(paramdl);

                //缴费金额
                SqlParameter parammval = new SqlParameter("@PaymentAmout", SqlDbType.Decimal);
                parammval.Value = moneyvalue;
                paramList.Add(parammval);

                //操作者Id
                SqlParameter paramopid = new SqlParameter("@OperatorId", SqlDbType.BigInt);
                paramopid.Value = operatorid;
                paramList.Add(paramopid);

                SqlParameter paramec = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramec.Direction = ParameterDirection.Output;
                paramList.Add(paramec);

                SqlParameter parammsg = new SqlParameter("@Message", SqlDbType.VarChar, -1); //-1代表max
                parammsg.Direction = ParameterDirection.Output;
                paramList.Add(parammsg);

                SqlCommand command = new SqlCommand();
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_set2Top", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                string message = command.Parameters["@Message"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return new OperationResult(OperationResultType.Success, "奖单置顶成功。", message);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "当前奖单已置顶过，上次置顶还未到期。", null);
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
        ///  获取奖单实体
        /// </summary>
        /// <param name="poid">奖单ID</param>
        public OperationResult GetPrizeOrderById(Guid poid)
        {
            try
            {
                var model = PrizeOrderRepository.Entities.Where(po => po.Id == poid && !po.IsDeleted).FirstOrDefault();
                return new OperationResult(OperationResultType.Success, "奖单获取成功。", model);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  同时发布奖品、发起抽奖
        /// </summary>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        public OperationResult BatchAdd(PrizeOrder porder, bool shouldMinus = false)
        {
            SqlTransaction tran = null;
            try
            {
                Database db = EFContext.DbContext.Database;
                using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
                {
                    conn.Open();
                    using (tran = conn.BeginTransaction())
                    {
                        var member = MemberRepository.Entities.Where(m => m.Id == porder.Prize.Member.Id).FirstOrDefault();
                        porder.Prize.Member = member;
                        if (shouldMinus)
                        {
                            if (member != null)
                            {
                                member.PubishingEnableTimes--;
                                MemberRepository.Update(member);
                            }
                        }
                        PrizeRepository.Insert(porder.Prize);
                        PrizeOrderRepository.Insert(porder);
                        tran.Commit();
                    }
                }
                return new OperationResult(OperationResultType.Success, "发布奖品、发起抽奖一次性操作成功。", porder);
            }
            catch (DataAccessException ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}