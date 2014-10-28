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
using System.Data.SqlClient;


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
        /// <returns>ҵ��������</returns>
        public OperationResult Add(PrizeOrder prizeorder)
        {
            int rcount = PrizeOrderRepository.Insert(prizeorder);
            if (rcount > 0)
            {
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
        public OperationResult GetTopPrizeOrders()
        {
            try
            {
                DataSet ds = PrizeOrderRepository.ExecProcdureReturnDataSet("sp_getTopPrizeOrders", null);
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

                SqlParameter paramErrorString = new SqlParameter("@errorString", SqlDbType.VarChar,-1); //-1����max
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
    }
}