// Դ�ļ�ͷ��Ϣ��
// <copyright file="AccountService.cs">
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


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IPrizeContract))]
    public class PrizeService : CoreServiceBase, IPrizeContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<Prize> Prizes
        {
            get { return PrizeRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prize">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(Prize prize) {
            int rcount = PrizeRepository.Insert(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "������Ʒ�ɹ���", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "������Ʒʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ���½�Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Update(Prize prize)
        {
            return null;
        }

        /// <summary>
        ///     ɾ����Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Delete(Prize prize)
        {
            return null;
        }
    }
}