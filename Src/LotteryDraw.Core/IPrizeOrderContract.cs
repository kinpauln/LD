// Դ�ļ�ͷ��Ϣ��
// <copyright file="IPrizeOrderContract.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core
// ����޸ģ�������
// ����޸ģ�2013/05/27 23:06
// </copyright>

using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories;
using LotteryDraw.Core.Models;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core
{
    /// <summary>
    ///     �˻�ģ�����ҵ����Լ
    /// </summary>
    public interface IPrizeOrderContract
    {
        #region ����

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<PrizeOrder> PrizeOrders { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     ��ӽ���
        /// </summary>
        /// <param name="prizeorder">������Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(PrizeOrder prizeorder);

        /// <summary>
        ///     ���½���
        /// </summary>
        /// <param name="prizeorder">������Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Update(PrizeOrder prizeorder);

        /// <summary>
        ///     ɾ������
        /// </summary>
        /// <param name="prizeorder">������Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Delete(PrizeOrder prizeorder);

        #endregion
    }
}