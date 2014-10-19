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


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IPrizeBettingContract))]
    public class PrizeBettingService : CoreServiceBase, IPrizeBettingContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IPrizeBettingRepository PrizeBettingRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<PrizeBetting> PrizeBettings
        {
            get { return PrizeBettingRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(PrizeBetting prizebetting)
        {
            var entity = PrizeBettingRepository.Entities.SingleOrDefault(pb => pb.Member.Id == prizebetting.Member.Id && pb.PrizeOrder.Id == prizebetting.PrizeOrder.Id && !pb.IsDeleted && !pb.IsLucky);
            if (entity != null)
            {
                return new OperationResult(OperationResultType.Warning, "��ֻ�ܳ齱һ�Σ������ظ��齱��", prizebetting);
            }
            int rcount = PrizeBettingRepository.Insert(prizebetting);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "����Ͷע�ɹ���", prizebetting);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "����Ͷעʧ�ܡ�");
            }
        }

        /// <summary>
        ///     �������Ͷע
        /// </summary>
        /// <param name="prizebetting">Ͷע��Ϣ����</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(IEnumerable<PrizeBetting> prizebettings)
        {
            int rcount = PrizeBettingRepository.Insert(prizebettings);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "��������Ͷע�ɹ���");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "��������Ͷעʧ�ܡ�");
            }
        }
    }
}