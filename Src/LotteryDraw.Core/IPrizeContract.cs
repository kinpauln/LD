// Դ�ļ�ͷ��Ϣ��
// <copyright file="IAccountService.cs">
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
    public interface IPrizeContract
    {
        #region ����

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<Prize> Prizes { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prizeorder">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(Prize prize);

        /// <summary>
        ///     ���½�Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Update(Prize prize);

        /// <summary>
        ///     ɾ����Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Delete(Prize prize);

        #endregion
    }
}