// Դ�ļ�ͷ��Ϣ��
// <copyright file="PrizeOrderService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core
// ����޸ģ�������
// ����޸ģ�2014/08/06 23:08
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
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IPrizeOrderContract))]
    public class PrizeOrderService : CoreServiceBase, IPrizeOrderContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IPrizeOrderRepository PrizeOrderRepository { get; set; }

        [Import]
        protected IMemberRepository MemberRepository { get; set; }

        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<PrizeOrder> PrizeOrders
        {
            get { return PrizeOrderRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <param name="shouldMinus">�Ƿ�ö��û��Ŀɷ���齱������</param>
        /// <returns>ҵ��������</returns>
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
                return new OperationResult(OperationResultType.Success, "���������ɹ���", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "��������ʧ�ܡ�");
            }
        }

        /// <summary>
        ///  ������ӽ���
        /// </summary>
        /// <param name="prizeorders">��������</param>
        /// <returns></returns>
        public OperationResult Add(IEnumerable<PrizeOrder> prizeorders)
        {
            int rcount = PrizeOrderRepository.Insert(prizeorders);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "������ӽ����ɹ���");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "������ӽ����ɹ���");
            }
        }

        /// <summary>
        ///     ���½�Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Update(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Update(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "���½����ɹ���", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "���½���ʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ɾ����Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Delete(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Delete(prizeorder);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "ɾ�������ɹ���", prizeorder);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "ɾ������ʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>������Ϣ�����</returns>
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
                return new OperationResult(OperationResultType.Success, "��ȡTop�����ɹ���", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  ����
        /// </summary>
        /// <param name="interval">�������ݿ�Ƶ��</param>
        /// <param name="errorString">������Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult RevealLottery(int interval, out string errorString)
        {
            errorString = string.Empty;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                SqlParameter paramInterval = new SqlParameter("@interval", SqlDbType.Int);
                paramInterval.Value = interval;
                paramList.Add(paramInterval);

                SqlParameter paramErrorString = new SqlParameter("@errorString", SqlDbType.VarChar, -1); //-1����max
                paramErrorString.Direction = ParameterDirection.Output;
                paramList.Add(paramErrorString);


                SqlCommand command = new SqlCommand();
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_revealLottery", out command, paramList.ToArray());
                errorString = command.Parameters["@errorString"].Value.ToString();
                return new OperationResult(OperationResultType.Success, "�����������ݿ����˳����", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  ȡ����
        /// </summary>
        /// <param name="pageSize">ÿҳ����ļ�¼��</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="whereString">�����ַ���</param>
        /// <param name="orderbyString">�����ַ���</param>
        /// <param name="totalCount">�����ܼ�¼</param>
        /// <param name="totalPageCount">������ҳ��</param>
        /// <param name="revealtype">��������</param>
        /// <param name="revealstate">����״̬</param>
        /// <returns></returns>
        public OperationResult GetLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0, int revealstate = 0)
        {
            totalCount = 0;
            totalPageCount = 0;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //��������
                SqlParameter paramRT = new SqlParameter("@RevealType", SqlDbType.Int);
                paramRT.Value = revealtype;
                paramList.Add(paramRT);
                //����״̬
                SqlParameter paramRS = new SqlParameter("@RevealState", SqlDbType.Int);
                paramRS.Value = revealstate;
                paramList.Add(paramRS);
                //ÿҳ����ļ�¼��
                SqlParameter paramPS = new SqlParameter("@PageSize", SqlDbType.Int);
                paramPS.Value = pageSize;
                paramList.Add(paramPS);
                //��ǰҳ��
                SqlParameter paramPI = new SqlParameter("@PageIndex", SqlDbType.Int);
                paramPI.Value = pageIndex;
                paramList.Add(paramPI);
                //�����ַ���
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //�����ַ���
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
                return new OperationResult(OperationResultType.Success, "�����������ݿ����˳����", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  �ö�
        /// </summary>
        /// <param name="poid">����ID</param>
        /// <param name="moneyvalue">�û��ɷѽ��</param>
        /// <param name="datelong">�ö�ʱ��</param>
        /// <param name="operatorid">������Id</param>
        public OperationResult Set2Top(Guid poid, decimal moneyvalue, int datelong, long operatorid)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                //�����ַ���
                SqlParameter paramPoId = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPoId.Value = poid.ToString();
                paramList.Add(paramPoId);

                //�ö�ʱ��
                SqlParameter paramdl = new SqlParameter("@DateLong", SqlDbType.Int);
                paramdl.Value = datelong;
                paramList.Add(paramdl);

                //�ɷѽ��
                SqlParameter parammval = new SqlParameter("@PaymentAmout", SqlDbType.Decimal);
                parammval.Value = moneyvalue;
                paramList.Add(parammval);

                //������Id
                SqlParameter paramopid = new SqlParameter("@OperatorId", SqlDbType.BigInt);
                paramopid.Value = operatorid;
                paramList.Add(paramopid);

                SqlParameter paramec = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramec.Direction = ParameterDirection.Output;
                paramList.Add(paramec);

                SqlParameter parammsg = new SqlParameter("@Message", SqlDbType.VarChar, -1); //-1����max
                parammsg.Direction = ParameterDirection.Output;
                paramList.Add(parammsg);

                SqlCommand command = new SqlCommand();
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_set2Top", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                string message = command.Parameters["@Message"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return new OperationResult(OperationResultType.Success, "�����ö��ɹ���", message);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "��ǰ�������ö������ϴ��ö���δ���ڡ�", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "�����ˡ�", null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  ��ȡ����ʵ��
        /// </summary>
        /// <param name="poid">����ID</param>
        public OperationResult GetPrizeOrderById(Guid poid)
        {
            try
            {
                var model = PrizeOrderRepository.Entities.Where(po => po.Id == poid && !po.IsDeleted).FirstOrDefault();
                return new OperationResult(OperationResultType.Success, "������ȡ�ɹ���", model);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  ͬʱ������Ʒ������齱
        /// </summary>
        /// <param name="shouldMinus">�Ƿ�ö��û��Ŀɷ���齱������</param>
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
                return new OperationResult(OperationResultType.Success, "������Ʒ������齱һ���Բ����ɹ���", porder);
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