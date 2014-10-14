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
    ///     ����Ͷעģ�����ҵ����Լ
    /// </summary>
    public interface IPrizeBettingContract
    {
        #region ����

        /// <summary>
        /// ��ȡ ����Ͷע��Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<PrizeBetting> PrizeBettings { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     ���Ͷע
        /// </summary>
        /// <param name="prizebetting">Ͷע��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(PrizeBetting prizebetting);

        #endregion
    }
}